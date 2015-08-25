namespace SexyFishHorse.Irc.Client.Models
{
    public class IrcMessage
    {
        public bool Prefixed { get; set; }

        public string Prefix { get; set; }

        public string Raw { get; set; }

        public string Command { get; set; }

        public bool Trailed { get; set; }

        public string Trail { get; set; }

        public string[] Parameters { get; set; }
    }
}
