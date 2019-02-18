using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arch;
/// <summary>
/// 이미지들 , prefab들 캐시해놓고 생성해서 전달
/// </summary>
/// 
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
        else
        {
            UnityEngine.Debug.LogError("SingleTone Error : ArchLoader");
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
    #endregion
}
