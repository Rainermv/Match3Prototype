using UnityEngine;
using System.Collections;

namespace Food{
	public class RuleRepresentationComponent : MonoBehaviour {
	
		public Sprite default_sprite;
	
		private int index = 0;
		private int c_max = 6;
		private SpriteSheetManager sprites;
	
		public int Index{
			get{return index;}
			set{ index = Mathf.Clamp(value,0,c_max);}
		}
	
		void Awake()
		{
			sprites = SpriteSheetManager.GetInstance ();
			c_max = sprites.IngredientsSpriteSheet.Length +1;
			
		}
	
		public void ChangeId(int id){
			Index = id;
			//ChangeColor ();
			ChangeSprite ();
		}
	
		private void ChangeSprite()
		{
			GetComponent<SpriteRenderer>().sprite = sprites.GetIngredientSprite (index);
			
		}
		
	}
}
