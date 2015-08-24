namespace SexyFishHorse.Irc.Client
{
    using Ninject;
    using SexyFishHorse.Irc.Client.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            var kernel = new StandardKernel(new IrcClientModule());

            var client = kernel.Get<IIrcClient>();

            client.Connect();
        }
    }
}
