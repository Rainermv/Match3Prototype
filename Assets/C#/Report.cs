using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

	public static class Report{
		
		static GameData data = GameData.GetInstance();

		public static void RestartLevel(bool complete)
		{
			if (complete) 	GA.API.Design.NewEvent("GUI:MenuButton:RestartCompleteLevel"  , 1.0f);
			else   			GA.API.Design.NewEvent("GUI:MenuButton:RestartIncompleteLevel", 1.0f);
		}
		
		public static void ToggleSound(bool active){	
			GA.API.Design.NewEvent("GUI:Button:Sound", (active)?1.0f:0.0f);
			TutorialManager.CheckTrigers (Action.Evento,"bla", 1);
		}

		public static void CreateRule(Rule rule){
			//Debug.Log (data.level_string);
			GA.API.Design.NewEvent(GameData.GetInstance().level_string+":Rule:Create:"+rule.Expression.ToString());
			TutorialManager.CheckTrigers (Action.Evento, "Rule", rule.IDResult);
		}

		public static void CreateRecipe(string level_string, Recipe rule){
			TutorialManager.CheckTrigers(Action.Evento, "Recipe" , 0);
		}

		public static void SlotAction(string level_string, Slot slot){
			TutorialManager.CheckTrigers (Action.Evento, "Slot", slot.index);
		}
		

//		public static void InterfaceAction(string level_string, int value){
//			TutorialManager.CheckTrigers (Action.Evento, "Interface", value);
//		}
		
		public static void InterfaceAction(int value){
			TutorialManager.CheckTrigers (Action.Evento, "Interface", value);
		}
	

		public static void MatchAction(string level_string, int ID){
			TutorialManager.CheckTrigers (Action.Evento, "Match", ID);
		}
		
		public static void DeleteRule(string level_string, Rule rule){
			GA.API.Design.NewEvent(level_string+":Rule:Delete:"+rule.Expression.ToString());
			TutorialManager.CheckTrigers (Action.Evento, "Delete", rule.IDResult);
		}
		
		public static void ActivateRule(string level_string, Rule rule){
			GA.API.Design.NewEvent(level_string+":Rule:Active:"+rule.Expression.ToString());
			TutorialManager.CheckTrigers (Action.Evento, "ChangeRule", rule.IDResult);
		}
		
		public static void Selection(string level_string, GameObject selected){
			if (selected == null)  return;
			
			if (selected.tag == "Block") 
				{
					GA.API.Design.NewEvent (level_string + ":Grid:" + selected.GetComponent<Block> ().Color_Index, 1.0f, selected.transform.position);
					TutorialManager.CheckTrigers(Action.Evento,"Grid",selected.GetComponent<Block> ().Color_Index);
				}
						
			else if (selected.tag == "Request" && selected.GetComponent<Request>().Color_Index != -1) 
				{
					GA.API.Design.NewEvent (level_string + ":Request:" + selected.GetComponent<Request> ().Color_Index, 1.0f);
					TutorialManager.CheckTrigers(Action.Evento,"Request",selected.GetComponent<Request> ().Color_Index);
				}
		}
		
		public static void Help(){
			GA.API.Design.NewEvent("MenuButton:Help");
		}

}
