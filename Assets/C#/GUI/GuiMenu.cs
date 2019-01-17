using UnityEngine;
using System.Collections;

namespace Food{

	public class GuiMenu : MonoBehaviour {
	
		public bool Parado = false;
		public GameObject bkg_dark;
		
		private bool mouse_down = false;
		//public GameObject menu_ingame;
	
		// Use this for initialization
		void Start(){
			Support.interactionEnable = true;
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (mouse_down) renderer.material.color = Color.gray;
			else renderer.material.color = Color.white;	
		}

		
		void OnMouseUp(){
			mouse_down = false;
		}
	
		void OnMouseDown()
		{
			mouse_down = true;
			ActivateMenu ();
		}
	
		void ActivateMenu()
		{
			if (Parado) 
			{
				//menu_ingame.SetActive(false);
				bkg_dark.SetActive(false);
			}
			else
			{
				//menu_ingame.SetActive(true);
				bkg_dark.SetActive(true);
			}
	
			Parado = !Parado;
			Support.interactionEnable = !Support.interactionEnable;
		}
	}
}
