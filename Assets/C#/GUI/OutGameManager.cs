using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food{
	public enum MenuEvent{	Null,Back,	// Generic
							ChapterScreen,  // Go to chapter selection screen
							Som, 			// enable/disable sound
							CreditsScreen,	// go to credits screen
							LevelScreen,	// Go to Level Selection Screen
							GameScreen, 	// Start level
							MenuScreen,
							RestartLevel,		// In game menu - restart level
							NextLevel,			// In game menu - next level
							ScrollLeft,			// Scroll to the Left
							ScrollRight,		// Scroll to the Right
							PreLevel,			// show the pre level screen
							LinkAtomic,
							LinkFacebook
							};
							
	public enum Screen {
							Splash, 
							Main, 
							Credits, 
							Achievments, 
							Ranking, 
							ChapterSelection, 
							LevelSelection, 
							Game,
							PreLevel,
							Confirmation
							};

	public class OutGameManager : MonoBehaviour {
		
		public Screen screen;
		public Screen last_screen;
				
		public GameObject pre_Level;
		public GameObject chapter_button_reference;
		public GameObject level_button_reference;
		public GameObject confirmation_window_reference;
		
		private GameObject confirmation_window;
		
		private List<GameObject> movable_objects = new List<GameObject>();
		
		private GameData data;
		
		private int views = 1;
		private int current_view = 1;
		
		private SoundControler Sound;
			
		void Awake(){
		
			GameObject _Sound = GameObject.FindGameObjectWithTag("Sound");
			if(_Sound != null) Sound = _Sound.GetComponent<SoundControler>();

			data = GameData.GetInstance();
		
			
			if (data == null)
				Debug.Log ("Error - Data did not load");
		}
		
		void Start(){
		
			switch(screen){
//			
//			case Screen.Main:
//				//Debug.Log("Play: Main_BGS");
//				Sound.PlayBGS("Main_BGS");
//				break;
////			
//			case Screen.Credits:
//				//Debug.Log("Play: Main_BGS");
//				Sound.PlayBGS("Credits_BGS");
//				break;

			case Screen.ChapterSelection:
				if (chapter_button_reference != null) DisplayChapters();
				//Debug.Log("Play: Main_BGS");
				//Sound.PlayBGS("Main_BGS");
					break;
			
			case Screen.LevelSelection:
				if (level_button_reference != null){
					DisplayBackground();
					DisplayLevels(0, 0.5f, 0.5f);
					}
					
//				for(int i=0; i< data.ChapterCount;i++)
//				{
//					if(data.Chapters[i] == data.current_chapter)
//					//Debug.Log("Play: " + "Chapter_" + (i+1) + "_BGS");
//					//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlayBGS("Chapter_" + (i+1) + "_BGS");
//				}
					break;
			
			}
			
		
		}
		
		void DisplayChapters(){
		
			int x = -3;
			int y = 1;
			foreach (Chapter chapter in data.Chapters){
				
				Vector2 position = new Vector2 (x, y);
				x+=3;
				
				GameObject chapter_button = (GameObject)GameObject.Instantiate(chapter_button_reference,position,Quaternion.identity);
				chapter_button.SendMessage ("SetChapter",chapter);
				chapter_button.transform.parent = transform;
					
				movable_objects.Add(chapter_button);
			
			}
			
			views = data.Chapters.Count / 2;
			
			// TODO Implementar capitulos "em lotes" (semelhante aos niveis)
		
		}
		
		void DisplayBackground(){
			Sprite background = data.current_chapter.Chapter_Background;
			if (background != null)
				GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite = background; 
		}
			
		void DisplayLevels(int count, float start_x, float start_y){
		
			
			for (int y = 1; y >= -1; y--){
				for (int x = -2; x <= 1; x++){
					
					if (count > data.current_chapter.levels.Count-1)
						return;
					
					LevelOut level = data.current_chapter.levels[count];
					
					Vector2 position = new Vector2 ((start_x + (float)x )*2f, (start_y + (float)y) * 2.0f);
					GameObject level_button = (GameObject)GameObject.Instantiate(level_button_reference,position,Quaternion.identity);	
					level_button.SendMessage("SetLevel",level);	
					level_button.transform.parent = transform;
					
					movable_objects.Add(level_button);
					
					count++;
					
				}
				
			}
			
			
			views++;
			DisplayLevels (count, start_x + 8, start_y);
		
		}
		

		public void ReportEvent(OutGameButton sender){
		
			MenuEvent menuEvent = sender.MENU_EVENT;
			
			bool conf = sender.require_confirmation;
					
			switch(menuEvent){
			
			case MenuEvent.Back:
				if (screen != Screen.Confirmation)
					LoadLastScreen();
				else
					DestroyConfirmationWindow();
				break;
				
			case MenuEvent.MenuScreen:
				LoadMenuScreen();
				break;

			case MenuEvent.ChapterScreen:
				if (screen == Screen.PreLevel)
					ClosePreLevel();
				else
					LoadChapterScreen();
				break;
				
			case MenuEvent.LevelScreen:
				if (screen == Screen.ChapterSelection)
					LoadLevelScreen(sender);
				else{
					if (!conf || screen == Screen.Confirmation){
						DestroyConfirmationWindow();
						BackToLevelScreen();
					}
					else		
						CreateConfirmationWindow(menuEvent);
				 } 
				break;
			
			case MenuEvent.PreLevel:
				ShowPreLevel(sender);
				break;
			
			case MenuEvent.GameScreen:
				StartLevel(sender);
				break;
				
			case MenuEvent.Som:
				ToggleSound();
				Report.ToggleSound(sender.pressed);
				
				break;

			case MenuEvent.CreditsScreen:
				LoadCreditsScreen();
				break;
				
			case MenuEvent.RestartLevel:
				if (!conf || screen == Screen.Confirmation){
					DestroyConfirmationWindow();
					RestartLevel();
				}
				else
					CreateConfirmationWindow(menuEvent);
				break;
				
			case MenuEvent.NextLevel:
				NextLevel();
				break;
				
			case MenuEvent.ScrollRight:
				if (screen == Screen.PreLevel)
					NextPreLevel(+1);
				else
					if (current_view+1 <= views){
						current_view++;
						ScrollCamera(sender);
				}
				
				break;
				
			case MenuEvent.ScrollLeft:
				if (screen == Screen.PreLevel)
					NextPreLevel(-1);
				else 
					if (current_view-1 >= 1){
						current_view--;
						ScrollCamera(sender);
					
				}
				break;
			
			case MenuEvent.LinkAtomic:
				OpenLink("http://atomicrocket.com.br");
				break;
						
			}
		}
		
		void OpenLink(string url){
		    Application.OpenURL(url);
		
		}
		
		void CreateConfirmationWindow(MenuEvent window_event){
			
			GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
			Vector3 position = new Vector3 (cam.transform.position.x, cam.transform.position.y, -1);
			confirmation_window = (GameObject)Instantiate (confirmation_window_reference,position,Quaternion.identity);
			confirmation_window.SendMessage("SetWindow",window_event);
			confirmation_window.transform.parent = transform;
			
			last_screen = screen;
			screen = Screen.Confirmation;
		
		}
		
		void DestroyConfirmationWindow(){
			
			if (confirmation_window != null){
				Destroy(confirmation_window);
				screen = last_screen;
			}

		}
		
		void ScrollCamera(OutGameButton sender){
		
			OutGameButtonScroll scroll = (OutGameButtonScroll) sender;
			
			//camera.transform.Translate (scroll.direction * scroll.speed);
			foreach (GameObject movable in movable_objects){
				movable.SendMessage("SetTarget", scroll.direction);
			}
		}
		
		void Update(){
			
			
		}

		void LoadCreditsScreen(){
			LoadScreen("OutGame_Credits_Screen"); 
			
		}

		void ToggleSound()
		{	
			Support.SoundEnable = !Support.SoundEnable;
			Debug.Log ("Toggle Sound: " + Support.SoundEnable);
		}
		
		void LoadMenuScreen(){
			//Debug.Log ("LoadChapterScreen");
			Sound.PlayBGS("Main_BGS");
			LoadScreen("OutGame_Main_Screen"); 
			
		}
		
		void LoadChapterScreen(){
			LoadScreen("OutGame_Chapter_Screen"); 
			
		}
		
		void LoadLevelScreen(OutGameButton sender){
		
			OutGameButtonChapter chapter_button = (OutGameButtonChapter) sender;
			data.current_chapter = chapter_button.Chapter;
			LoadScreen("OutGame_Level_Screen"); 
		
		}
		
		void BackToLevelScreen(){
			LoadScreen("OutGame_Level_Screen"); 
		}
		
		void ShowPreLevel(OutGameButton sender){
		
			
			OutGameButtonLevel level_button = (OutGameButtonLevel) sender;
			//data.LoadLevel (level_button.Level.chapter,level_button.Level.index);
			data.LoadLevel (level_button.Level);
			
			
			if (pre_Level != null){
				
				pre_Level.SetActive(true);
				pre_Level.SendMessage("Display");
				screen = Screen.PreLevel;
				
				
			}
			
		}
		
		void ClosePreLevel(){
			if (pre_Level != null){
				
				pre_Level.SetActive(false);
				//pre_Level.SendMessage("Display");
				screen = Screen.LevelSelection;
				
				
			}
		}
		
		void NextPreLevel(int direction){
		
			int next_index = data.current_level_out.index + direction;
								
			if (pre_Level != null && next_index >=0 && next_index < data.current_chapter.levels.Count && data.current_chapter.levels[next_index].Unlocked){
				
				data.LoadLevel (data.current_chapter.levels[next_index]);
				//pre_Level.SetActive(true);
				pre_Level.SendMessage("Display");
				screen = Screen.PreLevel;

			}
			
		}
		
		void StartLevel(OutGameButton sender){
	
			//Sound.PlayBGS("Game_BGS");
			
			PlayLevel ();
			
		}
		
		void LoadLastScreen(){
			LoadScreen(data.Last_Scene);
		}
		
		void LoadScreen(string name){
			data.Last_Scene = Application.loadedLevelName;
			
			if (name != null) Application.LoadLevel (name);
			else Application.LoadLevel ("InGame");
			
			
		}
		
		void NextLevel(){
			if (data.LoadNextLevel())
				PlayLevel ();
			
			
		}
		
		void RestartLevel(){
			
			Report.RestartLevel(false);
			
			PlayLevel();
			
		}

		void PlayLevel(){
			
			//Sound.PlayBGS("Game_BGS");
			
			Application.LoadLevel ("InGame");
			
		}
	}
}
