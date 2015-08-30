namespace SexyFishHorse.Irc.Client.EventHandling.EventArgs
{
    using System;
    using SexyFishHorse.Irc.Client.Models;

    public class OnIrcMessageReadEventArgs : EventArgs
    {
        public OnIrcMessageReadEventArgs(IrcMessage message)
        {
            Message = message;
        }

        public IrcMessage Message { get; set; }
    }
}
