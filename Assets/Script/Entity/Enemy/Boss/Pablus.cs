using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pablus : Enemy {

    protected override void Think()
    {
        if (currentRoom.enemyList.Count < 2)
        {
            currentActionList = spawnList;
        }
        else
        {
            if (currentHp < frontTurnHp)
            {
                currentActionList = blowAway;
            }
            else
            {
                currentActionList = justFireWall;
            }
        }

        frontTurnHp = currentHp;
    }


    int frontTurnHp;
    List<Action> spawnList;
    List<Action> justFireWall;
    List<Action> blowAway;

    protected override void SetActionLists()
    {
        DelayList = null;
        blowAway = new List<Action>() { new Action(RangeOn),new Action(BlowAway) };

        spawnList = new List<Action>()
        {
            new Action(Spawn)
        };

        justFireWall = new List<Action>()
        {
            new Action(FireWall)
        };
    }


    IEnumerator RangeOn()
    {
        List<Arch.Tile> targets = GetBlowAwayRange();
        for (int i = 0; i < targets.Count; i++)
        {
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, targets[i]));
        }

        yield return null;
    }
    IEnumerator BlowAway()
    {
        ClearRangeList();
        List<Arch.Tile> targets = GetBlowAwayRange();

        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX4, transform.position);

        for (int i = 0; i < targets.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.EXPLOSIONB, targets[i]);
        }

        if (TileUtils.AI_Find(targets))
        {
            PlayerControl.player.GetDamage(atk);

        }
        while (!PlayerControl.player.Teleport((new Vector2Int(Random.Range(2, 4), Random.Range(1, 6)))))
        { }

        yield return StartCoroutine(AnimationRoutine(0));
    }

    IEnumerator FireWall()
    {
        List<Arch.Tile> targets = GetFireWallRange();
        if(TileUtils.AI_Find(targets))
        {
            PlayerControl.player.GetDamage(atk);
        }

        for(int i=0; i<targets.Count;i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.FIREBREATH,targets[i]);
        }
        yield return null;
    }

  /*  List<Arch.Tile> move;
    IEnumerator SelectFireWall()
    {
        move = GetRandomFireRange();
        if(TileUtils.AI_Find(move))
        {
            PlayerControl.player.GetDamage(atk);
        }

        for (int i = 0; i < move.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.FIREBREATH, move[i]);
        }
        yield return StartCoroutine(AnimationRoutine(0));
    }
    IEnumerator FireWallMove()
    {
        move = GetFireMoveRange(move);
        for (int i = 0; i < move.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.FIREBREATH, move[i]);
        }
        if (TileUtils.AI_Find(move))
        {
            PlayerControl.player.GetDamage(atk);
        }

        yield return null;
    }*/


    IEnumerator Spawn()
    {
        SpawnEnemy(4001, currentRoom.GetTile(new Vector2Int(9, 5)));
        SpawnEnemy(4008, currentRoom.GetTile(new Vector2Int(9, 3)));
        SpawnEnemy(4001, currentRoom.GetTile(new Vector2Int(9, 1)));
        SpawnEnemy((Random.Range(4005, 4008)), currentRoom.GetTile(new Vector2Int(7, 2)));
        SpawnEnemy((Random.Range(4005, 4008)), currentRoom.GetTile(new Vector2Int(7, 4)));
        SpawnEnemy(4004, currentRoom.GetTile(new Vector2Int(6, 3)));


        yield return StartCoroutine(AnimationRoutine(0));
    }




    private List<Arch.Tile> GetFireWallRange()
    {
        List<Arch.Tile> targetTiles = new List<Arch.Tile>();
        int x = (int)pos.x; int y = (int)pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y+2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y +1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y - 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
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

  /*  private List<Arch.Tile> GetRandomFireRange()
    {
        int ran = Random.Range(0, 3);
        List<Arch.Tile> targetTiles = new List<Arch.Tile>();
        int x = (int)pos.x; int y = (int)pos.y;

        switch (ran)
        {
            case 0:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-2, y + 2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y + 2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y )));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y )));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y -1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y -1)));
                break;
            case 1:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y )));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y )));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y-1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y-1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y - 2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));
                break;
            case 2:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y + 2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y-2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y-2)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y - 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 1)));
                break;
        }

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    private List<Arch.Tile> GetFireMoveRange(List<Arch.Tile> tiles)
    {
        List<Arch.Tile> targetTiles = new List<Arch.Tile>();
        for(int i=0; i<tiles.Count;i++)
        {
            targetTiles.Add(currentRoom.GetTile(tiles[i].pos + Vector2Int.left));
        }
        return targetTiles;
    }*/

    private List<Arch.Tile> GetBlowAwayRange()
    {
        List<Arch.Tile> targetTiles = new List<Arch.Tile>();
        int x = (int)pos.x; int y = (int)pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y - 1)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    protected override void OnDieCallback()
    {
        for (int i = currentRoom.enemyList.Count - 1; i >= 0; i--)
        {
            if (currentRoom.enemyList[i] != this)
            {
                currentRoom.enemyList[i].DestroyThis();
            }
        }
        GameManager.instance.EndingConditions["Pablus"] = true;
        base.OnDieCallback();
    }

}
