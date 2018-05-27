using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class PlayerControl : MonoBehaviour {
    public static PlayerControl instance;
	void Awake()
    {
        instance = this;
        player = GetComponent<Player>();
    }
    void Start () {
        Input.multiTouchEnabled = false;
	}
     

	public void InitPlayer(Room room)
	{
		CameraFollow.instance.RoomTrace(room);
		player.SetRoom(room, new Vector2Int(4, 4));
		GameManager.instance.SetCurrentRoom (room);
		GameManager.instance.OnPlayerEnterNewRoom ();	
	}

    private Player player;
	public Player PlayerObject{
		get{ return player; }
	}
    private int curHp;

    private List<Tile> path;
	public int GetRemainAction(){
		if (path == null) {
			return 0;
		}
		return path.Count;
	}

	public bool MoveReserveResume(){
        if (curHp == player.currentHp && player.MoveTo(path[0].pos)) {
			path.RemoveAt (0);
			DrawCard ();
			return true;
		} else {
			path = null;
			return false;
		}
	}

	public bool PlayerMoveCommand(Tile pos)
    {
		Room cur = player.currentRoom;
		curHp = player.currentHp;
		path = PathFinding.instance.GeneratePath(player, pos);

		return MoveReserveResume ();
    }

	#region Card
	public Deck deck;
	public HandCard hand;

	public void DrawCard(){
		if (hand.CurrentHandCount < Config.HandMax && GameManager.instance.GetCurrentRoom().IsEnemyAlive()) {
			hand.DrawHand (deck.Draw ());
		}
	}

	public void AddCard(CardData cData){
		if (hand.CurrentHandCount < Config.HandMax) {
			hand.AddHand (cData.Instantiate ());
		}
	}

	public void ReLoadDeck(){
		deck.Load ();
	}

	/// <summary>
	/// Call from GameManager
	/// </summary>
	public void OnStartPlayerTurn(){
		hand.CheckAvailable ();
	}

	/// <summary>
	/// Call from Player(Move) or CardObject(Card)
	/// </summary>
	public void EndPlayerTurn(){
		GameManager.instance.OnEndPlayerTurn ();
	}
	#endregion
}
