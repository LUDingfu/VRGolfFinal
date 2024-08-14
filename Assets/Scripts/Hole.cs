using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class Hole : MonoBehaviour
{
    private Random random = new Random();
    private List<int> counts = new List<int>();
    private List<int> errorList= new List<int>();
    private List<int> errorListShuffled= new List<int>();
    private List<int> mediumList= new List<int>();
    public List<Vector2> FinalList= new List<Vector2>();
    

    public class Block
    {
        
    }
    
    [SerializeField]
    private int gridSize = 5;
    
    private List<Vector2> hasRun;

    void Awake()
    {
        RandomCounts.GenerateCounts();
        counts = new List<int> { RandomCounts.count_A, RandomCounts.count_B, RandomCounts.count_C, 
            RandomCounts.count_D, RandomCounts.count_E, RandomCounts.count_F };
        errorList = new List<int> { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4, 5 };
        errorListShuffled = RandomCounts.ShuffleList(errorList);
        mediumList = RandomCounts.GenerateMediumList(errorListShuffled);
        FinalList = GenerateFinalList(mediumList);
    }
    
    public List<Vector2> GenerateFinalList(List<int> mediumList)
    {
        
        List<Vector2> resultList = new List<Vector2>();

        foreach (int value in mediumList)
        {
            if (value == 0)
            {
                resultList.Add(new Vector2(transform.position.x, transform.position.z));
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

    private Vector2 GetA()
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

        return new Vector2(x,  z);
    }

    private Vector2 GetB()
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

        return new Vector2(x,  z);
    }

    private Vector2 GetC()
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

        return new Vector2(x,  z);
    }

    private Vector2 GetD()
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

        return new Vector2(x,  z);
    }

    private Vector2 GetE()
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

        return new Vector2(x,  z);
    }

    private Vector2 GetF()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (transform.position.x - 2.5f * gridSize, transform.position.x + 2.5f * gridSize, transform.position.z + 2.5f * gridSize, transform.position.z + 4.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Vector2(x,  z);
    }

    private void AssertListsEqual<T>(List<T> list1, List<T> list2)
    {
        if (list1.Count != list2.Count)
        {
            Debug.LogError("Lists do not have the same number of elements.");
            return;
        }

        var sortedList1 = list1.OrderBy(item => item).ToList();
        var sortedList2 = list2.OrderBy(item => item).ToList();

        for (int i = 0; i < sortedList1.Count; i++)
        {
            if (!sortedList1[i].Equals(sortedList2[i]))
            {
                Debug.LogError("Lists do not contain the same elements.");
                return;
            }
        }

        Debug.Log("Lists contain the same elements.");
    }
}