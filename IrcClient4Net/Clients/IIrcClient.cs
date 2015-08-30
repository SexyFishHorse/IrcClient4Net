namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using Models;
    using SexyFishHorse.Irc.Client.EventHandling.EventHandlers;

    public interface IIrcClient : IDisposable
    {
        event OnConnectedEventHandler OnConnected;

        event OnMessageSentEventHandler OnMessageSent;

        event OnDisconnectedEventHandler OnDisconnected;

        event OnRawMessageReadEventHandler OnRawMessageRead;

        event OnIrcMessageReadEventHandler OnIrcMessageRead;

        bool Connected { get; }

        void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password);

        void SendRawMessage(string message);

        string ReadRawMessage();

        IrcMessage ReadIrcMessage();

        void Disconnect(string message = null);
    }
}
