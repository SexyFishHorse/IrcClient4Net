namespace SexyFishHorse.Irc.Client.CommandFactories
{
    public static class CommandsFactory
    {
        public static string CapReq(string capability)
        {
            return string.Format("CAP REQ :{0}", capability);
        }
    }
}
