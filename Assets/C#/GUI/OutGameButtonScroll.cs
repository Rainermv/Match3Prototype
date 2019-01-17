using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameButtonScroll : OutGameButton {
		
		public Vector2 direction;
		public float speed;
		public bool hold;
			
		void Start(){ 
		
			if (MENU_EVENT != MenuEvent.ScrollLeft && MENU_EVENT != MenuEvent.ScrollRight){
				Debug.Log ("Error - Scroll Button " + name + " doesn't have a 'ScrollLeft' or 'ScrollRight' menu event. Set it on the inspector");
			}
		
		}
		
		new public void OnMouseDown(){ if (button_active) PressButton ();}
		
//		new public void PressButton(){
//			
//			SetPressed(!pressed);
//			
//			Debug.Log("Play: Button_SFX");
//			//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Button_SFX");
//			if (pressed){	
//				SendEvent();
//			}
//			
//			SetChildren(pressed);
//		}
		
		
		
	}
}
		
		
		