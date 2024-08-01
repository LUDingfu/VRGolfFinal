using UnityEngine;
using System.Collections.Generic;

public class Hole : MonoBehaviour
{
    [System.Serializable]
    public class Coordinate
    {
        public float x;
        public float y;
        public bool trigger;

        public Coordinate(float x, float y, bool trigger)
        {
            this.x = x;
            this.y = y;
            this.trigger = trigger;
        }
    }

    [SerializeField]
    private Vector2 holeCenter = new Vector2(0, 0);
    [SerializeField]
    private float blockSize = 1.0f;
    [SerializeField]
    private int gridSize = 5;

    private List<Coordinate> blockCoordinates;

    void Awake()
    {
        blockCoordinates = new List<Coordinate>();
        FillBlockCoordinates();
    }

    void FillBlockCoordinates()
    {
        float offset = (gridSize - 1) / 2.0f * blockSize;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                float x = holeCenter.x + (i - offset) * blockSize;
                float y = holeCenter.y + (j - offset) * blockSize;
                
                bool isCenter = (i == (gridSize - 1) / 2) && (j == (gridSize - 1) / 2);
                blockCoordinates.Add(new Coordinate(x, y, isCenter));
            }
        }
    }

    public Coordinate GetRandomCoordinate(float possibility)
    {
        if (blockCoordinates == null || blockCoordinates.Count == 0)
        {
            return null;
        }

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= possibility)
        {
            int randomIndex = Random.Range(0, blockCoordinates.Count);
            return blockCoordinates[randomIndex];
        }

        return null;
    }


    public Coordinate GetCoordinateByIndex(int index)
    {
        if (blockCoordinates == null || index < 0 || index >= blockCoordinates.Count)
        {
            return null;
        }
        return blockCoordinates[index];
    }

    public List<Coordinate> GetAllCoordinates()
    {
        return blockCoordinates;
    }

    public void PrintCoordinates()
    {
        foreach (Coordinate coord in blockCoordinates)
        {
            Debug.Log("Coordinate: (" + coord.x + ", " + coord.y + ")");
        }
    }

    public Vector3 GetPerfectCoordinate()
    {
        int centerIndex = (gridSize - 1) / 2 * gridSize + (gridSize - 1) / 2;
        Coordinate perfectCoordinate = GetCoordinateByIndex(centerIndex);
        return new Vector3(perfectCoordinate.x, 0, perfectCoordinate.y); 
    }
}