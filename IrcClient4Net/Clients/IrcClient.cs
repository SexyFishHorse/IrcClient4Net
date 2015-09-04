namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using System.Net.Sockets;
    using Models;
    using Parsers;
    using EventHandling.EventArgs;
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

        public event Action<OnConnectedEventArgs> Connected;

        public event Action<OnConnectionFailedEventArgs> ConnectionFailed;

        public event Action<OnMessageSentEventArgs> MessageSent;

        public event Action<OnDisconnectedEventArgs> Disconnected;

        public event Action<OnRawMessageReadEventArgs> RawMessageRead;

        public event Action<OnIrcMessageReadEventArgs> IrcMessageRead;

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

            connectionSocket.WriteLine(IrcCommandsFactory.Pass(password));
            connectionSocket.WriteLine(IrcCommandsFactory.Nick(nickname));
            connectionSocket.WriteLine(IrcCommandsFactory.User(username, realname));
            connectionSocket.Flush();

            connecting = true;
            try
            {
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.Welcome);
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.YourHost);
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.Created);
                responseValidator.ValidateCommand(ReadIrcMessage(), Rfc2812CommandResponse.MyInfo);

                if (Connected != null)
                {
                    Connected(new OnConnectedEventArgs());
                }
            }
            catch (ResponseValidationException)
            {
                if (ConnectionFailed != null)
                {
                    ConnectionFailed(new OnConnectionFailedEventArgs());
                }

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

            if (MessageSent != null)
            {
                MessageSent(new OnMessageSentEventArgs(message));
            }
        }

        public virtual string ReadRawMessage()
        {
            if (!connecting && !IsConnected)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            var message = socket.ReadLine();

            if (RawMessageRead != null)
            {
                RawMessageRead(new OnRawMessageReadEventArgs(message));
            }

            return message;
        }

        public virtual IrcMessage ReadIrcMessage()
        {
            var message = parser.ParseMessage(ReadRawMessage());

            if (IrcMessageRead != null)
            {
                IrcMessageRead(new OnIrcMessageReadEventArgs(message));
            }

            return message;
        }

        public virtual void Disconnect(string message = null)
        {
            SendRawMessage(IrcCommandsFactory.Quit(message));
            socket.Dispose();

            IsConnected = false;

            if (Disconnected != null)
            {
                Disconnected(new OnDisconnectedEventArgs());
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
