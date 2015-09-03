namespace SexyFishHorse.Irc.Client
{
    using System;
    using System.IO;
    using System.Threading;
    using Clients;
    using Configuration;
    using Models;
    using Ninject;
    using CommandFactories;

    public class Program
    {
        private readonly IIrcClient clientWith;

        private readonly IConfiguration configuration;

        public Program(IrcClientWithEventHandling clientWith, IConfiguration configuration)
        {
            this.clientWith = clientWith;
            this.configuration = configuration;
        }

        public bool Running { get; set; }

        public bool Ready { get; set; }

        public static void Main(string[] args)
        {
            var kernel = new StandardKernel(new IrcClientModule());

            var program = kernel.Get<Program>();
            program.Run();
        }

        private void Run()
        {
            Running = true;
            clientWith.Connect(configuration.TwitchIrcServerName, configuration.TwitchIrcPortNumber, configuration.TwitchIrcNickname, configuration.TwitchIrcNickname, configuration.TwitchIrcNickname, configuration.TwitchIrcTmiToken);

            var thread = new Thread(ReadChatMessages);
            thread.Start();

            clientWith.SendRawMessage(CommandsFactory.CapReq(configuration.TwitchIrcMembershipCapability));
            clientWith.SendRawMessage(CommandsFactory.Join(configuration.TwitchIrcNickname));

            while (!Ready)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("<SYSTEM> You are now ready to chat");

            while (Running)
            {
                var line = Console.ReadLine();

                if (line == "exit")
                {
                    Running = false;
                    clientWith.Dispose();
                }
                else
                {
                    clientWith.SendRawMessage(
                        CommandsFactory.PrivMsg(
                            configuration.TwitchIrcNickname,
                            string.Format("{0}@tmi.twitch.tv", configuration.TwitchIrcNickname),
                            configuration.TwitchIrcNickname,
                            line));
                }
            }
        }

        private void ReadChatMessages()
        {
            var channelName = string.Format("#{0}", configuration.TwitchIrcNickname);

            while (Running)
            {
                IrcMessage ircMessage;

                try
                {
                    ircMessage = clientWith.ReadIrcMessage();
                }
                catch (IOException)
                {
                    continue;
                }

                if (ircMessage == null)
                {
                    continue;
                }

                var rawMessage = ircMessage.Raw;

                if (rawMessage.Contains(string.Format(" PRIVMSG {0}", channelName)))
                {
                    var username = rawMessage.Substring(1, rawMessage.IndexOf('!') - 1);

                    var privMsgEnd = string.Format("PRIVMSG {0} :", channelName);
                    var message = rawMessage.Substring(rawMessage.IndexOf(privMsgEnd, StringComparison.Ordinal) + privMsgEnd.Length);

                    Console.WriteLine("<{0}> {1}", username, message.Trim());
                }
                else if (rawMessage.Contains(string.Format(" JOIN {0}", channelName)))
                {
                    var username = rawMessage.Substring(1, rawMessage.IndexOf('!') - 1);
                    Console.WriteLine("<SYSTEM> {0} JOINED!", username);
                }
                else if (rawMessage.Contains(string.Format(" MODE {0}", channelName)))
                {
                    var messageParts = rawMessage.Split(' ');
                    var username = messageParts[messageParts.Length - 1];

                    if (rawMessage.Contains("+o"))
                    {
                        Console.WriteLine("<SYSTEM> {0} OBTAINED OPERATOR", username);
                    }
                    else if (rawMessage.Contains("-o"))
                    {
                        Console.WriteLine("<SYSTEM> {0} LOST OPERATOR", username);
                    }
                    else
                    {
                        Console.WriteLine("<SYSTEM> Unknown mode for {0}, here's the raw message:\n{1}", username, rawMessage);
                    }
                }
                else
                {
                    Console.WriteLine("<RAW MESSAGE> {0}", rawMessage);

                    if (rawMessage.Contains("End of /NAMES list"))
                    {
                        Ready = true;
                    }
                }
            }
        }
    }
}
