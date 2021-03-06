﻿namespace SexyFishHorse.Irc.Client.Configuration
{
    public interface IConfiguration
    {
        string TwitchIrcServerName { get; }

        int TwitchIrcPortNumber { get; }

        string TwitchIrcNickname { get; }

        string TwitchIrcTmiToken { get; }

        string TwitchIrcMembershipCapability { get; }
    }
}
