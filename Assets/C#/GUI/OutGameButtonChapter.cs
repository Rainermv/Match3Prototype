using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameButtonChapter : OutGameButtonMovable {
		
		private Chapter _chapter;
		
		
		
		public Sprite chapter_unlocked;
		public Sprite chapter_locked;
		
		public bool unlocked = false;
		
		public Chapter Chapter{get{return _chapter;}set{_chapter=value;}}
		
		public Transform txt_name;
		private Transform hats_collected;
		private Transform hats_total;
		
		void Awake(){
			hats_collected = transform.FindChild("Hats_Collected");
			hats_total = transform.FindChild("Hats_Total");	
			txt_name = transform.FindChild ("Chapter_Name");
			
		}
		
		void Start(){
		
			SetLocked();
			InitMovable();
			
			RefreshComponents();
			
		}
		
		void SetLocked(){
			
			unlocked = _chapter.unlocked;
			
			if (unlocked) SetSprite(chapter_unlocked);
			else SetSprite (chapter_locked);
		
		}

		new public void OnMouseDown(){ if (button_active) PressButton ();}
		
		// PUBLIC 
		new public void PressButton(){

			if (unlocked){
				ViewButtonPressed();
				SetPressed(!pressed);
				SendEvent();
			}
			
			//SetChildren(pressed);

		}
						
//		new public void ViewButtonPressed(){
//			ChangeColor(new Color(0.3f,0.3f,0.3f));	
//		}

		
		public void SetChapter(Chapter chapter){
			_chapter = chapter;
			
			RefreshComponents();
				
		}
		
		private void RefreshComponents(){
			
			if (_chapter != null){
				if (_chapter.unlocked){
					
					
				
					SetThumb ();
					
					int hats_collected_num = CountHatsCollected();
					hats_collected.gameObject.GetComponent<TextMesh>().text = hats_collected_num.ToString();
					
					int hats_total_num =  _chapter.levels.Count * 3;
					hats_total.gameObject.GetComponent<TextMesh>().text = hats_total_num.ToString();
					
					string chapter_name = _chapter.Name;
					txt_name.gameObject.GetComponent<TextMesh>().text = chapter_name.ToString();
				}
				else 
					EnableChildren(false);
			}
		}
		
		public void EnableChildren(bool enable){
			
			hats_collected.gameObject.SetActive(enable);
			hats_total.gameObject.SetActive(enable);
			txt_name.gameObject.SetActive(enable);
		}
		
		public int CountHatsCollected(){
		
			int hats = 0;
			
			foreach (LevelOut level in _chapter.levels){
			
				if (level.trophy_one) hats++;
				if (level.trophy_two) hats++;
				if (level.trophy_three) hats++;
				
			}
			
			return hats;
		}
		
		public void SetThumb(){
		
			if (transform.childCount > 0 && _chapter.Chapter_Thumb != null){
					Transform thumb = transform.FindChild("Thumb");
				if (thumb != null) thumb.GetComponent<SpriteRenderer>().sprite = _chapter.Chapter_Thumb;
				}
			
		}
		
		new public void ViewButtonPressed(){
			ChangeColor(new Color(0.3f,0.3f,0.3f));	
		}
		
		
			
		
	}

}