namespace SexyFishHorse.Irc.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new IrcClient();

            client.Connect();
        }
    }
}
