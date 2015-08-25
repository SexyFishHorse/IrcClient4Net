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

        public string TwitchIrcTmiToken
        {
            get
            {
                return ConfigurationManager.AppSettings["irc.twitch.tmiToken"];
            }
        }

        public string TwitchIrcMembershipCapability
        {
            get
            {
                return ConfigurationManager.AppSettings["irc.twitch.capabilities.membership"];
            }
        }
    }
}
