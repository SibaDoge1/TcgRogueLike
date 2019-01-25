using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using System;

public enum NormalCard
{
    CROSS,
    X,
    HORIZION,
    VERTICAL,
    SQUARE,
    BIG_X,
    BIG_CROSS,
    BIG_HORIZONTAL,
    BIG_VERTICAL,
    BIG_SQUARE
}

public class Card_Normal : Card
{
    private Figure figure;
    private int range;
    NormalCard normal;

    /// <summary>
    /// 지정해서 생성
    /// </summary>
    public Card_Normal(int _cost,NormalCard _normal,CardType _type,bool upgrade)
    {
        cost = _cost;
        normal = _normal;
        cardType = _type;
        isUpgraded = upgrade;

        GenerateCardData();
    }

    /// <summary>
    /// 랜덤생성
    /// </summary>
    public Card_Normal()
    {

        normal = (NormalCard)UnityEngine.Random.Range(0, 10);

        if((int)normal<=4)
        {
            cost = MyRandom.RandomEvent(0,0,1,2,2,3,3,4,5);//0~5
        }
        else
        {
            cost = MyRandom.RandomEvent(1, 2, 2, 3, 3, 4, 4, 5);//1~5
        }

        if(cost == 0)
        {
            cardType = 0;
            isUpgraded = false;
        }
        else
        {
            cardType = (CardType)MyRandom.RandomEvent(1,2,3,4);//아카샤 제외 1~4
            isUpgraded = MyRandom.GetRandomBool(30);
        }



        GenerateCardData();
    }


    private void GenerateCardData()
    {
        index = 0;

        if (!isUpgraded) //Val1 설정
            val1 = 5;
        else
            val1 = 10;

        switch (cardType) //Info 설정
        {
            case CardType.A:
                info =  "게이지를 한칸 충전하고 주어진 범위의 적들에게 " + val1 + "만큼의 피해를 입힙니다.";
                break;
            case CardType.P:
                info = "주어진 범위의 적들에게" + val1 + "만큼의 피해를 입힙니다. 엘리트 몬스터와 보스 몬스터에게는 2배의 데미지를 입힙니다.";
                break;
            case CardType.T:
                info = "선택한 방향으로 3만큼 텔레포트한뒤 주어진 범위의 적들에게 "+val1+"만큼의 피해를 입힙니다.";
                isDirectionCard = true;
                break;
            case CardType.V:
                info = "주어진 범위의 적들에게" + val1 + "만큼의 피해를 입히고 1만큼 회복합니다.";
                break;
            case CardType.N:
                info = "주어진 범위의 적들에게" + val1 +"만큼의 피해를 입힙니다.";
                break;
        }

        switch(normal)
        {
            case NormalCard.X:
                figure = Figure.X;
                range = 1;
                spritePath = "x";
                break;
            case NormalCard.CROSS:
                figure = Figure.CROSS;
                range = 1;
                spritePath = "cross";
                break;
            case NormalCard.HORIZION:
                figure = Figure.HORIZION;
                range = 1;
                spritePath = "error";//이미지 현재 없음
                break;
            case NormalCard.SQUARE:
                figure = Figure.SQUARE;
                range = 1;
                spritePath = "square";
                break;
            case NormalCard.VERTICAL:
                figure = Figure.VERTICAL;
                range = 1;
                spritePath = "error";//이미지 현재 없음
                break;
            case NormalCard.BIG_CROSS:
                figure = Figure.CROSS;
                range = 2;
                spritePath = "error";//이미지 현재 없음
                break;
            case NormalCard.BIG_HORIZONTAL:
                figure = Figure.HORIZION;
                range = 2;
                spritePath = "error";//이미지 현재 없음
                break;
            case NormalCard.BIG_SQUARE:
                figure = Figure.EMPTYSQUARE;
                range = 2;
                spritePath = "error";//이미지 현재 없음
                break;
            case NormalCard.BIG_VERTICAL:
                figure = Figure.VERTICAL;
                range = 2;
                spritePath = "error";//이미지 현재 없음
                break;
            case NormalCard.BIG_X:
                figure = Figure.X;
                range = 2;
                spritePath = "error";//이미지 현재 없음
                break;
        }
        name = figure.ToString();
    }

    protected override void CardActive()
    {
        if(cardType == CardType.A)//CardType : A
        {
            PlayerData.AkashaGage += 1;
        } else if (cardType == CardType.V)
        {
            player.GetHeal(1); // CardType : V
        }

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].isElite && cardType == CardType.P)//CardType : P
                {
                    DamageToTarget(enemies[i], val1*2);
                }
                else
                {
                    DamageToTarget(enemies[i], val1);
                }
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

          player.MoveTo(player.pos + d * 3);       

        if (TileUtils.IsEnemyInRange(player.currentTile, range, figure))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, range, figure);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

    }

    /// <summary>
    /// effect preview
    /// </summary>
    protected List<Tile> targetTiles;
    public override void CardEffectPreview()
    {

            targetTiles = TileUtils.Range(player.currentTile, range, figure);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, targetTiles[i]));
                if (ranges[i] != null)
                {
                    ranges[i].transform.parent = player.transform;
                }
            }
        
    }

}