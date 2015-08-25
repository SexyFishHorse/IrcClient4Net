﻿namespace SexyFishHorse.Irc.Client.Clients
{
    using System.IO;
    using System.Net.Sockets;
    using SexyFishHorse.Irc.Client.Models;
    using SexyFishHorse.Irc.Client.Parsers;

    public class IrcClient : IIrcClient
    {
        private readonly IIrcMessageParser parser;

        private TcpClient client;

        private StreamReader inputStream;

        private StreamWriter outputStream;

        public IrcClient(IIrcMessageParser parser)
        {
            this.parser = parser;
        }

        public void Connect(string serverName, int portNumber, string username, string nickname, string realname, string password)
        {
            client = new TcpClient(serverName, portNumber);
            inputStream = new StreamReader(client.GetStream());
            outputStream = new StreamWriter(client.GetStream());

            outputStream.WriteLine(IrcCommandsFactory.Pass(password));
            outputStream.WriteLine(IrcCommandsFactory.Nick(nickname));
            outputStream.WriteLine(IrcCommandsFactory.User(username, realname));
            outputStream.Flush();
        }

        public void SendRawMessage(string message)
        {
            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        public string ReadRawMessage()
        {
            return inputStream.ReadLine();
        }

        public IrcMessage ReadIrcMessage()
        {
            return parser.ParseMessage(ReadRawMessage());
        }
    }
}
