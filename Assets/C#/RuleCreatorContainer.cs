using UnityEngine;
using System.Collections;

namespace Food{
	public class RuleCreatorContainer : InterfaceComponent {
	
		public GameObject[] button_recipes;
//		public GameObject AND;
//		public GameObject OR;
//		public GameObject NOT;
//		public GameObject TEO;
//		public GameObject IMP;

		public void OpenRecipes(GameObject open_button){
		
			foreach (GameObject button_recipe in button_recipes){
				
				if (!button_recipe.activeInHierarchy)
					return;
				
				if (button_recipe == open_button)	button_recipe.SendMessage("Open");
				else button_recipe.SendMessage("Close");
			}
		}
		
		public void OpenRecipes(ExpressionType type){
			foreach (GameObject button_recipe in button_recipes){
				
				if (!button_recipe.activeInHierarchy)
					return;
					
				if (button_recipe.GetComponent<Button_Recipes>().type == type)	button_recipe.SendMessage("Open");
				else button_recipe.SendMessage("Close");
			}
		}
		
		public void CloseRecipes(){
			
			foreach (GameObject button_recipe in button_recipes){
				if (button_recipe.activeInHierarchy)
					button_recipe.SendMessage("Close");
			}
			
		}
		
//		public void SetAnd(bool value){
//			//Debug.Log ("SETAND: " + value);
//			if (AND!= null) AND.GetComponent<InterfaceComponent>().SetAvailable(value);
//		}
//		public void SetOr(bool value){
//			if (OR!= null) OR.GetComponent<InterfaceComponent>().SetAvailable(value);
//		}
//		public void SetNot(bool value){
//			if (NOT!= null) NOT.GetComponent<InterfaceComponent>().SetAvailable(value);
//		}
//		public void SetTeo(bool value){
//			if (TEO!= null) TEO.GetComponent<InterfaceComponent>().SetAvailable(value);
//		}
//		public void SetImp(bool value){
//			if (IMP!= null) IMP.GetComponent<InterfaceComponent>().SetAvailable(value);
//		}
	}
}