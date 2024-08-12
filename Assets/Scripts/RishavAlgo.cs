using System;
using System.Collections.Generic;

public static class RishavAlgo
{
    private static System.Random random = new System.Random();

    public static List<float> GenerateValues(int n, float mean, float low, float high)
    {
        if (n != 90) throw new ArgumentException("Number of values must be exactly 90.");
        if (low > mean || mean > high) throw new ArgumentException("The target average must be between the lower and upper bounds.");
        
        List<float> values = new List<float>();

        List<float> randomDistances = GenerateRandomValues(30, mean, low, high);

        values = DistributeValues(randomDistances);

        return values;
    }

    private static List<float> GenerateRandomValues(int n, float mean, float low, float high)
    {
        List<float> values = new List<float>();
        float sum = 0f;

        for (int i = 0; i < n - 1; i++)
        {
            float value = (float)(low + (high - low) * random.NextDouble());
            values.Add(value);
            sum += value;
        }

        float targetSum = n * mean;
        float lastValue = targetSum - sum;

        if (lastValue < low || lastValue > high)
        {
            AdjustValues(ref values, ref lastValue, low, high, targetSum);
        }
        values.Add(lastValue);

        return values;
    }

    private static void AdjustValues(ref List<float> values, ref float lastValue, float low, float high, float targetSum)
    {
        while (lastValue < low || lastValue > high)
        {
            float adjustment = 0f;

            if (lastValue < low)
            {
                adjustment = low - lastValue;
                lastValue = low;
            }
            else if (lastValue > high)
            {
                adjustment = lastValue - high; 
                lastValue = high;
            }

            for (int i = 0; i < values.Count; i++)
            {
                float currentAdjustment = Math.Min(adjustment, values[i] - low);
                values[i] -= currentAdjustment;
                adjustment -= currentAdjustment;

                if (adjustment <= 0)
                    break;
            }

            float currentSum = 0f;
            foreach (var value in values)
            {
                currentSum += value;
            }
            lastValue = targetSum - currentSum;
        }
    }

    private static List<float> DistributeValues(List<float> randomDistances)
    {
        List<float> values = new List<float>(new float[90]);
        int elementsPerBlock = 6;
        int randomIndex = 0;

        for (int i = 0; i < 90; i += elementsPerBlock)
        {
            // Determine how many elements to use from randomDistances in this block
            int elementsFromRandomDistances = random.Next(1, Math.Min(elementsPerBlock, randomDistances.Count - randomIndex) + 1);

            // Generate random positions within the block for the selected elements
            HashSet<int> selectedPositions = new HashSet<int>();
            while (selectedPositions.Count < elementsFromRandomDistances)
            {
                selectedPositions.Add(random.Next(0, elementsPerBlock));
            }

            // Fill the block with coordinates
            for (int j = 0; j < elementsPerBlock; j++)
            {
                if (selectedPositions.Contains(j) && randomIndex < randomDistances.Count)
                {
                    values[i + j] = randomDistances[randomIndex];
                    randomIndex++;
                }
            }
        }

        return values;
    }
}
