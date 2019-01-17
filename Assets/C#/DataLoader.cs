using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Food{
	public static class DataLoader {
	
		
		public static void SaveData(ref List<Chapter> chapter_list,ref GameConfig config_to_save){
		
			//chapter_data = chapter_list;
			
			//GameConfig data_to_save = new GameConfig();
			
			//data_to_save.sound = sound;
			
			List<ChapterData> chapter_data = new List<ChapterData>();
			
			foreach (Chapter chapter in chapter_list) 
				chapter_data.Add (SaveChapter (chapter));
				
			config_to_save.chapter_data = chapter_data;
			
			BinaryFormatter bf = new BinaryFormatter();
			
			string path = Application.persistentDataPath + "/savedGames.txt";
			FileStream file = File.Create (path);
						
			bf.Serialize(file, config_to_save);
			
			Debug.Log("saved to " + path);
			
			file.Close();
			
		}
		
		public static bool LoadData(ref List<Chapter> chapters_to_load,ref GameConfig config_to_load) {
			if(File.Exists(Application.persistentDataPath + "/savedGames.txt")) {
			
				// open file to load
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/savedGames.txt", FileMode.Open);
				
				// deserialize file data into a gameconfig object
				config_to_load = (GameConfig)bf.Deserialize(file);
				
				// close the file
				file.Close();
				
				// load the chapter data into the game chapter list in memory
				for (int i = 0; i < chapters_to_load.Count; i++){
					if (i < config_to_load.chapter_data.Count && config_to_load.chapter_data[i] != null && chapters_to_load[i] != null){
						LoadChapter (config_to_load.chapter_data[i], chapters_to_load[i]);
						
					}
					//loaded_chapters.Add (LoadChapter (chapter));
				}
					
				return true;

			}
			
			return false;

		}

		private static void LoadChapter(ChapterData chapter_data, Chapter chapter){
			
			//Chapter chapter = new Chapter();
			chapter.unlocked = chapter_data.unlocked;
			chapter.trophies = chapter_data.trophies;
			
			//Debug.Log ("Chapter " + (chapter.index+1) + " loaded -- unlocked = " + chapter.unlocked);
			
			for (int i = 0; i < chapter.levels.Count; i++){
				if (i >= chapter_data.level_data.Count || chapter.levels[i] == null || chapter_data.level_data[i] == null) return;
				
				LoadLevel (chapter_data.level_data[i],chapter.levels[i]);
				
			}
		
		}
		
		private static void LoadLevel(LevelData data, LevelOut level){
			
			level.Unlocked = data.unlocked;
			
			level.top_score = data.top_score;
			
			level.trophy_one = data.trophy_one;
			level.trophy_two = data.trophy_two;
			level.trophy_three = data.trophy_three;
			
			level.golden_trophy_one = data.golden_trophy_one;
			level.golden_trophy_two = data.golden_trophy_two;
			level.golden_trophy_three = data.golden_trophy_three;
			
			//Debug.Log ("---Level " + (level.index+1) + " loaded -- unlocked = " + level.Unlocked);
			
		}
			
		
		private static ChapterData SaveChapter(Chapter chapter){
		
			ChapterData data = new ChapterData();
			data.unlocked = chapter.unlocked;
			data.trophies = chapter.trophies;
			
			//Debug.Log ("Chapter " + (chapter.index+1) + " saved -- unlocked = " + chapter.unlocked);
			
			foreach (LevelOut level in chapter.levels) 
				data.level_data.Add (SaveLevel (level));
			
			return data;
		}
				
		private static LevelData SaveLevel(LevelOut level){
			
			LevelData data  = new LevelData();
			
			data.unlocked = level.Unlocked;
			
			data.top_score = level.top_score;
			
			data.trophy_one = level.trophy_one;
			data.trophy_two = level.trophy_two;
			data.trophy_three = level.trophy_three;
			
			data.golden_trophy_one = level.golden_trophy_one;
			data.golden_trophy_two = level.golden_trophy_two;
			data.golden_trophy_three = level.golden_trophy_three;
			
			//Debug.Log ("Level " + (level.index+1) + " saved -- unlocked = " + level.Unlocked);
			
			return data;
			
		}
		
	}
	
	[System.Serializable]
	public class ChapterData{
//		public int index;
		public bool unlocked;
		public int trophies;	
		public List<LevelData> level_data = new List<LevelData>();
	}
	
	[System.Serializable]
	public class LevelData{
		
		public bool unlocked;
				
		public int top_score;
		
		public bool trophy_one;
		public bool trophy_two;
		public bool trophy_three;
		
		public bool golden_trophy_one;
		public bool golden_trophy_two;
		public bool golden_trophy_three;
	}
	
	[System.Serializable]
	public class GameConfig{
	
		//public bool first_execution;
		public string player_name;
		public bool sound;		
		
		public List<ChapterData> chapter_data = new List<ChapterData>();
	
	}
}