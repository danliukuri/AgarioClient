using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;

    [SerializeField] GameObject localPlayerPrefab;
    [SerializeField] GameObject globalPlayerPrefab;

    static Dictionary<int, Player> players = new Dictionary<int, Player>();
    static GameManager instance;

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

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
    {
        GameObject playerToSpawn = id == Client.Instance.MyId ? localPlayerPrefab : globalPlayerPrefab;
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
}