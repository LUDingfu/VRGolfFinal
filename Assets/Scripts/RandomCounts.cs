using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

class RandomCounts
{
    public static int count_A { get; private set; }
    public static int count_B { get; private set; }
    public static int count_C { get; private set; }
    public static int count_D { get; private set; }
    public static int count_E { get; private set; }
    public static int count_F { get; private set; }

    public static List<int> countsList;

    private static Random rand;

    public static void GenerateCounts()
    {
        int[] counts = new int[6];
        int sum = 0;
        int totalError = 30;
        for (int i = 0; i < counts.Length; i++)
        {
            counts[i] = 1;
            sum++;
        }
        Random random = new Random();
        while (sum < totalError)
        {
            int randomIndex = random.Next(0, counts.Length);
            counts[randomIndex]++;
            sum++;
        }
        count_A = counts[0];
        count_B = counts[1];
        count_C = counts[2];
        count_D = counts[3];
        count_E = counts[4];
        count_F = counts[5];
    }

    public static List<int> ShuffleList(List<int> list)
    {
        
        Random rand = new Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }
    public static List<int> GenerateMediumList(List<int> shuffledErrorList)
    {
        if (shuffledErrorList.Count != 15)
        {
            throw new Exception("The shuffledErrorList should have exactly 15 elements.");
        }
        List<int> mediumList = new List<int>();
        Random rand = new Random();

        foreach (int errorCount in shuffledErrorList)
        {
            List<int> block = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                if (i < errorCount)
                {
                    block.Add(1); 
                }
                else
                {
                    block.Add(0); 
                }
            }
            block = ShuffleList(block);
            mediumList.AddRange(block);
        }
        return mediumList;
    }
     
}
