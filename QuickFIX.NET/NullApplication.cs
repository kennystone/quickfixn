﻿
namespace QuickFix
{
    public class NullApplication : Application
    {
        public void FromAdmin(Message message, SessionID sessionID)
        { }

        public void FromApp(Message message, SessionID sessionID)
        { }

        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) { }
        public void ToAdmin(Message message, SessionID sessionID) { }
        public void ToApp(Message message, SessionID sessionID) { }
    }
}
