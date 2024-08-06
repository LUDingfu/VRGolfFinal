using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Hole : MonoBehaviour
{
    private static System.Random random = new System.Random();

    [Serializable]
    public class Coordinate
    {
        public float x;
        public float y;
        public Coordinate(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    [SerializeField]
    private float blockSize = 1.0f;
    [SerializeField]
    private int gridSize = 5;

    // [SerializeField, Range(0f, 1f)] private float errorDistanceScale;

    private List<Coordinate> hasRun;
    private List<Coordinate> blockCoordinates;

    void Awake()
    {
        blockCoordinates = new List<Coordinate>();
        
        // Example list of radii for testing
        List<float> Radiodistances = RishavAlgo.GenerateValues(30, 1f, 0.05f, 0.3f);
        FillCoordinatesList(Radiodistances);
    }

    void FillCoordinatesList(List<float> Radiodistances)
    {
        int count = 0;
        int elementsPerBlock = 6;
        Random random = new Random();

        for (int i = 0; i < 90; i += elementsPerBlock)
        {
            // Determine the remaining elements in Radiodistances
            int remainingElements = Radiodistances.Count - count;

            // Ensure at least 1 and at most 5 elements from Radiodistances per block
            int elementsFromRadiodistances = random.Next(1, Math.Min(elementsPerBlock, remainingElements) + 1);

            // Generate random positions within the block for the selected elements
            HashSet<int> selectedPositions = new HashSet<int>();
            while (selectedPositions.Count < elementsFromRadiodistances)
            {
                selectedPositions.Add(random.Next(0, elementsPerBlock));
            }

            // Fill the block with coordinates
            for (int j = 0; j < elementsPerBlock; j++)
            {
                if (selectedPositions.Contains(j) && count < Radiodistances.Count)
                {
                    float angle = (float)(random.NextDouble() * 2 * Mathf.PI);
                    float x = transform.position.x + Radiodistances[count] * Mathf.Cos(angle);
                    float y = transform.position.z + Radiodistances[count] * Mathf.Sin(angle);
                    blockCoordinates.Add(new Coordinate(x, y));
                    count++;
                }
                else
                {
                    blockCoordinates.Add(new Coordinate(transform.position.x, transform.position.z));
                }
            }
        }

        // Fill remaining elements from Radiodistances if any
        while (count < Radiodistances.Count)
        {
            float angle = (float)(random.NextDouble() * 2 * Mathf.PI);
            float x = transform.position.x + Radiodistances[count] * Mathf.Cos(angle);
            float y = transform.position.z + Radiodistances[count] * Mathf.Sin(angle);
            blockCoordinates.Add(new Coordinate(x, y));
            count++;
        }
    }



    public Coordinate GetCoordinateInList()
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
        return new Coordinate(transform.position.x, transform.position.z);
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
    
}