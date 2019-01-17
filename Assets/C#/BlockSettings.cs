using UnityEngine;
using System.Collections;

namespace Food {

	public class BlockSettings{
		
		private Vector2 position;
		public Vector2 GridPosition
		{
			get{return position;}
			set{int x = (int)value.x;
				int y = (int)value.y;
				position.x = x;
				position.y = y;}
		}
		public int id;
		
	}
}
