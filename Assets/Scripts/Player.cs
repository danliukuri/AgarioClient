using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    
    public void Initialize(int id, string username)
    {
        Id = id;
        Username = username;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
}