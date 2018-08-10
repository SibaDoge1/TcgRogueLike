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
    public void EnterRoom(Door door)
    {
        Vector2Int temp;

        if (currentRoom != null)
           currentTile.OnTileObj = null;

        Room old = currentRoom;

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

        GameManager.instance.SetCurrentRoom (door.TargetRoom);
		GameManager.instance.OnPlayerEnterRoom (old,currentRoom );
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
        PlayerData.AkashaGage -= 5;
        return base.GetDamage(damage, atker);
    }
}
