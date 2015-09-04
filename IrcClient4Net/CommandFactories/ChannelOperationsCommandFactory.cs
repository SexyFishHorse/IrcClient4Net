namespace SexyFishHorse.Irc.Client.CommandFactories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SexyFishHorse.Irc.Client.Models.Modes;

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

        public static string Mode(
            string channel,
            ModeOperation operation,
            ChannelMode[] modes,
            string modeParameters)
        {
            return
                string.Format(
                    "MODE {0} {1}{2} {3}",
                    channel,
                    operation == ModeOperation.Optain ? "+" : "-",
                    GetChannelModesAsString(modes),
                    modeParameters).Trim();
        }

        public static string Topic(string channel, string topic = null)
        {
            topic = topic == null ? string.Empty : string.Format(" :{0}", topic);

            return string.Format("TOPIC {0}{1}", channel, topic);
        }

        public static string Names(string[] channels = null, string target = null)
        {
            if (channels == null || channels.Length < 1)
            {
                return "NAMES";
            }

            target = target == null ? string.Empty : string.Format(" :{0}", target);

            return string.Format("NAMES {0}{1}", string.Join(",", channels), target);
        }

        public static string List(string[] channels = null, string target = null)
        {
            if (channels == null || channels.Length < 1)
            {
                return "LIST";
            }

            target = target == null ? string.Empty : string.Format(" :{0}", target);

            return string.Format("LIST {0}{1}", string.Join(",", channels), target);
        }

        public static string Invite(string nickname, string channel)
        {
            return string.Format("{0} {1}", nickname, channel);
        }

        public static string Kick(string[] channels, string[] users, string comment = null)
        {
            comment = comment == null ? string.Empty : string.Format(" :{0}", comment);

            return string.Format("KICK {0} {1}{2}", string.Join(",", channels), string.Join(",", users), comment);
        }

        private static string GetChannelModesAsString(IEnumerable<ChannelMode> modes)
        {
            var modesAsString = new StringBuilder();

            foreach (var mode in modes)
            {
                modesAsString.Append(GetChannelModeAsString(mode));
            }

            return modesAsString.ToString();
        }

        private static char GetChannelModeAsString(ChannelMode mode)
        {
            switch (mode)
            {
                case ChannelMode.Creator:
                    return 'O';
                case ChannelMode.Operator:
                    return 'o';
                case ChannelMode.VoicePrivilege:
                    return 'v';
                case ChannelMode.Anonymous:
                    return 'a';
                case ChannelMode.InviteOnly:
                    return 'i';
                case ChannelMode.Moderated:
                    return 'm';
                case ChannelMode.NoOutsideMessages:
                    return 'n';
                case ChannelMode.Quiet:
                    return 'q';
                case ChannelMode.Private:
                    return 'p';
                case ChannelMode.Secret:
                    return 's';
                case ChannelMode.Reop:
                    return 'r';
                case ChannelMode.OperatorOnlyTopic:
                    return 't';
                case ChannelMode.Key:
                    return 'k';
                case ChannelMode.Limit:
                    return 'l';
                case ChannelMode.BanMask:
                    return 'b';
                case ChannelMode.ExceptionMask:
                    return 'e';
                case ChannelMode.InvitationMask:
                    return 'I';
                default:
                    throw new ArgumentException(
                        string.Format("Mode to string mapping not defined for mode {0}", mode),
                        "mode");
            }
        }
    }
}
