using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDP
{
    #region Properties
    public UdpClient Socket { get; private set; }
    #endregion

    #region Fields
    IPEndPoint endPoint;
    Client client;
    #endregion

    #region Methods
    public UDP(Client client, string ip, int port)
    {
        this.client = client;
        endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    public void Connect(int localPort)
    {
        Socket = new UdpClient(localPort);

        Socket.Connect(endPoint);
        Socket.BeginReceive(ReceiveCallback, null);

        using (Packet packet = new Packet())
        {
            SendData(packet);
        }
    }
    void Disconnect()
    {
        Client.Disconnect();

        endPoint = null;
        Socket = null;
    }

    public void SendData(Packet packet)
    {
        try
        {
            packet.InsertInt(Client.Id);
            if (Socket != null)
            {
                Socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to server via UDP: {ex}");
        }
    }
    void HandleData(byte[] data)
    {
        using (Packet packet = new Packet(data))
        {
            int packetLength = packet.ReadInt();
            data = packet.ReadBytes(packetLength);
        }

        ThreadManager.ExecuteOnMainThread(() =>
        {
            using (Packet packet = new Packet(data))
            {
                int packetId = packet.ReadInt();
                Client.GetPacketHandler(packetId)(packet);
            }
        });
    }

    void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            byte[] data = Socket.EndReceive(result, ref endPoint);
            Socket.BeginReceive(ReceiveCallback, null);

            if (data.Length < 4)
            {
                Client.Disconnect();
                return;
            }

            HandleData(data);
        }
        catch
        {
            Disconnect();
        }
    }
    #endregion
}