using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncServerClient
{
    public class Client
    {
        TcpClient tcpClient;
        public string UserName { get; set; }
    }

    class Program
    {
        private static readonly object _lock = new object();
        private static readonly List<TcpClient> clients = new List<TcpClient>();

        public static TcpClient[] GetClients()
        {
            return clients.ToArray();
        }

        public static int GetClientCount()
        {
            return clients.Count;
        }


        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener ServerSocket = new TcpListener(ip, 14000);
            ServerSocket.Start();

            Console.WriteLine("Server start");
            while (true)
            {
                TcpClient clientSocket = ServerSocket.AcceptTcpClient();
                Console.WriteLine($"client ip: {clientSocket.Client.RemoteEndPoint}");
                clients.Add(clientSocket);
                handleClient client = new handleClient();
                client.startClient(clientSocket);

                Console.WriteLine($"{GetClientCount()} clients connected");
            }
        }

        public class handleClient
        {
            TcpClient clientSocket;

            public void startClient(TcpClient inClientSocket)
            {
                clientSocket = inClientSocket;
                Thread ctThread = new Thread(Chat);
                ctThread.Start();
            }

            private void Chat()
            {
                BinaryReader reader = new BinaryReader(clientSocket.GetStream());
                while (true)
                {
                    string clientName = reader.ReadString();
                    Console.WriteLine(clientName);

                    int number;
                    bool isParsable = Int32.TryParse(clientName, out number);

                 

                    if (isParsable)
                    {
                        int clientId = Convert.ToInt32(clientName);
                        //if (GetClients()[clientId] != null)
                        //{

                        //}

                       var client = GetClients()[clientId];
                        while (true)
                        {
                            string message = reader.ReadString();
                            if (message == "//")
                            {
                                break;
                            }
                            if (message != null && message != "//")
                            {

                           
                                BinaryWriter writer = new BinaryWriter(client.GetStream());
                                writer.Write(message);
                            }
                         
                         
                        }
                    }

                    if (clientName == "*")
                    {
                        
                           
                                while (true)
                                {
                                    string message = reader.ReadString();
                                    if (message == "//")
                                    {
                                        break;
                                    }
                                    if (message != null && message != "//")
                                    {
                                foreach (var client in GetClients())
                                {

                                    BinaryWriter writer = new BinaryWriter(client.GetStream());
                                    writer.Write(message);
                                }
                                    }


                                }
                            
                           
                        
                    }
                
                }                       
                }
            }
        }
    }


