namespace SexyFishHorse.Irc.Client.Validators
{
    using System;
    using SexyFishHorse.Irc.Client.Models;

    public class ResponseValidator : IResponseValidator
    {
        public void ValidateCommand(IrcMessage message, string expectedCommand)
        {
            if (string.IsNullOrWhiteSpace(expectedCommand))
            {
                throw new ArgumentException("Expected command is not set", "expectedCommand");
            }

            if (message == null)
            {
                throw new ResponseValidationException("Did not receive a response from the server.");
            }

            if (message.Command == expectedCommand)
            {
                return;
            }

            var errorMessage =
                string.Format(
                    "Expected the message from the server to have command code \"{0}\", received \"{1}\" instead",
                    expectedCommand,
                    message.Command);
            throw new ResponseValidationException(errorMessage, expectedCommand, message);
        }
    }
}
