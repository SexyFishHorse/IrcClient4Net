namespace SexyFishHorse.Irc.Client.EventHandlers
{
    using SexyFishHorse.Irc.Client.Models.EventArgs;

    public delegate void OnMessageSentEventHandler(object sender, OnMessageSentEventArgs args);
}
