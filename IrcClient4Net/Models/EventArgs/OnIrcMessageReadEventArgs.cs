namespace SexyFishHorse.Irc.Client.Models.EventArgs
{
    using System;

    public class OnIrcMessageReadEventArgs : EventArgs
    {
        public OnIrcMessageReadEventArgs(IrcMessage message)
        {
            Message = message;
        }

        public IrcMessage Message { get; set; }
    }
}
