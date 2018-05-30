using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDatabase
{
    public static readonly string[] enemyPaths =
    {
        "enemy_fastgoblin",
        "enemy_slowgoblin",
        "enemy_bossgoblin"
    };
    public static List<int> pool1 = new List<int>{0,1,1,1};
    public static List<int> bossPool = new List<int>{0,0,0,1,2};
}
