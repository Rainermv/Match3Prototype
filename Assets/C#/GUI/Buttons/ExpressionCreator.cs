using UnityEngine;
using System.Collections;
using Food;

public enum ExpressionType{
	
	And,
	Or,
	Not,
	Theorem,
	Implication,
}

public class ExpressionCreator : MonoBehaviour {

	public Button_Recipes recipe_container;
	private Main main;
	
	private bool mouse_down = false;

	public ExpressionType type;

	void Awake()
	{
		main = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Main> ();
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mouse_down) renderer.material.color = Color.gray;
		else renderer.material.color = Color.white;

	}

	void OnMouseDown()
	{
		if(!Support.interactionEnable)
			return;
			
		mouse_down = true;
		
		Slot[] children = GetComponentsInChildren<Slot>();

		if (type == ExpressionType.Implication && children[0].Expression != null)// Implication
		{ 
			CreateImpRecipe(children[0].Expression);
			children[0].ClearExpression();
		}

		else if (children[0].Expression != null && children[1].Expression != null)
		{
		
			if (!recipe_container.CanAdd()){
				recipe_container.Open ();
				return;
			}
		
		
			if (type == ExpressionType.And || type == ExpressionType.Theorem){ // AND and THEOREM
				CreateAndRecipe(children[0].Expression, children[1].Expression);
			}
			else if (type == ExpressionType.Or){ // OR
				CreateOrRecipe(children[0].Expression, children[1].Expression);
			}
			else if (type == ExpressionType.Not){ // NOT
				CreateNotRecipe(children[0].Expression, children[1].Expression);
			}
		
			children[0].ClearExpression();
			children[1].ClearExpression();
		}

	}
	
	void OnMouseUp(){
	
		if(Support.interactionEnable) mouse_down = false;
	}

	public void SetId(int id)
	{

	}

	void CreateAndRecipe(Expression left, Expression right)
	{
		AndExpressionComposite new_and = new AndExpressionComposite ();
		new_and.left = left;
		new_and.right = right;
		//Sound.PlaySFX("Create_SFX");
		GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Create_SFX");

		AddRecipe(new_and);

		
		//Debug.Log ("Rule created: " + new_and.ToString());
	}
	
	void CreateOrRecipe(Expression left, Expression right)
	{
		OrExpressionComposite new_or = new OrExpressionComposite ();
		new_or.left = left;
		new_or.right = right;
		
		AddRecipe(new_or);
		
		//Debug.Log ("Rule created: " + new_and.ToString());
	}
	
	void AddRecipe (Expression expression){
		recipe_container.SendMessage ("AddRecipe", expression);
		gameObject.SendMessageUpwards("OpenRecipes",type);
		//recipe_container.SendMessage("OpenRecipes",type);
	}
	
	void CreateNotRecipe(Expression left, Expression right){
	
	// NOT IMPLEMENTED YET
	
	}

	void CreateImpRecipe(Expression left)
	{
		ImplicationExpressionComposite new_imp = new ImplicationExpressionComposite ();
		new_imp.left = left;

		bool nomatch = true;

		for (int i=0; i<main.RuleCheker.Count; i++) 
			{
				if(main.RuleCheker [i].left == new_imp.left.ToString ())
				{
					nomatch = false;
					Number numb_aux = new Number ();
					numb_aux.value = System.Convert.ToInt32(main.RuleCheker [i].right);
					new_imp.right = numb_aux;
					Rule new_rule = new Rule (main.RuleCheker [i].name, new_imp);
					main.AddRule (new_rule);
				}
			}

		if (nomatch) 
			{
				Number numb_aux = new Number ();
				numb_aux.value = System.Convert.ToInt32("0");
				new_imp.right = numb_aux;
				Rule new_rule = new Rule ("Queimado", new_imp);
				main.AddRule (new_rule);
			}
		
		
		ResetInterface();

	}
	
	void ResetInterface(){
		mouse_down = false;
		SendMessageUpwards("CloseRecipes");
		SendMessageUpwards("Swich");
	}
}
