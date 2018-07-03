using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Arandom
{
    static int ranSeed = 5;

    /// <summary>
    /// Size between numbers must be more than 2 and e is inexclusive
    /// </summary>
    public static int GetRandomEvenInt(int s,int e)
    {
        if (e - s < 2)
        {
            Debug.Log("Random error");
            return 0;
        }

        while (true)
        {
            int even = Random.Range(s, e);
            if (even % 2 == 0)
                return even;
        }            
    }
    /// <summary>
    /// Size between numbers must be more than 2 and e is inexclusive
    /// </summary>
    public static int GetRandomOddInt(int s, int e)
    {
        if(e-s<2)
        {
            Debug.Log("Random error");
            return 0;
        }
        while (true)
        {
            int odd = Random.Range(s, e);
            if (odd % 2 != 0)
                return odd;
        }
    }
}
