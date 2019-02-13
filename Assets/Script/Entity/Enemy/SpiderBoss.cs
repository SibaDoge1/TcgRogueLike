using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
public class SpiderBoss : Enemy {

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    #region AI

    List<Action> MeleeAttackList;
    List<Action> WebAttackList;
    List<Action> SpawnList;
    protected override void Think()
    {
        if (TileUtils.AI_CrossFind(currentTile, 2))
        {
            currentActionList = MeleeAttackList;
        }else if(currentRoom.enemyList.Count>=2)
        {
            currentActionList = WebAttackList;
        }
        else
        {
            currentActionList = SpawnList;
        }
    }

    protected override void SetActionLists()
    {
        DelayList = null;
        MeleeAttackList = new List<Action>()
        { new Action(AtkReady),new Action(Attack),new Action(RunAway)};
        WebAttackList = new List<Action>()
        { new Action(WebReady), new Action(WebAttack)};
        SpawnList = new List<Action>()
        { new Action(Delay),new Action(SpawnSpider)};
    }

    IEnumerator AtkReady()
    {
        List<Arch.Tile> tiles = TileUtils.CrossRange(currentTile, 2);
        for (int i = 0; i < tiles.Count; i++)
        {
            rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
        }
        yield return null;
    }
    IEnumerator Delay()
    {
        yield return null;
    }
    IEnumerator Attack()
    {
        if(TileUtils.AI_CrossFind(currentTile,2))
        {
            PlayerControl.player.GetDamage(atk);
        }
        ClearRangeList();
        yield return null;
    }
    IEnumerator RunAway()
    {
        List<Tile> tiles = currentRoom.GetTileToList();
        List<Tile> removeTiles = TileUtils.SquareRange(PlayerControl.player.currentTile,2);//지워야 하는 타일들
        removeTiles.Add(PlayerControl.player.currentTile);
            
        for(int i=0; i<removeTiles.Count;i++)
        {
            tiles.Remove(removeTiles[i]);
        }
        
        while(!Teleport(tiles[Random.Range(0, tiles.Count)].pos)) { }
        yield return null;
    }
    Tile aimedTile;
    IEnumerator WebReady()
    {
        aimedTile = PlayerControl.player.currentTile;
        rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, PlayerControl.player.currentTile));
        yield return null;
    }
    IEnumerator WebAttack()
    {
        if(PlayerControl.player.currentTile == aimedTile)
        {
           // PlayerControl.instance.SetDebuff(new Debuff_Move());
            PlayerControl.player.GetDamage(atk * 0.5f);
        }
        ClearRangeList();
        yield return null;
    }
    IEnumerator SpawnSpider()
    {
        List<Arch.Tile> tiles = TileUtils.SquareRange(currentTile, 1);
        for(int i=0; i<tiles.Count;i++)
        {
            if(tiles[i].OnTileObj ==null)
            {
                Entity e = ArchLoader.instance.GetEntity(4005);
                e.SetRoom(currentRoom, tiles[i]);
            }
        }
        yield return null;
    }

    #endregion
    protected override void OnDieCallback()
    {
        GameManager.instance.GameWin();
        base.OnDieCallback();
    }
}
