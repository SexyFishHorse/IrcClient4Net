namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using Models;
    using SexyFishHorse.Irc.Client.EventHandling.EventArgs;

    public interface IIrcClient : IDisposable
    {
        bool IsConnected { get; }

        void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password);

        void SendRawMessage(string message);

        string ReadRawMessage();

        IrcMessage ReadIrcMessage();

        void Disconnect(string message = null);

        event Action<OnConnectedEventArgs> Connected;

        event Action<OnConnectionFailedEventArgs> ConnectionFailed;

        event Action<OnMessageSentEventArgs> MessageSent;

        event Action<OnDisconnectedEventArgs> Disconnected;

        event Action<OnRawMessageReadEventArgs> RawMessageRead;

        event Action<OnIrcMessageReadEventArgs> IrcMessageRead;
    }
}
