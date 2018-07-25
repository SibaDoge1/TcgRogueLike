using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class Enemy : Character {

    protected Animator enemyAnimator;
    protected List<Tile> rangeList = new List<Tile>();

    public Attribute Atr;

    protected virtual void Start()
    {
        currentRoom.enemyList.Add(this);
		enemyAnimator = transform.Find ("Renderer").GetComponent<Animator> ();
        characterUI.SetAtt(Atr);
        SetActionLists();
    }
    protected override void OnDieCallback ()
    {
        ClearRangeList();
		currentRoom.OnEnemyDead (this);
		base.OnDieCallback ();
	}

    protected override void OnEndTurn()
    {
        base.OnEndTurn();
        EnemyControl.instance.EnemyEndCallBack();
    }


    protected virtual void PlayAttackMotion()
    {
        enemyAnimator.Play("Attack");
    }

    protected virtual void ClearRangeList()
    {
        if (rangeList != null)
        {
            for (int i = 0; i < rangeList.Count; i++)
            {
                EffectDelegate.instance.DeleteRange(rangeList[i]);
            }
            rangeList = new List<Tile>();
        }
    }






    #region AI ActRoutine 
    protected enum State
    {
        DELAY, THINK, ACT
    }
    protected State currentState;
    private int delayCount; private int actCount;

    protected List<Action> DelayList;
    protected List<Action> currentActionList;


    public bool AIRoutine()
    {
        if (currentState == State.DELAY)
        {
            if(DelayList != null)
            {
                DelayList[delayCount].Do();
                delayCount++;
                if (delayCount >= DelayList.Count)
                {
                    delayCount = 0;
                    currentState = State.THINK;
                }
                OnEndTurn();
                return false;
            }
            else
            {
                currentState = State.THINK;
            }
        }

        if (currentState == State.THINK)
        {
            Think(); //생각해서 ActionList에 행동해야할 List들을 대입
            currentState = State.ACT;
            return AIRoutine();
        }
        else if (currentState == State.ACT)
        {
            currentActionList[actCount].Do();
            actCount++;
            if (actCount >= currentActionList.Count)
            {
                actCount = 0;
                currentState = State.DELAY;
            }
        }
        OnEndTurn();
        return true;
    }

    protected abstract void Think(); //THINK에서는 조건에따라 STATE 상태를 바꿉니다.
    protected abstract void SetActionLists();
    protected abstract class Action
    {
        protected Enemy enemy;
        public Action(Enemy e)
        {
            enemy = e;
        }
        public abstract void Do();
    }
    protected class SimpleDelayAction : Action
    {
        public SimpleDelayAction(Enemy e) : base(e){}
        public override void Do(){}
    }
    /// <summary>
    /// 플레이어에게 한칸 이동
    /// </summary>
    protected class SimpleMoveToWard : Action
    {
        public SimpleMoveToWard(Enemy e) : base(e){ }
        public override void Do() { enemy.MoveTo(PathFinding.GeneratePath(enemy, PlayerControl.instance.PlayerObject)[0].pos); }
    }
    /// <summary>
    /// 플레이어로부터 한칸 도망
    /// </summary>
    protected class SimpleRunAway : Action
    {
        Vector2Int dir;
        public SimpleRunAway(Enemy e) : base(e){ }
        public override void Do()
        {
            if(dir == Vector2Int.zero)
            {
                List<Tile> nearTiles = TileUtils.CircleRange(enemy.currentTile, 1);
                for (int i = 0; i < nearTiles.Count; i++)
                {
                    if (nearTiles[i].OnTileObj == null)
                    {
                        dir = TileUtils.GetDir(enemy.currentTile, nearTiles[i]);
                        break;
                    }
                }
            }


            enemy.MoveTo(enemy.currentTile.pos + dir);
        }
    }
    protected class SimpleAttack : Action
    {
        public SimpleAttack(Enemy e) : base(e){ }
        public override void Do()
        {
            PlayerControl.instance.PlayerObject.GetDamage(enemy.atk);
            enemy.PlayAttackMotion();
            EffectDelegate.instance.MadeEffect(CardEffectType.Hit, PlayerControl.instance.PlayerObject.currentTile);
        }
    }
    
    #endregion
}
