using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameButtonPreLevel : OutGameButton {
	
		public void Display(){
			
			GameData data = GameData.GetInstance();
			
			SetLevelNumber (data.current_level_out.index+1);
			SetLevelScore (data.current_level.targetScore);
			SetLevelScore1 (data.current_level.trophyScore1);
			SetLevelScore2 (data.current_level.trophyScore2);
			SetLevelBurn1 (data.current_level.burnLimit);
			SetLevelBurn2 (0);
			SetLevelLost1 (data.current_level.lostLimit);
			SetLevelLost2 (0);
			
			SetLevelScoretop (data.current_level_out.top_score);
			
		}
	
	
		void SetLevelNumber(int num){
			transform.FindChild("lvl_num").GetComponent<TextMesh>().text = "Fase " + num.ToString("0000");
		}
		
		void SetLevelScore(int num){
			transform.FindChild("lvl_score").GetComponent<TextMesh>().text = num.ToString("0000");
		}
		
		void SetLevelScore1(int num){
			transform.FindChild("lvl_score1").GetComponent<TextMesh>().text = num.ToString("0000");
		}
		
		void SetLevelScore2(int num){
			transform.FindChild("lvl_score2").GetComponent<TextMesh>().text = num.ToString("0000");
		}
		
		void SetLevelBurn1(int num){
			transform.FindChild("lvl_burn1").GetComponent<TextMesh>().text = num.ToString("00");
		}
		
		void SetLevelBurn2(int num){
			transform.FindChild("lvl_burn2").GetComponent<TextMesh>().text = num.ToString("00");
		}
		
		void SetLevelLost1(int num){
			transform.FindChild("lvl_lost1").GetComponent<TextMesh>().text = num.ToString("00");
		}
		
		void SetLevelLost2(int num){
			transform.FindChild("lvl_lost2").GetComponent<TextMesh>().text = num.ToString("00");
		}
		
		void SetLevelScoretop(int num){
			
			if (num > 0)
				transform.FindChild("lvl_score_top").GetComponent<TextMesh>().text = num.ToString();
			else transform.FindChild("lvl_score_top").gameObject.SetActive(false);
		}
		
		
	}
}
