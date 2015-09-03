namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.Net.Sockets;
    using Models;
    using Parsers;
    using CommandFactories;
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

        public IrcClient(IMessageParser parser, IResponseValidator responseValidator, ISocket socket, bool isConnected, bool connecting)
        {
            this.parser = parser;
            this.responseValidator = responseValidator;
            this.socket = socket;
            IsConnected = isConnected;
            this.connecting = connecting;
        }

        public bool IsConnected { get; private set; }

        public virtual void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password)
        {
            Connect(new Models.Socket(new TcpClient(serverName, portNumber)), username, nickname, realname, password);
        }

        public void Connect(ISocket connectionSocket, string username, string nickname, string realname, string password)
        {
            connecting = false;
            IsConnected = false;

            socket = connectionSocket;

            connectionSocket.WriteLine(IrcConnectionRegistrationCommandFactory.Pass(password));
            connectionSocket.WriteLine(IrcConnectionRegistrationCommandFactory.Nick(nickname));
            connectionSocket.WriteLine(IrcConnectionRegistrationCommandFactory.User(username, realname));
            connectionSocket.Flush();

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
            SendRawMessage(IrcConnectionRegistrationCommandFactory.Quit(message));
            socket.Dispose();

            IsConnected = false;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
