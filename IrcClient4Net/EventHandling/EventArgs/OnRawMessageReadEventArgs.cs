namespace SexyFishHorse.Irc.Client.EventHandling.EventArgs
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
