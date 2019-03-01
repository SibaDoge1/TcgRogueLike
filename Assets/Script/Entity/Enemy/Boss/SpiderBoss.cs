using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
public class SpiderBoss : Enemy {

    #region AI

    List<Action> MeleeAttackList;
    List<Action> WebAttackList;
    List<Action> SpawnList;
    List<Action> changePhaseList;
    List<Action> SpawnEliteList;
    List<Action> WideAttackList;
    List<Action> MoveList;

    bool isSecondPhase = false;
    int frontHp;

    protected override void Think()
    {
        if(isSecondPhase)
        {
            if(currentHp<frontHp)
            {
                currentActionList = SpawnEliteList;
            }else if(TileUtils.AI_CircleFind(currentTile, 2))
            {
                currentActionList = WideAttackList;
            }else
            {
                currentActionList = MoveList;
            }
        }else
        {
            if (currentHp<=12)
            {
                currentActionList = changePhaseList;
            }else
            {
                if (TileUtils.AI_CrossFind(currentTile, 2))
                {
                    currentActionList = MeleeAttackList;
                }
                else if (currentRoom.enemyList.Count >= 2)
                {
                    currentActionList = WebAttackList;
                }
                else
                {
                    currentActionList = SpawnList;
                }
            }
        }

        frontHp = currentHp;
    }

    protected override void SetActionLists()
    {
        DelayList = null;
        MeleeAttackList = new List<Action>()
        { new Action(AtkReady),new Action(Attack),new Action(RunAway)};
        WebAttackList = new List<Action>()
        { new Action(WebReady), new Action(WebAttack)};
        SpawnList = new List<Action>()
        { new Action(SpawnSpider),new Action(Delay)};

         changePhaseList = new List<Action>()
         {new Action(ChangePhase) };
        SpawnEliteList = new List<Action>()
         {new Action(SpawnElite) , new Action(Delay) } ;
         WideAttackList = new List<Action>()
         {new Action(WideAttackReady),new Action(WideAttack) } ;
         MoveList = new List<Action>()
         { new Action(MoveEightWay) , new Action(Delay) , new Action(Delay) , new Action(Delay)};
    }
    List<Arch.Tile> targetList;
    IEnumerator WideAttackReady()
    {
        targetList = TileUtils.CircleRange(currentTile, 2);
        for (int i = 0; i < targetList.Count; i++)
        {
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, targetList[i]));
        }
        yield return null;
    }
    IEnumerator WideAttack()
    {
        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX9, transform.position);

        ClearRangeList();
        for(int i=0; i<targetList.Count;i++)
        {
            ArchLoader.instance.MadeEffect(CardEffect.DARKSUN, targetList[i]);
        }
        if (TileUtils.AI_Find(targetList))
        {
            PlayerControl.player.GetDamage(atk);
        }
        yield return StartCoroutine(AnimationRoutine(2));
    }
    IEnumerator MoveEightWay()
    {
        MoveTo(PathFinding.GenerateAllDirectionPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }
    IEnumerator SpawnElite()
    {
        int x = pos.x; int y = pos.y;
        SpawnEnemy(Random.Range(4005,4009),currentRoom.GetTile(new Vector2Int(x+2,y)));
        SpawnEnemy(Random.Range(4005, 4009), currentRoom.GetTile(new Vector2Int(x-2, y)));
        SpawnEnemy(Random.Range(4005, 4009), currentRoom.GetTile(new Vector2Int(x, y+2)));
        SpawnEnemy(Random.Range(4005, 4009), currentRoom.GetTile(new Vector2Int(y, y-2)));
        yield return StartCoroutine(AnimationRoutine(2));
    }
    IEnumerator AtkReady()
    {
        List<Arch.Tile> tiles = TileUtils.CrossRange(currentTile, 2);
        for (int i = 0; i < tiles.Count; i++)
        {
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
        }
        yield return null;
    }
    IEnumerator Delay()
    {
        yield return null;
    }
    IEnumerator Attack()
    {
        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX9, transform.position);

        List<Tile> tiles = TileUtils.CrossRange(currentTile, 2);
        for(int i=0; i<tiles.Count;i++)
        {
            ArchLoader.instance.MadeEffect(CardEffect.DARKSUN, tiles[i]);
        }

        if (TileUtils.AI_Find(tiles))
        {
            PlayerControl.player.GetDamage(atk);
        }
        ClearRangeList();
        yield return StartCoroutine(AnimationRoutine(0));
    }
    IEnumerator RunAway()
    {
        List<Tile> tiles = new List<Tile>(currentRoom.GetTileToList());
        List<Tile> removeTiles = TileUtils.SquareRange(PlayerControl.player.currentTile,2);//지워야 하는 타일들
        removeTiles.Add(PlayerControl.player.currentTile);
            
        for(int i=0; i<removeTiles.Count;i++)
        {
            tiles.Remove(removeTiles[i]);
        }
        
        while(tiles.Count>1)
        {
            int rand = UnityEngine.Random.Range(0, tiles.Count);

            if (tiles[rand].IsStandAble(this))
            {
                MoveTo(tiles[rand].pos);
                break;
            }
            else
            {
                tiles.RemoveAt(rand);
            }
        }

        yield return null;
    }
    Tile aimedTile;
    IEnumerator WebReady()
    {
        aimedTile = PlayerControl.player.currentTile;
        rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, PlayerControl.player.currentTile));
        yield return null;
    }
    IEnumerator WebAttack()
    {
        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX9, transform.position);
        ArchLoader.instance.MadeEffect(CardEffect.DARKSUN, aimedTile);
        if (PlayerControl.player.currentTile == aimedTile)
        {
            PlayerControl.playerBuff.UpdateBuff(BUFF.MOVE, 3);
            PlayerControl.player.GetDamage(1);
        }
        ClearRangeList();
        yield return StartCoroutine(AnimationRoutine(0));
    }
    IEnumerator SpawnSpider()
    {
        SpawnEnemy(4012, TileUtils.SquareRange(currentTile, 1));
        yield return StartCoroutine(AnimationRoutine(0));
    }
    IEnumerator ChangePhase()
    {
        isSecondPhase = true;
        yield return StartCoroutine(AnimationRoutine(1));
    }


    #endregion
    protected override void OnDieCallback()
    {
        //TODO : 게임 승리 트리거
        base.OnDieCallback();
    }

    private void SpawnEnemy(int num, List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].IsStandAble(this))
            {
                Entity e = ArchLoader.instance.GetEntity(num);
                e.Init((short)num);
                e.SetRoom(currentRoom, tiles[i]);
            }
        }
    }

    private void SpawnEnemy(int num, Tile tile)
    {
        if (tile.IsStandAble(this))
        {
            Entity e = ArchLoader.instance.GetEntity(num);
            e.Init((short)num);
            e.SetRoom(currentRoom, tile);
        }
    }

}
