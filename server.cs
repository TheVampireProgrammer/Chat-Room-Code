using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatRoomServer {
    private Dictionary<Socket, string> clients = new Dictionary<Socket, string>();
    private const int bufferSize = 1024;
    private const int port = 8888;

    public void Start() {
        TcpListener serverSocket = new TcpListener(IPAddress.Any, port);
        serverSocket.Start();
        Console.WriteLine("Server started.");

        while (true) {
            Socket clientSocket = serverSocket.AcceptSocket();
            string username = ReceiveUsername(clientSocket);
            clients.Add(clientSocket, username);

            SendMessageToAllClients(username + " has joined.");

            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(clientSocket);
        }
    }

    private string ReceiveUsername(Socket clientSocket) {
        byte[] buffer = new byte[bufferSize];
        int bytesRead = clientSocket.Receive(buffer);
        return Encoding.ASCII.GetString(buffer, 0, bytesRead);
    }

    private void HandleClient(object clientSocket) {
        Socket client = (Socket)clientSocket;
        string username = clients[client];
        Console.WriteLine("Client connected: " + username);

        while (true) {
            try {
                byte[] buffer = new byte[bufferSize];
                int bytesRead = client.Receive(buffer);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                if (message.ToLower() == "exit") {
                    SendMessageToAllClients(username + " has left.");
                    clients.Remove(client);
                    break;
                }

                if (message.ToLower().Contains("placeholder")) {
                    message = message.Replace("placeholder", "[CENSORED]");
                }

                Console.WriteLine("Received from client: " + message);

                SendMessageToAllClients(message);
            }
            catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
                clients.Remove(client);
                client.Close();
                break;
            }
        }
    }

    private void SendMessageToAllClients(string message)
    {
        foreach (Socket client in clients.Keys)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            client.Send(buffer);
        }
    }
}

class Program {
    static void Main(string[] args) {
        ChatRoomServer server = new ChatRoomServer();
        server.Start();
    }
}
