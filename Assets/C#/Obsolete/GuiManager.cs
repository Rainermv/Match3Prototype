using UnityEngine;
using System.Collections;

struct GuiState{
	public static int 	RULE_SELECTION 	= 0;	
	public static int 	RULE_CREATION 	= 1;	
}


public class GuiManager : MonoBehaviour {

	public Texture texture;
	public GameObject gui_obj;

	// GUI CONFIGURATION
	int GUI_STATE = GuiState.RULE_SELECTION;
	
	int GUI_WIDTH = 130;
	int GUI_HEIGHT = 300;
	
	int border = 10;
	
	int GUI_X = 0;
	int GUI_Y = 10;
	
	int offset_x = 5;
	int offset_y = 10;
	
	int BUTTON_SIZE_X = 0;
	int BUTTON_SIZE_Y = 30;

	Main main;

	BoxGUIObject gui_box;
	//GroupGUIObject rule_gui = new GroupGUIObject();
	Rect RULES_GUI_AREA;

	void Awake(){

		main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Main>();
	}

	// Use this for initialization
	void Start () {

		GUI_X = Screen.width - GUI_WIDTH - border;
		//BUTTON_SIZE_X = GUI_WIDTH - offset_y;

		gui_box = new AlignedBoxGUIObject (" ", new Rect (0,0,GUI_WIDTH,GUI_HEIGHT));

		BoxGUIObject child_box= new BoxGUIObject ("Simple box", new Rect (10,5,100,20));
		BoxGUIObject child_box2= new BoxGUIObject ("Textures box", new Rect (10,0,100,50));
			TextureGUIObject child_texture1 = new TextureGUIObject (texture, new Rect (15,25,20,20));
			TextureGUIObject child_texture2 = new TextureGUIObject (texture, new Rect (65,25,20,20));

		BoxGUIObject child_of_child_box1 = new BoxGUIObject ("Container Box", new Rect (10,10,100,80));
			ContainerGUIObject cont1 = new ContainerGUIObject (gui_obj, new Rect (0, 25, 50, 50));
			ContainerGUIObject cont2 = new ContainerGUIObject (gui_obj, new Rect (50, 25, 50, 50));

		ButtonGUIObject child_button1 = new ButtonGUIObject ("PRESS ME", new Rect (10,10,100,20));
			
	
		gui_box.AddChild (child_box);

		gui_box.AddChild (child_box2);
			child_box2.AddChild (child_texture1);
			child_box2.AddChild (child_texture2);

		gui_box.AddChild (child_of_child_box1);
			child_of_child_box1.AddChild (cont1);
			child_of_child_box1.AddChild (cont2);

		gui_box.AddChild (child_button1);
			
		RULES_GUI_AREA = new Rect(GUI_X, GUI_Y,GUI_WIDTH,GUI_HEIGHT);

	}

	void OnGUI () {

		string result = gui_box.Draw (RULES_GUI_AREA);

		if (result != null)
				Debug.Log (result);

		if (result == "PRESS ME") {
				gui_box.SetActive (false);
		}

	}

	void ChangeGuiState()
	{
	}
	
	void xxOnGUI () {
				
		int GUI_Y = 10;
		
		// Make a background box
		GUI.Box(new Rect(GUI_X,GUI_Y,GUI_WIDTH,GUI_HEIGHT), "Rule: " + main.current_rule.Name);
		
		GUI_Y += 10;
		
		if (GUI_STATE == GuiState.RULE_SELECTION) {
			
			for (int i = 0; i < main.GetRulesCount(); i++) {
				
				if (GUI.Button (new Rect(GUI_X + offset_x,
				                         GUI_Y + BUTTON_SIZE_Y * i + offset_y,
				                         BUTTON_SIZE_X,
				                         BUTTON_SIZE_Y),
				                main.GetRule(i).Name)) {
					
					//main.SwitchRule (i);
					//ChangeState (GameState.LOGIC);
				}
				
			}
			
		}
		
		if (GUI_STATE == GuiState.RULE_CREATION) {
			
			//ChangeState (GameState.LOGIC);
			
		}
		
		if (GUI.Button (new Rect (GUI_X + offset_x, GUI_Y + GUI_HEIGHT - BUTTON_SIZE_Y - offset_y, BUTTON_SIZE_X / 2, BUTTON_SIZE_Y),
		                "Selection")) {
			GUI_STATE = GuiState.RULE_SELECTION;
		}
		
		if (GUI.Button (new Rect (GUI_X + GUI_WIDTH - BUTTON_SIZE_X / 2 - offset_x, GUI_Y + GUI_HEIGHT - BUTTON_SIZE_Y - offset_y, BUTTON_SIZE_X / 2, BUTTON_SIZE_Y),
		                "Creation")) {
			GUI_STATE = GuiState.RULE_CREATION;
		}
		
		
		
		
	}
}
