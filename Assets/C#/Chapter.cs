using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food{

	[System.Serializable]
	public class ChapterConfig
	{
		public string Name;
		public bool Unlocked;
		
		public Sprite Chapter_Thumb;
//		private Sprite chapter_Thumb;
//		public Sprite Chapter_Thumb{get{return chapter_Thumb;}set{chapter_Thumb = value;}}
		
		public Sprite Chapter_Background;
//		private Sprite chapter_Background;
//		public Sprite Chapter_Background{get{return chapter_Background;}set{chapter_Background = value;}}
		
		public LevelConfig[] levels;
	}
	
	
	public class Chapter{
	
		public string Name;
		public int index;
	
		//public ChapterData data;
		
		public Sprite Chapter_Thumb;
		public Sprite Chapter_Background;
		
		public bool unlocked 		= false;
		public int trophies			= 0;	
							
		//public List<int> levels  = new List<int>();
		public List <LevelOut> levels = new List<LevelOut>();
		//public TextAsset[] level_data;
		
	}
}
