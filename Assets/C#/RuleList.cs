using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Food;

public class RuleList : MonoBehaviour {

	public GameObject rule_reference;
	private List<GameObject> rules = new List<GameObject>();
	public GameObject ruless_starting_position;
	private int maxRules = 5;

	public float translate_y = 1.1f;


	bool showing = true;
	
	void Awake(){}
	
	// Use this for initialization
	void Start () 
	{
		showing = true;	
	}
	
	// Update is called once per frame
	void Update () {}

	public void setActiveRule(GameObject _ru)
	{
		for (int i=0;i<rules.Count;i++) 
		{
			if(_ru == rules[i])
				rules[i].GetComponent<RuleButon>().Activate(true);
				//rules[i].SendMessage("Activate",true);
			else 
				rules[i].GetComponent<RuleButon>().Activate(false);
				//rules[i].SendMessage("Activate",false);
		}
	}
	
	public GameObject AddRule(Rule rule)
	{
		GameObject new_recipe = (GameObject)Instantiate (rule_reference, ruless_starting_position.transform.position,Quaternion.identity);
		new_recipe.GetComponent<RuleButon> ().RULE = rule;
		rules.Add (new_recipe);
		new_recipe.transform.parent = ruless_starting_position.transform;
		new_recipe.transform.Translate(0,translate_y-rules.Count*translate_y,0);
		ruless_starting_position.SetActive (showing);
		return new_recipe;
		
	}

	public void DeleteRule(GameObject _ru)
	{
		//Debug.Log ("Rule count " + rules.Count);
		Report.DeleteRule(GameData.GetInstance().level_string, _ru.GetComponent<RuleButon>().RULE);
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Main> ().SwitchRule(null);
		for (int i=0;i<rules.Count;i++) 
		{
			if(_ru == rules[i])
			{
				//Debug.Log(i);
				rules.RemoveAt(i);
				GameObject.Destroy(_ru);
			}
		}
		for (int i=0;i<rules.Count;i++) 
		{
			rules[i].transform.position = ruless_starting_position.transform.position;
			rules[i].transform.Translate(0,-i*translate_y,0);
		}
	}
	public bool CanAdd()
	{
		if (rules.Count + 1 <= maxRules)
			return true;
		return false;
	}
	public void SetRuleMax(int value)
	{
		maxRules = value;
	}
}
