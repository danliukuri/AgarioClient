using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Properties
    public static string PlayerUsername => instance.username.text;
    #endregion

    #region Fields
    [SerializeField] GameObject startMenu;
    [SerializeField] TMP_InputField username;

    [SerializeField] GameObject userLossGameObject;

    [SerializeField] GameObject uiCamera;

    static TextMeshProUGUI usernamePlaceholder;
    static string defaultUsernamePlaceholderText;

    static UIManager instance;
    #endregion

    #region Methods
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    void Initialize()
    {
        usernamePlaceholder = (TextMeshProUGUI)instance.username.placeholder;
        defaultUsernamePlaceholderText = usernamePlaceholder.text;
        userLossGameObject.SetActive(false);
    }
    static void Reset()
    {
        instance.username.text = default;
        usernamePlaceholder.text = defaultUsernamePlaceholderText;

        instance.userLossGameObject.SetActive(false);

        instance.username.interactable = true;
        instance.startMenu.SetActive(true);

        instance.uiCamera.SetActive(true);
    }

    public void ConnectToServer()
    {
        if (!Regex.IsMatch(username.text, @"^(?i)[A-Z](([\'\-][A-Z])?[A-Z]*)*$"))
        {
            username.text = default;
            usernamePlaceholder.text = "Try again";
        }
        else
        {
            startMenu.SetActive(false);
            username.interactable = false;
            uiCamera.SetActive(false);
            Client.ConnectToServer();
        }
    }

    public static void UserLoss()
    {
        Reset();
        instance.userLossGameObject.SetActive(true);
    }
    #endregion
}