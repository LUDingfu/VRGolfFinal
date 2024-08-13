using System;
using UnityEngine;
using System.Collections.Generic;

public class Hole : MonoBehaviour
{
    private static System.Random random;
    private List<int> counts;
    private List<int> errorList;
    private List<int> errorListShuffled;
    private List<int> mediumList;
    public List<Coordinate> FinalList;
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
    private int gridSize = 5;
    
    private List<Coordinate> hasRun;

    void Awake()
    {
        counts = new List<int> { RandomCounts.count_A, RandomCounts.count_B, RandomCounts.count_C, 
            RandomCounts.count_D, RandomCounts.count_E, RandomCounts.count_F };
        errorList = new List<int> { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4, 5 };
        errorListShuffled = RandomCounts.ShuffleList(errorList);
        mediumList = RandomCounts.GenerateMediumList(errorListShuffled);
        FinalList = GenerateFinalList(mediumList);
    }
    
    public List<Coordinate> GenerateFinalList(List<int> mediumList)
    {
        
        List<Coordinate> resultList = new List<Coordinate>();

        foreach (int value in mediumList)
        {
            if (value == 0)
            {
                resultList.Add(new Coordinate(transform.position.x, transform.position.z));
            }
            else if (value == 1)
            {
                int index = SelectRandomNonZeroCount();
                
                switch (index)
                {
                    case 0:
                        counts[0]--;
                        resultList.Add(GetA());
                        break;
                    case 1:
                        counts[1]--;
                        resultList.Add(GetB());
                        break;
                    case 2:
                        counts[2]--;
                        resultList.Add(GetC());
                        break;
                    case 3:
                        counts[3]--;
                        resultList.Add(GetD());
                        break;
                    case 4:
                        counts[4]--;
                        resultList.Add(GetE());
                        break;
                    case 5:
                        counts[5]--;
                        resultList.Add(GetF());
                        break;
                }
            }
        }
        if (resultList.Count!=90)
        {
            throw new Exception("There should be 90 elements");
        }
        return resultList;
    }

    private int SelectRandomNonZeroCount()
    {
        int randomIndex = random.Next(0, counts.Count);
        if (counts[randomIndex] > 0)
        {
            return randomIndex;
        }
        return SelectRandomNonZeroCount();
        
    }

    private Coordinate GetA()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x + 0.5f * gridSize, transform.position.x + 1.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z + 0.5f * gridSize),
            (transform.position.x - 1.5f * gridSize, transform.position.x - 0.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z + 0.5f * gridSize),
            (transform.position.x - 0.5f * gridSize, transform.position.x + 0.5f * gridSize, transform.position.z + 0.5f * gridSize, transform.position.z + 1.5f * gridSize),
            (transform.position.x - 0.5f * gridSize, transform.position.x + 0.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z - 1.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Coordinate(x,  z);
    }

    private Coordinate GetB()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x + 0.5f * gridSize, transform.position.x + 1.5f * gridSize, transform.position.z + 0.5f * gridSize, transform.position.z + 1.5f * gridSize),
            (transform.position.x - 1.5f * gridSize, transform.position.x - 0.5f * gridSize, transform.position.z + 0.5f * gridSize, transform.position.z + 1.5f * gridSize),
            (transform.position.x + 0.5f * gridSize, transform.position.x + 1.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z - 1.5f * gridSize),
            (transform.position.x - 1.5f * gridSize, transform.position.x - 0.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z - 1.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Coordinate(x,  z);
    }

    private Coordinate GetC()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x + 0.5f * gridSize, transform.position.x + 1.5f * gridSize, transform.position.z + 1.5f * gridSize, transform.position.z + 2.5f * gridSize),
            (transform.position.x - 1.5f * gridSize, transform.position.x - 0.5f * gridSize, transform.position.z + 1.5f * gridSize, transform.position.z + 2.5f * gridSize),
            (transform.position.x + 0.5f * gridSize, transform.position.x + 1.5f * gridSize, transform.position.z - 2.5f * gridSize, transform.position.z - 1.5f * gridSize),
            (transform.position.x - 1.5f * gridSize, transform.position.x - 0.5f * gridSize, transform.position.z - 2.5f * gridSize, transform.position.z - 1.5f * gridSize),
            (transform.position.x + 1.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z + 0.5f * gridSize, transform.position.z + 1.5f * gridSize),
            (transform.position.x - 2.5f * gridSize, transform.position.x - 1.5f * gridSize, transform.position.z + 0.5f * gridSize, transform.position.z + 1.5f * gridSize),
            (transform.position.x + 1.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z - 1.5f * gridSize),
            (transform.position.x - 2.5f * gridSize, transform.position.x - 1.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z - 1.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Coordinate(x,  z);
    }

    private Coordinate GetD()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x + 1.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z + 1.5f * gridSize, transform.position.z + 2.5f * gridSize),
            (transform.position.x + 1.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z - 2.5f * gridSize, transform.position.z - 1.5f * gridSize),
            (transform.position.x - 2.5f * gridSize, transform.position.x - 1.5f * gridSize, transform.position.z + 1.5f * gridSize, transform.position.z + 2.5f * gridSize),
            (transform.position.x - 2.5f * gridSize, transform.position.x - 1.5f * gridSize, transform.position.z - 2.5f * gridSize, transform.position.z - 1.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Coordinate(x,  z);
    }

    private Coordinate GetE()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x + 1.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z + 0.5f * gridSize),
            (transform.position.x - 2.5f * gridSize, transform.position.x - 1.5f * gridSize, transform.position.z - 0.5f * gridSize, transform.position.z + 0.5f * gridSize),
            (transform.position.x - 0.5f * gridSize, transform.position.x + 0.5f * gridSize, transform.position.z - 2.5f * gridSize, transform.position.z - 1.5f * gridSize),
            (transform.position.x - 0.5f * gridSize, transform.position.x + 0.5f * gridSize, transform.position.z + 1.5f * gridSize, transform.position.z + 2.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Coordinate(x,  z);
    }

    private Coordinate GetF()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x - 2.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z + 2.5f * gridSize, transform.position.z + 4.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Coordinate(x,  z);
    }
}