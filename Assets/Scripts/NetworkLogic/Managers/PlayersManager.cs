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

    public static void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
    {
        GameObject playerToSpawn = id == Client.Id ?
            instance.localPlayerPrefab : instance.globalPlayerPrefab;
        GameObject playerGameObject = Instantiate(playerToSpawn, position, rotation);

        Player player = playerGameObject.GetComponent<Player>();
        player.Initialize(id, username);
        players.Add(id, player);
    }
    public static void RemovePlayer(int id)
    {
        Destroy(players[id].gameObject);
        players.Remove(id);
    }
    #endregion
}