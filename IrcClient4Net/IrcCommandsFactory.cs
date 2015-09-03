namespace SexyFishHorse.Irc.Client
{
    using System;
    using SexyFishHorse.Irc.Client.Models.UserMode;

    public static class IrcCommandsFactory
    {
        public static string Pass(string password)
        {
            return string.Format("PASS {0}", password);
        }

        public static string Nick(string nickname)
        {
            return string.Format("NICK {0}", nickname);
        }

        public static string User(string username, string realname)
        {
            return string.Format("USER {0} 8 * :{1}", username, realname);
        }

        public static string Join(string channel)
        {
            return string.Format("JOIN #{0}", channel);
        }

        public static string PrivMsg(string username, string host, string channel, string message)
        {
            return string.Format(":{0}!{1} PRIVMSG #{2} :{3}", username, host, channel, message);
        }

        public static string Part(string channel)
        {
            return string.Format("PART #{0}", channel);
        }

        public static string CapReq(string capability)
        {
            return string.Format("CAP REQ :{0}", capability);
        }

        public static string Quit(string message = null)
        {
            return string.IsNullOrWhiteSpace(message) ? "QUIT" : string.Format("QUIT :{0}", message);
        }

        public static string Oper(string username, string password)
        {
            return string.Format("OPER {0} {1}", username, password);
        }

        public static string Mode(string nickname, UserModeOperation operation, UserMode userMode)
        {
            var userModeAsString = UserModeToString(userMode);

            return string.Format(
                "MODE {0} {1}{2}",
                nickname,
                operation == UserModeOperation.Optain ? "+" : "-",
                userModeAsString);
        }

        public static string ServerMessage(
            string nickname,
            string reserved1,
            string distribution,
            string type,
            string reserved2,
            string info)
        {
            throw new NotImplementedException("I have no idea how to implement this method. https://tools.ietf.org/html/rfc2812#section-3.1.6 is not very clear!");
        }

        public static string UserModeToString(UserMode userMode)
        {
            switch (userMode)
            {
                case UserMode.Away:
                    return "a";
                case UserMode.Invisible:
                    return "i";
                case UserMode.ReceiveWallops:
                    return "w";
                case UserMode.RestrictedConnection:
                    return "r";
                case UserMode.Operator:
                    return "o";
                case UserMode.LocalOperator:
                    return "O";
                case UserMode.ReceiveServerNotices:
                    return "s";
                default:
                    throw new ArgumentException(
                        string.Format("No mapping defined for user mode {0}", userMode),
                        "userMode");
            }
        }
    }
}
