namespace SexyFishHorse.Irc.Client.UnitTests.CommandFactories
{
    using System.Collections.Generic;
    using SexyFishHorse.Irc.Client.CommandFactories;
    using SexyFishHorse.Irc.Client.Models.Modes;
    using Should;
    using Xunit;

    public class ChannelOperationsCommandFactoryTest
    {
        [Theory]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3" }, new[] { "key1", "key2", "key3" }, false, "JOIN #channel3,#channel2,#channel1 key3,key2,key1")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3" }, new[] { "key1", "key2", "key3" }, true, "JOIN #channel3,#channel2,#channel1 key3,key2,key1 0")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3" }, new[] { "key1", "key2", "" }, false, "JOIN #channel2,#channel1,#channel3 key2,key1")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3" }, new[] { "key1", "key2", "" }, true, "JOIN #channel2,#channel1,#channel3 key2,key1 0")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3" }, new[] { "key1", "", "key3" }, false, "JOIN #channel3,#channel1,#channel2 key3,key1")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3" }, new[] { "key1", "", "key3" }, true, "JOIN #channel3,#channel1,#channel2 key3,key1 0")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3", "#channel4" }, new[] { "key1", "", "key3", "" }, false, "JOIN #channel3,#channel1,#channel2,#channel4 key3,key1")]
        [InlineData(new[] { "#channel1", "#channel2", "#channel3", "#channel4" }, new[] { "key1", "", "key3", "" }, true, "JOIN #channel3,#channel1,#channel2,#channel4 key3,key1 0")]
        public void Join_MultipleChannelsAndKeys_ShouldReturnProperlyFormatedCommandString(string[] channels, string[] keys, bool leaveExisting, string expected)
        {
            var list = new List<KeyValuePair<string, string>>();
            for (var i = 0; i < channels.Length; i++)
            {
                var channel = channels[i];
                var key = keys[i];

                list.Add(new KeyValuePair<string, string>(channel, key));
            }

            ChannelOperationsCommandFactory.Join(list, leaveExisting).ShouldEqual(expected);
        }

        [Theory]
        [InlineData("#channel", ModeOperation.Optain, new[] { ChannelMode.InviteOnly, ChannelMode.Moderated, ChannelMode.InvitationMask }, "*!*@*.fi", "MODE #channel +imI *!*@*.fi")]
        [InlineData("#channel", ModeOperation.Optain, new[] { ChannelMode.Operator }, "Kilroy", "MODE #channel +o Kilroy")]
        [InlineData("#channel", ModeOperation.Optain, new[] { ChannelMode.VoicePrivilege }, "Wiz", "MODE #channel +v Wiz")]
        [InlineData("#channel", ModeOperation.Remove, new[] { ChannelMode.Secret }, null, "MODE #channel -s")]
        [InlineData("#channel", ModeOperation.Optain, new[] { ChannelMode.Key }, "oulu", "MODE #channel +k oulu")]
        [InlineData("#channel", ModeOperation.Remove, new[] { ChannelMode.Key }, "oulu", "MODE #channel -k oulu")]
        public void Mode_MultipleModesWithParams_ShouldReturnProperlyFormatedCommandString(
            string channel,
            ModeOperation operation,
            ChannelMode[] modes,
            string modeParameters, string expected)
        {
            ChannelOperationsCommandFactory.Mode(channel, operation, modes, modeParameters).ShouldEqual(expected);
        }
    }
}
