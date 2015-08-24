namespace SexyFishHorse.Irc.Client
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

            outputStream.WriteLine("PASS {0}", configuration.TwitchIrcPassword);
            outputStream.WriteLine("NICK {0}", configuration.TwitchIrcNickname);
            outputStream.WriteLine("USER {0} 8 * : {0}", configuration.TwitchIrcNickname);
            outputStream.Flush();
        }

        public void JoinRoom()
        {
            SendIrcMessage(string.Format("JOIN #{0}", configuration.TwitchIrcNickname));
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
                string.Format(configuration.TwitchIrcPrivmsgFormat, configuration.TwitchIrcNickname, message));
        }

        public string ReadRawMessage()
        {
            return inputStream.ReadLine();
        }

        public void LeaveRoom()
        {
            SendIrcMessage(string.Format("PART #{0}", configuration.TwitchIrcNickname));
        }

        public void RequestMembershipCapability()
        {
            SendIrcMessage(string.Format("CAP REQ :{0}", configuration.TwitchIrcMembershipCapability));
        }
    }
}
