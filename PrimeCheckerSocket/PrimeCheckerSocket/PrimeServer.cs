
using PrimeCheckerSocket;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketListener
{
    public static int Main(string[] args)
    {
        StartServer();
        return 0;
    }

    public static void StartServer()
    {
        // Hosting Address
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1238);

        try
        {
            // Create and bind socket
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(10); // Socket will only listen to up to 10 requests at a time

            // Open client
            //ProcessStartInfo myInfo = new ProcessStartInfo();
            //myInfo.FileName = ;
            //myInfo.WorkingDirectory = Path.GetDirectoryName(myInfo.FileName);
            //myInfo.CreateNoWindow = true;

            Process client = Process.Start("E:\\Valencia College\\Fall 2022\\Software Dev 1\\PrimeCheckerClient\\PrimeCheckerClient\\bin\\Debug\\net6.0\\PrimeCheckerClient.exe");




            Console.WriteLine("Server: Waiting for a connection from the client...");
            Socket handler = listener.Accept();
            bool shutdown = false;

            

            do
            {
                // Incoming data from client
                string data = null, conclusion = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    
                    if (data.Length > 0)
                    {
                        break;
                    }
                }
                if (data == "shutdown")
                {
                    shutdown = true;
                } else
                {
                    Console.WriteLine("Message from client : {0}", data);

                    conclusion = PrimeChecker.Conclusion(Double.Parse(data));

                    byte[] msg = Encoding.ASCII.GetBytes(conclusion);
                    handler.Send(msg);
                }

                

            } while (shutdown == false);

            // shutdown socket
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("\nThe client has severed the connection.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}