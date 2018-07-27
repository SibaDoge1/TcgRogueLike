using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;


public abstract class Enemy : Character {
    protected delegate IEnumerator Action();
    protected Animator enemyAnimator;
    protected List<GameObject> rangeList = new List<GameObject>();

    public Attribute Atr;

    protected virtual void Start()
    {
		enemyAnimator = transform.Find ("Renderer").GetComponent<Animator> ();
        characterUI.SetAtt(Atr);
        SetActionLists();
    }
    protected override void OnDieCallback ()
    {
        ClearRangeList();
		currentRoom.OnEnemyDead (this);
        if(!InputModule.IsPlayerTurn)
        OnEndTurn();
		base.OnDieCallback ();
	}
    public override void SetRoom(Room room, Tile _pos)
    {
        base.SetRoom(room, _pos);
        currentRoom.enemyList.Add(this);
    }
    public override void SetRoom(Room room, Vector2Int _pos)
    {
        base.SetRoom(room, _pos);
        currentRoom.enemyList.Add(this);
    }
    protected override void OnEndTurn()
    {
        base.OnEndTurn();
        EnemyControl.instance.EnemyEndCallBack();
    }


    protected virtual void PlayAnimation(string s)
    {
        enemyAnimator.Play(s);
    }

    protected virtual void ClearRangeList()
    {
        if (rangeList != null)
        {
            EffectDelegate.instance.DestroyEffect(rangeList);
            rangeList = new List<GameObject>();
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
    

    public IEnumerator AIRoutine()
    {
        if (currentState == State.DELAY)
        {
            if(DelayList != null)
            {
                yield return StartCoroutine(DelayList[delayCount]());
                delayCount++;
                if (delayCount >= DelayList.Count)
                {
                    delayCount = 0;
                    OnEndTurn();
                    currentState = State.THINK;
                    yield break;
                } 
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
        }
        if (currentState == State.ACT)
        {
            yield return StartCoroutine(currentActionList[actCount]());
            actCount++;
            if (actCount >= currentActionList.Count)
            {
                actCount = 0;
                currentState = State.DELAY;
            }
        }
        OnEndTurn();
    }

    protected abstract void Think(); //THINK에서는 조건에따라 STATE 상태를 바꿉니다.
    protected abstract void SetActionLists();
    #endregion
}
