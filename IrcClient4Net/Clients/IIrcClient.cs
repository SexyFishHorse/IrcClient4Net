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

        /// <summary>
        /// Throws an ApplicationException if the message isn't of the expected command
        /// </summary>
        /// <param name="message">The message to validate</param>
        /// <param name="expectedCommand">The command the message is expected to have</param>
        void ValidateCommand(IrcMessage message, string expectedCommand);
    }
}
