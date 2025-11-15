using System;

public class WeightedRandom
{

    // Helper method for weighted random selection (reusable across seasons or elsewhere)
    public static T WeightedRandomPicker<T>(Dictionary<T, int> weights, Random rng)
        where T : struct, Enum // Assuming WeatherData is an enum
    {
        int totalWeight = weights.Values.Sum();
        int randomValue = rng.Next(totalWeight);
        int cumulative = 0;

        foreach (var kvp in weights)
        {
            cumulative += kvp.Value;
            if (randomValue < cumulative)
                return kvp.Key;
        }

        return weights.Keys.Last(); // Fallback (shouldn't hit)
    }
}
