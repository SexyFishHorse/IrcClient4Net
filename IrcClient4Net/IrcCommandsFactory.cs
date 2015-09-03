namespace SexyFishHorse.Irc.Client
{
    public static class IrcCommandsFactory
    {
        public static string Pass(string password)
        {
            return string.Format("PASS {0}", password);
        }

        public static string Nick(string nickname)
        {
            return string.Format("NICK {0}", nickname);
        }

        public static string User(string username, string realname)
        {
            return string.Format("USER {0} 8 * :{1}", username, realname);
        }

        public static string Join(string channel)
        {
            return string.Format("JOIN #{0}", channel);
        }

        public static string PrivMsg(string username, string host, string channel, string message)
        {
            return string.Format(":{0}!{1} PRIVMSG #{2} :{3}", username, host, channel, message);
        }

        public static string Part(string channel)
        {
            return string.Format("PART #{0}", channel);
        }

        public static string CapReq(string capability)
        {
            return string.Format("CAP REQ :{0}", capability);
        }

        public static string Quit(string message = null)
        {
            return string.IsNullOrWhiteSpace(message) ? "QUIT" : string.Format("QUIT :{0}", message);
        }

        public static string Oper(string username, string password)
        {
            return string.Format("OPER {0} {1}", username, password);
        }
    }
}
