using Pool;
using System.Collections.Generic;
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

    [SerializeField] GameObject gameplayMenu;
    [SerializeField] GameObject bestPlayersBySize;
    [SerializeField] GameObject itemOfListOfTheBestPlayersBySize;

    static List<GameObject> listOfTheBestPlayersBySize = new List<GameObject>();

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
        gameplayMenu.SetActive(false);
        bestPlayersBySize.SetActive(false);
    }
    static void Reset()
    {
        instance.username.text = default;
        usernamePlaceholder.text = defaultUsernamePlaceholderText;

        instance.userLossGameObject.SetActive(false);

        instance.username.interactable = true;
        instance.startMenu.SetActive(true);
        instance.gameplayMenu.SetActive(false);
        instance.bestPlayersBySize.SetActive(false);
        ResetListOfTheBestPlayersBySize();
    }
    static void ResetListOfTheBestPlayersBySize()
    {
        foreach (GameObject item in listOfTheBestPlayersBySize)
            item.SetActive(false);
        listOfTheBestPlayersBySize = new List<GameObject>();
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
            Client.ConnectToServer();
            gameplayMenu.SetActive(true);
        }
    }

    public static void UserLoss()
    {
        Reset();
        instance.userLossGameObject.SetActive(true);
    }

    public static void DisplayListOfTheBestPlayersNamesBySize(string[] playerNames)
    {
        ResetListOfTheBestPlayersBySize(); 
        instance.bestPlayersBySize.SetActive(true);
        for (int i = 0; i < playerNames.Length; i++)
        {
            GameObject item = PoolManager.GetGameObject(instance.itemOfListOfTheBestPlayersBySize.name);
            listOfTheBestPlayersBySize.Add(item);
            TextMeshProUGUI itemTMP = item.GetComponentInChildren<Transform>().
                                           GetComponentInChildren<TextMeshProUGUI>();
            itemTMP.text = (i+1) + ". " + playerNames[i];

            item.SetActive(true);
        }
    }
    #endregion
}