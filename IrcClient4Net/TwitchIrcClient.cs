namespace SexyFishHorse.Irc.Client
{
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

            outputStream.WriteLine("PASS " + configuration.TwitchIrcPassword);
            outputStream.WriteLine("NICK " + configuration.TwitchIrcNickname);
            outputStream.WriteLine("USER " + configuration.TwitchIrcNickname + " 8 * : " + configuration.TwitchIrcNickname);
            outputStream.Flush();
        }
    }
}
