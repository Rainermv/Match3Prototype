using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameButtonLevel : OutGameButtonMovable {
	
		//public Sprite 		sprite_hat;
		//public Sprite 		sprite_golden_hat;
		//public Sprite[]		sprite_number;
	
		public Sprite chapter_unlocked;
		public Sprite chapter_locked;
		
		public bool unlocked = false;

		private LevelOut _level;
		public LevelOut Level{get{return _level;}set{_level=value;}}
		
		private Transform child_level_number;
		private Transform hat_1;
		private Transform hat_2;
		private Transform hat_3;
		
		
		void Awake(){
			child_level_number = transform.FindChild("Level_Number");
			hat_1 = transform.FindChild ("Hat_1");
			hat_2 = transform.FindChild ("Hat_2");
			hat_3 = transform.FindChild ("Hat_3");
		}
		
		void Start(){
			SetLocked();
			InitMovable();
			
			MENU_EVENT = MenuEvent.PreLevel;
			
			RefreshComponents();
				
		}
		
		void SetLocked(){
			
			unlocked = _level.Unlocked;
			
			if (unlocked)  SetSprite(chapter_unlocked);		 	 
			else SetSprite (chapter_locked);
			
			//EnableChildren(unlocked);
			
			
		}
		
		// PUBLIC 
		new public void OnMouseDown(){ if (button_active) PressButton ();}
		
		new public void OnMouseUp(){
			if (unlocked){
				ChangeColor(Color.white);
				EnableChildren(true);
			}
		}
		
		// PUBLIC 
		new public void PressButton(){
			
			if (unlocked){
				ViewButtonPressed();
				SetPressed(!pressed);
				SendEvent();
				
				//SetChildren(pressed);
			}
	
		}
						
		new public void ViewButtonPressed(){
			ChangeColor(new Color(0.3f,0.3f,0.3f));	
		}
		
		public void SetLevel(LevelOut level){
			_level = level;
			
			//Debug.Log (level.index);
			RefreshComponents();

		}
		
		private void RefreshComponents(){
		
			if (_level != null){
				if (_level.Unlocked){
					int level_number = _level.index+1;
					child_level_number.gameObject.GetComponent<TextMesh>().text = level_number.ToString();
					
					hat_1.gameObject.SetActive(_level.trophy_one);
					hat_2.gameObject.SetActive(_level.trophy_two);
					hat_3.gameObject.SetActive(_level.trophy_three);
				}
				else 
					EnableChildren(false);
			}
		}
		
		public void EnableChildren(bool enable){
			
			child_level_number.gameObject.SetActive(enable);
			hat_1.gameObject.SetActive(enable);
			hat_2.gameObject.SetActive(enable);
			hat_3.gameObject.SetActive(enable);
		}
		
		public void DebugUnlock(){
			unlocked = Level.Unlocked = true;
			
			if (unlocked) SetSprite(chapter_unlocked);
			else SetSprite (chapter_locked);
		}
		
	}

}