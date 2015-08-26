namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using SexyFishHorse.Irc.Client.Models;

    public interface IIrcClient : IDisposable
    {
        void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password);

        void SendRawMessage(string message);

        string ReadRawMessage();

        IrcMessage ReadIrcMessage();

        void Disconnect(string message = null);
    }
}
