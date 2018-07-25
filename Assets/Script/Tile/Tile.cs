using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arch{
	public class Tile :MonoBehaviour
	{
        static GameObject Load;//rangeLayer
        GameObject rangeLayer;
       
        public void InstantiateRangeLayer(Color c)
        {
            if(Load == null)
                Load = (GameObject)Resources.Load("RangeLayer");

            if(rangeLayer == null)
            {
                rangeLayer = Instantiate(Load, transform.position, Quaternion.identity);
                rangeLayer.GetComponent<RangeLayer>().SetColor(c);
            }
        }
        public void DestroyRangeLayer()
        {
            DestroyImmediate(rangeLayer);           
        }
        #region variables
        public List<Tile> neighbours;

	    public Vector2Int pos;
		private Entity onTileObj;//타일위에 있는 mapObject
	    public Entity OnTileObj {
			get {
				return onTileObj;
			}
			set {
				onTileObj = value;
			}
		}
		private OffTile _offTile;
		public OffTile offTile
        {
            get
            { return _offTile; }
            set
            {
                if (_offTile != null && offTile is EventLayer)
                    return;

                _offTile = value; _offTile.ThisTile = this;
            }
        }

	    #endregion
	    /// <summary>
	    /// Defalut : GroundTile
	    /// </summary>
	    public void SetTile(Vector2Int _pos,Vector2Int roomSize)
		{
			pos = _pos;
			transform.localPosition = new Vector3(pos.x,pos.y, 0);

            name = "Tile" + _pos;
		}
	
	    public bool IsStandAble(Entity ot)
		{			
				if (onTileObj != null) {
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
		}       
	}
}