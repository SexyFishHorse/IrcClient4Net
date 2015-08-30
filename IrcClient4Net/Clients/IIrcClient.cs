namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using SexyFishHorse.Irc.Client.EventHandlers;
    using SexyFishHorse.Irc.Client.Models;

    public interface IIrcClient : IDisposable
    {
        event OnConnectedEventHandler OnConnected;

        event OnMessageSentEventHandler OnMessageSent;

        event OnDisconnectedEventHandler OnDisconnected;

        bool Connected { get; }

        void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password);

        void SendRawMessage(string message);

        string ReadRawMessage();

        IrcMessage ReadIrcMessage();

        void Disconnect(string message = null);
    }
}
