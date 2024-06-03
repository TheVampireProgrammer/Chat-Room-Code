using System;
using System.Net.Sockets;
using System.Text;

class ChatRoomClient {
    private const string serverIp = "0.0.0.0";
    private const int serverPort = 8888;
    private string username;

    public void Start() {
        Console.Write("Enter your username: ");
        username = Console.ReadLine();

        try {
            TcpClient client = new TcpClient(serverIp, serverPort);
            NetworkStream stream = client.GetStream();
            Console.WriteLine("Connected to chat room server.");

            string joinMessage = "Join:" + username;
            byte[] joinBuffer = Encoding.ASCII.GetBytes(joinMessage);
            stream.Write(joinBuffer, 0, joinBuffer.Length);

            byte[] initialBuffer = new byte[1024];
            int initialBytesRead = stream.Read(initialBuffer, 0, initialBuffer.Length);
            string initialMessage = Encoding.ASCII.GetString(initialBuffer, 0, initialBytesRead);
            Console.WriteLine(initialMessage);

            while (true) {
                string message = Console.ReadLine();
                if (message.ToLower() == "exit") {
                    byte[] exitBuffer = Encoding.ASCII.GetBytes("exit");
                    stream.Write(exitBuffer, 0, exitBuffer.Length);
                    break;
                }

                message = username + " " + message;
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }

            stream.Close();
            client.Close();
        }
        catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}

class Program {
    static void Main(string[] args) {
        ChatRoomClient client = new ChatRoomClient();
        client.Start();
    }
}
