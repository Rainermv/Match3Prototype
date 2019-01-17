using UnityEngine;
using System.Collections;

public class SpriteSheetManager : MonoBehaviour {

	// SINGLETON
	private static SpriteSheetManager instance;
	
	private SpriteSheetManager()
	{
		
	}
	
	public static SpriteSheetManager GetInstance(){
		return GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SpriteSheetManager> ();
	}

	//---------------------------------------------------------

	public Sprite[] IngredientsSpriteSheet;
	public Sprite[] SignSpriteSheet;

	public Sprite GetIngredientSprite(int id){
		if(id >= 0)
			if (IngredientsSpriteSheet [id] != null)
				return  IngredientsSpriteSheet [id];

		return IngredientsSpriteSheet[0];
	}

	public Sprite GetSignSprite(int id){
		if(id >= 0)
			if (SignSpriteSheet [id] != null)
				return  SignSpriteSheet [id];
		
		return SignSpriteSheet[0];
	}
}
