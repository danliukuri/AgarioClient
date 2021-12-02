using System.Net;
using UnityEngine;

static class ClientPacketsHandler
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

        Vector2 position = packet.ReadVector2();
        Vector2 sectorSize = packet.ReadVector2();

        int expansionMagnitudeOfVisibleSectors = packet.ReadInt();
        int expansionMagnitudeOfInvisibleSectors = packet.ReadInt();

        FoodManager.Initialize(height, width);
        Field.Initialize(height, width, position, sectorSize,
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

        PlayersManager.SpawnPlayer(id, username, position);
    }
    public static void RemovePlayer(Packet packet)
    {
        int id = packet.ReadInt();
        PlayersManager.RemovePlayer(id);
    }

    public static void PlayerMovement(Packet packet)
    {
        int id = packet.ReadInt();
        Vector2 position = packet.ReadVector2();

        PlayersManager.GetPlayer(id).SetPosition(position);
    }

    public static void SpawnFood(Packet packet)
    {
        int hightIndex = packet.ReadInt();
        int widthIndex = packet.ReadInt();
        Vector2 position = packet.ReadVector2();

        FoodManager.SpawnFood(hightIndex, widthIndex, position);
    }
    public static void RemoveFood(Packet packet)
    {
        int hightIndex = packet.ReadInt();
        int widthIndex = packet.ReadInt();

        FoodManager.RemoveFood(hightIndex, widthIndex);
    }
    #endregion
}