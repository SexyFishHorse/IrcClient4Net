namespace SexyFishHorse.Irc.Client.Parsers
{
    using SexyFishHorse.Irc.Client.Models;

    public interface IMessageParser
    {
        IrcMessage ParseMessage(string rawMessage);
    }
}
