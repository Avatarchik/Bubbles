using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Sockets;

public class TCPClient : MonoBehaviour
{
    public const int PORT = 10000;

    public Queue<Command> commands = new Queue<Command>();

    private Socket clientSocket;
    private readonly byte[] readBuffer = new byte[1024];

    private void OnDestroy()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket = null;
        }
    }

    public void StartConnect(string ip)
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IAsyncResult result = clientSocket.BeginConnect(ip, 10000, EndConnect, null);

            bool connectSuccess = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(10));

            if (!connectSuccess)
            {
                clientSocket.Close();
                Debug.LogError("Client unable to connect. Failed");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(string.Format("Client exception on beginconnect: {0}", ex.Message));
        }
    }

    private void EndConnect(IAsyncResult iar)
    {
        clientSocket.EndConnect(iar);
        clientSocket.NoDelay = true;
        BeginReceiveData();

        Debug.Log("Client connected");
    }

    private void BeginReceiveData()
    {
        clientSocket.BeginReceive(readBuffer, 0, readBuffer.Length, SocketFlags.None, EndReceiveData, null);
    }

    private void EndReceiveData(IAsyncResult iar)
    {
        clientSocket.EndReceive(iar);
        ProcessData();

        BeginReceiveData();
    }

    private void ProcessData()
    {
        var command = (Command)Utils.ByteArrayToObject(readBuffer);

        CommandHadler.Instance.PostCommand(command);
    }

}