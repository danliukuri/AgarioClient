using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    #region Properties
    public static string PlayerUsername => instance.usernameField.text;
    #endregion

    #region Fields
    [SerializeField] GameObject startMenu;
    [SerializeField] TMP_InputField usernameField;

    static UIManager instance;
    #endregion

    #region Methods
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

    public void ConnectToServer()
    {
        if(!Regex.IsMatch(usernameField.text, @"^(?i)[A-Z](([\'\-][A-Z])?[A-Z]*)*$"))
        {
            usernameField.text = default;
            ((TextMeshProUGUI)usernameField.placeholder).text = "Try again";
        }
        else
        {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            Client.ConnectToServer();
        }
    }
    #endregion
}