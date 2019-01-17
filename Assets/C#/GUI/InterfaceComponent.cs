using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food{
	public class InterfaceComponent : MonoBehaviour {
		
		public bool is_available;
		//public Sprite spr_available;
		public Sprite spr_unavailable;
		
		bool first_enable = true;
		
//		void OnEnable(){
//			if (!is_available && first_enable){
//			
////				DeactivateChildren();
////			
////				if (collider != null) collider.enabled = false;
////				
////				if (spr_unavailable != null)
////					GetComponent<SpriteRenderer>().sprite = spr_unavailable;
////				
////				else GetComponent<SpriteRenderer>().color = Color.gray;
////				
////				first_enable = false;
//					
//			}
//				//Deactivate();
//				//DeactivateChildren();		
//		}
		
		public void LockInterface(List<string> names){
	
			foreach (string obj_name in names)
			{
				if (gameObject.name == obj_name){
				
					Debug.Log ("locking " + gameObject.name);
					
					DeactivateChildren();
					
					if (collider != null) collider.enabled = false;
					
					if (spr_unavailable != null)
						GetComponent<SpriteRenderer>().sprite = spr_unavailable;
					
					else GetComponent<SpriteRenderer>().color = Color.gray;
					
				}
			}
				
			for (int i = 0; i < transform.childCount; i++){
				
				GameObject child = 	transform.GetChild(i).gameObject;
				bool active = child.activeSelf;
				if (!active) child.SetActive(true);
				child.SendMessage("LockInterface",names,SendMessageOptions.DontRequireReceiver);
				if (!active) child.SetActive (false);	
				//InterfaceComponent Interface = transform.GetChild(i).GetComponent<InterfaceComponent>();
				//if (Interface != null) Interface.Lock(names);
			}
			
//			
						
		}
//		
//		void Start(){
//			if (!is_available)
//				Deactivate();
//			//DeactivateChildren();		
//		}
		
		public void SetAvailableTrue(string name){
			if (gameObject.name == name){
				SetAvailable(true);
			}
		}
		
		public void SetAvailableFalse(string name){
			if (gameObject.name == name){
				SetAvailable(false);
			}
		}
		
		public void SetAvailable(bool value){
			//Debug.Log ("SETANDAvailable: " + value);
			is_available = value;
			
			if (!is_available)
				Deactivate();	
		}
	
		public void Activate(){
			//if (is_available)
				gameObject.SetActive (true);
		}
	
		public void Deactivate(){
				gameObject.SetActive (false);
		}
		
		public void DeactivateChildren(){
			for (int i = 0; i < transform.childCount; i++){
				InterfaceComponent comp = transform.GetChild (i).GetComponent<InterfaceComponent>();
				if (comp == null) transform.GetChild(i).gameObject.SetActive (false);
			}
			
		}
	}
}
