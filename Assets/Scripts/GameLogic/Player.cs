using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Properties
    public float Size
    {
        get => transform.localScale.x;
        private set => transform.localScale = new Vector3(value, value, defaultSize);
    }
    #endregion

    #region Fields
    [SerializeField] TextMeshProUGUI usernameTMP;

    static Vector3 defaultPosition;
    static float defaultSize;
    #endregion

    #region Methods
    void Awake()
    {
        defaultPosition = transform.position;
        defaultSize = transform.localScale.x;
    }
    public void Initialize(string username, Vector3 position, float size)
    {
        usernameTMP.text = username;
        transform.position = position;
        Size = size;
    }
    public void Reset()
    {
        usernameTMP.text = default;
        transform.position = defaultPosition;
        Size = defaultSize;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void EatFood(float sizeChange)
    {
        Size += sizeChange;
    }
    #endregion
}