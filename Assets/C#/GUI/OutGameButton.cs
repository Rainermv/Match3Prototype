using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameButton : OutGameElement {
	
		protected SoundControler Sound;
		
		public bool require_confirmation;
		
		public bool mouse_over_enabled = false;
		public bool pressed = false; 
		public bool over = false;
		public bool button_active = true;

		public MenuEvent MENU_EVENT;
		
		void Awake(){
			Sound = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>();
		}
		
		public void SetEvent(MenuEvent ev){
					
			MENU_EVENT = ev;
		}
		
		// PRIVATE
		public void OnMouseDown(){ if (button_active) PressButton ();}
		
		public void OnMouseUp(){ if (button_active) UnPressButton ();}

		public void OnMouseOver(){  if (button_active && mouse_over_enabled) MouseOver (); }

		public void OnMouseExit(){ if (button_active && mouse_over_enabled) MouseExit (); }

		public void Update(){
			
			ViewButton();

		}
		
		// PUBLIC 
		public void PressButton(){
		
			SetPressed(true);
			
			//Debug.Log("Play: Button_SFX");
			//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Button_SFX");
			Sound.PlaySFX("Button_SFX");
			if (pressed){	
				SendEvent();
			}
			
			//SetChildren(pressed);
		}
		
		public void UnPressButton(){
			SetPressed(false);
			
			//SetChildren(false);
		}
		
		public void MouseOver(){ over = true; }
		
		public void MouseExit(){ over = false; }
		
		public void SetPressed(bool pressed){ 
			this.pressed = pressed; 
		}
		
		public void SetOver(bool over){ this.over = over; }
				
		public void SetChildren(bool active){
			for (int i = 0; i < transform.childCount; i++){
				transform.GetChild(i).gameObject.SetActive(active);
			}
		}
		
		public void SendEvent(){ 
			SendMessageUpwards ("ReportEvent", this);
		}
		
		// VIEWS
		
		public void ViewButton(){
		
			if (button_active){
				if (pressed) 		ViewButtonPressed();
				else if (!pressed)
					if (over)		ViewButtonOver();
				else 
					if (!over) 		ViewButtonNotOver();
			}
			else 					ViewButtonNotActive();
			
		}
				
		public void ViewButtonNotActive(){ ChangeColor ( new Color(0.2f,0.2f,0.2f)); }
		
		public void ViewButtonOver(){ ChangeColor (Color.gray);}
		
		public void ViewButtonNotOver(){ ChangeColor (Color.white);}
		
		public void ViewButtonPressed(){
			ChangeColor(new Color(0.3f,0.3f,0.3f));	
		}
		
		public void ViewButtonReleased(){ 
			ChangeColor(Color.white);
			}
		
		public void ChangeColor(Color color){ renderer.material.color = color; }
		
		public void SetSprite(Sprite sprite){ GetComponent<SpriteRenderer>().sprite = sprite; }
			

	}

}