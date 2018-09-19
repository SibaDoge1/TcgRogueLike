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
     

    private static Player player;
	public static Player Player{
		get{ return player; }
	}

    public void EndTurnButton()
    {
        if (GameManager.instance.CurrentTurn == Turn.PLAYER && player.currentRoom.IsEnemyAlive())
        {
            NaturalDraw();
            GameManager.instance.OnEndPlayerTurn();
            PlayerData.AkashaGage += 2;
        }
    }

    #region Card
    private Deck _deck;
    private Hand _hand;

	public Deck deck { get { return _deck; } set { _deck = value; } }
	public Hand hand { get { return _hand; } set { _hand = value; } }

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
        Debug.Log(hand);
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
    /// 인벤토리에 카드 추가, 애니메이션 재생
    /// </summary>
    /// <param name="cData"></param>
    public void AddToAttain(CardData cData)
    {
        PlayerData.AttainCards.Add(cData);
        switch(cData.Rating)
        {
            case Rating.R0:
                EffectDelegate.instance.MadeEffect(UIEffect.AttainR0, UIManager.canvas);
                break;
            case Rating.R1:
                EffectDelegate.instance.MadeEffect(UIEffect.AttainR1, UIManager.canvas);
                break;
            default:
                break;
        }
    }


    public void ReLoadDeck()
    {
        hand.RemoveAll();
		deck.Load ();
        hand.DrawHand(deck.Draw());
        hand.DrawHand(deck.Draw());
        hand.DrawHand(deck.Draw());
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
                PlayerData.AkashaGage -= 1;
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
                PlayerData.AkashaGage -= 1;
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
                PlayerData.AkashaGage -= 1;
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
                PlayerData.AkashaGage -= 1;
            }
            GameManager.instance.OnEndPlayerTurn();
        }
    }
    public void ToggleHand()
    {
        hand.ToggleHand();
    }
    public void EnableCards(bool enable)
    {
        hand.EnableCards(enable);
    }

    /*
    /// <summary>
    /// 역장
    /// </summary>
    public void StationField()
    {
        if(PlayerData.AkashaCount>0 && GameManager.instance.CurrentTurn == Turn.PLAYER && player.currentRoom.IsEnemyAlive())
        {
            PlayerData.AkashaCount--;
            player.GetHeal(2);
        }
    } 
    */

    #region statusRegion //상태이상 스테이터스 관리
    
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
