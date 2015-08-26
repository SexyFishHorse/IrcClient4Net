namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using SexyFishHorse.Irc.Client.Models;
    using SexyFishHorse.Irc.Client.Parsers;
    using SexyFishHorse.Irc.Client.Validators;

    public class IrcClient : IIrcClient
    {
        private readonly IMessageParser parser;

        private readonly IResponseValidator responseValidator;

        private TcpClient client;

        private StreamReader inputStream;

        private StreamWriter outputStream;

        private bool connecting;

        public IrcClient(IMessageParser parser, IResponseValidator responseValidator)
        {
            this.parser = parser;
            this.responseValidator = responseValidator;
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
            try
            {
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.Welcome);
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.YourHost);
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.Created);
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.MyInfo);
            }
            catch (ResponseValidationException ex)
            {
                throw new ApplicationException("Unable to establish a connection to the server");
            }

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

        public void Dispose()
        {
            Disconnect();
        }
    }
}
