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
        }
    }

    #region Card
    private Deck _deck;
    private Hand _hand;

	public Deck deck { get { return _deck; } set { _deck = value; } }
	public Hand hand { get { return _hand; } set { _hand = value; } }

    private bool isDirCardSelected = false;
    public bool IsDirCardSelected
    {
        get { return isDirCardSelected; }
    }
    private Card selectedDirCard = null;
    public Card SelectedDirCard
    {
        get { return selectedDirCard; }
        set
        {
            if (value == null)
            {
                isDirCardSelected = false;
                selectedDirCard.CancelPreview();
            }
            else
            {
                isDirCardSelected = true;
                value.CardEffectPreview();
            }
            selectedDirCard = value;

        }
    }
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
		}else
        {
            hand.RemoveLeftCard();//가장왼쪽의 카드 제거
            hand.DrawHand(deck.Draw());
        }
	}

    /// <summary>
    /// Attain 카드 추가, 애니메이션 재생
    /// </summary>
    /// <param name="cData"></param>
    public void AddToAttain(Card cData)
    {
        PlayerData.AttainCards.Add(cData);
        switch(cData.Cost)
        {
            case 0:
                EffectDelegate.instance.MadeEffect(UIEffect.AttainR0, UIManager.canvas);
                break;
            case 1:
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
    #region PlayerInput
    public void MoveToDirection(Direction dir)
    {
        if (!IsMoveAble)
        {
            EndTurnButton();
        }else
        {
            Vector2Int d = Vector2Int.zero;
            switch (dir)
            {
                case Direction.NORTH:
                    d = Vector2Int.up;
                    break;

                case Direction.EAST:
                    d = Vector2Int.right;
                    break;

                case Direction.SOUTH:
                    d = Vector2Int.down;
                    break;

                case Direction.WEST:
                    d = Vector2Int.left;
                    break;
            }

            if (player.MoveTo(player.pos + d))
            {
                if (player.currentRoom.IsEnemyAlive())
                {
                    NaturalDraw();
                    if(PlayerData.AkashaGage<5)
                    {
                        PlayerData.AkashaGage += 1;
                    }
                }
                GameManager.instance.OnEndPlayerTurn();
            }
        }
    }
    public void DoDirCard(Direction dir)
    {
        SelectedDirCard.DoCard(dir);
        if (selectedDirCard.IsConsumeTurn())
        {
            GameManager.instance.OnEndPlayerTurn();
        }
        SelectedDirCard = null;
    }
    public void ToggleHand()
    {
        hand.ToggleHand();
    }
    #endregion
    public void EnableCards(bool enable)
    {
        hand.EnableCards(enable);
    }

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
