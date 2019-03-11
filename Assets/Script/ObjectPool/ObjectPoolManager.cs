using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Arch;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this.gameObject);
        }
    }

    public void MakeEffects()
    {
        MakeCardEffects();
        MakeEnemyEffects();
        MakeRangeEffects();
        MakeTextEffects();
        MakeSoundEffects();
    }

    private ObjectPool GetObjectPool(EffectObject poolingObject, int num)
    {
        ObjectPool pool =  new GameObject(poolingObject.gameObject.name+" Pool").AddComponent<ObjectPool>();
        pool.transform.parent = transform;
        pool.SetEffect(poolingObject,num);
        return pool;
    }

    #region CardEffectPool

    Dictionary<CardEffect, ObjectPool> cardEffectPool = new Dictionary<CardEffect, ObjectPool>();
    /*public void AddCardEffectPool(CardEffect card)
     {
        if (cardEffectPool.ContainsKey(card))
        {
            Debug.Log(card.ToString() + "is already exists in cardEffectPool");
        }
        else
        {
            cardEffectPool[card] = GetObjectPool(ArchLoader.instance.GetCardEffect(card),20);
        }
     }

    public void DeleteCardEffectPool(CardEffect card)
    {
        if(cardEffectPool.ContainsKey(card))
        {
            ObjectPool temp = cardEffectPool[card];
            cardEffectPool.Remove(card);
            temp.DestroyAll();
        }else
        {
            Debug.LogError(card.ToString() + "isn't exists in cardEffectPool");
        }
    }

    public void UpdateCardEffectPool(List<Card> deck)
    {
        for(int i=0; i<deck.Count;i++)
        {
            AddCardEffectPool(deck[i].CardEffect);
        }
    }*/
    private void MakeCardEffects()
    {
        Dictionary<CardEffect, EffectObject> dictionary = ArchLoader.instance.GetCardEffect();
        List<CardEffect> keys = dictionary.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
        {
            cardEffectPool[keys[i]] = GetObjectPool(dictionary[keys[i]], 8);
        }
    }


    #endregion

    #region EnemyEffectPool
    Dictionary<EnemyEffect, ObjectPool> enemyEffectPools = new Dictionary<EnemyEffect, ObjectPool>();
    private void MakeEnemyEffects()
    {
       Dictionary<EnemyEffect,EffectObject> dictionary = ArchLoader.instance.GetEnemyEffectDictionary();
        List<EnemyEffect> keys = dictionary.Keys.ToList();
        
        for(int i=0; i<keys.Count;i++)
        {
            enemyEffectPools[keys[i]] = GetObjectPool(dictionary[keys[i]],20);
        }
    }
    #endregion

    #region RangeEffectPool
    Dictionary<RangeEffectType, ObjectPool> rangeEffectPools= new Dictionary<RangeEffectType, ObjectPool>();
    private void MakeRangeEffects()
    {
        Dictionary<RangeEffectType, EffectObject> dictionary = ArchLoader.instance.GetRangeEffectDictionary();
        List<RangeEffectType> keys = dictionary.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
        {
            rangeEffectPools[keys[i]] = GetObjectPool(dictionary[keys[i]], 20);
        }
    }
    #endregion

    #region textEffectPool
    ObjectPool damageEffectPool;
    private void MakeTextEffects()
    {
        damageEffectPool = GetObjectPool(ArchLoader.instance.GetDamageEffect(),20);
    }
    #endregion

    #region SoundEffectPool
    ObjectPool soundEffectPool;
    private void MakeSoundEffects()
    {
        soundEffectPool = GetObjectPool(ArchLoader.instance.GetSoundObject(),30);
    }
    #endregion

    #region Get
    public void PoolEffect(CardEffect card,Vector3 position)
    {
        cardEffectPool[card].ActiveObject(position + new Vector3(0, 0.5f));
    }
    public void PoolEffect(CardEffect card, Tile tile)
    {
        cardEffectPool[card].ActiveObject(tile.transform.position + new Vector3(0, 0.5f));
    }

    public void PoolEffect(EnemyEffect enemy,Vector3 pos)
    {
        enemyEffectPools[enemy].ActiveObject(pos + new Vector3(0, 0.5f));
    }
    public void PoolEffect(EnemyEffect enemy, Tile pos)
    {
        enemyEffectPools[enemy].ActiveObject(pos.transform.position + new Vector3(0, 0.5f));
    }

    public void PoolEffect(int damage, Vector3 position)
    {
        EffectText text = damageEffectPool.ActiveObject(position) as EffectText;
        text.Init(damage.ToString(), damage > 0 ? TextColorType.Green : TextColorType.Red);
    }

    public EffectObject PoolRangeEffect(RangeEffectType range, Entity entity, Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure || targetTile.tileNum == 0)
            return null;
        EffectObject effect = rangeEffectPools[range].ActiveObject();
        Vector2 dif = targetTile.pos - entity.pos;
        effect.transform.parent = entity.transform;
        effect.transform.localPosition = dif;

        return effect;
    }

    public EffectObject PoolRangeEffect(RangeEffectType range, Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure || targetTile.tileNum == 0)
            return null;
        EffectObject effect = rangeEffectPools[range].ActiveObject(targetTile.transform.position);

        return effect;
    }
    public SoundObject PoolSoundEffect()
    {
         return (soundEffectPool.ActiveObject() as SoundObject);
    }



    /// <summary>
    /// Range Effect Delete할때 사용
    /// </summary>
    public void DeActiveRange(List<EffectObject> go)
    {
        if (go != null)
        {
            for (int i = 0; i < go.Count; i++)
            {
                if(go[i] != null)
                {
                    go[i].OffObject();
                }
            }
        }
    }
    public void DeActiveEffect(EffectObject go)
    {
        go.OffObject();
    }
    #endregion
}