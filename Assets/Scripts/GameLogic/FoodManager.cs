using Pool;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject foodPrefab;

    static Dictionary<int, GameObject> allFood;
    static FoodManager instance;
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
    static void Initialize()
    {
        allFood = new Dictionary<int, GameObject>();
    }

    public static void SpawnFood(int foodId, Vector2 position)
    {
        GameObject foodGameObject = PoolManager.GetGameObject(instance.foodPrefab.name);

        allFood.Add(foodId, foodGameObject);

        foodGameObject.transform.position = position;
        foodGameObject.SetActive(true);
    }
    public static void RemoveFood(int foodId)
    {
        allFood[foodId].SetActive(false);
        allFood.Remove(foodId);
    }
    #endregion
}