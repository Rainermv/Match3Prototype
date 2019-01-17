using UnityEngine;
using System.Collections;
using Food;

namespace Food{
	public class RuleButon : MonoBehaviour {
	
		public bool isDestroy;
		private Rule rule;
		public Rule RULE
		{
			get{return rule;} 
			set{rule=value;}
		}
		private Main main;
		public GameObject child;
		public GameObject element_position;
		public GameObject rule_element_ref;
		
		private float x_space =  -0.6f;
		private int max_components = 5;
		private float component_scale = 0.8f;
		private float scale_rate = 0.1f;
		private float space_rate = -0.06f;
		//private int max_components = 9;
	
		// Use this for initialization
		void Start () 
		{
			main = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Main> ();
			
			if (RULE != null && child != null){
				
				string rule_string = RULE.Expression.ToString();
				
				string[] array = rule_string.Split(' ');

				if (array.Length > max_components){
					component_scale -= (array.Length - max_components) * scale_rate;
					x_space -= (array.Length - max_components) * space_rate;
					}
				
				for (int i = array.Length-1; i >= 0; i--){
					AddRuleElement(array[i]);					
				}

			      
			}
		}
		
		void AddRuleElement(string element){
		
			GameObject element_obj = (GameObject)GameObject.Instantiate (rule_element_ref,element_position.transform.position,Quaternion.identity);
			
			switch (element){
				
			case "+" : element_obj.SendMessage("LoadSign",1); break;
			case "|" : element_obj.SendMessage("LoadSign",2); break;
			case "=" : element_obj.SendMessage("LoadSign",3); break;
			default:   
				element_obj.SendMessage("LoadIngredient",System.Convert.ToInt32(element)); 
				break;

			}
			element_obj.SendMessage ("ScaleTo",component_scale);
			
			element_position.transform.Translate(x_space,0,0);
			
			element_obj.transform.parent = gameObject.transform;
					
			
			//element.SendMessage(
			
		}
		
		// Update is called once per frame
		void Update () {}
	
		public void Activate(bool active)
		{
			child.SetActive(active);
		}
	
		void OnMouseDown()
		{
	
			if(Support.interactionEnable)
			{
				GameObject rule_object = gameObject.transform.parent.gameObject;
				if (isDestroy) 
				{
					SendMessageUpwards("DeleteRule",rule_object);
					//main.RemoveRule(rule_object.GetComponent<Rule>());
				}
	
				else
				{
					main.SwitchRule(gameObject);
					Report.ActivateRule(GameData.GetInstance().level_string, gameObject.GetComponent<RuleButon>().RULE);
				}
			}
		}
	}
}
