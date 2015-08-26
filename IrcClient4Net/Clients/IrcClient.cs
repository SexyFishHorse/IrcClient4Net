namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using SexyFishHorse.Irc.Client.Models;
    using SexyFishHorse.Irc.Client.Parsers;

    public class IrcClient : IIrcClient
    {
        private readonly IIrcMessageParser parser;

        private TcpClient client;

        private StreamReader inputStream;

        private StreamWriter outputStream;

        private bool connecting;

        public IrcClient(IIrcMessageParser parser)
        {
            this.parser = parser;
        }

        public bool Connected { get; private set; }

        public void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password)
        {
            connecting = false;
            Connected = false;

            client = new TcpClient(serverName, portNumber);
            inputStream = new StreamReader(client.GetStream());
            outputStream = new StreamWriter(client.GetStream());

            outputStream.WriteLine(IrcCommandsFactory.Pass(password));
            outputStream.WriteLine(IrcCommandsFactory.Nick(nickname));
            outputStream.WriteLine(IrcCommandsFactory.User(username, realname));
            outputStream.Flush();

            connecting = true;
            ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.Welcome);
            ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.YourHost);
            ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.Created);
            ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.MyInfo);
            connecting = false;

            Connected = true;
        }

        public void SendRawMessage(string message)
        {
            if (!Connected)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        public string ReadRawMessage()
        {
            if (!connecting && !Connected)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            return inputStream.ReadLine();
        }

        public IrcMessage ReadIrcMessage()
        {
            return parser.ParseMessage(ReadRawMessage());
        }

        public void Disconnect(string message = null)
        {
            SendRawMessage(IrcCommandsFactory.Quit(message));
            inputStream.Dispose();
            outputStream.Dispose();
            client.Close();

            Connected = false;
        }

        public void ValidateCommand(IrcMessage message, string expectedCommand)
        {
            if (message == null)
            {
                throw new ApplicationException("Did not receive a response from the server.");
            }

            if (message.Command != expectedCommand)
            {
                throw new ApplicationException(
                    string.Format(
                        "Expected the message from the server to have command code \"{0}\", received \"{1}\" instead",
                        expectedCommand,
                        message.Command));
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
