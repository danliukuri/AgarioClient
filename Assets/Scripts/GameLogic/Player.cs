using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    public int Id { get; private set; }
    public string Username { get; private set; }
    #endregion

    #region Methods
    public void Initialize(int id, string username)
    {
        Id = id;
        Username = username;
    }
    public void Reset()
    {
        Id = default;
        Username = default;
        transform.position = Vector3.zero;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void EatFood(float sizeChange)
    {
        transform.localScale += new Vector3(sizeChange, sizeChange);
    }
    #endregion
}