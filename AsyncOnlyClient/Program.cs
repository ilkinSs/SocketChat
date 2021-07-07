using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncOnlyClient
{
    class Program
    {

        public static void Write(TcpClient client)
        {
           
                string str;
                SocketShutdown reason = SocketShutdown.Send;
            while ((str = Console.ReadLine()) != "")
                {
                   
                        BinaryWriter writer = new BinaryWriter(client.GetStream());
                        writer.Write(str);

                                      
                }

                client.Client.Shutdown(reason);
       
        }

        public static void Read(TcpClient client)
        {
           
                while (true)
                {

                    BinaryReader reader = new BinaryReader(client.GetStream());

                    Console.WriteLine("Server yazir ki: " + reader.ReadString());


                }
          
          
        }

        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 14000);
            Thread writeThread = new Thread(() => Write(client));
            Thread readThread = new Thread(() => Read(client));
            writeThread.Start();
            readThread.Start();

            writeThread.Join();
            readThread.Join();

            client.Close();
            Console.WriteLine("client exiting");
        }

    }
}

