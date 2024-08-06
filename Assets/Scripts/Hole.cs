using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Hole : MonoBehaviour
{
    [Serializable]
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
    private float blockSize = 1.0f;
    [SerializeField]
    private int gridSize = 5;

    [SerializeField, Range(0f, 1f)] private float errorDistanceScale;
    

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
                float x = transform.position.x + (i - offset) * blockSize * errorDistanceScale;
                float y = transform.position.z + (j - offset) * blockSize * errorDistanceScale;
                bool isCenter = (i == (gridSize - 1) / 2) && (j == (gridSize - 1) / 2);
                blockCoordinates.Add(new Coordinate(x, y, isCenter));
            }
        }
    }

    public Coordinate GetRandomCoordinate(float possibility)
    {
        if (blockCoordinates == null || blockCoordinates.Count == 0)
        {
            throw new Exception("Set Block Coordinates or its count!");
        }

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= possibility)
        {
            int randomIndex = Random.Range(0, blockCoordinates.Count);
            return blockCoordinates[randomIndex];
        }
        return new Coordinate(transform.position.x, transform.position.z, false);

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