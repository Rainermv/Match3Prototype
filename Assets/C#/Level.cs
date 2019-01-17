using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Food{

	[System.Serializable]
	public class LevelConfig{
		public string Name;
		public bool unlocked;
		public TextAsset Level_txt;
		public GameObject Tutorial_Object;
	}

	public class LevelOut{
		public int chapter;
		public int index;
	
		public string Name;
		
		//public LevelData data;
		
		public bool Unlocked 				= false;
		
		public int top_score 				= 0;
		
		public bool trophy_one 				= false;
		public bool trophy_two				= false;
		public bool trophy_three			= false;
		
		public bool golden_trophy_one		= false;
		public bool golden_trophy_two		= false;
		public bool golden_trophy_three		= false;
		
		public TextAsset Level_text;
		public GameObject Tutorial_manager;
	
	}

	public class Level
	{
		//Vars
		public bool isRandom 		= false;
		
		// operators
		public bool lock_and				= false;
		public bool lock_or					= false;
		public bool lock_not				= false;
		public bool lock_teo				= false;
		public bool lock_imp				= false;
		
		public bool lock_switcher_button	= false;
		public bool lock_rule_selector		= false;
		
		public int targetScore		= 100;
		public int trophyScore1		= 150;
		public int trophyScore2		= 200;
		public int burnLimit		= 1;
		public int lostLimit		= 1;
		//public int requestLost		= 0;
	
		private List<int> TiposDeIngredientes = new List<int>();
		public List<int> tiposDeIngredientes {
					get{ return TiposDeIngredientes;}
			}
	
		private List<BlockSettings> GridDeIngredientes = new List<BlockSettings>();
		public List<BlockSettings> gridDeIngredientes{
			get{return GridDeIngredientes;}
		}
	
		private List<int> ListaDePedidos = new List<int>();
		public List<int> listaDePedidos{
			get{return ListaDePedidos;}
		}
	
		public int GetRandomID(int id){
					
			int total = tiposDeIngredientes.Count;
			return tiposDeIngredientes[Random.Range (0, total)];
		}
	}
}