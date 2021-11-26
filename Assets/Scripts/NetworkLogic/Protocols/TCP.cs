using System;
using System.Net.Sockets;
using UnityEngine;

public class TCP
{
    #region Properties
    public TcpClient Socket { get; private set; }
    #endregion

    #region Fields
    static int dataBufferSize = 4096;
    NetworkStream stream;
    Packet receivedData;
    byte[] receiveBuffer;
    Client client;
    #endregion

    #region Methods
    public TCP(Client client)
    {
        this.client = client;
    }

    public void Connect(string ip, int port)
    {
        Socket = new TcpClient
        {
            ReceiveBufferSize = dataBufferSize,
            SendBufferSize = dataBufferSize
        };

        receiveBuffer = new byte[dataBufferSize];
        Socket.BeginConnect(ip, port, ConnectCallback, Socket);
    }
    void Disconnect()
    {
        Client.Disconnect();

        stream = null;
        receivedData = null;
        receiveBuffer = null;
        Socket = null;
    }

    public void SendData(Packet packet)
    {
        try
        {
            if (Socket != null)
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to server via TCP: {ex}");
        }
    }
    bool HandleData(byte[] data)
    {
        int packetLength = 0;

        receivedData.SetBytes(data);

        if (receivedData.UnreadLength() >= 4)
        {
            packetLength = receivedData.ReadInt();
            if (packetLength <= 0)
            {
                return true;
            }
        }

        while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
        {
            byte[] packetBytes = receivedData.ReadBytes(packetLength);
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Client.GetPacketHandler(packetId)(packet);
                }
            });

            packetLength = 0;
            if (receivedData.UnreadLength() >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }
        }

        if (packetLength <= 1)
        {
            return true;
        }

        return false;
    }

    void ConnectCallback(IAsyncResult result)
    {
        Socket.EndConnect(result);

        if (!Socket.Connected)
        {
            return;
        }

        stream = Socket.GetStream();

        receivedData = new Packet();

        stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
    }
    void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                Client.Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);

            receivedData.Reset(HandleData(data));
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }
        catch
        {
            Disconnect();
        }
    }
    #endregion
}