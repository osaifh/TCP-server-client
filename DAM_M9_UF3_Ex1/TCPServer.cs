using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAM_M9_UF3_Ex1
{
    class TCPServer
    {
        static IPAddress ServerIP { get; set; }
        static int ServerPort;
        static TcpListener server;

        static void Main(string[] args)
        {

            ServerIP = IPAddress.Parse("127.0.0.1");
            ServerPort = 11000;
            server = new TcpListener(ServerIP, ServerPort);
            server.Start();
            Thread thread = new Thread(() => ClientManager());
            thread.Start();
            Console.ReadKey();
        }

        static async Task ClientManager()
        {
            TcpClient CurrentClient;
            Console.WriteLine("Escoltant...");
            while (true)
            {
                CurrentClient = await server.AcceptTcpClientAsync();
                if (CurrentClient.Connected)
                {
                    Console.WriteLine("Client acceptat");
                    NetworkStream NSServer = CurrentClient.GetStream();
                    Thread thread = new Thread(() => ClientConnection(NSServer));
                    thread.Start();
                }
            }
        }
        
        static void ClientConnection(NetworkStream NSServer)
        {
            byte[] BufferLocal = new byte[256];
            Console.WriteLine("Escoltant client:");
            Boolean escoltant = true;
            while (escoltant)
            {
                try
                {
                    int BytesRebuts = NSServer.Read(BufferLocal, 0, BufferLocal.Length);
                    string missatgeEnviat = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);
                    escoltant = (missatgeEnviat != "q");
                    Console.WriteLine(missatgeEnviat);
                    byte[] message = Encoding.UTF8.GetBytes(ReverseString(missatgeEnviat));
                    NSServer.Write(message, 0, message.Length);
                }
                catch(Exception ex)
                {
                    break;
                }
            }
            Console.WriteLine("Client desconectat");
        }

        static string ReverseString(String Input)
        {
            char[] arr = Input.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }
}
