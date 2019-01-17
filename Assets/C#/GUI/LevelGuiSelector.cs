using UnityEngine;
using System.Collections;

namespace Food{
	public class LevelGuiSelector : MonoBehaviour {
	
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	
		void OnGUI () {
			
			int GUI_WIDTH = 200;
			int GUI_HEIGHT = 200;
			
			//int border = 10;
			
			int GUI_X = UnityEngine.Screen.width / 2 - GUI_WIDTH/2;
			int GUI_Y = UnityEngine.Screen.height / 2 - GUI_HEIGHT / 2;
			
			// Make a background box
			GUI.Box(new Rect(GUI_X,GUI_Y,GUI_WIDTH,GUI_HEIGHT), ".:Fases:.");
			
			GUI_Y += 40;
			
			int offset = 10;
			
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if(GUI.Button(new Rect(GUI_X + offset/2 ,GUI_Y,GUI_WIDTH-offset,20), "Fase 1" )) {
				Application.LoadLevel(1);
			}
			
			GUI_Y += 30;
			
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if(GUI.Button(new Rect(GUI_X + offset/2 ,GUI_Y,GUI_WIDTH-offset,20), "Fase 2")) {
				Application.LoadLevel(2);
			}
			
			GUI_Y += 30;
			
			// Make the second button.
			if(GUI.Button(new Rect(GUI_X+ offset/2,GUI_Y,GUI_WIDTH-offset,20), "Fase 3")) {
				Application.LoadLevel(3);
			}
		}
	}
}
