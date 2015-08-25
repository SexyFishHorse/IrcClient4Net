namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using SexyFishHorse.Irc.Client.Configuration;

    public class TwitchIrcClient : ITwitchIrcClient
    {
        private readonly IConfiguration configuration;

        private TcpClient client;

        private StreamReader inputStream;

        private StreamWriter outputStream;

        public TwitchIrcClient(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Connect()
        {
            client = new TcpClient(configuration.TwitchIrcServerName, configuration.TwitchIrcPortNumber);
            inputStream = new StreamReader(client.GetStream());
            outputStream = new StreamWriter(client.GetStream());

            outputStream.WriteLine(IrcCommands.Pass(configuration.TwitchIrcPassword));
            outputStream.WriteLine(IrcCommands.Nick(configuration.TwitchIrcNickname));
            outputStream.WriteLine(IrcCommands.User(configuration.TwitchIrcNickname, configuration.TwitchIrcNickname));
            outputStream.Flush();
        }

        public void JoinRoom()
        {
            SendIrcMessage(IrcCommands.Join(configuration.TwitchIrcNickname));
        }

        public void SendIrcMessage(string message)
        {
            Console.WriteLine("<SENT> " + message);
            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        public void SendChatMessage(string message)
        {
            SendIrcMessage(
                IrcCommands.PrivMsg(
                    configuration.TwitchIrcNickname,
                    string.Format("{0}@tmi.twitch.tv", configuration.TwitchIrcNickname),
                    configuration.TwitchIrcNickname,
                    message));
        }

        public string ReadRawMessage()
        {
            return inputStream.ReadLine();
        }

        public void LeaveRoom()
        {
            SendIrcMessage(IrcCommands.Part(configuration.TwitchIrcNickname));
        }

        public void RequestMembershipCapability()
        {
            SendIrcMessage(IrcCommands.CapReq(configuration.TwitchIrcMembershipCapability));
        }
    }
}
