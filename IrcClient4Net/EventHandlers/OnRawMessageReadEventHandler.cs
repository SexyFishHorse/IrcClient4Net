namespace SexyFishHorse.Irc.Client.EventHandlers
{
    using SexyFishHorse.Irc.Client.Models.EventArgs;

    public delegate void OnRawMessageReadEventHandler(object sender, OnRawMessageReadEventArgs args);
}
