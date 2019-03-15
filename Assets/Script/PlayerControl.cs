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
        if (GameManager.instance.CurrentTurn == Turn.PLAYER && selectedDirCard == null)
        {
            if(player.currentRoom.IsEnemyAlive())
            {
                NaturalDraw();
            }
            GameManager.instance.OnEndPlayerTurn();
        }
    }

    #region Card
    private DeckManager deck;
    private HandManager hand;

	public DeckManager DeckManager { get { return deck; } set { deck = value; } }
	public HandManager HandManager { get { return hand; } set { hand = value; } }

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
		if (HandManager.CurrentHandCount < Config.instance.HandMax) {
			HandManager.DrawHand (DeckManager.Draw ());
		}else
        {
            HandManager.ReturnCard();//가장왼쪽의 카드 제거           
            HandManager.DrawHand(DeckManager.Draw());
        }
	}

    /// <summary>
    /// Attain 카드 추가, 애니메이션 재생
    /// </summary>
    /// <param name="cData"></param>
    public void AddToAttain(Card cData)
    {
        DeckManager.AttainCards.Add(cData);
        //UIManager.instance.StartUIAnim(UIAnimation.Attain);
    }


    public void ReLoadDeck()
    {
        HandManager.DumpAll();
		DeckManager.ReLoad ();
        for(int i=0; i<3; i++)
        {
            HandManager.DrawHand(DeckManager.Draw());
        }
    }

    public void OnRoomClear()
    {
        ReLoadDeck();
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
                    AkashaGage += 1;              
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
    public void ReturnToStart()
    {
        if(!GameManager.instance.CurrentRoom().IsEnemyAlive())
        {
            if(GameManager.instance.IsInputOk && GameManager.instance.CurrentTurn == Turn.PLAYER)
            {
                player.EnterRoom(GameManager.instance.CurrentMap.StartRoom);
                MinimapTexture.DrawPlayerPos(GameManager.instance.CurrentRoom().transform.position, PlayerControl.player.pos);
            }
        }
    }
    #endregion

    #region Akasha

    int akashaGage;
    public int AkashaGage
    {
        get { return akashaGage; }
        set
        {
            if (value >= 10)
            {
                akashaGage = 10;
            }
            else if (value < 0)
            {
                akashaGage = 0;
            }
            else
            {
                if (PlayerControl.playerBuff.IsAkashaAble)
                {
                    akashaGage = value;
                }
                else
                {
                    if (value <= akashaGage)
                    {
                        akashaGage = value;
                    }
                }
            }
            UIManager.instance.AkashaUpdate(AkashaGage);
        }
    }

    public void AttackedTarget()
    {
        if (!isAttacked)
        {
            AkashaGage++;
            isAttacked = true;
        }
    }

    private bool isAttacked = false;
    public void OnPlayerTurn()
    {
        isAttacked = false;
    }

    #endregion
}
