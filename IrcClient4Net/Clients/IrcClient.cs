namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using Models;
    using Parsers;
    using Validators;

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

        public bool IsConnected { get; private set; }

        public virtual void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password)
        {
            connecting = false;
            IsConnected = false;

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
            catch (ResponseValidationException)
            {
                throw new ApplicationException("Unable to establish a connection to the server");
            }

            connecting = false;

            IsConnected = true;
        }

        public virtual void SendRawMessage(string message)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        public virtual string ReadRawMessage()
        {
            if (!connecting && !IsConnected)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            var message = inputStream.ReadLine();

            return message;
        }

        public virtual IrcMessage ReadIrcMessage()
        {
            var message = parser.ParseMessage(ReadRawMessage());

            return message;
        }

        public virtual void Disconnect(string message = null)
        {
            SendRawMessage(IrcCommandsFactory.Quit(message));
            inputStream.Dispose();
            outputStream.Dispose();
            client.Close();

            IsConnected = false;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
