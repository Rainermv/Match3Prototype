using UnityEngine;
using System.Collections;

namespace Food{

	public class Slot : MonoBehaviour {

		public bool accept_recipes;
		public bool accept_ingredients;

		private Expression _expression;

		public int index = 0;
		private SpriteSheetManager sprites;
		
		private bool mouse_down;
		
		private bool first_click = true;


	//	private bool destroy =  false;

		public int ID{
			get{return index;}
			set{ index = value;}
		}

		void Awake()
		{
			sprites = SpriteSheetManager.GetInstance ();
		}

		public Expression Expression{
			get{return _expression;}
		}

		public void SetExpression(int sprite_id, Expression expression)
		{
			ChangeId (sprite_id);
			_expression = expression;
			first_click = true;
		}

		public void SetExpression(int id)
		{
			ChangeId (id);

			Number other_number = new Number ();
			other_number.value = id;
			_expression = other_number;
			first_click = true;
		}
		public void ClearExpression()
		{
			ChangeId (0);
			_expression = null;
		}


		// Use this for initialization
		void Start () {
			ClearExpression();
		}

		private void ChangeId(int id){
			ID = id;
			ChangeSprite ();
			Report.SlotAction (GameData.GetInstance ().level_string, gameObject.GetComponent<Slot>());
		}
		
		private void ChangeSprite()
		{
			if (ID > 0)
				GetComponent<SpriteRenderer>().sprite = sprites.GetIngredientSprite (ID);
			else
				GetComponent<SpriteRenderer>().sprite = null;
			
		}

		
		void Update () 
		{
			if (mouse_down) renderer.material.color = Color.gray;
			else renderer.material.color = Color.white;
			
		}
		
		void OnMouseDown(){
			if(Support.interactionEnable) mouse_down = true;
			
		}
		
		void OnMouseUp(){
			if(Support.interactionEnable){
				mouse_down = false;
				
				if (first_click){
					first_click = false;
					return;
				}
				
				if (!first_click){
					ClearExpression();
					first_click = true;
					return;
				}
	
			}
		}
		
		
	}

}

