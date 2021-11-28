using Pool;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject sector;

    static Vector2[,] sectorsPositions;
    static GameObject[,] sectors;

    static ((int Min, int Max) Hight, (int Min, int Max) Width) sectorsRemovalZone;
    static int sectorsRemovalDelay = 1; // Removal zone expansion amount
    static int sectorsSpawnDelay = 1; // Spawn zone expansion amount
    static (int Hight, int Width) sectorsArrayDimensions;
    static Queue<GameObject> freeSectorGameObjects = new Queue<GameObject>();

    static Field instance;
    #endregion

    #region MainMethods
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
    public static void GenerateSectorsPositions(int height, int width,
        Vector2 startSectorPosition, Vector2 sectorSize)
    {
        sectors = new GameObject[height, width];
        sectorsArrayDimensions = (height, width);

        sectorsPositions = new Vector2[height, width];
        Vector2 position = startSectorPosition;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                sectorsPositions[i,j] = position;

                position.x += sectorSize.x;
            }
            position.x = startSectorPosition.x;
            position.y += sectorSize.y;
        }
    }
    public static void DrawSectors(int hightIndex, int widthIndex)
    {
        ((int Min, int Max) Hight, (int Min, int Max) Width) sectorsSpawnZone =
            GetExpandedZone(((hightIndex, hightIndex), (widthIndex, widthIndex)),
            sectorsSpawnDelay, sectorsArrayDimensions);

        RemoveSectors(sectorsSpawnZone);

        for (int i = sectorsSpawnZone.Hight.Min; i <= sectorsSpawnZone.Hight.Max; i++)
            for (int j = sectorsSpawnZone.Width.Min; j <= sectorsSpawnZone.Width.Max; j++)
                if(sectors[i, j] == null)
                {
                    GameObject sectorGameObject = freeSectorGameObjects.Count != 0 ?
                        freeSectorGameObjects.Dequeue() :
                        PoolManager.GetGameObject(instance.sector.name);

                    sectorGameObject.transform.position = sectorsPositions[i, j];
                    sectorGameObject.SetActive(true);
                    sectors[i, j] = sectorGameObject;
                }
    }
    static void RemoveSectors(((int Min, int Max) Hight, (int Min, int Max) Width) sectorsSpawnZone)
    {
        ((int Min, int Max) Hight, (int Min, int Max) Width) sectorsSpawnExpandedZone =
            GetExpandedZone(sectorsSpawnZone, sectorsRemovalDelay, sectorsArrayDimensions);

        for (int i = sectorsRemovalZone.Hight.Min; i <= sectorsRemovalZone.Hight.Max; i++)
            for (int j = sectorsRemovalZone.Width.Min; j <= sectorsRemovalZone.Width.Max; j++)
                if (i < sectorsSpawnExpandedZone.Hight.Min || i > sectorsSpawnExpandedZone.Hight.Max ||
                    j < sectorsSpawnExpandedZone.Width.Min || j > sectorsSpawnExpandedZone.Width.Max)
                    if (sectors[i, j] != null)
                    {
                        freeSectorGameObjects.Enqueue(sectors[i, j]);
                        sectors[i, j].SetActive(false);
                        sectors[i, j] = null;
                    }

        sectorsRemovalZone = sectorsSpawnExpandedZone;
    }
    #endregion

    #region ArrayZoneMethods
    static int GetExpandedMinIndex(int index, int expansionAmount) =>
        index < expansionAmount ? 0 : index - expansionAmount;
    static int GetExpandedMaxIndex(int index, int expansionAmount, int maxArrayLength) =>
        index >= maxArrayLength - expansionAmount ? maxArrayLength - 1 : index + expansionAmount;
    static ((int Min, int Max) Hight, (int Min, int Max) Width) GetExpandedZone(
           ((int Min, int Max) Hight, (int Min, int Max) Width) zone,
           int expansionAmount, (int Hight, int Width) arrayDimensions)
    {
        ((int Min, int Max) Hight, (int Min, int Max) Width) expandedZone;
        expandedZone.Hight.Min = GetExpandedMinIndex(zone.Hight.Min, expansionAmount);
        expandedZone.Hight.Max = GetExpandedMaxIndex(zone.Hight.Max, expansionAmount, arrayDimensions.Hight);
        expandedZone.Width.Min = GetExpandedMinIndex(zone.Width.Min, expansionAmount);
        expandedZone.Width.Max = GetExpandedMaxIndex(zone.Width.Max, expansionAmount, arrayDimensions.Width);
        return expandedZone;
    }
    #endregion
}