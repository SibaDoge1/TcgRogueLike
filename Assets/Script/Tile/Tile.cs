using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arch{
	public class Tile :MonoBehaviour
	{
        #region variables
        public int objectNum;

        public List<Tile> neighbours;

	    public Vector2Int pos;
		private Entity onTileObj;//타일위에 있는 mapObject
	    public Entity OnTileObj {
			get {
				return onTileObj;
			}
			set {
				onTileObj = value;
                SomethingUpOnThis(value);
			}
		}
		private OffTile _offTile;
		public OffTile offTile
        {
            get
            { return _offTile; }
            set
            {
                if (_offTile != null && _offTile.isEvent)
                    return;

                _offTile = value;

                if(_offTile !=null)
                _offTile.CurrentTile = this;
            }
        }
        #endregion


        public void SetTile(Vector2Int _pos,Vector2Int roomSize)
		{
			pos = _pos;
			transform.localPosition = new Vector3(pos.x,pos.y, 0);

		}
	
	    public bool IsStandAble(Entity ot)
		{			
				if (onTileObj != null || (offTile != null && !offTile.IsStandAble(ot)))
                {
					return false;
				} 
                else if(objectNum == 0 && ot is Character)
                {
					return false;
				}
                else
                {
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