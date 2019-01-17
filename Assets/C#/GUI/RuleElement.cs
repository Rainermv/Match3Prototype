using UnityEngine;
using System.Collections;

namespace Food{

//	public enum RuleElementType{
//	
//		recipe,
//		ingredient,
//		plusSign,
//		barSign,
//		arrowSign
//	
//	}

	public class RuleElement : MonoBehaviour {
	
		private int index = 0;
		private SpriteSheetManager sprites;
		//private int c_max;
		
		
		//public float scale;
		
		public float Scale{
			get{return transform.lossyScale.y;}
			set{transform.localScale =  new Vector3(Mathf.Clamp(value,0.5f,2f),Mathf.Clamp(value,0.5f,2f),1f);}
		}
	
		public int ID{
			get{return index;}
			set{ index = value;}
		}
		
		void Awake()
		{
			sprites = SpriteSheetManager.GetInstance ();
			
		}
		
		// Use this for initialization 
		void Start () {

		}

		public void ScaleTo(float scale){
			Scale = scale;
		}
		
//		public void SetType(RuleElementType type){
//		
//			switch (type){
//			
//				case RuleElementType.ingredient: 	LoadIngredient (); 	break;
//				case RuleElementType.plusSign: 	 	LoadSign (0);		break;
//				case RuleElementType.barSign: 		LoadSign (1); 		break;
//				case RuleElementType.arrowSign: 	LoadSign (2);		break;
//			}
//		
//		}
		
		private void LoadSign(int index){

			GetComponent<SpriteRenderer>().sprite = sprites.GetSignSprite(index-1);
		}
		
		private void LoadIngredient(int index){
			GetComponent<SpriteRenderer>().sprite = sprites.GetIngredientSprite (index);
		}

			
	}
}
