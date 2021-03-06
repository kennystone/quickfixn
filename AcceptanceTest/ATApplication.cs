﻿using System.Collections.Generic;
using QuickFix;

namespace AcceptanceTest
{
    public class ATApplication : MessageCracker, Application
    {
        private HashSet<KeyValuePair<string,SessionID>> clOrdIDs_ = new HashSet<KeyValuePair<string,SessionID>>();
        private FileLog log_;

        public ATApplication(FileLog debugLog)
        {
            log_ = debugLog;
        }

        public void OnMessage(QuickFix.FIX40.NewOrderSingle nos, SessionID sessionID)
        {
            ProcessNOS(nos, sessionID);
        }

        public void OnMessage(QuickFix.FIX41.NewOrderSingle nos, SessionID sessionID)
        {
            ProcessNOS(nos, sessionID);
        }

        public void OnMessage(QuickFix.FIX42.NewOrderSingle nos, SessionID sessionID) 
        {
            ProcessNOS(nos, sessionID);
        }

        public void OnMessage(QuickFix.FIX42.SecurityDefinition message, SessionID sessionID)
        {
            Echo(message, sessionID);
        }

        public void OnMessage(QuickFix.FIX43.NewOrderSingle nos, SessionID sessionID)
        {
            ProcessNOS(nos, sessionID);
        }

        public void OnMessage(QuickFix.FIX43.SecurityDefinition message, SessionID sessionID)
        {
            Echo(message, sessionID);
        }

        public void OnMessage(QuickFix.FIX44.NewOrderSingle nos, SessionID sessionID)
        {
            ProcessNOS(nos, sessionID);
        }

        public void OnMessage(QuickFix.FIX44.SecurityDefinition message, SessionID sessionID)
        {
            Echo(message, sessionID);
        }

        protected void Echo(Message message, SessionID sessionID)
        {
            Message echo = new Message(message);
            Session.SendToTarget(echo, sessionID);
        }
        
        protected void ProcessNOS(Message message, SessionID sessionID)
        {
            Message echo = new Message(message);
 
                bool possResend = false;
                if (message.Header.IsSetField(QuickFix.Fields.Tags.PossResend))
                    possResend = message.Header.GetBoolean(QuickFix.Fields.Tags.PossResend);

                KeyValuePair<string, SessionID> pair = new KeyValuePair<string, SessionID>(message.GetField(QuickFix.Fields.Tags.ClOrdID), sessionID);
                if (possResend && clOrdIDs_.Contains(pair))
                    return;
                clOrdIDs_.Add(pair);

            Session.SendToTarget(echo, sessionID);
        }

        #region Application Methods

        public void OnCreate(SessionID sessionID)
        {
            Session session = Session.LookupSession(sessionID);
            if (null != session)
                session.Reset();
        }
        
        public void OnLogout(SessionID sessionID)
        {
            clOrdIDs_.Clear();
        }

        public void OnLogon(SessionID sessionID)
        { }

        public void FromApp(Message message, SessionID sessionID)
        {
            try
            {
                string msgType = message.Header.GetField(QuickFix.Fields.Tags.MsgType);
                log_.OnEvent("Got message " + msgType);
                Crack(message, sessionID);
            }
            catch (QuickFix.UnsupportedMessageType e)
            {
                throw e;
            }
            catch (System.Exception e)
            {
                log_.OnEvent("FromApp: " + e.ToString() + " while processing msg (" + message.ToString() + ")");
            }
        }

        public void FromAdmin(Message message, SessionID sessionID)
        { }

        public void ToAdmin(Message message, SessionID sessionID) { }
        public void ToApp(Message message, SessionID sessionID) { }

        #endregion
    }
}
