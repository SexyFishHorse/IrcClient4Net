namespace SexyFishHorse.Irc.Client.Models
{
    using System;

    public interface ISocket : IDisposable
    {
        void WriteLine(string line);

        string ReadLine();

        void Flush();

        void WriteLineAndFlush(string message);
    }
}
