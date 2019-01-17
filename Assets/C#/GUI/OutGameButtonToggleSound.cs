using UnityEngine;
using System.Collections;


namespace Food{
	public class OutGameButtonToggleSound : OutGameButton {
		
		public Sprite toggle_on;
		public Sprite toggle_off;
		
		private new SpriteRenderer renderer;
		
		public void Awake(){
			renderer = GetComponent<SpriteRenderer>();
		}
		
		public void Start(){
			pressed = Support.SoundEnable;
			ToggleSprite(pressed);
		}
				
		new public void OnMouseDown(){ if (button_active) PressButton ();	}
		
		new public void PressButton(){
			
			ChangeColor(new Color(0.3f,0.3f,0.3f));
			
			Debug.Log("Play: Button_SFX");
			//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Button_SFX");
			SetPressed(!pressed);
			
			//if (pressed)
			{	
				SendEvent();
			}
			
			SetChildren(pressed);
			ToggleSprite (pressed);
		}
		
		new public void OnMouseUp(){ChangeColor(Color.white);}
		
		
		new public void Update(){	ViewButton();}
		
		
		new public void ViewButton(){
			
			if (button_active){
				
				if (mouse_over_enabled){
					if (over)		ViewButtonOver();
					else 
						if (!over) 		ViewButtonNotOver();
				}
			}
			else 					ViewButtonNotActive();
			
		}
		
		void ToggleSprite(bool on){
			if (on && toggle_on != null){
				renderer.sprite = toggle_on;
			}
			else if (!on && toggle_off != null){
				renderer.sprite = toggle_off;
			}
		}
		
		
		
	}

}