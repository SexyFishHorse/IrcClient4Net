namespace SexyFishHorse.Irc.Client.Models.EventArgs
{
    using System;

    public class OnMessageSentEventArgs : EventArgs
    {
        public OnMessageSentEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
