using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Food;


public class Button_Recipes : MonoBehaviour {

	public GameObject recipe_reference;
	private List<GameObject> recipes = new List<GameObject>();
	public GameObject recipes_position;
	public ExpressionType type;

	bool showing;
	bool mouse_down = false;
	
	int recipes_max = 5;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		showing = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mouse_down) renderer.material.color = Color.gray;
		else renderer.material.color = Color.white;
		
	}

	void AddRecipe(Expression recipe_expression)
	{
		GameObject new_recipe = (GameObject)Instantiate (recipe_reference, recipes_position.transform.position,Quaternion.identity);
		new_recipe.GetComponent<Recipe> ().expression = recipe_expression;
		//Debug.Log ("Setting type to " + type);
		new_recipe.GetComponent<Recipe> ().SetType(type);
		recipes.Add (new_recipe);
		Report.CreateRecipe (GameData.GetInstance ().level_string, new_recipe.GetComponent<Recipe> ());
		
		new_recipe.transform.parent = recipes_position.transform;
		Reorganize ();
		//new_recipe.transform.Translate(0,1 -recipes.Count,0);
		recipes_position.SetActive (showing);
		//recipe.transform.position = Vector3(Transform.position.

	}
	
	void Reorganize(){
		
		int i = 0;
		foreach (GameObject recipe in recipes){
			
			recipe.transform.position = new Vector2 (recipes_position.transform.position.x, recipes_position.transform.position.y - i);
			i++;
		
		}
	
	}
	
	void RemoveRecipe(GameObject recipe_to_delete){
	
		foreach (GameObject recipe in recipes){
			if (recipe_to_delete == recipe){
				Destroy (recipe_to_delete);
				recipes.Remove(recipe_to_delete);
				Reorganize();
				return;
				}	
		}
		
	}
	
	public bool CanAdd(){
		if (recipes.Count+1 <= recipes_max)
			return true;
		//else

		return false;
	}
	
	public void Open(){
		recipes_position.SetActive(true);
		showing = true;
	}
	
	public void Close(){
		recipes_position.SetActive(false);
		showing = false;
	}

	void OnMouseDown(){
		if(Support.interactionEnable){
			mouse_down = true;
			showing = !showing;
			
			if (showing)
				gameObject.SendMessageUpwards("OpenRecipes", gameObject);
			else
				gameObject.SendMessageUpwards("CloseRecipes");
				
		}
	}
	
	void OnMouseUp(){
		if(Support.interactionEnable) mouse_down = false;
	}
}
