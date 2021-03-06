﻿using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace QuickFix
{
    public class SocketInitiatorThread : Responder
    {
        public Session Session { get { return session_; } }
        public Transport.SocketInitiator Initiator { get { return initiator_; } }

        public const int BUF_SIZE = 512;

        private Thread thread_ = null;
        private byte[] readBuffer_ = new byte[BUF_SIZE];
        private Parser parser_;
        private Socket socket_;
        private Transport.SocketInitiator initiator_;
        private Session session_;
        private IPEndPoint socketEndPoint_;

        public SocketInitiatorThread(Transport.SocketInitiator initiator, Session session, IPEndPoint socketEndPoint)
        {
            initiator_ = initiator;
            session_ = session;
            socketEndPoint_ = socketEndPoint;
            parser_ = new Parser();
            socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            session_ = session;
        }

        public void Start()
        {
            thread_ = new Thread(new ParameterizedThreadStart(Transport.SocketInitiator.SocketInitiatorThreadStart));
            thread_.Start(this);
        }

        public void Join()
        {
            if (null == thread_)
                return;
            Disconnect();
            thread_.Join(5000);
            thread_ = null;
        }

        public void Connect()
        {
            socket_.Connect(socketEndPoint_);
            session_.SetResponder(this);
        }

        public bool Read()
        {
            try
            {
                if (socket_.Poll(1000000, SelectMode.SelectRead)) // one-second timeout
                {
                    int bytesRead = socket_.Receive(readBuffer_);
                    if (0 == bytesRead)
                        throw new SocketException(System.Convert.ToInt32(SocketError.ConnectionReset));
                    parser_.AddToStream(System.Text.Encoding.UTF8.GetString(readBuffer_, 0, bytesRead));
                }
                else if (null != session_)
                {
                    session_.Next();
                }
                else
                {
                    throw new QuickFIXException("Initiator timed out while reading socket");
                }
            
                ProcessStream();
                return true;
            }
            catch (System.Exception e)
            {
                if (null != session_)
                    session_.Disconnect(e.Message);
                else
                    Disconnect();
                return false;
            }
        }

        private void ProcessStream()
        {
            string msg;
            while (parser_.ReadFixMessage(out msg))
                session_.Next(msg);
        }

        #region Responder Members

        public bool Send(string data)
        {
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes(data);
            int bytesSent = socket_.Send(rawData);
            return bytesSent > 0;
        }

        public void Disconnect()
        {
            socket_.Close();
        }

        #endregion
    }
}
