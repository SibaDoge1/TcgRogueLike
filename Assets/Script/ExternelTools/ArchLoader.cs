using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
/// <summary>
/// 이미지 , prefab , 이펙트 들 캐시해놓고 생성해서 전달
/// </summary>

public enum CardEffect { HEAL, TELEPORT, REINFORCE, EMPTY, SMOKE, RADAR, HIT, DRAIN, HITA, FIRE, EXPLOSION, OUTWARD, AIR, DARKSUN, HEART, FIREBALLC, FIREBALLA, SLASHORANGE, SLASHBLUE, POWERAURA ,CLAWS,FIRESPIN,FREEZING,THUNDER}
public enum EnemyEffect { SPACE, POW, FORCE, ELECTRICG, HITBLUE, FIREBREATH, EXPLOSIONB, ENEMYEXPLOSION, ENEMYEXPLOSIONC,DIE }
public enum RangeEffectType { CARD, ENEMY, DIR }
public enum UIEffect { CARD, REPORT, ELECTRICMESH }


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
            UnityEngine.Debug.LogError("SingleTone Error : " + this.name);
            Destroy(this);
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
        if(num!=0)
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
    public AudioClip GetSoundEffect(EffectSound sound)
    {
        return soundEffects[sound.ToString()];
    }
    public AudioSource GetSoundObject()
    {
        return soundObject;
    }
    #endregion
    #region MADE EFFECT

    public GameObject MadeEffect(CardEffect eType, Vector3 position)
    {
        return Instantiate(cardEffectPrefabs[eType.ToString()], position + new Vector3(0,0.5f),Quaternion.identity);
    }
    public GameObject MadeEffect(CardEffect eType, Tile position)
    {
        return Instantiate(cardEffectPrefabs[eType.ToString()], position.transform.position + new Vector3(0, 0.5f), Quaternion.identity);
    }


    public GameObject MadeEffect(EnemyEffect eType, Vector3 position)
    {
        return Instantiate(monsterEffectPrefabs[eType.ToString()], position + new Vector3(0, 0.5f), Quaternion.identity);
    }
    public GameObject MadeEffect(EnemyEffect eType, Tile position)
    {
        return Instantiate(monsterEffectPrefabs[eType.ToString()], position.transform.position + new Vector3(0, 0.5f), Quaternion.identity);
    }
    public GameObject MadeEffect(RangeEffectType eType, Entity entity,Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure || targetTile.tileNum == 0)
            return null;
        GameObject go = Instantiate(rangeEffectPrefabs[eType.ToString()]);
        Vector2 dif = targetTile.pos - entity.pos;
        go.transform.parent = entity.transform;
        go.transform.localPosition = dif;

        return go;
    }

    public GameObject MadeEffect(RangeEffectType range, Tile targetTile)
    {
        if (targetTile == null || targetTile.OnTileObj is Structure || targetTile.tileNum == 0)
            return null;
        GameObject go = Instantiate(rangeEffectPrefabs[range.ToString()],
     targetTile.transform.position, Quaternion.identity);

        return go;
    }

    public GameObject MadeEffect(UIEffect eType, Transform parent)
    {
        return Instantiate(uiEffectPrefabs[eType.ToString()], parent);
    }
    public GameObject MadeEffect(int damage, Transform parent)
    {
        GameObject go =  Instantiate(textEffectPrefab, parent);
        go.GetComponent<EffectText>().Init(damage.ToString(), damage > 0 ? TextColorType.Green : TextColorType.Red);
        return go;
    }
    public GameObject MadeEffect(int damage, Vector3 position)
    {
        GameObject go = Instantiate(textEffectPrefab, position,Quaternion.identity);
        go.GetComponent<EffectText>().Init(damage.ToString(), damage > 0 ? TextColorType.Green : TextColorType.Red);
        return go;
    }
    /// <summary>
    /// Range Effect Delete할때 사용
    /// </summary>
    public void DestroyEffect(List<GameObject> go)
    {
        if (go != null)
        {
            for (int i = 0; i < go.Count; i++)
            {
                DestroyImmediate(go[i]);
            }
        }
    }
    public void DestroyEffect(GameObject go)
    {
        DestroyImmediate(go);
    }
    #endregion
    #region Cache
    /// <summary>
    /// Entity들 로딩 , Structure는 이미지도 로딩
    /// </summary>
    Dictionary<int, Sprite> structureImages = new Dictionary<int, Sprite>();
    Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();
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
    }


    Dictionary<string, GameObject> cardEffectPrefabs = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> monsterEffectPrefabs = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> uiEffectPrefabs = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> rangeEffectPrefabs = new Dictionary<string, GameObject>();

    GameObject textEffectPrefab;

    private void CacheEffects()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Effect/CARD");//카드 이펙트
         for(int i=0; i<prefabs.Length; i++)
        {
            cardEffectPrefabs.Add(prefabs[i].name,prefabs[i]);
        }
        prefabs = Resources.LoadAll<GameObject>("Effect/ENEMY");
        for (int i = 0; i < prefabs.Length; i++)
        {
            monsterEffectPrefabs.Add(prefabs[i].name, prefabs[i]);
        }
        prefabs = Resources.LoadAll<GameObject>("EFFECT/UI");
        for (int i = 0; i < prefabs.Length; i++)
        {
            uiEffectPrefabs.Add(prefabs[i].name, prefabs[i]);
        }
        prefabs = Resources.LoadAll<GameObject>("EFFECT/RANGE");
        for (int i = 0; i < prefabs.Length; i++)
        {
            rangeEffectPrefabs.Add(prefabs[i].name, prefabs[i]);
        }

        textEffectPrefab = Resources.Load<GameObject>("EFFECT/TEXT/DamageText");

    }

    Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> bgms = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> monos = new Dictionary<string, AudioClip>();
    AudioSource soundObject;

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

        soundObject = Resources.Load<GameObject>("SOUND/SoundObject").GetComponent<AudioSource>();
    }
    #endregion
}
