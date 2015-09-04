namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.Net.Sockets;
    using Models;
    using Parsers;
    using Validators;

    public class IrcClient : IIrcClient
    {
        private readonly IMessageParser parser;

        private readonly IResponseValidator responseValidator;

        private ISocket socket;

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

            socket = new Models.Socket(new TcpClient(serverName, portNumber));

            socket.WriteLine(IrcCommandsFactory.Pass(password));
            socket.WriteLine(IrcCommandsFactory.Nick(nickname));
            socket.WriteLine(IrcCommandsFactory.User(username, realname));
            socket.Flush();

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

            socket.WriteLineAndFlush(message);
        }

        public virtual string ReadRawMessage()
        {
            if (!connecting && !IsConnected)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            var message = socket.ReadLine();

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
            socket.Dispose();

            IsConnected = false;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
