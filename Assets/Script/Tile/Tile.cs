using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arch{
	public class Tile :MonoBehaviour
	{
	    #region variables
		public List<Tile> neighbours;
	    SpriteRenderer sprite;
	    public Vector2Int pos;
		private OnTileObject onTileObj;//타일위에 있는 mapObject
	    public OnTileObject OnTileObj {
			get {
				return onTileObj;
			}
			set {
				onTileObj = value;
				SomethingUpOnThis (onTileObj);
			}
		}
		private OffTile offTileObj;
	    public OffTile OffTileObj {
			set {
				if (offTileObj != null)
					Destroy (offTileObj.gameObject);
				offTileObj = value;
				offTileObj.setTile (this);
			}
		}
	    #endregion
	    /// <summary>
	    /// Defalut : GroundTile
	    /// </summary>
	    public void SetTile(Vector2Int _pos)
		{
			pos = _pos;
			transform.localPosition = MapGenerator.instance.GetTilePosition (_pos);
	
			name = "Tile" + _pos;
		}
	
	    public bool IsStandAble(OnTileObject ot)
		{
			if (offTileObj != null) {
				return offTileObj.IsStandAble (ot);
			} else {
				if (OnTileObj) {
					return false;
				} else {
					return true;
				}
			}
		}
    	public void SomethingUpOnThis (OnTileObject ot)
		{
			if (offTileObj != null) {
				offTileObj.SomethingUpOnThis (ot);
			}
		}

	}
}