namespace SexyFishHorse.Irc.Client.CommandFactories
{
    using System.Collections.Generic;
    using System.Linq;

    public class ChannelOperationsCommandFactory
    {
        public static string Join(string channel, string key, bool leaveExistingChannels)
        {
            return Join(new Dictionary<string, string> { { channel, key } }, leaveExistingChannels);
        }

        public static string Join(IEnumerable<KeyValuePair<string, string>> channelAndKeys, bool leaveExistingChannels)
        {
            var channelAndKeysList = channelAndKeys.OrderByDescending(x => x.Value).ToList();

            var channels = (from channelAndKey in channelAndKeysList
                            where !string.IsNullOrWhiteSpace(channelAndKey.Key)
                            select channelAndKey.Key).ToList();

            var keys = (from channelAndKey in channelAndKeysList
                        where !string.IsNullOrWhiteSpace(channelAndKey.Value)
                        select channelAndKey.Value).ToList();

            return
                string.Format(
                    "JOIN {0} {1}{2}",
                    string.Join(",", channels),
                    string.Join(",", keys),
                    leaveExistingChannels ? " 0" : string.Empty).Trim();
        }

        public static string Part(string channel, string message = null)
        {
            return Part(new[] { channel }, message);
        }

        public static string Part(string[] channels, string message = null)
        {
            return string.Format(
                "PART {0}{1}",
                string.Join(",", channels),
                string.IsNullOrWhiteSpace(message) ? null : string.Format(" :{0}", message));
        }
    }
}
