using UnityEngine;
using System.Collections;
using Food;

namespace Food{
	public class RuleInterfaceSwitcher : InterfaceComponent {
	
		public GameObject child;
	
		//private bool _state = true;
		// TRUE = RULE SELECTOR
		// FALSE = RULE CREATOR
		
		private bool mouse_down = false;
	
		void Start()
		{
			ChangeRule ();
		}
		
		void Update () 
		{
			if (mouse_down) renderer.material.color = Color.gray;
			else renderer.material.color = Color.white;
			
		}

		void OnMouseUp(){
			if(Support.interactionEnable) mouse_down = false;
		}
	
		// Use this for initialization
		void OnMouseDown()
		{
			
			if(Support.interactionEnable){
				mouse_down = true;
				Switch ();
			}
		}
	
		void Switch()
		{
			//_state = !_state;
	
			ChangeRule ();
			//Report.InterfaceAction (GameData.GetInstance ().level_string, 0);
			Report.InterfaceAction (0);
	
		}
	
		void ChangeRule()
		{
			child.SendMessage ("Swich");
//			if (_state == true)
//				ActivateRuleSelector ();
//	
//			else if (_state == false)
//				ActivateRuleCreator ();
		}
	
//		void ActivateRuleCreator(){
//			child.SendMessage ("ActivateRuleCreator");
//			_state = false;
//	
//		}
//	
//		void ActivateRuleSelector(){
//			child.SendMessage ("ActivateRuleSelector");
//			_state = true;
//			
//		}
	}
}
