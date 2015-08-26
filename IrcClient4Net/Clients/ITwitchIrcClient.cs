namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using SexyFishHorse.Irc.Client.Models;

    public interface ITwitchIrcClient : IDisposable
    {
        void Connect();

        void JoinRoom();

        void SendIrcMessage(string message);

        void SendChatMessage(string message);

        string ReadRawMessage();

        void LeaveRoom();

        void RequestMembershipCapability();

        IrcMessage ReadIrcMessage();
    }
}
