namespace SexyFishHorse.Irc.Client.UnitTests.Parsers
{
    using SexyFishHorse.Irc.Client.Parsers;
    using Should;
    using Xunit;

    public class MessageParserTest
    {
        [Theory]
        [InlineData(":irc.example.com 001 MyNickname :Welcome to the Internet Relay Network MyNickname!~MyUsername@client.example.com", true, true, 1, "irc.example.com", "001", "Welcome to the Internet Relay Network MyNickname!~MyUsername@client.example.com")]
        [InlineData(":irc.example.com 002 MyNickname :Your host is irc.example.com, running version 2.10.3p5", true, true, 1, "irc.example.com", "002", "Your host is irc.example.com, running version 2.10.3p5")]
        [InlineData(":irc.example.com 003 MyNickname :This server was created Mon Oct 13 2003 at 15:56:53 EEST", true, true, 1, "irc.example.com", "003", "This server was created Mon Oct 13 2003 at 15:56:53 EEST")]
        [InlineData(":irc.example.com 004 MyNickname irc.example.com 2.10.3p5 aoOirw abeiIklmnoOpqrstv", true, false, 5, "irc.example.com", "004", null)]
        [InlineData(":irc.example.com 251 MyNickname :There are 123375 users and 7 services on 48 servers", true, true, 1, "irc.example.com", "251", "There are 123375 users and 7 services on 48 servers")]
        [InlineData(":irc.example.com 252 MyNickname 204 :operators online", true, true, 2, "irc.example.com", "252", "operators online")]
        [InlineData(":irc.example.com 253 MyNickname 6 :unknown connections", true, true, 2, "irc.example.com", "253", "unknown connections")]
        [InlineData(":irc.example.com 254 MyNickname 55926 :channels formed", true, true, 2, "irc.example.com", "254", "channels formed")]
        [InlineData(":irc.example.com 255 MyNickname :I have 3981 users, 0 services and 1 servers", true, true, 1, "irc.example.com", "255", "I have 3981 users, 0 services and 1 servers")]
        [InlineData(":MyNickname MODE MyNickname :+i", true, true, 1, "MyNickname", "MODE", "+i")]
        [InlineData(":irc.example.com 353 MyNickname @ #twilight_zone :MyNickname @thor-work @Diazemuls @thor @thor-away @Actilyse", true, true, 3, "irc.example.com", "353", "MyNickname @thor-work @Diazemuls @thor @thor-away @Actilyse")]
        [InlineData(":irc.example.com 366 MyNickname #twilight_zone :End of NAMES list.", true, true, 2, "irc.example.com", "366", "End of NAMES list.")]
        public void ParseMessage_ValidRawMessages_ReturnsValidIrcMessageObjects(string rawMessage, bool prefixed, bool trailed, int numberOfParameters, string prefix, string command, string trail)
        {
            var instance = new MessageParser();

            var message = instance.ParseMessage(rawMessage);

            message.Raw.ShouldEqual(rawMessage);
            message.Command.ShouldEqual(command);
            message.Prefix.ShouldEqual(prefix);
            message.Trail.ShouldEqual(trail);

            message.Parameters.Length.ShouldEqual(numberOfParameters);

            if (prefixed)
            {
                message.Prefixed.ShouldBeTrue();
            }
            else
            {
                message.Prefixed.ShouldBeFalse();
            }

            if (trailed)
            {
                message.Trailed.ShouldBeTrue();
            }
            else
            {
                message.Trailed.ShouldBeFalse();
            }
        }
    }
}
