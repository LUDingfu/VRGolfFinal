using System;
using System.Collections.Generic;

public static class RishavAlgo
{
    private static System.Random random = new System.Random();

    public static List<float> GenerateValues(int n, float mean, float low, float high)
    {
        if (n <= 0) throw new ArgumentException("Number of values must be greater than 0.");
        if (low > mean || mean > high) throw new ArgumentException("The target average must be between the lower and upper bounds.");
        
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
}