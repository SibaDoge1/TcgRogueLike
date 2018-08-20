using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
public class SpiderBoss : Enemy {

    GameObject spider;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        spider = ResourceLoader.instance.LoadEntity(4005);
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
        PlayAnimation("AttackReady");
        yield return null;
    }
    IEnumerator Delay()
    {
        PlayAnimation("Idle");
        yield return null;
    }
    IEnumerator Attack()
    {
        if(TileUtils.AI_CrossFind(currentTile,2))
        {
            PlayerControl.Player.GetDamage(atk);
        }
        PlayAnimation("Attack");
        ClearRangeList();
        yield return null;
    }
    IEnumerator RunAway()
    {
        PlayAnimation("Teleport");
        List<Tile> tiles = currentRoom.GetTileToList();
        List<Tile> removeTiles = TileUtils.SquareRange(PlayerControl.Player.currentTile,2);//지워야 하는 타일들
        removeTiles.Add(PlayerControl.Player.currentTile);
            
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
        aimedTile = PlayerControl.Player.currentTile;
        rangeList.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.ENEMY, PlayerControl.Player.currentTile));
        PlayAnimation("WebReady");
        yield return null;
    }
    IEnumerator WebAttack()
    {
        if(PlayerControl.Player.currentTile == aimedTile)
        {
            PlayerControl.instance.SetDebuff(new Debuff_Move());
            PlayerControl.Player.GetDamage(atk * 0.5f);
        }
        PlayAnimation("Web");
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
                Entity e = InstantiateDelegate.Instantiate(spider).GetComponent<Entity>();
                e.SetRoom(currentRoom, tiles[i]);
            }
        }
        PlayAnimation("Spawn");
        yield return null;
    }

    #endregion
    protected override void OnDieCallback()
    {
        GameManager.instance.GameWin();
        base.OnDieCallback();
    }
}
