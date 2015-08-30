namespace SexyFishHorse.Irc.Client.EventHandling.EventHandlers
{
    using EventArgs;

    public delegate void OnIrcMessageReadEventHandler(object sender, OnIrcMessageReadEventArgs args);
}
