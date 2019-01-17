using UnityEngine;
using System.Collections;

namespace Food{
	public class ConfirmationWindow :  OutGameElement{
	
		public void SetWindow(MenuEvent menu_event){
		
			string confirmation_text = " ";
		
			switch (menu_event){
				case MenuEvent.RestartLevel: confirmation_text = "Reiniciar nivel?"; break;
				case MenuEvent.MenuScreen: confirmation_text = "Voltar para o Menu?"; break;
				case MenuEvent.LevelScreen: confirmation_text = "Selecionar Niveis?"; break;
			}
		
			if (transform.childCount > 1){
				transform.GetChild(0).SendMessage("SetEvent",menu_event,SendMessageOptions.DontRequireReceiver);
				transform.GetChild(1).SendMessage("SetEvent",MenuEvent.Back,SendMessageOptions.DontRequireReceiver);
				TextMesh textMesh = transform.GetChild (2).GetComponent<TextMesh>();
				if (textMesh != null) textMesh.text = confirmation_text;
			}
		}
		
	}
}
