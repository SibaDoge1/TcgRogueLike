using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xynus : Enemy {

    protected override void Think()
    {
        if (TileUtils.AI_Find(GetEnvironment()))
        {
            currentActionList = environmentAttack;
        } else if (TileUtils.AI_SquareFind(currentTile,1))
        {
            currentActionList = meeleAttack;
        }
        else
        {
            currentActionList = moveList;
        }
    }

    int monsterSpawnCool;
    int frontTurnHp;
    List<Action> environmentAttack;
    List<Action> meeleAttack;
    List<Action> moveList;

    protected override void SetActionLists()
    {
        DelayList = null;
        moveList = new List<Action>()
        { new Action(Move) ,new Action(Delay),new Action(Delay)  };

        environmentAttack = new List<Action>()
        {
            new Action(RangeOn),new Action(EnvironAttack),new Action(SpawnOrDelay)
        };

        meeleAttack = new List<Action>()
        {
            new Action(MeeleAttack),new Action(Delay),new Action(Delay)
        };
    }

    List<Arch.Tile> environ;
    IEnumerator RangeOn()
    {
        environ = GetEnvironment();
        for (int i = 0; i < environ.Count; i++)
        {
            //TODO : MAKE EFFECT
            rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, environ[i]));
        }

        yield return null;
    }

    IEnumerator EnvironAttack()
    {
        ClearRangeList();

        if(TileUtils.AI_Find(environ))
        {
            PlayerControl.player.GetDamage(atk);
        }

        //yield return StartCoroutine(AnimationRoutine(0));
        yield return null;
    }

    IEnumerator SpawnOrDelay()
    {
        if(currentRoom.enemyList.Count<=3) //소환
        {
            List<Arch.Tile> targets = TileUtils.DiagonalRange(currentTile, 1);
            
            for(int i=0; i<targets.Count; i++)
            {
                if(targets[i].IsStandAble(this))
                {
                    Entity e = ArchLoader.instance.GetEntity(4007);
                    e.Init(4007);
                    e.SetRoom(currentRoom, targets[i]);
                }
            }

            //yield return StartCoroutine(AnimationRoutine(0));
            yield return null;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator Move()
    {
        MoveTo(PathFinding.GenerateAllDirectionPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }

    IEnumerator MeeleAttack()
    {
        
        if(TileUtils.AI_SquareFind(currentTile,1))
        {
            PlayerControl.player.GetDamage(atk);
        }

        //yield return StartCoroutine(AnimationRoutine(0));
        yield return null;
    }
    IEnumerator Delay()
    {
        yield return null;
    }
    /// <summary>
    /// 포위 범위
    /// </summary>
    /// <returns></returns>
    public List<Arch.Tile> GetEnvironment()
    {
        List<Arch.Tile> targetTiles = TileUtils.EmptySquareRange(currentTile,3);
        int x = (int)pos.x; int y = (int)pos.y;
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(x-2,y-2)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(x + 2, y - 2)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(x + 2, y + 2)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(x - 2, y + 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;

    }

}
