using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class TCPClient
    {
        static IPAddress ServerIP { get; set; }
        static int ClientPort;

        static void Main(string[] args)
        {
            ServerIP = IPAddress.Parse("127.0.0.1");
            ClientPort = 11000;

            TcpClient Client = new TcpClient();
            try
            {
                Client.Connect(ServerIP, ClientPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (Client.Connected)
            {
                Console.WriteLine("Conectat al servidor");
                NetworkStream stream = Client.GetStream();
                String line;
                do
                {
                    line = Console.ReadLine();
                    if (line != "" && line != "q")
                    {
                        byte[] message = Encoding.UTF8.GetBytes(line);
                        stream.Write(message, 0, message.Length);
                        byte[] BufferLocal = new byte[256];
                        int BytesRebuts = stream.Read(BufferLocal, 0, BufferLocal.Length);
                        string missatgeEnviat = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);
                        Console.WriteLine(missatgeEnviat);
                    }
                } while (line != "" && line != "q");
                Console.WriteLine("Desconectant del servidor...");
                stream.Close();
                Client.Close();
            }
            else Console.WriteLine("Conexio fallida");
        }
    }
}
