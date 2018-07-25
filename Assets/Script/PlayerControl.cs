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
     

	public void InitPlayer(Room room)
	{
		CameraFollow.instance.PlayerTrace(player);
		player.SetRoom(room, new Vector2Int(3,3));
		GameManager.instance.SetCurrentRoom (room);
		GameManager.instance.OnPlayerEnterRoom ();
    }

    private Player player;
	public Player PlayerObject{
		get{ return player; }
	}

    public void EndTurnButton()
    {
        NaturalDraw();
        EndPlayerTurn();
    }

	#region Card
	public Deck deck;
	public HandCard hand;

	/// <summary>
	/// Draw Anyway even if Hand is Full
	/// </summary>
	public void ForceDraw(){
		hand.DrawHand (deck.Draw ());
	}

	/// <summary>
	/// Draw With MagicCard
	/// </summary>
	public void MagicDraw(){
		if (hand.CurrentHandCount < Config.instance.HandMax) {
			hand.DrawHand (deck.Draw ());
		}
	}

	/// <summary>
	/// Draw Each Turn (Check Remain Monsters)
	/// </summary>
	public void NaturalDraw(){
		if (hand.CurrentHandCount < Config.instance.HandMax && GameManager.instance.GetCurrentRoom().IsEnemyAlive()) {
			hand.DrawHand (deck.Draw ());
		}
	}

	public void AddCard(CardData cData){
		if (hand.CurrentHandCount < Config.instance.HandMax) {
			hand.AddHand (cData.Instantiate ());
		}
	}

	public void ReLoadDeck(){
        hand.RemoveAll();
		deck.Load ();
        hand.DrawHand(deck.Draw());
        hand.DrawHand(deck.Draw());
        hand.DrawHand(deck.Draw());
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

    public void MoveLeft()
    {
        if (player.MoveTo(player.pos + Vector2Int.left))
        {
            NaturalDraw();
            InputModule.IsPlayerTurn = false;
        }
    }
    public void MoveUP()
    {
        if (player.MoveTo(player.pos + Vector2Int.up))
        {
            NaturalDraw();
            InputModule.IsPlayerTurn = false;
        }
    }
    public void MoveRight()
    {
        if (player.MoveTo(player.pos + Vector2Int.right))
        {
            NaturalDraw();
            InputModule.IsPlayerTurn = false;
        }
    }
    public void MoveDown()
    {
        if (player.MoveTo(player.pos + Vector2Int.down))
        {
            NaturalDraw();
            InputModule.IsPlayerTurn = false;
        }
    }
    public void ToggleHand()
    {
        hand.ToggleHand();
    }

    #region Status //상태이상 스테이터스 관리

    public bool Move = true; public bool Draw = true;

    private Debuffs debuff;
    public void SetDebuff(Debuffs d)
    {
        if(debuff != null)
        {
           EraseDebuff();
        }
        debuff = d;
    }
    public void CountDebuff()
    {
        if(debuff != null)
        {
            debuff.CountTurn();
        }
    }
    public void EraseDebuff()
    {
        debuff.OnDestroy();
        debuff = null;
    }
    
    #endregion
}
