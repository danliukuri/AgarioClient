using Pool;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject sector;

    static Vector2[,] sectorsPositions;
    static GameObject[,] sectors;

    static Queue<GameObject> freeSectors = new Queue<GameObject>();
    static List<GameObject> usedSectors = new List<GameObject>();
    static ((int Min, int Max) Height, (int Min, int Max) Width) sectorsRemovalZone;
    static (int Height, int Width) sectorsArrayDimensions;

    /// <summary>
    /// The number by how much you need to expand the area of visible sectors.
    /// Means that if this value is 1, the visible zone will be 3x3, if 2 then 5x5, etc.
    /// </summary>
    static int expansionMagnitudeOfVisibleSectors;
    /// <summary>
    /// The number by how much you need to expand the zone of invisible sectors.
    /// Means that if this value is 1, the invisible zone will be 3x3, if 2 then 5x5, etc.
    /// </summary>
    static int expansionMagnitudeOfInvisibleSectors;

    static Field instance;
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
    public static void Initialize(int height, int width, Vector2 position, Vector2 sectorSize,
        int expansionMagnitudeOfVisibleSectors, int expansionMagnitudeOfInvisibleSectors)
    {
        Field.expansionMagnitudeOfVisibleSectors = expansionMagnitudeOfVisibleSectors;
        Field.expansionMagnitudeOfInvisibleSectors = expansionMagnitudeOfInvisibleSectors;

        sectors = new GameObject[height, width];
        sectorsArrayDimensions = (height, width);

        instance.transform.position = position;
        Vector2 startSectorPosition = new Vector2(sectorSize.x / 2f + position.x,
                                                  sectorSize.y / 2f + position.y);

        GenerateSectorsPositions(height, width, startSectorPosition, sectorSize);
    }
    static void GenerateSectorsPositions(int height, int width,
        Vector2 startSectorPosition, Vector2 sectorSize)
    {
        sectorsPositions = new Vector2[height, width];
        Vector2 position = startSectorPosition;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                sectorsPositions[i, j] = position;

                position.x += sectorSize.x;
            }
            position.x = startSectorPosition.x;
            position.y += sectorSize.y;
        }
    }
    public static void Reset()
    {
        freeSectors = new Queue<GameObject>();
        if (sectors != default)
        {
            foreach (GameObject usedSector in usedSectors)
                usedSector.SetActive(false);
            sectors = new GameObject[sectorsArrayDimensions.Height, sectorsArrayDimensions.Width];
        }
    }

    public static void DrawSectors(int heightIndex, int widthIndex)
    {
        ((int Min, int Max) Height, (int Min, int Max) Width) sectorsSpawnZone =
            ArrayZone.GetExtendedZone((heightIndex, widthIndex),
            expansionMagnitudeOfVisibleSectors, sectorsArrayDimensions);

        RemoveSectors(heightIndex, widthIndex);

        for (int i = sectorsSpawnZone.Height.Min; i <= sectorsSpawnZone.Height.Max; i++)
            for (int j = sectorsSpawnZone.Width.Min; j <= sectorsSpawnZone.Width.Max; j++)
                if (sectors[i, j] == null)
                {
                    GameObject sectorGameObject = freeSectors.Count != 0 ?
                        freeSectors.Dequeue() :
                        PoolManager.GetGameObject(instance.sector.name);

                    sectorGameObject.transform.position = sectorsPositions[i, j];
                    sectorGameObject.SetActive(true);
                    usedSectors.Add(sectorGameObject);
                    sectors[i, j] = sectorGameObject;
                }
    }
    static void RemoveSectors(int heightIndex, int widthIndex)
    {
        ((int Min, int Max) Height, (int Min, int Max) Width) sectorsSpawnExpandedZone =
            ArrayZone.GetExtendedZone((heightIndex, widthIndex),
            expansionMagnitudeOfInvisibleSectors, sectorsArrayDimensions);

        for (int i = sectorsRemovalZone.Height.Min; i <= sectorsRemovalZone.Height.Max; i++)
            for (int j = sectorsRemovalZone.Width.Min; j <= sectorsRemovalZone.Width.Max; j++)
                if (i < sectorsSpawnExpandedZone.Height.Min || i > sectorsSpawnExpandedZone.Height.Max ||
                    j < sectorsSpawnExpandedZone.Width.Min || j > sectorsSpawnExpandedZone.Width.Max)
                    if (sectors[i, j] != null)
                    {
                        usedSectors.Remove(sectors[i, j]);
                        freeSectors.Enqueue(sectors[i, j]);
                        sectors[i, j].SetActive(false);
                        sectors[i, j] = null;
                    }

        sectorsRemovalZone = sectorsSpawnExpandedZone;
    }
    #endregion
}