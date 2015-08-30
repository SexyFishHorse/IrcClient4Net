namespace SexyFishHorse.Irc.Client.EventHandlers
{
    using SexyFishHorse.Irc.Client.Models.EventArgs;

    public delegate void OnIrcMessageReadEventHandler(object sender, OnIrcMessageReadEventArgs args);
}
