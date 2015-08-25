﻿namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using SexyFishHorse.Irc.Client.Configuration;

    public class TwitchIrcClient : ITwitchIrcClient
    {
        private readonly IConfiguration configuration;

        private readonly IIrcClient client;

        public TwitchIrcClient(IConfiguration configuration, IIrcClient client)
        {
            this.configuration = configuration;
            this.client = client;
        }

        public void Connect()
        {
            client.Connect(
                configuration.TwitchIrcServerName,
                configuration.TwitchIrcPortNumber,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcPassword);
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
    }
}