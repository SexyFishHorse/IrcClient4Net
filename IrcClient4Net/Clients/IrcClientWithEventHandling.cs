namespace SexyFishHorse.Irc.Client.Clients
{
    using System;
    using EventHandling.EventArgs;
    using Models;
    using Parsers;
    using Validators;

    public class IrcClientWithEventHandling : IrcClient
    {
        public IrcClientWithEventHandling(IMessageParser messageParser, IResponseValidator responseValidator)
            : base(messageParser, responseValidator)
        {
        }

        public event Action<OnConnectedEventArgs> Connected;

        public event Action<OnConnectionFailedEventArgs> ConnectionFailed;

        public event Action<OnMessageSentEventArgs> OnMessageSent;

        public event Action<OnDisconnectedEventArgs> OnDisconnected;

        public event Action<OnRawMessageReadEventArgs> OnRawMessageRead;

        public event Action<OnIrcMessageReadEventArgs> OnIrcMessageRead;

        public override void Connect(
            string serverName,
            int portNumber,
            string username,
            string nickname,
            string realname,
            string password)
        {
            try
            {
                base.Connect(serverName, portNumber, username, nickname, realname, password);

                if (Connected != null)
                {
                    Connected(new OnConnectedEventArgs());
                }
            }
            catch (ApplicationException)
            {
                if (ConnectionFailed != null)
                {
                    ConnectionFailed(new OnConnectionFailedEventArgs());
                }
            }
        }

        public override void SendRawMessage(string message)
        {
            base.SendRawMessage(message);

            if (OnMessageSent != null)
            {
                OnMessageSent(new OnMessageSentEventArgs(message));
            }
        }

        public override string ReadRawMessage()
        {
            var message = base.ReadRawMessage();

            if (OnRawMessageRead != null)
            {
                OnRawMessageRead(new OnRawMessageReadEventArgs(message));
            }

            return message;
        }

        public override IrcMessage ReadIrcMessage()
        {
            var message = base.ReadIrcMessage();

            if (OnIrcMessageRead != null)
            {
                OnIrcMessageRead(new OnIrcMessageReadEventArgs(message));
            }

            return message;
        }

        public override void Disconnect(string message = null)
        {
            base.Disconnect();

            if (OnDisconnected != null)
            {
                OnDisconnected(new OnDisconnectedEventArgs());
            }
        }
    }
}
