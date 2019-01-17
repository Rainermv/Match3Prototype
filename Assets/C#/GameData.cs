using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food{
	public class GameData : MonoBehaviour {
	
		// PRIVATE
		private int WIDTH = 8;
		private int HEIGHT = 6;
		private static GameData instance;
		
		private string last_scene;
		public string Last_Scene{get{return last_scene;}set{last_scene=value;}}
		
		private List <Chapter> chapters = new List<Chapter>();
		public List<Chapter> Chapters {get{return chapters;}}
		
		// PUBLIC]
		[HideInInspector]
		public Chapter current_chapter;
		[HideInInspector]
		public Level current_level;
		[HideInInspector]
		public LevelOut current_level_out;
		[HideInInspector]
		public GameObject current_level_tutorial;
		[HideInInspector]
		public string level_string;
		[HideInInspector]
		public int ChapterCount{get{return chapters.Count;}}
		
		//public List<ChapterData> chapter_data;
		//public List<LevelData> level_data;
		
		
		// INSPECTOR CONFIGURATION
		
		//public List<ChapterConfig> chapter_configuration = new List<ChapterConfig>();
		public ChapterConfig[] chapter_configuration;
		
		//public List<ChapterConfig> chapter_config;

		
		//public List<Chapter> Chapters{get{return chapters;}}

		// PUBLIC FUNCIONS ---------------------------------------------------------------------------------------------------
		
		void OnMouseDown(){
			SaveGame ();
		}
	
		// Use this for initialization
		void Awake () {
	
			DontDestroyOnLoad(gameObject);
			
			InitChapters();
			
			LoadGame ();
			
			//Debug.Log ("SOUND IS " + Support.SoundEnable);

		}
				
		
		public void InitChapters(){

			int chapter_index = 0;	
			if (chapter_configuration == null) return;
					
			foreach (ChapterConfig chapter_config in chapter_configuration){
				AddChapter (chapter_config, chapter_index++);
			}
		
		}
		
		public void AddChapter(ChapterConfig chapter_config, int ch_ind){
		
			Chapter chapter = new Chapter();
			chapters.Add (chapter);
			
			chapter.Name = chapter_config.Name;
			chapter.index = ch_ind;
			chapter.unlocked = chapter_config.Unlocked;
			chapter.Chapter_Background = chapter_config.Chapter_Background;
			chapter.Chapter_Thumb = chapter_config.Chapter_Thumb;
			
			int level_index = 0;	
			foreach (LevelConfig level_config in chapter_config.levels)
				AddLevel(chapter, level_config, ch_ind, level_index++);
		}
		
		public void AddLevel(Chapter chapter, LevelConfig level_config, int ch_ind, int lvl_ind){
			
			LevelOut level = new LevelOut();
			chapter.levels.Add (level);
			
			level.chapter = ch_ind;
			level.index = lvl_ind;
			
			level.Name = level_config.Name;
			level.Unlocked = level_config.unlocked;
			level.Level_text = level_config.Level_txt;
			level.Tutorial_manager = level_config.Tutorial_Object;
			
		}


		public void SaveGame(){
			
				GameConfig config = new GameConfig();
			
				config.sound = Support.SoundEnable;
				
				
			
				DataLoader.SaveData(ref chapters,ref config);
				
				//Debug.Log ("Game Saved || Sound is " + config.sound);
			
			}

		public void LoadGame(){
				
				GameConfig configuration = new GameConfig();
				
				if (DataLoader.LoadData (ref chapters,ref configuration)){
					
					Support.SoundEnable = configuration.sound;
				}
				
				else Debug.Log ("Cannot find save file");
				
			}

		public static GameData GetInstance(){
			
			GameObject instance_obj = GameObject.FindGameObjectWithTag ("Data");
			if (instance_obj != null)
				return instance_obj.GetComponent<GameData> ();
			
			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
			camera.AddComponent<GameData>();
			return camera.GetComponent<GameData>();
		}
		
//		public void LoadLevel(int chapter, int level){
//			
//			if (chapters[chapter] == null || chapters[chapter].levels[level] == null){
//				Debug.Log("Cannot load Level, check indexes");
//				return;
//			}
//			
//			current_level = Reader.LoadLevel(chapters[chapter].levels[level].Level_text,HEIGHT,WIDTH);
//		}
		
		public void LoadLevel(LevelOut level_out){
		
			current_level_out = level_out;
			current_level = Reader.LoadLevel (level_out.Level_text,HEIGHT,WIDTH);
			current_level_tutorial = current_level_out.Tutorial_manager;
			level_string = "Chapter_"+current_level_out.chapter+":"+"Level_"+current_level_out.index;
		}
		
		public Chapter GetChapter(int index){
			return chapters[index];
		}
		
		public bool LoadNextLevel(){
			
			if (current_level == null || current_level_out == null) return false;
			
			
			int chapter_index = current_chapter.index;
			int level_index = current_level_out.index;
			
			int level_count = current_chapter.levels.Count;
			
			if (level_index + 1 < level_count && current_chapter.levels[level_index+1] != null){
				LoadLevel (current_chapter.levels[level_index+1]);
				return true;
				//current_chapter.levels[level_index+1].Unlocked = true;
			}
			else if (chapter_index +1 < chapters.Count && chapters[chapter_index+1] != null) {
				//chapters[chapter_index+1].unlocked = true;
				Application.LoadLevel ("OutGame_Chapter_Screen");
				//return false;
			}
			else
				Application.LoadLevel ("OutGame_Credits_Screen");
			
			return false;
		}
		
		public void UnlockNextLevel(){
			
			if (current_level == null || current_level_out == null) return;
			
			
			int chapter_index = current_chapter.index;
			int level_index = current_level_out.index;
			
			int level_count = current_chapter.levels.Count;
			
			if (level_index + 1 < level_count && current_chapter.levels[level_index+1] != null){
				current_chapter.levels[level_index+1].Unlocked = true;
			}
			else if (chapter_index +1 < chapters.Count && chapters[chapter_index+1] != null) {
				chapters[chapter_index+1].unlocked = true;
				chapters[chapter_index+1].levels[0].Unlocked = true;
			}
//			else 
//				Application.LoadLevel ("OutGame_Credits_Screen");
			
		}
		
	
	}
}
