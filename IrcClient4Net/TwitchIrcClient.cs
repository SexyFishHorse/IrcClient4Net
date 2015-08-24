namespace SexyFishHorse.Irc.Client
{
    using System;
    using SexyFishHorse.Irc.Client.Configuration;

    public class TwitchIrcClient : ITwitchIrcClient
    {
        private readonly IConfiguration configuration;

        public TwitchIrcClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }
    }
}
