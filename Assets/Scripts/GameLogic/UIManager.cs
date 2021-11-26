using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Properties
    public static TMP_InputField UsernameField => instance.usernameField;
    #endregion

    #region Fields
    [SerializeField] GameObject startMenu;
    [SerializeField] TMP_InputField usernameField;

    static UIManager instance;
    #endregion

    #region Methods
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
        Client.ConnectToServer();
    }
    #endregion
}
