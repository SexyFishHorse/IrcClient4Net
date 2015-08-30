namespace SexyFishHorse.Irc.Client.Models.EventArgs
{
    using System;

    public class OnRawMessageReadEventArgs : EventArgs
    {
        public OnRawMessageReadEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
