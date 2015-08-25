namespace SexyFishHorse.Irc.Client.Clients
{
    using SexyFishHorse.Irc.Client.Models;

    public interface IIrcClient
    {
        void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password);

        void SendRawMessage(string message);

        string ReadRawMessage();
    }
}
