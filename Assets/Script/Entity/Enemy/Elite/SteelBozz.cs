using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelBozz : Enemy {

    protected override void Think()
    {
        if(TileUtils.AI_SquareFind(currentTile,1))
        {
            currentActionList = attackList;
        }else if(currentHp<fullHp)
        {
            currentActionList = suicideList;
        }else
        {
            currentActionList = justDeleay;
        }
    }

    List<Action> attackList;
    List<Action> suicideList;
    List<Action> justDeleay;

    protected override void SetActionLists()
    {
        DelayList = null;
        justDeleay = new List<Action>() { new Action(DelayAction) };
        attackList = new List<Action>()
        { new Action(RangeOn),new  Action(Attack),new Action(Attack),new Action(Suicide)};
        suicideList = new List<Action>()
        {
            new Action(SelfDestruct),new Action(Suicide)
        };
    }


    IEnumerator RangeOn()
    {
        enemyUI.ActionImageOn();
        yield return null;
    }

    bool isSecond = false;
    IEnumerator Attack()
    {
        if(isSecond)
        {
            enemyUI.ActionImageOff();
        }
        SoundDelegate.instance.PlayEffectSound(EffectSound.SFX5, transform.position);

        List<Arch.Tile> tiles = TileUtils.EmptySquareRange(currentTile, 2);
        for (int i = 0; i < tiles.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.FIREBREATH, tiles[i]);
        }

        if (TileUtils.AI_Find(tiles))
        {
            PlayerControl.player.GetDamage(atk);
        }
        isSecond = true;
        yield return StartCoroutine(AnimationRoutine(0));
    }



    IEnumerator SelfDestruct()
    {
        List<Arch.Tile> tiles = TileUtils.EmptySquareRange(currentTile, 2);
        for (int i = 0; i < tiles.Count; i++)
        {
            ArchLoader.instance.MadeEffect(EnemyEffect.EXPLOSIONB, tiles[i]);
        }

        if (TileUtils.AI_Find(tiles))
        {
            PlayerControl.player.GetDamage(atk);
        }
        SoundDelegate.instance.PlayEffectSound(EffectSound.SFX4, transform.position);

        yield return StartCoroutine(AnimationRoutine(0));
    }

    IEnumerator Suicide()
    {
        DestroyThis();
        yield return null;
    }

    IEnumerator DelayAction()
    {
        yield return null;
    }
}
