using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
using System;
/// <summary>
/// 이미지 , prefab , 이펙트 들 캐시해놓고 생성해서 전달
/// </summary>

public enum CardEffect { HEAL, TELEPORT, REINFORCE, EMPTY, SMOKE, RADAR, HIT, DRAIN, HITA, FIRE, EXPLOSION, OUTWARD, AIR, DARKSUN, HEART, FIREBALLC, FIREBALLA, SLASHORANGE, SLASHBLUE, POWERAURA ,CLAWS,FIRESPIN,FREEZING,THUNDER}
public enum EnemyEffect { SPACE, POW, FORCE, ELECTRICG, HITBLUE, FIREBREATH, EXPLOSIONB, ENEMYEXPLOSION, ENEMYEXPLOSIONC,DIE }
public enum RangeEffectType { CARD, ENEMY, DIR }

public class ArchLoader : MonoBehaviour {
    //Changes: 캐싱하고 파싱하는걸 메인메뉴씬으로 옮겨둠
    public static ArchLoader instance;

    private void Awake()
    {//Changes: destroy 부분 추가
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            UnityEngine.Debug.LogWarning("SingleTone Error : " + this.name);
            Destroy(gameObject);
        }
    }

    private bool isCached;
    public bool IsCached
    {
        get { return isCached; }
    }

    public void StartCache()
    {
        if (isCached) return;
        CacheEntity();
        CacheTile();
        CacheOffTile();
        CachePlayer();
        CacheRoom();
        CacheCardObject();
        CacheEffects();
        CacheBGM();
        isCached = true;
    }

    #region Get
    public Sprite GetWarningImage()
    {
        return warningImage;
    }
    public Sprite GetCardAttribute(CardType type)
    {
        switch(type)
        {
            case CardType.A:
                return attributes["apas"];
            case CardType.P:
                return attributes["prithivi"];
            case CardType.T:
                return attributes["tejas"];
            case CardType.V:
                return attributes["vayu"];
            case CardType.S:
                return attributes["akasha"];
            default :
                return null;
        }
    }
    public Sprite GetCardRangeImage(string name)
    {
        return cardRangeImage[name];
    }

    public HandCardObject GetCardObject()
    {
        return Instantiate(cardObject).GetComponent<HandCardObject>();
    }
    public EditCardObject GetEditCard()
    {
        return Instantiate(editCardObject).GetComponent<EditCardObject>();
    }
    public CheckCardObject GetCheckCard()
    {
        return Instantiate(checkCardObject).GetComponent<CheckCardObject>();
    }

    public Sprite GetCardSprite(string name)
    {
        if (cardSprites.ContainsKey(name))
            return cardSprites[name];
        else
            return cardSprites["error"];
    }
    public Sprite GetDoorSprite(string name)
    {
        return doorSprites[name];
    }
    public Sprite GetCardFrame(string name)
    {
        return cardFrame[name];
    }
    public Room GetRoom()
    {
        return Instantiate(room).GetComponent<Room>();
    }
    public Entity GetEntity(int num)
    {
        if(num<4000)       ///Structure일때는 이미지 적용시켜줘야함
        {
            Structure s = Instantiate(enemies[0]).GetComponent<Structure>();
            s.SetSprite(structureImages[num]);
            return s;     
        }
        else
        {
            return Instantiate(enemies[num]).GetComponent<Entity>();
        }
    }

    public Tile GetTile(int num)
    {
        //타일 생성하고 이미지 입힌다음 리턴
        Tile t = Instantiate(tile).GetComponent<Tile>();
        if(num!=0 && tileImages.ContainsKey(num))
        {
            t.SetSprite(tileImages[num]);
        }
        return t;
    }
    public OffTile GetOffTile(int num)
    {
        return Instantiate(offtiles[num]).GetComponent<OffTile>();
    }
    public Player GetPlayer()
    {
        return Instantiate(player).GetComponent<Player>();
    }
    public AudioClip GetBGM(BGM bgm)
    {
        return bgms[bgm.ToString()];
    }
    public AudioClip GetMono(MonoSound mono)
    {
        return monos[mono.ToString()];
    }
    public AudioClip GetSoundEffect(SoundEffect sound)
    {
        return soundEffects[sound.ToString()];
    }
    public EffectObject GetSoundObject()
    {
        return soundObject;
    }
    public Sprite GetPopUpImage(string s)
    {
        return popUpImages[s];
    }
    #endregion
    #region Get EFFECT

    public Dictionary<CardEffect, EffectObject> GetCardEffect()
    {
        return cardEffectPrefabs;
    }

    public Dictionary<EnemyEffect,EffectObject> GetEnemyEffectDictionary()
    {
        return monsterEffectPrefabs;
    }

    public Dictionary<RangeEffectType, EffectObject> GetRangeEffectDictionary()
    {
        return rangeEffectPrefabs;
    }

    public EffectObject GetDamageEffect()
    {
        return textEffectPrefab;
    }

    #endregion
    #region Cache
    /// <summary>
    /// Entity들 로딩 , Structure는 이미지도 로딩
    /// </summary>
    Dictionary<int, Sprite> structureImages = new Dictionary<int, Sprite>();
    Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
    Sprite warningImage;
    private void CacheEntity()
    {
        GameObject[] objects = Resources.LoadAll<GameObject>("Fields/Entity");
        for (int i = 0; i < objects.Length; i++)
        {
             enemies.Add(int.Parse(objects[i].name), objects[i]);           
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>("Graphic/Entity/Structure");
        for(int i=0; i<sprites.Length;i++)
        {
            structureImages.Add(int.Parse(sprites[i].name), sprites[i]);
        }
        warningImage = Resources.Load<Sprite>("Graphic/warning");
    }

    /// <summary>
    /// Tile 로드 하고 , 이미지들도 로드
    /// </summary>
    GameObject tile;
    Dictionary<int, Sprite> tileImages = new Dictionary<int, Sprite>();
    private void CacheTile()
    {
        tile = Resources.Load<GameObject>("Fields/Tile/tile");
        Sprite[] sprites = Resources.LoadAll<Sprite>("Graphic/Tile");
        for(int i=0; i<sprites.Length;i++)
        {
            tileImages.Add(int.Parse(sprites[i].name), sprites[i]);
        }
    }
    /// <summary>
    /// Offtile들 로딩 , Door Sprite들도 로딩
    /// </summary>
    Dictionary<int, GameObject> offtiles = new Dictionary<int, GameObject>();
    Dictionary<string, Sprite> doorSprites = new Dictionary<string, Sprite>();
    private void CacheOffTile()
    {
        GameObject[] objects = Resources.LoadAll<GameObject>("Fields/OffTile");
        for(int i=0; i<objects.Length;i++)
        {
            offtiles.Add(int.Parse(objects[i].name), objects[i]);
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>("Graphic/OffTile/Door");
        for (int i = 0; i < sprites.Length; i++)
        {
            doorSprites.Add(sprites[i].name, sprites[i]);
        }
    }

    /// <summary>
    /// Player로딩
    /// </summary>
    GameObject player;
    private void CachePlayer()
    {
        player =  Resources.Load<GameObject>("Player");
    }

    GameObject room;
    private void CacheRoom()
    {
        room = Resources.Load<GameObject>("Room");
    }

    GameObject cardObject;
    GameObject editCardObject;
    GameObject checkCardObject;

    Dictionary<string, Sprite> cardRangeImage = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> cardSprites = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> cardFrame = new Dictionary<string, Sprite>();
    Dictionary<string, Sprite> attributes = new Dictionary<string, Sprite>();
    private void CacheCardObject()
    {
        cardObject = Resources.Load<GameObject>("Card/HandCardObject");
        editCardObject = Resources.Load<GameObject>("Card/EditCardObject");
        checkCardObject = Resources.Load < GameObject >( "Card/CheckCardObject" );
        Sprite[] sprites = Resources.LoadAll<Sprite>("Card/Graphic");
        for(int i=0; i<sprites.Length;i++)
        {
            cardSprites.Add(sprites[i].name, sprites[i]);
        }

        sprites = Resources.LoadAll<Sprite>("Card/Frame");
        for(int i=0; i<sprites.Length;i++)
        {
            cardFrame.Add(sprites[i].name, sprites[i]);
        }

        sprites = Resources.LoadAll<Sprite>("Attribute");
        for (int i = 0; i < sprites.Length; i++)
        {
            attributes.Add(sprites[i].name, sprites[i]);
        }

        sprites = Resources.LoadAll<Sprite>("Card/Range");
        for(int i=0; i<sprites.Length;i++)
        {
            cardRangeImage.Add(sprites[i].name, sprites[i]);
        }
    }


    Dictionary<CardEffect, EffectObject> cardEffectPrefabs = new Dictionary<CardEffect, EffectObject>();
    Dictionary<EnemyEffect, EffectObject> monsterEffectPrefabs = new Dictionary<EnemyEffect, EffectObject>();
    Dictionary<RangeEffectType, EffectObject> rangeEffectPrefabs = new Dictionary<RangeEffectType, EffectObject>();
    Dictionary<string, Sprite> popUpImages = new Dictionary<string, Sprite>();
    EffectObject textEffectPrefab;

    private void CacheEffects()
    {
        EffectObject[] prefabs = Resources.LoadAll<EffectObject>("Effect/CARD");//카드 이펙트
         for(int i=0; i<prefabs.Length; i++)
        {
            cardEffectPrefabs.Add((CardEffect)Enum.Parse(typeof(CardEffect),prefabs[i].gameObject.name),prefabs[i]);
        }
        prefabs = Resources.LoadAll<EffectObject>("Effect/ENEMY");
        for (int i = 0; i < prefabs.Length; i++)
        {
            monsterEffectPrefabs.Add((EnemyEffect)Enum.Parse(typeof(EnemyEffect),prefabs[i].gameObject.name), prefabs[i]);
        }
        prefabs = Resources.LoadAll<EffectObject>("EFFECT/RANGE");
        for (int i = 0; i < prefabs.Length; i++)
        {
            rangeEffectPrefabs.Add((RangeEffectType)Enum.Parse(typeof(RangeEffectType), prefabs[i].gameObject.name), prefabs[i]);
        }
        Sprite[] images = Resources.LoadAll<Sprite>("Graphic/UI/PopUP");
        for(int i=0; i<images.Length;i++)
        {
            popUpImages.Add(images[i].name, images[i]);
        }

        textEffectPrefab = Resources.Load<EffectObject>("EFFECT/TEXT/DamageText");

    }

    Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> bgms = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> monos = new Dictionary<string, AudioClip>();
    EffectObject soundObject;

    private void CacheBGM()
    {
        AudioClip[] prefabs = Resources.LoadAll<AudioClip>("SOUND/BGM");
        for (int i = 0; i < prefabs.Length; i++)
        {
            bgms.Add(prefabs[i].name, prefabs[i]);
        }
        prefabs = Resources.LoadAll<AudioClip>("SOUND/MONO");
        for (int i = 0; i < prefabs.Length; i++)
        {
            monos.Add(prefabs[i].name, prefabs[i]);
        }
        prefabs = Resources.LoadAll<AudioClip>("SOUND/SFX");
        for (int i = 0; i < prefabs.Length; i++)
        {
            soundEffects.Add(prefabs[i].name, prefabs[i]);
        }

        soundObject = Resources.Load<EffectObject>("SOUND/SoundObject");
    }
    #endregion
}
