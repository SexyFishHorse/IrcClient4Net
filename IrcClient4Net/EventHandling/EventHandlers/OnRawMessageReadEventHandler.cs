namespace SexyFishHorse.Irc.Client.EventHandling.EventHandlers
{
    using EventArgs;

    public delegate void OnRawMessageReadEventHandler(object sender, OnRawMessageReadEventArgs args);
}
