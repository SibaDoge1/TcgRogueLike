using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private void Start()
    {
        FullHp = SettingHp; CurrentHp = SettingHp;
        Atk = SettingAtk; Def = SettingDef;
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
            UIManager.instance.HpUpdate(currentHp, fullHp);
        }
    }

    public override int CurrentHp
    {
        set
        {
          base.CurrentHp = value;
            UIManager.instance.HpUpdate(currentHp, fullHp);
        }
    }
    public override bool GetDamage(int damage, Entity atker = null)
    {
        SoundDelegate.instance.PlayEffectSound(EffectSoundType.GetHit,transform.position);
        MyCamera.instance.ShakeCamera();
        EffectDelegate.instance.MadeEffect(CardEffectType.Shield, this);
        PlayerData.AkashaGage -= 5;
        return base.GetDamage(damage, atker);
    }
}
