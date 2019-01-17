using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameElement : MonoBehaviour {
	
		void OnMouseDown(){
						
		}

		public void ToggleActive(bool active)
		{
			Debug.Log("Play: Slide_SFX");
			//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlayBGS("Slide_SFX");
			gameObject.SetActive(active);
		}

		void OnMouseOver(){
			
		}

		void OnMouseExit(){
			
		}

		void Update(){

		}

	}

}