namespace SexyFishHorse.Irc.Client
{
    using System;
    using SexyFishHorse.Irc.Client.Configuration;

    public class IrcClient : IIrcClient
    {
        private readonly IConfiguration configuration;

        public IrcClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }
    }
}
