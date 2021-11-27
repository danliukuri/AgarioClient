using System.Net;
using UnityEngine;

public class ClientPacketsHandler : MonoBehaviour
{
    #region PacketsHandling
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int id = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.Id = id;
        ClientPacketsSender.WelcomeReceived();

        Client.Udp.Connect(((IPEndPoint)Client.Tcp.Socket.Client.LocalEndPoint).Port);
    }
    public static void PlayerDisconnected(Packet packet)
    {
        int id = packet.ReadInt();
        PlayersManager.RemovePlayer(id);
    }

    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector2 position = packet.ReadVector2();

        PlayersManager.SpawnPlayer(id, username, position);
    }
    public static void PlayerMovement(Packet packet)
    {
        int id = packet.ReadInt();
        Vector2 position = packet.ReadVector2();

        PlayersManager.GetPlayer(id).SetPosition(position);
    }
    #endregion
}