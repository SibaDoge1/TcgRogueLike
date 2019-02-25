using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;

public class PlayerControl : MonoBehaviour {
    public static PlayerControl instance;
    public static BuffManager playerBuff;//Status
    public static Player player;//PlayerEntity


    void Awake()
    {
        instance = this;
        player = GetComponent<Player>();
        playerBuff = new BuffManager();
    }

    public void EndTurnButton()
    {
        if (GameManager.instance.CurrentTurn == Turn.PLAYER)
        {
            if(player.currentRoom.IsEnemyAlive())
            {
                NaturalDraw();
            }
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
            Card old = selectedDirCard;
            selectedDirCard = value;

            if (selectedDirCard == null)
            {
                isDirCardSelected = false;
                old.CancelPreview();
            }
            else
            {
                isDirCardSelected = true;
                selectedDirCard.CardEffectPreview();
            }

        }
    }

	/// <summary>
	/// Draw Each Turn
	/// </summary>
	public void NaturalDraw(){
		if (hand.CurrentHandCount < Config.instance.HandMax) {
			hand.DrawHand (deck.Draw ());
		}else
        {
            if(!deck.isDrawEnd)
            {
                hand.ReturnCard();//가장왼쪽의 카드 제거
            }
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
        ArchLoader.instance.MadeEffect(UIEffect.CARD, UIManager.instance.transform);
        //TODO : UI 애니메이션 Instantiate가 아니라 그냥 재생만 하는식으로 바꾸기
    }


    public void ReLoadDeck()
    {
        hand.DumpAll();
		deck.ReLoad ();
        for(int i=0; i<3; i++)
        {
            hand.DrawHand(deck.Draw());
        }
    }

    public void OnRoomClear()
    {
        hand.DumpAll();
        deck.OnRoomClear();
        for (int i = 0; i < 3; i++)
        {
            hand.DrawHand(deck.Draw());
        }
        playerBuff.EraseDeBuff();
    }

    #endregion
    #region PlayerInput
    public void MoveToDirection(Direction dir)
    {
        if (!playerBuff.IsMoveAble)
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
                    PlayerData.AkashaGage += 1;              
                }
                GameManager.instance.OnEndPlayerTurn();
            }
        }
    }
    public void DoDirCard(Direction dir)
    {
        SelectedDirCard.OnCardPlayed(dir);
        if (selectedDirCard.IsConsumeTurn())
        {
            GameManager.instance.OnEndPlayerTurn(selectedDirCard.effectTime);
        }
        SelectedDirCard = null;
    }
    public void ToggleHand()
    {
        hand.ToggleHand();
    }
    #endregion
}
