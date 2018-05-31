using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public enum Shape
{
    NONE,
    CENTER,
    RANDOM
}

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
        room.OpenDoors();
    }

}
public class BattleRoom : RoomSeed
{
    List<int> enemyList;
    int num;
    Shape shape;

    public BattleRoom(Room room, List<int> _enemyList,int _num) :base(room)
    {       
        enemyList = _enemyList;
        num = _num;
        shape = Shape.NONE;

        MakeObstacle();
        MakeEnemy();
    }
    public BattleRoom(Room room,Shape _shape ,List<int> _enemyList, int _num) : base(room)
    {
        enemyList = _enemyList;
        num = _num;
        shape = _shape;

        MakeObstacle();
        MakeEnemy();
    }
    void MakeObstacle()
    {
        Vector2Int center;
        switch (shape)
        {
            case Shape.CENTER:
                center = new Vector2Int(room.size.x / 2, room.size.y / 2);
                List<Tile> centerTiles = TileUtils.SquareRange(room, room.GetTile(center), 1);
                centerTiles.Add(room.GetTile(center));
                foreach(Tile v in centerTiles)
                {
                    if(v.OnTileObj ==null)
                    {
                        OnTileObject tempWall = InstantiateDelegate.Instantiate(Resources.Load("Structure/wall") as GameObject, room.transform).GetComponent<OnTileObject>();
                        tempWall.SetRoom(room,v.pos);
                    }
                }
                break;
            case Shape.RANDOM:


                break;
            default:
                break;
        }

    }
    void MakeEnemy()
    {
        int count = 0;
        List<Tile> tiles = room.GetSpawnableTiles();
        Tile tempTile;

        while (num > count)
        {
            int ranNum = Random.Range(count + 1, tiles.Count);
            tempTile = tiles[count];
            tiles[count] = tiles[ranNum];
            tiles[ranNum] = tempTile;

            int ran = enemyList[Random.Range(0, enemyList.Count - 1)];
            Enemy tempEnemy = InstantiateDelegate.Instantiate(Resources.Load("Enemy/" + EnemyDatabase.enemyPaths[ran]) as GameObject).GetComponent<Enemy>();
            tempEnemy.SetRoom(room, tiles[count].pos);
            count++;
        }
    }
}
public class BossRoom : RoomSeed
{
    List<int> enemyList;
    int num;
    public BossRoom(Room room, List<int> _enemyList,int _num) :base(room)
    {
        enemyList = _enemyList;
        num = _num;
        MakeObstacle();
        MakeEnemy();
    }
    void MakeObstacle()
    {

    }
    void MakeEnemy()
    {
        int count=0;
        List<Tile> tiles = room.GetSpawnableTiles();
        Tile tempTile;

        while (num>count)
        {
            int ranNum = Random.Range(count+1, tiles.Count);
           tempTile = tiles[count];
           tiles[count] = tiles[ranNum];
           tiles[ranNum] = tempTile;

            int ran = enemyList[Random.Range(0, enemyList.Count-1)];
            Enemy tempEnemy = InstantiateDelegate.Instantiate(Resources.Load("Enemy/" + EnemyDatabase.enemyPaths[ran]) as GameObject).GetComponent<Enemy>();
            tempEnemy.SetRoom(room, tiles[count].pos);
            count++;
        }

        tempTile = tiles[Random.Range(count + 1, tiles.Count)];
        int bossNum = enemyList[enemyList.Count - 1];
        Enemy boss = InstantiateDelegate.Instantiate(Resources.Load("Enemy/" + EnemyDatabase.enemyPaths[bossNum]) as GameObject).GetComponent<Enemy>();
        boss.SetRoom(room, tempTile.pos);
    }
}
