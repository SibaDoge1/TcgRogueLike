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
        base.OnDieCallback();
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
    public override bool GetDamage(int damage, Entity atker = null)
    {
        SoundDelegate.instance.PlayEffectSound(EffectSoundType.GetHit,transform.position);
        MyCamera.instance.ShakeCamera();
        EffectDelegate.instance.MadeEffect(CardEffectType.Shield, this);

        if (PlayerControl.status.IsHitAble)
        {
            return base.GetDamage(damage, atker);
        }
        else
            return false;
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


}
