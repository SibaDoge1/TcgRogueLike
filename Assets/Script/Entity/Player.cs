using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    protected override void Awake()
    {
        base.Awake();
        FullHp = 10; CurrentHp = 10;
        Atk = 1;
    }
    //문을 통해서 이동
    public void EnterRoom(OffTile_Door door)
    {
        Vector2Int temp;

        if (currentRoom != null)
           currentTile.OnTileObj = null;
		//Spawn Position Set
        if (door.Dir == Direction.NORTH)
        {
            temp = door.ConnectedDoor.CurrentTile.pos + new Vector2Int(0,1);
        }
        else if (door.Dir == Direction.EAST)
        {
            temp = door.ConnectedDoor.CurrentTile.pos + new Vector2Int(1, 0);
        }
        else if (door.Dir == Direction.WEST)
        {
            temp = door.ConnectedDoor.CurrentTile.pos + new Vector2Int(-1, 0);
        }
        else//_room == currentRoom.SouthRoom
        {
            temp = door.ConnectedDoor.CurrentTile.pos + new Vector2Int(0,-1);
        }

        SetRoom(door.TargetRoom,temp);

		GameManager.instance.OnPlayerEnterRoom(currentRoom);
    }
    //워프하듯 이동
    public void EnterRoom(Room room)
    {

        if (currentRoom != null)
            currentTile.OnTileObj = null;

        SetRoom(room, new Vector2Int(room.size.x/2,room.size.y/2));


         GameManager.instance.OnPlayerEnterRoom(currentRoom);
        
    }
	protected override void OnDieCallback()
    {
        gameObject.SetActive(false);
        GameManager.instance.GameOver();
    }

    public override int FullHp
    {
        set
        {
            base.FullHp = value;
            UIManager.instance.HpUpdate(currentHp);
        }
    }

    protected override int CurrentHp
    {
        set
        {
            base.CurrentHp = value;
            UIManager.instance.HpUpdate(currentHp);
        }
    }
    public override bool GetDamage(int damage, Entity atker = null, bool isReturn = false)
    {
        if(!PlayerControl.playerBuff.IsHitAble)
        {
            damage = 0;
        }
        if(atker == this && isReturn)//반환
        {
            SetPlayerAnim(2);
            SoundDelegate.instance.PlayEffectSound(SoundEffect.CARD, transform.position);
        }
        else//일반 피격
        {
            SoundDelegate.instance.PlayEffectSound(SoundEffect.DAMAGE, transform.position);
            MyCamera.instance.ShakeCamera();
            SetPlayerAnim(1);
        }


        return base.GetDamage(damage, atker);
    }
    public void SetHp(int hp)
    {
        int dif = currentHp - hp;
        if(dif>=0)
        {
            GetDamage(dif);
        }
        else
        {
            GetHeal(-dif);
        }
    }
    public override bool GetHeal(int heal)
    {
        SoundDelegate.instance.PlayEffectSound(SoundEffect.HEAL, transform.position);
        return base.GetHeal(heal);
    }

    public override bool MoveTo(Vector2Int _pos)
    {
        if(PlayerControl.playerBuff.IsMoveAble)
        {
            return base.MoveTo(_pos);
        }else
        {
            return false;
        }
    }
    private Coroutine playerAnim;
    public Coroutine PlayerAnim
    {
        get { return playerAnim; }
    }

    /// <summary>
    /// Set Animation
    /// </summary>
    public void OnAttack()
    {
        SetPlayerAnim(0);
    }


    private void SetPlayerAnim(int num)
    {
        if(playerAnim != null)
        {
            StopCoroutine(playerAnim);
        }
        playerAnim = StartCoroutine(AnimationRoutine(num));
    }
    protected override IEnumerator AnimationRoutine(int num, float animationTime = 0.5F)
    {
        SpriteRender.sprite = actionSprites[num];
        yield return new WaitForSeconds(animationTime);
        SpriteRender.sprite = normalSprites;
        playerAnim = null;
        yield return null;
    }
}
