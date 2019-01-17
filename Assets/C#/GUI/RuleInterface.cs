using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food{

	public class RuleInterface : InterfaceComponent{
		private bool state = true;
		// true = creator
		// false = selector

//		public void ActivateRuleCreator(){
//			
//			transform.GetChild(0).GetComponent<RuleInterfaceComponent>().Activate();
//			transform.GetChild(1).GetComponent<RuleInterfaceComponent>().Deactivate();
//			
//		}
//	
//		public void ActivateRuleSelector(){
//
//			transform.GetChild(1).GetComponent<RuleInterfaceComponent>().Activate();
//			transform.GetChild(0).GetComponent<RuleInterfaceComponent>().Deactivate();
//
//		}


		public void Swich()
		{
			if (state) 
			{

				//transform.GetChild(0).GetComponent<RuleCreatorContainer>().Deactivate();
				gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RuleCreatorContainer>().Deactivate();
				//transform.GetChild(1).GetComponent<RuleInterfaceComponent>().Activate();
				gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

				state = !state;
			}
			else
			{
				//transform.GetChild(0).GetComponent<RuleCreatorContainer>().Deactivate();
				gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RuleCreatorContainer>().Activate();
				//transform.GetChild(1).GetComponent<RuleInterfaceComponent>().Activate();
				gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
				state = !state;
			}

		}


	}
}
