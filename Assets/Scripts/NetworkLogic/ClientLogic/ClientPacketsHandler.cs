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

    public static void FieldGenerated(Packet packet)
    {
        int height = packet.ReadInt();
        int width = packet.ReadInt();

        Vector2 startSectorPosition = packet.ReadVector2();
        Vector2 sectorSize = packet.ReadVector2();

        int expansionMagnitudeOfVisibleSectors = packet.ReadInt();
        int expansionMagnitudeOfInvisibleSectors = packet.ReadInt();

        Field.Initialize(height, width, startSectorPosition, sectorSize,
            expansionMagnitudeOfVisibleSectors, expansionMagnitudeOfInvisibleSectors);
    }
    public static void CurrentFieldSectorUpdate(Packet packet)
    {
        int hightIndex = packet.ReadInt();
        int widthIndex = packet.ReadInt();

        Field.DrawSectors(hightIndex, widthIndex);
    }

    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector2 position = packet.ReadVector2();

        if (!PlayersManager.PlayersContainsKey(id))
            PlayersManager.SpawnPlayer(id, username, position);
        else
            Debug.Log("An unnecessary \"SpawnPlayer\" package received.");
    }
    public static void RemovePlayer(Packet packet)
    {
        int id = packet.ReadInt();
        if (PlayersManager.PlayersContainsKey(id))
            PlayersManager.RemovePlayer(id);
        else
            Debug.Log("An unnecessary \"RemovePlayer\" package received.");
    }

    public static void PlayerMovement(Packet packet)
    {
        int id = packet.ReadInt();
        Vector2 position = packet.ReadVector2();

        if (PlayersManager.PlayersContainsKey(id))
            PlayersManager.GetPlayer(id).SetPosition(position);
        else
            Debug.Log("An unnecessary \"PlayerMovement\" package received.");
    }
    #endregion
}