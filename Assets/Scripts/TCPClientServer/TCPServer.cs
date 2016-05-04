using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class TCPServer : MonoBehaviour
{
    protected TcpListener tcpListener;
    protected Socket clientSocket;
    protected byte[] readBuffer = new byte[1024];

    public void StartServer()
    {
        tcpListener = new TcpListener(IPAddress.Any, 10000);
        tcpListener.Start();

        StartListeningForConnections();
    }

    private void StartListeningForConnections()
    {
        tcpListener.BeginAcceptSocket(AcceptNewSocket, tcpListener);
    }

    private void AcceptNewSocket(System.IAsyncResult iar)
    {
        clientSocket = null;

        readBuffer = new byte[1024];

        clientSocket = tcpListener.EndAcceptSocket(iar);
        clientSocket.NoDelay = true;
    }

    private void OnDestroy()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket = null;
        }
        if (tcpListener != null)
        {
            tcpListener.Stop();
            tcpListener = null;
        }
    }

    private void SendMessage(byte[] msg)
    {
        clientSocket.BeginSend(msg, 0, msg.Length, SocketFlags.None, EndSend, msg);
    }

    void EndSend(System.IAsyncResult iar)
    {
        clientSocket.EndSend(iar);
    }

    public void Send(Command dequeue)
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            SendMessage(Utils.ObjectToByteArray(dequeue));
        }

    }
}