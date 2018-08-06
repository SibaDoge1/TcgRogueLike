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
        /// 가변인자로 여러개 받고 거기서 당첨된거 넘버로 변환
        /// </summary>
        /// <returns></returns>
        public static int RandomEvent(params float[] list)
      {
        float total=0;
        float adds = 0;
        for (int i=0; i<list.Length;i++)
        {
            total += list[i];
        }
        float num = Random.Range(1, total);
        for (int i=0; i<list.Length;i++)
        {
            adds += list[i];
            if(num <= adds)
            {
                return i+1;
            }
        }
        return list.Length;
    }
    public static int RandomEvent(params int[] list)
      {
        int total = 0;
        int adds = 0;
        for (int i = 0; i < list.Length; i++)
        {
            total += list[i];
        }
        int num = Random.Range(1, total+1);
        for (int i = 0; i < list.Length; i++)
        {
            adds += list[i];
            if (num <= adds)
            {
                return i+1;
            }
        }
        return list.Length;
    }
}

