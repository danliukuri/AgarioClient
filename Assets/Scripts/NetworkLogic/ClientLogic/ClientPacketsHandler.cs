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

        Field.Initialize(height, width, position, sectorSize,
            expansionMagnitudeOfVisibleSectors, expansionMagnitudeOfInvisibleSectors);
    }
    public static void CurrentFieldSectorUpdate(Packet packet)
    {
        int heightIndex = packet.ReadInt();
        int widthIndex = packet.ReadInt();

        Field.DrawSectors(heightIndex, widthIndex);
    }

    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector2 position = packet.ReadVector2();
        float size = packet.ReadFloat();

        PlayersManager.SpawnPlayer(id, username, position, size);
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

        if (PlayersManager.ContainsPlayer(id))
            PlayersManager.GetPlayer(id).SetPosition(position);
        else
            Debug.Log("An unnecessary \"PlayerMovement\" UDP package for player " + id + " received.");
    }

    public static void SpawnFood(Packet packet)
    {
        int foodId = packet.ReadInt();
        Vector2 position = packet.ReadVector2();

        FoodManager.SpawnFood(foodId, position);
    }
    public static void RemoveFood(Packet packet)
    {
        int foodId = packet.ReadInt();
        FoodManager.RemoveFood(foodId);
    }

    public static void EatingFood(Packet packet)
    {
        int playerId = packet.ReadInt();
        float sizeChange = packet.ReadFloat();

        if (PlayersManager.ContainsPlayer(playerId))
            PlayersManager.GetPlayer(playerId).EatFood(sizeChange);
        else
            Debug.Log("An unnecessary \"EatingFood\" UDP package for player " + playerId + " received.");
    }

    public static void Losing(Packet packet)
    {
        Client.Disconnect();

        PlayersManager.Reset();
        FoodManager.Reset();
        Field.Reset();
        UIManager.UserLoss();
    }

    public static void ListOfTheBestPlayersNamesBySize(Packet packet)
    {
        int playersCount = packet.ReadInt();
        string[] playerNames = new string[playersCount];
        for (int i = 0; i < playersCount; i++)
            playerNames[i] = packet.ReadString();

        UIManager.DisplayListOfTheBestPlayersNamesBySize(playerNames);
    }

    public static void ResetPlayerSizes(Packet packet)
    {
        PlayersManager.ResetPlayerSizes();
    }
    #endregion
}