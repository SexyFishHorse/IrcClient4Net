namespace SexyFishHorse.Irc.Client.Validators
{
    using SexyFishHorse.Irc.Client.Models;

    public interface IResponseValidator
    {
        /// <summary>
        /// Throws an ApplicationException if the message isn't of the expected command
        /// </summary>
        /// <param name="message">The message to validate</param>
        /// <param name="expectedCommand">The command the message is expected to have</param>
        /// <returns>The <see cref="IrcMessage"/>.</returns>
        IrcMessage ValidateCommand(IrcMessage message, string expectedCommand);
    }
}
