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
		CameraFollow.instance.PlayerTrace(player);
		player.SetRoom(room, new Vector2Int(4, 4));
		GameManager.instance.SetCurrentRoom (room);
		GameManager.instance.OnPlayerEnterNewRoom ();	
	}

    private Player player;
	public Player PlayerObject{
		get{ return player; }
	}
    
    private List<Tile> path;
	public int GetRemainAction(){
		if (path == null) {
			return 0;
		}
		return path.Count;
	}

	public bool MoveReserveResume(){
        if (moveStartRoom == player.currentRoom && 
            moveStartHP == player.currentHp && player.MoveTo(path[0].pos))
        {
			path.RemoveAt (0);
			DrawCard ();
			return true;
		} else {
			path = null;
			return false;
		}
	}
    private int moveStartHP;
    Room moveStartRoom;
    public bool PlayerMoveCommand(Tile pos)
    {
		moveStartRoom = player.currentRoom;
		moveStartHP = player.currentHp;
		path = PathFinding.instance.GeneratePath(player, pos);

		return MoveReserveResume ();
    }

    public void EndTurnButton()
    {
        DrawCard();
        EndPlayerTurn();
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
	/// Call from Player(Move) or CardObject(Card) or EndTurnButton
	/// </summary>
	public void EndPlayerTurn(){
		GameManager.instance.OnEndPlayerTurn ();
	}
	#endregion
}
