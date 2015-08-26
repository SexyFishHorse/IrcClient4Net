namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using SexyFishHorse.Irc.Client.Configuration;
    using SexyFishHorse.Irc.Client.Models;

    public class TwitchIrcClient : ITwitchIrcClient
    {
        private readonly IConfiguration configuration;

        private readonly IIrcClient client;

        public TwitchIrcClient(IConfiguration configuration, IIrcClient client)
        {
            this.configuration = configuration;
            this.client = client;
        }

        public bool Connected
        {
            get
            {
                return client.Connected;
            }
        }

        public void Connect()
        {
            client.Connect(
                configuration.TwitchIrcServerName,
                configuration.TwitchIrcPortNumber,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcTmiToken);
        }

        public void JoinRoom()
        {
            SendIrcMessage(IrcCommandsFactory.Join(configuration.TwitchIrcNickname));
        }

        public void SendIrcMessage(string message)
        {
            Console.WriteLine("<SENT> " + message);
            client.SendRawMessage(message);
        }

        public void SendChatMessage(string message)
        {
            SendIrcMessage(
                IrcCommandsFactory.PrivMsg(
                    configuration.TwitchIrcNickname,
                    string.Format("{0}@tmi.twitch.tv", configuration.TwitchIrcNickname),
                    configuration.TwitchIrcNickname,
                    message));
        }

        public void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password)
        {
            client.Connect(serverName, portNumber, username, nickname, realname, password);
        }

        public void SendRawMessage(string message)
        {
            client.SendRawMessage(message);
        }

        public string ReadRawMessage()
        {
            return client.ReadRawMessage();
        }

        public void LeaveRoom()
        {
            SendIrcMessage(IrcCommandsFactory.Part(configuration.TwitchIrcNickname));
        }

        public void RequestMembershipCapability()
        {
            SendIrcMessage(IrcCommandsFactory.CapReq(configuration.TwitchIrcMembershipCapability));
        }

        public IrcMessage ReadIrcMessage()
        {
            return client.ReadIrcMessage();
        }

        public void Disconnect(string message = null)
        {
            client.Disconnect(message);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
