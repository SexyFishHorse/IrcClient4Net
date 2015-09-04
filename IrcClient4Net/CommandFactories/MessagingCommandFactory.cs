namespace SexyFishHorse.Irc.Client.CommandFactories
{
    public class MessagingCommandFactory
    {
        public static string PrivMsg(string username, string host, string channel, string message)
        {
            return string.Format(":{0}!{1} PRIVMSG {2} :{3}", username, host, channel, message);
        }
    }
}
