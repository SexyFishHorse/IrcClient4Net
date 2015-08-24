namespace SexyFishHorse.Irc.Client
{
    using System;
    using System.Threading;
    using Ninject;
    using SexyFishHorse.Irc.Client.Configuration;

    public class Program
    {
        private readonly ITwitchIrcClient client;

        public Program(ITwitchIrcClient client)
        {
            this.client = client;
        }

        public bool Running { get; set; }

        public static void Main(string[] args)
        {
            var kernel = new StandardKernel(new IrcClientModule());

            var program = kernel.Get<Program>();
            program.Run();
        }

        private void Run()
        {
            Running = true;
            client.Connect();

            var thread = new Thread(ReadChatMessages);
            thread.Start();

            client.JoinRoom();
            client.SendChatMessage("Test");

            Console.WriteLine("TO CHAT: Enter your line and press enter");

            while (Running)
            {
                Console.Write(">>> ");
                client.SendChatMessage(Console.ReadLine());
            }
        }

        private void ReadChatMessages()
        {
            while (Running)
            {
                var rawMessage = client.ReadRawMessage();
                    Console.WriteLine("<RAW MESSAGE> {0}", rawMessage);
            }
        }
    }
}
