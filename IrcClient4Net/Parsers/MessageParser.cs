namespace SexyFishHorse.Irc.Client.Parsers
{
    using System;
    using SexyFishHorse.Irc.Client.Models;

    public class MessageParser : IMessageParser
    {
        public IrcMessage ParseMessage(string rawMessage)
        {
            if (string.IsNullOrWhiteSpace(rawMessage))
            {
                return null;
            }

            var prefix = ExtractPrefix(rawMessage);
            var trail = ExtractTrail(rawMessage);
            var command = ExtractCommand(rawMessage, prefix);
            var parameters = ExtractParameters(rawMessage, command, trail);

            return new IrcMessage
            {
                Raw = rawMessage,
                Prefixed = !string.IsNullOrWhiteSpace(prefix),
                Prefix = prefix,
                Command = command,
                Trailed = !string.IsNullOrWhiteSpace(trail),
                Trail = trail,
                Parameters = parameters,
            };
        }

        /// <summary>
        /// Extracts the parameters from the raw message
        /// </summary>
        /// <param name="rawMessage">The raw message</param>
        /// <param name="command">The command in the raw message</param>
        /// <param name="trail">The trail (if any) in the message</param>
        /// <returns>The parameters from the raw message</returns>
        private static string[] ExtractParameters(string rawMessage, string command, string trail)
        {
            var rest = rawMessage.Substring(rawMessage.IndexOf(command, StringComparison.Ordinal) + command.Length + 1);
            if (!string.IsNullOrWhiteSpace(trail))
            {
                rest = rest.Substring(0, rest.LastIndexOf(trail, StringComparison.Ordinal) - 2);
            }

            return rest.Split(' ');
        }

        /// <summary>
        /// Extracts the trail from the raw message
        /// </summary>
        /// <param name="rawMessage">The raw message</param>
        /// <returns>The trail (if any) without the semicolon prefixed, or null</returns>
        private static string ExtractTrail(string rawMessage)
        {
            string trail = null;
            if (rawMessage.Contains(" :"))
            {
                trail = rawMessage.Substring(rawMessage.IndexOf(" :", StringComparison.Ordinal) + 2);
            }

            return trail;
        }

        /// <summary>
        /// Extracts the command from the raw message
        /// </summary>
        /// <param name="rawMessage">The raw message</param>
        /// <param name="prefix">The prefix of the raw message (if any)</param>
        /// <returns>The command</returns>
        private static string ExtractCommand(string rawMessage, string prefix)
        {
            var rest = rawMessage;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                rest = rawMessage.Substring(prefix.Length + 2);
            }

            var command = rest.Substring(0, rest.IndexOf(" ", StringComparison.Ordinal));

            return command;
        }

        /// <summary>
        /// Extracts the prefix from a message (if any)
        /// </summary>
        /// <param name="rawMessage">The entire raw message</param>
        /// <returns>The prefix (if any) without the semicolon prefixed, or null</returns>
        private static string ExtractPrefix(string rawMessage)
        {
            string prefix = null;
            if (rawMessage.StartsWith(":"))
            {
                prefix = rawMessage.Substring(1, rawMessage.IndexOf(" ", StringComparison.Ordinal) - 1);
            }

            return prefix;
        }
    }
}
