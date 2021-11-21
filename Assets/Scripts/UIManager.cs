using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance => instance;
    public TMP_InputField UsernameField  => usernameField;

    [SerializeField] GameObject startMenu;
    [SerializeField] TMP_InputField usernameField;

    static UIManager instance;

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

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.Instance.ConnectToServer();
    }
}
