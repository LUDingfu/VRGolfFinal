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
    public Vector2 holeTransformation;
    public List<Vector2> finalAdaptiveList;
    
    
    [SerializeField]
    private float gridSize = 1;
    
    private List<Vector2> hasRun;

    void Awake()
    {
        RandomCounts.GenerateCounts();
        counts = new List<int> { RandomCounts.count_A, RandomCounts.count_B, RandomCounts.count_C, 
            RandomCounts.count_D, RandomCounts.count_E, RandomCounts.count_F };
        errorList = new List<int> { 5, 4, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };
        errorListShuffled = RandomCounts.ShuffleList(errorList);
        mediumList = RandomCounts.GenerateMediumList(errorListShuffled);
        FinalList = GenerateFinalList(mediumList);
        holeTransformation = new Vector2(holeTransformation.x, holeTransformation.y);
    }
    
    public List<Vector2> GenerateFinalList(List<int> mediumList)
    {
        
        List<Vector2> resultList = new List<Vector2>();

        foreach (int value in mediumList)
        {
            if (value == 0)
            {
                resultList.Add(new Vector2(holeTransformation.x, holeTransformation.y));
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



    
    public float CalculateMRE(List<Vector2> coordinateList)
    {
        float totalError = 0f;
        int count = coordinateList.Count;

        if (count == 0)
            return 0f;

        foreach (Vector2 coordinate in coordinateList)
        {
            Vector2 errorVector = coordinate - holeTransformation;

            totalError += errorVector.magnitude;
        }
        float meanError = totalError / count;
        return meanError;
    }
    public List<Vector2> GenerateAdaptiveList(List<int> errorList)
    {
        finalAdaptiveList = new List<Vector2>();
        int elementEachBlock = 6;
        float initialMRE = CalculateMRE(new List<Vector2> { GetE(), GetF(), GetE(), GetE(), GetE(), holeTransformation });
        float MRELastTime = initialMRE;

        if (errorList.Count != 15)
        {
            throw new Exception("The errorList should have exactly 15 elements.");
        }

        for (int i = 0; i < errorList.Count; i++)
        {
            bool findLowerMRE = false;
            List<Vector2> block = new List<Vector2>();
            int numberOfErrorsThisBlock = errorList[i];
            for (int j = 0; j < elementEachBlock - numberOfErrorsThisBlock; j++)
            {
                block.Add(holeTransformation);
            }

            while (!findLowerMRE)
            {
                List<Vector2> errorListInOneBlock = RandomGenerateErrorList(numberOfErrorsThisBlock);
                block.AddRange(errorListInOneBlock);
                float MRE_ThisTempBlockList = CalculateMRE(block);
                if (MRE_ThisTempBlockList < MRELastTime)
                {
                    finalAdaptiveList.AddRange(block);
                    MRELastTime = MRE_ThisTempBlockList;
                    findLowerMRE = true;
                }
            }
        }
        return finalAdaptiveList;
    }
    

    private List<Vector2> RandomGenerateErrorList(int numberOfErrorsThisBlock)
    {
        List<Vector2> errorListInOneBlock = new List<Vector2>(numberOfErrorsThisBlock);
        int numberOfErrors = 6;
        for (int i = 0; i < numberOfErrors; i++)
        {
            int randomNumber=random.Next(0, numberOfErrors);
            switch (randomNumber)
            {
                case 0:
                    errorListInOneBlock.Add(GetA());
                    break;
                case 1:
                    errorListInOneBlock.Add(GetB());
                    break;
                case 2:
                    errorListInOneBlock.Add(GetC());
                    break;
                case 3:
                    errorListInOneBlock.Add(GetD());
                    break;
                case 4:
                    errorListInOneBlock.Add(GetE());
                    break;
                case 5:
                    errorListInOneBlock.Add(GetF());
                    break;
            }
        }

        return errorListInOneBlock;
    }
        private Vector2 GetA()
    {
        var ranges = new List<(float xMin, float xMax, float zMin, float zMax)>
        {
            (holeTransformation.x + 0.5f * gridSize, holeTransformation.x + 1.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y + 0.5f * gridSize),
            (holeTransformation.x - 1.5f * gridSize, holeTransformation.x - 0.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y + 0.5f * gridSize),
            (holeTransformation.x - 0.5f * gridSize, holeTransformation.x + 0.5f * gridSize, holeTransformation.y + 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize),
            (holeTransformation.x - 0.5f * gridSize, holeTransformation.x + 0.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y - 1.5f * gridSize)
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
            (holeTransformation.x + 0.5f * gridSize, holeTransformation.x + 1.5f * gridSize, holeTransformation.y + 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize),
            (holeTransformation.x - 1.5f * gridSize, holeTransformation.x - 0.5f * gridSize, holeTransformation.y + 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize),
            (holeTransformation.x + 0.5f * gridSize, holeTransformation.x + 1.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y - 1.5f * gridSize),
            (holeTransformation.x - 1.5f * gridSize, holeTransformation.x - 0.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y - 1.5f * gridSize)
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
            (holeTransformation.x + 0.5f * gridSize, holeTransformation.x + 1.5f * gridSize, holeTransformation.y + 1.5f * gridSize, holeTransformation.y + 2.5f * gridSize),
            (holeTransformation.x - 1.5f * gridSize, holeTransformation.x - 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize, holeTransformation.y + 2.5f * gridSize),
            (holeTransformation.x + 0.5f * gridSize, holeTransformation.x + 1.5f * gridSize, holeTransformation.y - 2.5f * gridSize, holeTransformation.y - 1.5f * gridSize),
            (holeTransformation.x - 1.5f * gridSize, holeTransformation.x - 0.5f * gridSize, holeTransformation.y - 2.5f * gridSize, holeTransformation.y - 1.5f * gridSize),
            (holeTransformation.x + 1.5f * gridSize, holeTransformation.x + 2.5f * gridSize, holeTransformation.y + 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize),
            (holeTransformation.x - 2.5f * gridSize, holeTransformation.x - 1.5f * gridSize, holeTransformation.y + 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize),
            (holeTransformation.x + 1.5f * gridSize, holeTransformation.x + 2.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y - 1.5f * gridSize),
            (holeTransformation.x - 2.5f * gridSize, holeTransformation.x - 1.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y - 1.5f * gridSize)
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
            (holeTransformation.x + 1.5f * gridSize, holeTransformation.x + 2.5f * gridSize, holeTransformation.y + 1.5f * gridSize, holeTransformation.y + 2.5f * gridSize),
            (holeTransformation.x + 1.5f * gridSize, holeTransformation.x + 2.5f * gridSize, holeTransformation.y - 2.5f * gridSize, holeTransformation.y - 1.5f * gridSize),
            (holeTransformation.x - 2.5f * gridSize, holeTransformation.x - 1.5f * gridSize, holeTransformation.y + 1.5f * gridSize, holeTransformation.y + 2.5f * gridSize),
            (holeTransformation.x - 2.5f * gridSize, holeTransformation.x - 1.5f * gridSize, holeTransformation.y - 2.5f * gridSize, holeTransformation.y - 1.5f * gridSize)
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
            (holeTransformation.x + 1.5f * gridSize, holeTransformation.x + 2.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y + 0.5f * gridSize),
            (holeTransformation.x - 2.5f * gridSize, holeTransformation.x - 1.5f * gridSize, holeTransformation.y - 0.5f * gridSize, holeTransformation.y + 0.5f * gridSize),
            (holeTransformation.x - 0.5f * gridSize, holeTransformation.x + 0.5f * gridSize, holeTransformation.y - 2.5f * gridSize, holeTransformation.y - 1.5f * gridSize),
            (holeTransformation.x - 0.5f * gridSize, holeTransformation.x + 0.5f * gridSize, holeTransformation.y + 1.5f * gridSize, holeTransformation.y + 2.5f * gridSize)
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
            (holeTransformation.x - 2.5f * gridSize, holeTransformation.x + 2.5f * gridSize, holeTransformation.y + 2.5f * gridSize, holeTransformation.y + 4.5f * gridSize)
        };

        var selectedRange = ranges[random.Next(ranges.Count)];

        float x = (float)(random.NextDouble() * (selectedRange.xMax - selectedRange.xMin) + selectedRange.xMin);
        float z = (float)(random.NextDouble() * (selectedRange.zMax - selectedRange.zMin) + selectedRange.zMin);

        return new Vector2(x,  z);
    }
}