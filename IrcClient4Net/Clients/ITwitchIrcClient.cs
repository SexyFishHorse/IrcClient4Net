namespace SexyFishHorse.Irc.Client.Clients
{
    public interface ITwitchIrcClient : IIrcClient
    {
        void Connect();

        void JoinRoom();

        void SendIrcMessage(string message);

        void SendChatMessage(string message);

        void LeaveRoom();

        void RequestMembershipCapability();

        void Timeout(string username, int seconds);

        void Ban(string username);
    }
}
