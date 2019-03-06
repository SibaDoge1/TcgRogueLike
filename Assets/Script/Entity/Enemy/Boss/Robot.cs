using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Enemy
{
    protected override void Think()
    {
        if(currentRoom.enemyList.Count<2)
        {
            if(firstSpawn)
            {
                currentActionList = firstSpawnList;
                firstSpawn = false;
            }else
            {
                currentActionList = spawnList;
            }
        }
        else if(TileUtils.AI_CrossFind(currentTile,4))
        {
            currentActionList = attackList;
        }else
        {
            currentActionList = justDeleay;
        }
    }

    bool firstSpawn = true;
    List<Action> spawnList;
    List<Action> attackList;
    List<Action> justDeleay;
    List<Action> firstSpawnList;

    protected override void SetActionLists()
    {
        DelayList = null;
        justDeleay = new List<Action>()
        {
            new Action(Delay)
        };
        firstSpawnList = new List<Action>()
        {
            new Action(Spawn)
        };
        spawnList = new List<Action>()
        {
            new Action(SelfDestruct), new Action(Spawn)
        };
        attackList = new List<Action>()
        {
            new Action(RangeOn), new Action(Attack)
        };
    }

    IEnumerator Delay()
    {
        yield return null;
    }
    IEnumerator SelfDestruct()
    {
        GetDamage(FullHp/4);
        yield return null;
    }
    int turn = 0;
    IEnumerator Spawn()
    {
        switch(turn)
        {
            case 0:
                SpawnEnemy(4005, currentRoom.GetTile(new Vector2Int(4, 5)));
                SpawnEnemy(4005, currentRoom.GetTile(new Vector2Int(5, 5)));
                SpawnEnemy(4005, currentRoom.GetTile(new Vector2Int(6, 5)));
                SpawnEnemy(4001, currentRoom.GetTile(new Vector2Int(3, 6)));
                SpawnEnemy(4001, currentRoom.GetTile(new Vector2Int(7, 6)));
                SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(4, 6)));
                SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(6, 6)));
                SpawnEnemy(4003, currentRoom.GetTile(new Vector2Int(5, 7)));
                SpawnEnemy(4003, currentRoom.GetTile(new Vector2Int(4, 7)));
                SpawnEnemy(4003, currentRoom.GetTile(new Vector2Int(6, 7)));

                break;
            case 1:
                SpawnEnemy(4002, currentRoom.GetTile(new Vector2Int(2, 5)));
                SpawnEnemy(4002, currentRoom.GetTile(new Vector2Int(2, 9)));
                SpawnEnemy(4002, currentRoom.GetTile(new Vector2Int(6, 5)));
                SpawnEnemy(4002, currentRoom.GetTile(new Vector2Int(6, 9)));
                SpawnEnemy(4006, currentRoom.GetTile(new Vector2Int(4, 7)));
                SpawnEnemy(4006, currentRoom.GetTile(new Vector2Int(6, 7)));
                SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(3, 6)));
                SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(7, 6)));
                SpawnEnemy(4005, currentRoom.GetTile(new Vector2Int(5, 5)));
                SpawnEnemy(4005, currentRoom.GetTile(new Vector2Int(5, 7)));
                SpawnEnemy(4003, currentRoom.GetTile(new Vector2Int(5, 8)));

                break;
            case 2:
                SpawnEnemy(4006, currentRoom.GetTile(new Vector2Int(2, 6)));
                SpawnEnemy(4006, currentRoom.GetTile(new Vector2Int(8, 6)));
                SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(3, 6)));
                SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(5, 8)));
                SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(7, 6)));
                SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(5, 4)));
                SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(2, 3)));
                SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(2, 9)));
                SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(8, 9)));
                SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(8, 3)));
                SpawnEnemy(4002, currentRoom.GetTile(new Vector2Int(4, 7)));
                SpawnEnemy(4007, currentRoom.GetTile(new Vector2Int(5, 9)));

                break;
        }
        turn++;
        yield return StartCoroutine(AnimationRoutine(0));
    }
    IEnumerator RangeOn()
    {
        List<Arch.Tile> tiles = TileUtils.CrossRange(currentTile, 4);
        for (int i = 0; i < tiles.Count; i++)
        {
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
        }

        yield return null;
    }
    IEnumerator Attack()
    {
        SoundDelegate.instance.PlayEffectSound(SoundEffect.ATTACK, transform.position);

        ClearRangeList();
        List<Arch.Tile> targets = TileUtils.CrossRange(currentTile, 4);
        for (int i = 0; i < targets.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.ENEMYEXPLOSIONC, targets[i]);
        }
        if (TileUtils.AI_Find(targets))
        {
            PlayerControl.player.GetDamage(atk);
        }

        yield return StartCoroutine(AnimationRoutine(0));
    }

    private void SpawnEnemy(int num, Arch.Tile tile)
    {
        if (tile.IsStandAble(this))
        {
            Entity e = ArchLoader.instance.GetEntity(num);
            e.Init((short)num);
            e.SetRoom(currentRoom, tile);
        }
    }

    protected override void OnDieCallback()
    {
        for(int i=currentRoom.enemyList.Count-1; i>=0;i--)
        {
            if(currentRoom.enemyList[i] != this)
            {
                currentRoom.enemyList[i].DestroyThis();
            }
        }

        if(GameManager.instance.EndingConditions["Pablus"] && GameManager.instance.EndingConditions["Xynus"] )
        {
            OffTile_Floor stair = ArchLoader.instance.GetOffTile(95) as OffTile_Floor;
            stair.Init(95);
            currentTile.offTile = stair;
            stair.targetFloor = 5;
        }else
        {
            //BAD END
        }


        base.OnDieCallback();
    }
}
