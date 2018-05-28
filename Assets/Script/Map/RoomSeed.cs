using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public abstract class RoomSeed
{
    protected Room room;
    public RoomSeed(Room _room)
    {
        room = _room;
    }
}
public class StartRoom : RoomSeed
{
    public StartRoom(Room room):base(room)
    {
    }

}
public class BattleRoom : RoomSeed
{
    List<int> enemyList;
    int num;
    public BattleRoom(Room room, List<int> _enemyList,int _num) :base(room)
    {       
        enemyList = _enemyList;
        num = _num;
        MakeEnemy();
    }
    public void MakeEnemy()
    {
        while(num>0)
        {
            Vector2Int temp1 = new Vector2Int(Random.Range(2, room.size.x-3), Random.Range(2, room.size.y - 3));
            if (room.GetTile(temp1).OnTileObj != null)
                continue;

            int ran = Random.Range(0, enemyList[enemyList.Count-1]+1);
            Enemy temp = InstantiateDelegate.Instantiate(Resources.Load("Enemy/"+EnemyDatabase.enemyPaths[ran]) as GameObject).GetComponent<Enemy>();
            temp.SetRoom(room, temp1);
            num--;
        }
    }
}
public class BossRoom : RoomSeed
{
    List<int> enemyList;
    public BossRoom(Room room, List<int> _enemyList) :base(room)
    {
        enemyList = _enemyList;
        MakeEnemy();
    }
    public void MakeEnemy()
    {
        int count = 0;
        while (enemyList.Count>count)
        {
            Vector2Int temp1 = new Vector2Int(Random.Range(2, room.size.x - 4), Random.Range(2, room.size.y - 4));
            if (room.GetTile(temp1).OnTileObj != null)
                continue;

            Enemy temp = InstantiateDelegate.Instantiate(Resources.Load("Enemy/" + EnemyDatabase.enemyPaths[enemyList[count]]) as GameObject).GetComponent<Enemy>();
            temp.SetRoom(room, temp1);
            count++;
        }
    }
}
