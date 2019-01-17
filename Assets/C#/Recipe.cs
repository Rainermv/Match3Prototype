using UnityEngine;
using System.Collections;

namespace Food{

	public class Recipe : MonoBehaviour {

		public Expression expression;
		private SpriteSheetManager sprites;
		//private ExpressionType type;
		public int ID;
	
	//	private bool mouse_down = false;
		
		//private bool selected = false;

		void Awake()
		{
			sprites = SpriteSheetManager.GetInstance ();
		
		}

		// Use this for initialization 
		void Start () {
			//SetType (ExpressionType.And);
		}
		
		void Update () 
		{
			
		}
		
		public void Select(){
			//this.selected = true;
			renderer.material.color = Color.gray;
		}
		
		public void Unselect(){
			//this.selected = false;
			renderer.material.color = Color.white;
		}
		
		void OnMouseDown(){
			//if(Support.interactionEnable) this.mouse_down = true;
			//selected = !selected;
		}
		
		void OnMouseUp(){
			//if(Support.interactionEnable) this.mouse_down = false;
		}

		public void SetType(ExpressionType type){
			//this.type = type;
			switch (type){
			case ExpressionType.And: ID = 1; break;
			case ExpressionType.Or: ID = 2; break;
			case ExpressionType.Theorem: ID = 3; break;
			case ExpressionType.Not: ID = 4; break;
			default: ID = 0; break;
			}
			//Debug.Log ("Type changed to " + type);
			ChangeSprite();
			
		}
		
		private void ChangeSprite()
		{
			GetComponent<SpriteRenderer>().sprite = sprites.GetIngredientSprite (ID);
			if (ID == 0)
				renderer.material.color = Color.clear;	
			else
				renderer.material.color = Color.white;
			
		}
		
		public void Remove(){
			SendMessageUpwards("RemoveRecipe",gameObject);
		}



	}

}

