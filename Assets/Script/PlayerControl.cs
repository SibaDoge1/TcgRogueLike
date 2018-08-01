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

    private static Player player;
	public static Player Player{
		get{ return player; }
	}

    public void EndTurnButton()
    {
        if(GameManager.instance.CurrentTurn == Turn.PLAYER && player.currentRoom.IsEnemyAlive())
        {
            NaturalDraw();
            GameManager.instance.OnEndPlayerTurn();
        }
    }

    #region Card
    public GameDeck _deck;
    public HandUI _hand;

	public GameDeck deck { get { return _deck; } set { _deck = value; } }
	public HandUI hand { get { return _hand; } set { _hand = value; } }

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
		if (hand.CurrentHandCount < Config.instance.HandMax) {
			hand.DrawHand (deck.Draw ());
		}
	}
    /*
    /// <summary>
    /// *핸드에 해당 카드 추가, 덱에 추가 아님
    /// </summary>
    /// <param name="cData"></param>
	public void AddCard(Card cData){
		if (hand.CurrentHandCount < Config.instance.HandMax) {
			hand.AddHand (cData.Instantiate ());
		}
	}*/

    /// <summary>
    /// 덱에 카드 추가
    /// </summary>
    /// <param name="cData"></param>
    public void AddToDeck(CardData cData)
    {
        PlayerData.Deck.Add(cData);
        deck.Load();
    }


    public void ReLoadDeck(){
        hand.RemoveAll();
		deck.Load ();
        hand.DrawHand(deck.Draw());
        hand.DrawHand(deck.Draw());
        hand.DrawHand(deck.Draw());
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
        if(!IsMoveAble)
        {
            EndTurnButton();
        }
        else if (player.MoveTo(player.pos + Vector2Int.left))
        {
            if(player.currentRoom.IsEnemyAlive())
            {
                NaturalDraw();
            }
            GameManager.instance.OnEndPlayerTurn();
        }
    }
    public void MoveUP()
    {
        if (!IsMoveAble)
        {
            EndTurnButton();
            
        }
        else if(player.MoveTo(player.pos + Vector2Int.up))
        {
            if (player.currentRoom.IsEnemyAlive())
            {
                NaturalDraw();
            }
            GameManager.instance.OnEndPlayerTurn();
        }
    }
    public void MoveRight()
    {
        if (!IsMoveAble)
        {
            EndTurnButton();
        }
        else if(player.MoveTo(player.pos + Vector2Int.right))
        {
            if (player.currentRoom.IsEnemyAlive())
            {
                NaturalDraw();
            }
            GameManager.instance.OnEndPlayerTurn();
        }
    }
    public void MoveDown()
    {
        if (!IsMoveAble)
        {
            EndTurnButton();
        }
        else if(player.MoveTo(player.pos + Vector2Int.down))
        {
            if (player.currentRoom.IsEnemyAlive())
            {
                NaturalDraw();
            }
            GameManager.instance.OnEndPlayerTurn();
        }
    }
    public void ToggleHand()
    {
        hand.ToggleHand();
    }


    /// <summary>
    /// 역장
    /// </summary>
    public void StationField()
    {
        if(PlayerData.AkashaCount>0 && GameManager.instance.CurrentTurn == Turn.PLAYER && player.currentRoom.IsEnemyAlive())
        {
            PlayerData.AkashaCount--;
            player.GetHeal(2);
            EndTurnButton();
        }
    }

    #region Status //상태이상 스테이터스 관리
    private bool isMoveAble = true; private bool isDrawAble = true;
    public bool IsMoveAble
    {
        get { return isMoveAble; }
        set { isMoveAble = value; }
    }
    public bool IsDrawAble
    {
        get { return isDrawAble; }
        set { isDrawAble = value; }
    }

    private Debuffs debuff;
    public void SetDebuff(Debuffs d)
    {
        if(debuff != null)
        {
           EraseDebuff();
        }
        debuff = d;
        d.Active();
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
