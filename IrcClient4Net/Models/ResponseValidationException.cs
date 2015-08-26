namespace SexyFishHorse.Irc.Client.Models
{
    using System;

    public class ResponseValidationException : ApplicationException
    {
        public ResponseValidationException(string message)
            : base(message)
        {
        }

        public ResponseValidationException(
            string message,
            string expectedCommand,
            IrcMessage ircMessage)
            : base(message)
        {
            ExpectedCommand = expectedCommand;
            IrcMessage = ircMessage;
        }

        public string ExpectedCommand { get; private set; }

        public IrcMessage IrcMessage { get; private set; }
    }
}
