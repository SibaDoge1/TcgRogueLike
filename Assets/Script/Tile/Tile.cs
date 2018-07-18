using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arch{
	public class Tile :MonoBehaviour
	{
		void Awake(){
			sprite = GetComponent<SpriteRenderer> ();
		}

	    #region variables
		public List<Tile> neighbours;
	    private SpriteRenderer sprite;
		public SpriteRenderer mySprite{
			get { return sprite; }
		}
	    public Vector2Int pos;
		private Entity onTileObj;//타일위에 있는 mapObject
	    public Entity OnTileObj {
			get {
				return onTileObj;
			}
			set {
				onTileObj = value;
				if (onTileObj != null) {
					SomethingUpOnThis (onTileObj);
				}
			}
		}
		private OffTile _offTile;
		public OffTile offTile
        { get { return _offTile; }  set { _offTile = value; _offTile.ThisTile = this; } }
        private EventLayer _eventLayer;
        public EventLayer eventLayer
        { get { return _eventLayer;  } set { _eventLayer = value; _eventLayer.ThisTile = this; } }

	    #endregion
	    /// <summary>
	    /// Defalut : GroundTile
	    /// </summary>
	    public void SetTile(Vector2Int _pos,Vector2Int roomSize)
		{
			pos = _pos;
			transform.localPosition = new Vector3(0.5f + pos.y, -0.5f-pos.x, 0);

            name = "Tile" + _pos;
		}
	
	    public bool IsStandAble(Entity ot)
		{			
				if (OnTileObj) {
					return false;
				} else {
					return true;
				}			
		}
    	public void SomethingUpOnThis (Entity ot)
		{
			if (offTile != null) {
				offTile.SomethingUpOnThis (ot);
			}
            if(eventLayer != null)
            {
                eventLayer.SomethingUpOnThis(ot);
            }
		}       
	}
}