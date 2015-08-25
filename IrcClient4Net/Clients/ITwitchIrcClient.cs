namespace SexyFishHorse.Irc.Client.Clients
{
    using SexyFishHorse.Irc.Client.Models;

    public interface ITwitchIrcClient
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
