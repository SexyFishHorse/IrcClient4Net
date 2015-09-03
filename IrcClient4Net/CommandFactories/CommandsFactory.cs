namespace SexyFishHorse.Irc.Client.CommandFactories
{
    public static class CommandsFactory
    {

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
    }
}
