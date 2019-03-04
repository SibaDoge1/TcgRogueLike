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
        { new Action(Move) ,new Action(Delay),new Action(Delay1)  };

        environmentAttack = new List<Action>()
        {
            new Action(RangeOn),new Action(EnvironAttack),new Action(SpawnOrDelay)
        };

        meeleAttack = new List<Action>()
        {
            new Action(MeeleAttack),new Action(Delay),new Action(Delay1)
        };
    }

    List<Arch.Tile> environ;
    IEnumerator RangeOn()
    {
        environ = GetEnvironment();
        for (int i = 0; i < environ.Count; i++)
        {
            //TODO : MAKE EFFECT
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, environ[i]));
        }

        yield return null;
    }

    IEnumerator EnvironAttack()
    {
        enemyUI.ActionImageOff();
        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX5, transform.position);

        ClearRangeList();

        for(int i=0; i<environ.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.SPACE, environ[i]);
        }
        if (TileUtils.AI_Find(environ))
        {
            PlayerControl.player.GetDamage(atk);
        }

        //yield return StartCoroutine(AnimationRoutine(0));
        yield return null;
    }

    IEnumerator SpawnOrDelay()
    {
        enemyUI.ActionImageOn();

        if (currentRoom.enemyList.Count<=3) //소환
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
        enemyUI.ActionImageOff();
        MoveTo(PathFinding.GenerateAllDirectionPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }

    IEnumerator MeeleAttack()
    {
        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX5, transform.position);
        enemyUI.ActionImageOff();
        List<Arch.Tile> tiles = TileUtils.SquareRange(currentTile, 1);
        for (int i = 0; i < tiles.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.SPACE, tiles[i]);
        }

        if (TileUtils.AI_SquareFind(currentTile,1))
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
    IEnumerator Delay1()
    {
        enemyUI.ActionImageOn();
        yield return null;
    }
    /// <summary>
    /// 포위 범위
    /// </summary>
    /// <returns></returns>
    public List<Arch.Tile> GetEnvironment()
    {
        List<Arch.Tile> targetTiles = new List<Arch.Tile>() ;
   
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1,1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1, 2)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1, 3)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1, 4)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1, 5)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1, 6)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(2, 1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(3, 1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(4, 1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(5, 1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(6, 1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 1)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 2)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 3)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 4)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 5)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 6)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(7, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(1, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(2, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(3, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(4, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(5, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(6, 7)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(2, 6)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(6, 6)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(6, 2)));
        targetTiles.Add(currentRoom.GetTile(new Vector2Int(2, 2)));

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

        base.OnDieCallback();
    }
}
