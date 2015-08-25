namespace SexyFishHorse.Irc.Client.Parsers
{
    using SexyFishHorse.Irc.Client.Models;

    public interface IIrcMessageParser
    {
        IrcMessage ParseMessage(string rawMessage);
    }
}
