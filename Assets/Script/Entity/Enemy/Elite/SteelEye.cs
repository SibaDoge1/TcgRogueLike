using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelEye : Enemy {

    protected override void Think()
    {
        currentActionList = attackList;
    }

    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = new List<Action>()
        { new Action(DelayAction) };
        attackList = new List<Action>()
        { new Action(RangeOnAction),new  Action(AttackThenRangeOffAction)};
    }

    Vector2Int dir;
    IEnumerator RangeOnAction()
    {
        Arch.Tile target = PathFinding.GenerateAllDirectionPath(this, PlayerControl.player.currentTile)[0];
        dir = target.pos - pos;

        List<Arch.Tile> tiles = GetTileList(dir);

        for (int i = 0; i < tiles.Count; i++)
        {
            rangeList.Add(ArchLoader.instance.MadeEffect(RangeEffectType.ENEMY, tiles[i]));
        }

        enemyUI.ActionImageOn();
        SpriteRender.sprite = actionSprites[0];
        yield return null;
    }


    IEnumerator AttackThenRangeOffAction()
    {
        List<Arch.Tile> tiles = GetTileList(dir);

        SoundDelegate.instance.PlayEffectSound(SoundEffect.SFX9, transform.position);
        for(int i=0; i<tiles.Count;i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.HITBLUE, tiles[i]);
        }
        for(int i=0; i<3; i++)
        {
           if(!MoveTo(tiles[i].pos))
            {
                if(tiles[i].OnTileObj != null && tiles[i].OnTileObj is Player)
                {
                    PlayerControl.player.GetDamage(atk);
                }
                break;
            }
        }

        ClearRangeList();
        enemyUI.ActionImageOff();

        yield return StartCoroutine(AnimationRoutine(1));
    }


    IEnumerator DelayAction()
    {
        yield return null;
    }

    private List<Arch.Tile> GetTileList(Vector2Int dir)
    {
        List<Arch.Tile> tiles = new List<Arch.Tile>();

        Vector2Int current = pos;
        for (int i = 0; i < 3; i++)
        {
            current += dir;
            tiles.Add(currentRoom.GetTile(current));
        }

        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            if (tiles[i] == null)
            {
                tiles.RemoveAt(i);
            }
        }

        return tiles;
    }
}
