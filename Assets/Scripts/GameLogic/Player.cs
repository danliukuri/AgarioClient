using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields
    [SerializeField] TextMeshProUGUI usernameTMP;
    #endregion

    #region Methods
    public void Initialize(string username)
    {
        usernameTMP.text = username;
    }
    public void Reset()
    {
        usernameTMP.text = default;
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