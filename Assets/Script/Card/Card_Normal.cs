using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using System;


public class Card_Normal : Card
{
    private int range;

    /// <summary>
    /// 타입, 형태 지정해서 생성 
    /// </summary>
    public Card_Normal(Figure _figure,CardType _type)
    {

        cardFigure = _figure;
        cardType = _type;

        GenerateCardData();
    }

    /// <summary>
    /// 타입 지정해서 생성 
    /// </summary>
    public Card_Normal(CardType _type)
    {

        cardFigure = (Figure)UnityEngine.Random.Range(1, 6);
        cardType = _type;

        GenerateCardData();
    }

    /// <summary>
    /// 랜덤생성
    /// </summary>
    public Card_Normal()
    {
        cardFigure = (Figure)UnityEngine.Random.Range(1, 6);
        cardType = (CardType)UnityEngine.Random.Range(0, 4);

        GenerateCardData();
    }


    private void GenerateCardData()
    {       
        cardSound = SoundEffect.ATTACK;


        switch (cardType) //Info 설정
        {
            case CardType.A:
                val1 = 1;
                cost = 1;
                index = 91;

                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입힙니다.";
                cardEffect = CardEffect.EMPTY;
                break;
            case CardType.T:
                val1 = 3;
                cost = 3;
                index = 93;

                cardEffect = CardEffect.REINFORCE;
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입힙니다.";
                break;
            case CardType.P:
                val1 = 1;
                cost = 2;
                index = 92;

                cardEffect = CardEffect.TELEPORT;
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입히고 선택한 방향으로 3만큼 텔레포트합니다.";
                isDirectionCard = true;
                break;
            case CardType.V:
                val1 = 2;
                cost = 4;
                index = 94;

                cardEffect = CardEffect.HEAL;
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입히고 1만큼 회복합니다.";
                break;

        }

        switch(cardFigure)
        {
            case Figure.Diagonal:
                range = 1;
                spritePath = "Card_X";
                cardRange = "range_5_1";
                name = "DX-2 결정형";
                break;
            case Figure.CROSS:
                range = 1;
                spritePath = "Card_Cross";
                cardRange = "range_1_1";
                name = "CR-1 집중형";
                break;
            case Figure.HORIZION:
                range = 1;
                cardRange = "range_3_1";
                spritePath = "Card_Horizon";//이미지 현재 없음
                name = "HZ-3 사격형";
                break;
            case Figure.SQUARE:
                range = 1;
                spritePath = "Card_Square";
                cardRange = "range_2_1";
                name = "SQ-5 격류형";
                break;
            case Figure.VERTICAL:
                range = 1;
                cardRange = "range_4_1";
                spritePath = "Card_Vertical";//이미지 현재 없음
                name = "VT-4 낙뢰형";
                break;
            default:
                Debug.LogError("Normal카드 FigureError");
                break;
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, range, cardFigure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, cardFigure);
            for (int i = 0; i < enemies.Count; i++)
            {

                    DamageToTarget(enemies[i], val1);              
            }
            if (cardType == CardType.V)
            {
                player.GetHeal(1); // CardType : V
            }
        }
        //MakeEffect(TileUtils.Range(player.currentTile,range,figure));
    }

    protected override void CardActive(Direction dir)//CardType : T
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

          player.Teleport(player.pos + d * 2);       



    }

    /// <summary>
    /// effect preview
    /// </summary>
    private List<Tile> targetTiles;
    public override void CardEffectPreview()
    {            
        if(PlayerControl.instance.SelectedDirCard == this)
        {
            targetTiles = new List<Tile>();

            int x = (int)player.currentTile.pos.x; int y = (int)player.currentTile.pos.y;
            int range = 2;

            targetTiles.Add(player.currentRoom.GetTile(new Vector2Int(x + range, y)));
            targetTiles.Add(player.currentRoom.GetTile(new Vector2Int(x - range, y)));
            targetTiles.Add(player.currentRoom.GetTile(new Vector2Int(x, y + range)));
            targetTiles.Add(player.currentRoom.GetTile(new Vector2Int(x, y - range)));

            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(ArchLoader.instance.MadeEffect(RangeEffectType.DIR,player, targetTiles[i]));
                if (ranges[i] != null)
                {
                    ranges[i].transform.parent = player.transform;
                }
            }

        }
        else
        {
            targetTiles = TileUtils.Range(player.currentTile, range, cardFigure);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(ArchLoader.instance.MadeEffect(RangeEffectType.CARD,player, targetTiles[i]));
                if (ranges[i] != null)
                {
                    ranges[i].transform.parent = player.transform;
                }
            }
        }
    }

    public override void UpgradeReset()
    {
        isUpgraded = false;
        switch (cardType)
        {
            case CardType.V:
                val1 = 2;
                break;
            case CardType.T:
                val1 = 3;
                break;
            case CardType.P:
                val1 = 1;
                break;
            case CardType.A:
                val1 = 1;
                break;
        }
    }
    public override void UpgradeThis()
    {
        base.UpgradeThis();
        if(cardType == CardType.T)
        {
            val1--;
        }
    }
}