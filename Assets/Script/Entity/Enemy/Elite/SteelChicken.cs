using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelChicken : Enemy {

    protected override void Think()
    {
        if (TileUtils.AI_SquareFind(currentTile, 1))
        {
            currentActionList = attackList;
        }
        else
        {
            currentActionList = moveList;
        }
    }
    List<Action> moveList;
    List<Action> attackList;
    protected override void SetActionLists()
    {
        DelayList = null;
        moveList = new List<Action>() { new Action(MoveToward) , new Action(DelayAction)};
        attackList = new List<Action>() { new Action(Attack) ,new Action(SimpleRunAway),
        new Action(SimpleRunAway),new Action(SimpleRunAway)};
    }

    IEnumerator MoveToward()
    {
        MoveTo(PathFinding.GenerateAllDirectionPath(this, PlayerControl.player.currentTile)[0].pos);
        yield return null;
    }
    IEnumerator SimpleRunAway()
    {
        Vector2Int dir = pos - PlayerControl.player.pos;//도망쳐야할 방향

        if(dir.x>0&&currentRoom.GetTile(pos+new Vector2Int(1,0)).IsStandAble(this))
        {
            MoveTo(pos + new Vector2Int(1, 0));
        }
        else if(dir.y > 0 && currentRoom.GetTile(pos + new Vector2Int(0, 1)).IsStandAble(this))
        {
            MoveTo(pos + new Vector2Int(0, 1));
        }
        else if(dir.x < 0 && currentRoom.GetTile(pos + new Vector2Int(-1, 0)).IsStandAble(this))
        {
            MoveTo(pos + new Vector2Int(-1,0));
        }
        else if(dir.y<0&& currentRoom.GetTile(pos + new Vector2Int(0, -1)).IsStandAble(this))
        {
            MoveTo(pos + new Vector2Int(0, -1));
        }

        yield return null;
    }

    IEnumerator DelayAction()
    {
        yield return null;
    }


    IEnumerator Attack()
    {
        PlayerControl.player.GetDamage(atk);
        PlayerControl.playerBuff.UpdateBuff(BUFF.MOVE);
        yield return StartCoroutine(AnimationRoutine(0));
    }

}
