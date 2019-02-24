﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public delegate IEnumerator Action();
public abstract class Enemy : Character {
    protected List<GameObject> rangeList = new List<GameObject>();
    protected EnemyUI enemyUI;

    public bool isElite;//엘리트 몹인가?

    protected int value;
    protected bool vision;//시야 유무
    protected byte visionDistance;

    public override void Init(short _entityNum)
    {
        base.Init(_entityNum);

        MonsterData data = Database.GetMonsterData(entityNum);

        fullHp = data.hp; currentHp = fullHp;
        Atk = data.atk;
        value = data.rank;
        vision = data.vision;
        visionDistance = data.visionDistance;
        isElite = data.elite;

        if (vision)
        {
            currentState = State.OFF;
        }
        else
        {
            currentState = State.DELAY;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        enemyUI = transform.Find("Canvas").GetComponent<EnemyUI>();
    }
    protected virtual void Start()
    {
        SetActionLists();
    }

    protected override void OnDieCallback ()
    {
        ClearRangeList();
		currentRoom.OnEnemyDead (this);
        EffectDelegate.instance.MadeEffect(CardEffectType.Blood, currentTile);
        if(GameManager.instance.CurrentTurn == Turn.ENEMY)
        {
            OnEndTurn();
        }
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
    protected virtual void OnEndTurn()
    {
        EnemyControl.instance.EnemyEndCallBack();
    }


    protected override void SetLocalScale(int x)
    {
        base.SetLocalScale(x);
        enemyUI.SetLocalScale(x);
    }

    protected override int CurrentHp
    {
        get
        {
            return base.CurrentHp;
        }

        set
        {
            base.CurrentHp = value;
            enemyUI.HpOn(FullHp, value);
        }
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
       OFF, DELAY, THINK, ACT
    }
    protected State currentState;
    private int delayCount; private int actCount;

    protected List<Action> DelayList;
    protected List<Action> currentActionList;


    public IEnumerator AIRoutine()
    {
        if(currentState == State.OFF)
        {
            if(TileUtils.AI_SquareFind(currentTile,visionDistance))
            {
                currentState = State.DELAY;
            }else
            {
                OnEndTurn();
                yield break;
            }      
        }
        if (currentState == State.DELAY)
        {
            if(DelayList != null)
            {
                yield return StartCoroutine(DelayList[delayCount]());
                delayCount++;
                if (delayCount >= DelayList.Count)
                {
                    delayCount = 0;
                    currentState = State.THINK;
                    OnEndTurn();
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
