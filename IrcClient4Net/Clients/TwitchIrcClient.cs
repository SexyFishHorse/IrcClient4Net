﻿namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using Configuration;
    using Parsers;
    using CommandFactories;
    using Validators;

    public class TwitchIrcClient : IrcClient, ITwitchIrcClient
    {
        private readonly IConfiguration configuration;

        public TwitchIrcClient(IMessageParser messageParser, IResponseValidator responseValidator, IConfiguration configuration)
            : base(messageParser, responseValidator)
        {
            this.configuration = configuration;
        }

        public void Connect()
        {
            Connect(
                configuration.TwitchIrcServerName,
                configuration.TwitchIrcPortNumber,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcNickname,
                configuration.TwitchIrcTmiToken);
        }

        public void JoinRoom()
        {
            SendIrcMessage(CommandsFactory.Join(configuration.TwitchIrcNickname));
        }

        public void SendIrcMessage(string message)
        {
            Console.WriteLine("<SENT> " + message);
            SendRawMessage(message);
        }

        public void SendChatMessage(string message)
        {
            SendIrcMessage(
                CommandsFactory.PrivMsg(
                    configuration.TwitchIrcNickname,
                    string.Format("{0}@tmi.twitch.tv", configuration.TwitchIrcNickname),
                    configuration.TwitchIrcNickname,
                    message));
        }

        public void LeaveRoom()
        {
            SendIrcMessage(CommandsFactory.Part(configuration.TwitchIrcNickname));
        }

        public void RequestMembershipCapability()
        {
            SendIrcMessage(CommandsFactory.CapReq(configuration.TwitchIrcMembershipCapability));
        }

        public void Timeout(string username, int seconds)
        {
            SendRawMessage(
                CommandsFactory.PrivMsg(
                    configuration.TwitchIrcNickname,
                    string.Format("{0}@tmi.twitch.tv", configuration.TwitchIrcNickname),
                    configuration.TwitchIrcNickname,
                    string.Format(".timeout {0} {1}", username, seconds)));
        }

        public void Ban(string username)
        {
            SendRawMessage(
                CommandsFactory.PrivMsg(
                    configuration.TwitchIrcNickname,
                    string.Format("{0}@tmi.twitch.tv", configuration.TwitchIrcNickname),
                    configuration.TwitchIrcNickname,
                    string.Format(".timeout {0}", username)));
        }
    }
}
