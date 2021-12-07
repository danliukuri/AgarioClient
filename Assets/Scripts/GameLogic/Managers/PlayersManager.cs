using Pool;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject localPlayerPrefab;
    [SerializeField] GameObject globalPlayerPrefab;

    static Dictionary<int, Player> players = new Dictionary<int, Player>();
    static PlayersManager instance;
    #endregion

    #region Methods
    public static Player GetPlayer(int index) => players[index];
    public static bool ContainsPlayer(int playerId) => players.ContainsKey(playerId);

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public static void Reset()
    {
        foreach (int playerId in players.Keys)
        {
            players[playerId].Reset();
            players[playerId].gameObject.SetActive(false);
        }
        players = new Dictionary<int, Player>();
    }
    public static void ResetPlayerSizes()
    {
        foreach (Player player in players.Values)
            player.ResetSize();
    }
    public static void SpawnPlayer(int id, string username, Vector3 position, float size)
    {
        string playerName = id == Client.Id ?
            instance.localPlayerPrefab.name :
            instance.globalPlayerPrefab.name;
        GameObject playerGameObject = PoolManager.GetGameObject(playerName);

        Player player = playerGameObject.GetComponent<Player>();
        player.Initialize(username, position, size);
        players.Add(id, player);

        playerGameObject.SetActive(true);
    }
    public static void RemovePlayer(int id)
    {
        Player player = players[id];
        players.Remove(id);

        player.Reset();
        player.gameObject.SetActive(false);
    }
    #endregion
}