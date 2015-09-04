namespace SexyFishHorse.Irc.Client.Models
{
    using System.IO;
    using System.Net.Sockets;

    public class Socket : ISocket
    {
        private readonly TcpClient client;

        private readonly StreamReader inputStream;

        private readonly StreamWriter outputStream;

        public Socket(TcpClient client)
        {
            this.client = client;
            inputStream = new StreamReader(client.GetStream());
            outputStream = new StreamWriter(client.GetStream());
        }

        public void WriteLine(string line)
        {
            outputStream.WriteLine(line);
        }

        public void Flush()
        {
            outputStream.Flush();
        }

        public void WriteLineAndFlush(string line)
        {
            WriteLine(line);
            Flush();
        }

        public string ReadLine()
        {
            return inputStream.ReadLine();
        }

        public void Dispose()
        {
            inputStream.Dispose();
            outputStream.Dispose();
            client.Close();
        }
    }
}
