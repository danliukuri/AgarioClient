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

    private void Awake()
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

    public static void SpawnPlayer(int id, string username, Vector3 position)
    {
        string playerName = id == Client.Id ?
            instance.localPlayerPrefab.name :
            instance.globalPlayerPrefab.name;
        GameObject playerGameObject = PoolManager.GetGameObject(playerName);
        playerGameObject.transform.position = position;

        Player player = playerGameObject.GetComponent<Player>();
        player.Initialize(username);
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