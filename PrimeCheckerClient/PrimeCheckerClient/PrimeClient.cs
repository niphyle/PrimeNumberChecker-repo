using System.Net;
using System.Net.Sockets;
using System.Text;

public class PrimeClient
{
    public static int Main(string[] args)
    {
        StartClient();
        return 0;
    }

    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {
            // Connect to hosting address
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1238);

            // Create a TCP/IP  socket
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint
            try
            {
                // Connect to Remote EndPoint
                sender.Connect(remoteEP);

                Console.WriteLine("Client: Server connected successfully...");
                bool repeat = true;
                do
                {
                    // Read user input and verify the input is valid.
                    bool check = false;
                    string userInput;
                    do
                    {
                        Console.WriteLine("Please enter a number.");
                        userInput = Console.ReadLine();
                        check = IsValidInput(userInput);

                    } while (check == false);
                

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes($"{userInput}");  

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Server response: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // See if the user would like to try another number

                    do
                    {
                        Console.WriteLine("Would you like to try another number?");

                        userInput= Console.ReadLine().ToLower();

                        check = ContinueOrExit(userInput);

                    } while(check == false);

                    if (userInput == "y" || userInput == "yes")
                    {
                        repeat = true;

                    } else
                    {
                        // send socket shutdown message
                        msg = Encoding.ASCII.GetBytes("shutdown");

                        bytesSent = sender.Send(msg);

                        repeat = false;
                    }
                
                } while (repeat != false);


                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    /// <summary>
    /// A method to verify that the user's input was a positive number.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>
    /// True if the user's input can be converted to a number and if the number is positive or 0.
    /// False if the user's input is not a positive number or 0.
    /// </returns>
    public static bool IsValidInput(string input)
    {
        double userNumber;

        try
        {
            userNumber = Convert.ToDouble(input); // Verify user entered a number.

            if (userNumber >= 0)
            {
                return true;

            } else
            {
                Console.WriteLine("You did not enter a positive number. Please try again.");
                return false;
            }

        } catch(Exception e)
        {
            Console.WriteLine("You did not enter a positive number. Please try again.");
            e.ToString();
            return false;
        }
        
    }

    /// <summary>
    /// A method to verify that the user entered a confirmation or denial to continue the program.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>
    /// True if the user inputs yes or no.
    /// False if the user inputs anything else.
    /// </returns>
    public static bool ContinueOrExit(string input) 
    { 
        if (input == "n" || input== "no" || input == "y" || input == "yes")
        {
            return true;
        } else
        {
            Console.WriteLine("Sorry, your response was unclear.");
            return false;
        }
    }
}

