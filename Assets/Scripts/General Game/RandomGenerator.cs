using UnityEngine;
using System.Collections;

public class RandomGenerator : MonoBehaviour
{

    public string customSeed;
    static string seed;

    // Use this for initialization
    void Start()
    {
        seed = RandomSeed();
        if (customSeed != "")
            seed = customSeed;
        Random.seed = seed.GetHashCode();
        customSeed = seed;
    }

    string RandomSeed()
    {
        string output = "";
        for (int i = 0; i < 10; ++i)
        {
            char letter = (char)('a' + Random.Range(0, 26));
            output += letter;
        }
        return output;
    }

    public static int RandomInt(int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }

    public static float RandomFloat(float minInclusive, float maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive);
    }

    public static string Seed
    {
        get { return seed; }
    }

}
