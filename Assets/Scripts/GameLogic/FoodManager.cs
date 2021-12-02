using Pool;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject foodPrefab;

    static List<GameObject>[,] allFoodBySector;
    static FoodManager instance;
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
    public static void Initialize(int hight, int width)
    {
        allFoodBySector = new List<GameObject>[hight, width];
        for (int i = 0; i < hight; i++)
            for (int j = 0; j < width; j++)
                allFoodBySector[i, j] = new List<GameObject>();
    }

    public static void SpawnFood(int hightIndex, int widthIndex, Vector2 position)
    {
        GameObject foodGameObject = PoolManager.GetGameObject(instance.foodPrefab.name);
        List<GameObject> foodInSector = allFoodBySector[hightIndex, widthIndex];
        foodInSector.Add(foodGameObject);

        foodGameObject.transform.position = position;
        foodGameObject.SetActive(true);
    }
    public static void RemoveFood(int hightIndex, int widthIndex)
    {
        List<GameObject> foodInSector = allFoodBySector[hightIndex, widthIndex];
        for (int i = 0; i < foodInSector.Count; i++)
        {
            GameObject food = foodInSector[i];
            foodInSector.Remove(food);
            food.SetActive(false);
        }
    }
    #endregion
}