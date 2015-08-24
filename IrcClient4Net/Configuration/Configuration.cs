namespace SexyFishHorse.Irc.Client.Configuration
{
    using System.Configuration;

    public class Configuration : IConfiguration
    {
        public string TwitchIrcServerName
        {
            get
            {
                return ConfigurationManager.AppSettings["irc.twitch.serverName"];
            }
        }

        public int TwitchIrcPortNumber
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["irc.twitch.portNumber"]);
            }
        }

        public string TwitchIrcNickname
        {
            get
            {
                return ConfigurationManager.AppSettings["irc.twitch.nickname"];
            }
        }

        public string TwitchIrcPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["irc.twitch.password"];
            }
        }

        public string TwitchIrcPrivmsgFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["irc.twitch.privmsgFormat"];
            }
        }
    }
}
