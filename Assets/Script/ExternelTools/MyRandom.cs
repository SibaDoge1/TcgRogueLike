using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public static class MyRandom
    {
        
        public static void SetSeed(int seed)
        {
            ranSeed = seed;
            Random.InitState(seed);           
        }
        static int ranSeed;

        /// <summary>
        /// Size between numbers must be more than 2 and e is inexclusive
        /// </summary>
        public static int GetRandomEvenInt(int s, int e)
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
            if (e - s < 2)
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
    /// <summary>
    /// n%확률로 true 리턴
    /// </summary>

        public static bool GetRandomBool(float n)
        {
            float b = Random.Range(0, 100f);
            if(b<=n)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    /// <summary>
    /// 가변인자로 여러개 받고 거기서 당첨된 수 리턴
    /// </summary>
    public static int RandomEvent(params int[] list)
    {
        return list[Random.Range(0,list.Length)];
    }
}

