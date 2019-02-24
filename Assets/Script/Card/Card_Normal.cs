using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using System;


public class Card_Normal : Card
{
    private Figure figure;
    private int range;

    /// <summary>
    /// 지정해서 생성
    /// </summary>
    public Card_Normal(Figure _figure,CardType _type,bool upgrade)
    {

        figure = _figure;
        cardType = _type;

        switch (cardType)
        {
            case CardType.V:
                val1 = 2;
                cost = 3;
                break;
            case CardType.T:
                val1 = 3;
                cost = 2;
                break;
            case CardType.P:
                val1 = 1;
                cost = 4;
                break;
            case CardType.A:
                val1 = 1;
                cost = 1;
                break;
        }

        GenerateCardData();
    }

    /// <summary>
    /// 랜덤생성
    /// </summary>
    public Card_Normal()
    {
        figure = (Figure)UnityEngine.Random.Range(1, 6);
        cardType = (CardType)UnityEngine.Random.Range(0, 4);

        switch (cardType)
        {
            case CardType.V:
                val1 = 2;
                cost = 3;
                break;
            case CardType.T:
                val1 = 3;
                cost = 2;
                break;
            case CardType.P:
                val1 = 1;
                cost = 4;
                break;
            case CardType.A:
                val1 = 1;
                cost = 1;
                break;
        }

        GenerateCardData();
    }


    private void GenerateCardData()
    {
        index = 0;

        switch (cardType) //Info 설정
        {
            case CardType.A:
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입힙니다.";
                break;
            case CardType.T:
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입힙니다.";
                break;
            case CardType.P:
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입히고 선택한 방향으로 3만큼 텔레포트합니다.";
                isDirectionCard = true;
                break;
            case CardType.V:
                info = "주어진 범위의 적들에게 <val1>만큼의 피해를 입히고 1만큼 회복합니다.";
                break;

        }

        switch(figure)
        {
            case Figure.Diagonal:
                range = 1;
                spritePath = "Card_X";
                break;
            case Figure.CROSS:
                range = 1;
                spritePath = "Card_Cross";
                break;
            case Figure.HORIZION:
                range = 1;
                spritePath = "Card_Horizon";//이미지 현재 없음
                break;
            case Figure.SQUARE:
                range = 1;
                spritePath = "Card_Square";
                break;
            case Figure.VERTICAL:
                range = 1;
                spritePath = "Card_Vertical";//이미지 현재 없음
                break;
            default:
                Debug.LogError("Normal카드 FigureError");
                break;
        }
        name = figure.ToString();
    }

    protected override void CardActive()
    {
         if (cardType == CardType.V)
        {
            player.GetHeal(1); // CardType : V
        }

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {

                    DamageToTarget(enemies[i], val1);
                
            }
        }
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
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.DIR,player, targetTiles[i]));
                if (ranges[i] != null)
                {
                    ranges[i].transform.parent = player.transform;
                }
            }

        }
        else
        {
            targetTiles = TileUtils.Range(player.currentTile, range, figure);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD,player, targetTiles[i]));
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
                val1 = 1;
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