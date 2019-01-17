using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;
 
public class RuleString
{
	public string name;
	public string left;
	public string right;
}

public class Main : MonoBehaviour 
{

	// Struct that defines the state of the gameplay
	struct GameState{
		public static int 	PLAYER 	= 0;	// the game is waiting for the player action
		public static int 	PHYSIC 	= 1;	// the game is executing the game physics
		public static int 	LOGIC 	= 2;	// the game is testing the game rules
		public static int   END  	= 3;	// the game is on the end screen
	}

	Level ref_level = new Level();

	// RULES
	List <Rule> rules = new List<Rule> ();
	public List <RuleString> RuleCheker = new List<RuleString> ();

	private int rule_pointer = 0;
	public Rule current_rule = null;

	// STATES
	private int STATE = GameState.PHYSIC;	// current state of the game
	private bool first_cycle = true;
	private bool first_cycle_player = true;		// true if the first cicle of the current state is running
	public bool interationEnable = true;

	// LISTS
	private List<GameObject> blocks = 					new List<GameObject>();			// all the blocks in the scene
	private List<BlockSettings> new_blocks = 			new List<BlockSettings>();	// settings of new blocks that must be created (if BLOCK_RENEW)
	private List<GameObject> blocks_to_destroy = 		new List<GameObject>();
	
	private List<GameObject> requests = 				new List<GameObject> ();		// requests in the game
	private List<int> new_requests =					new List<int>();			// IDs of the new requests
	private List<GameObject> requests_to_destroy = 		new List<GameObject>();

	//GAME DATA
	public TextAsset testLevel = null;
	private GameData data;


	// UNITY EDITOR
	private GameObject GAME_AREA;			// reference to the "game area" object (TAG = "Game_Area")
	public GameObject GAME_ENDSCREEN;		// reference to the "game end screen" object (TAG = "Game_EndScreen")
	private GameObject GAME_PAUSESCREEN;	// reference to the "game pause screen" object (TAG = "Game_Pause_Screen")
	//private GameObject REQUEST_AREA;		// reference to the "request_area" object (TAG = "Request_Area")
	private Vector3 STARTING_POSITION;		// top left position of the game area (defined by tag "Game_Start")
	private Vector3 AREA_STARTING_POSITION; // left position of the request area (defined by tag "Request_Start")
	private RuleList ruleList;
	private int WIDTH = 8;					// width (in blocks) of the game area
	private int HEIGHT = 6;					// height (in blocks) of the game area
	
	// public
	//public GameObject RuleCreator;		  // referecen to the Rule Creator object
	public GameObject timer;					// reference to the timer object
	public TextAsset rule_txt;				//reference to the game "rules" configuration
	public GameObject block_ref;			// reference to the "block" object
	public GameObject request_ref;			// reference to the "request" object
	
	// selection variables
	private GameObject[] selected_objects = new GameObject[2];
	const int first = 0;
	const int second = 1;
	const float selection_timer_max = 0.25f;
	float selection_timer = 0.0f;
	bool selection_timer_running = false;
 
	// CLASS REFERENCES
	Factory factory;

	// CONSTANTS
	
	// Colors
	private Color GreenColor = new Color (171f/255f, 213f/255f, 93f/255f);
	private Color RedColor   = new Color (200f/255f, 40f/255f, 40f/255f); 

	// SCALE AND POSITIONING
	private const float 	BLOCK_SIZE = 1.0f; 	  // scale of the blocks 
	private const float 	Z_POS = 0;			  // Z position of the blocks
	private const float 	request_space = 1.2f; // space between requests
	private const float		request_max_area = 7f;
	 

	// LEVEL CONFIGURATION
	private int 			rules_max				= 5;
	
	private float 			Level_Time_Limit = 		20f;
	// TODO Dynamic level time (level file)
	
	private const bool 		BLOCK_RENEW 			= true;		// define if the blocks will be renewed when destroyed
	
	private const bool 		REQUEST_RENEW 			= true; 	// define if the requests will be renewed
	private bool 			request_max_defeat		= true;		// wether reaching maximum request numbe equals defeat
	private float			request_add_time		= 5f;		// maximum time for request adding to the renewal buffer
	private float			request_variation		= 1.5f;		// maximum time for request adding to the renewal buffer
	//private int				request_renew_buffer 	= 1;		// maximum number of requests that will renew at each renewal
	//private float			request_renew_time 		= 15f;		// maximum time of the request renewal 
	//private float			request_renew_var		= 2f;		// maximum variation of request adding

	private int				request_max				= 6;		// maximum number of requests on the list
	
	//private bool Interpolate = false;

	private float 			level_timer;							// timer for the game time
	private float			request_add_timer = 2;					// timer for the request adding
	//private float			request_renew_timer = 0;				// timer for the request renewal
	
	// PHYSICS
	private float physics_sleep_timer;
	private const float physics_sleep_time = 1.0f;

	// WIDGETS
	private int last_score;
	public TextMesh widget_score;
	public TextMesh widget_score_target;
	
	float timer_scale_x;
	float timer_pos_x;

	// OTHER
	const int destroy = -1;
	string level_string;
	bool grid_action = false;
	bool stop_time = false;
	bool test_data = false;
	private SoundControler Sound;

	//Pontuaçao
	
	
	private int score=0;
	private int hiScore=0;
	private int targetScore=0;
	private int trophyScore1=0;
	private int trophyScore2=0;
	private int burnLimit=0;
	private int lostLimit=0;
	public int requestLost=0;
	
	
	// INITIALIZATION ----------------------------------------------------------------------------------------------------


	
	
	void Awake(){
		
		GameObject _Sound = GameObject.FindGameObjectWithTag("Sound");
		if(_Sound != null) Sound = _Sound.GetComponent<SoundControler>();
	
		// LOAD GAME DATA
		data = GameData.GetInstance();
		if (data == null) 		Debug.Log ("Error - Data not found");
				
		// LOAD FACTORY COMPONENT
		factory = Factory.GetInstance ();
		if (factory == null) 		Debug.Log ("Error - Factory not found");
		else 
			factory.InitFactory (BLOCK_SIZE, Z_POS, block_ref, request_ref);

		// LOAD GAME AREA
		GAME_AREA = GameObject.FindGameObjectWithTag ("Game_Area");
		if (GAME_AREA == null) 		Debug.Log ("Error - Game Area not found");

		// LOAD ENDSCREEN
//		GAME_ENDSCREEN = GameObject.FindGameObjectWithTag ("Game_EndScreen");
//		if (GAME_ENDSCREEN == null) 	Debug.Log ("Error - EndScreen not found");
//		else 
//			GAME_ENDSCREEN.GetComponent<RuleInterfaceComponent>().Deactivate();
					
		// GET BLOCK STARTING POSITION
		STARTING_POSITION = GameObject.FindGameObjectWithTag ("Game_Start").transform.position;
		if (STARTING_POSITION == Vector3.zero)		Debug.Log ("Error - Block Starting Position not found");
		else
			STARTING_POSITION.z = Z_POS;
		
		// GET REQUEST STARTING POSITION
		AREA_STARTING_POSITION = GameObject.FindGameObjectWithTag ("Request_Start").transform.position;
		if (AREA_STARTING_POSITION == Vector3.zero) 	Debug.Log ("Error - Requst Starting Position not found");
		else
			AREA_STARTING_POSITION.z = Z_POS;
		
		// GET RULE LIST
		GameObject go_rulelist = GameObject.FindGameObjectWithTag ("RuleList");
		if (go_rulelist == null)  Debug.Log ("Error - Rule List not found");
		else{
			ruleList = go_rulelist.GetComponent<RuleList> ();
			ruleList.SendMessage("SetRuleMax",rules_max);
		}

	}

	// Use this for initialization
	void Start () {

		InitRules ();
		InitLevel ();
		InitRequests ();
		InitTimers ();
		InitWidgets();
		//Debug.Log("Play: Game_BGS");
		Sound.PlayBGS("Game_BGS");
		//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlayBGS("Game_BGS");

	}

	private void InitLevel(){

		//int level_number = Application.loadedLevel - level_diff;
		//Reader.IniciaLevel (ref_level,level_txt[level_number],HEIGHT,WIDTH);
		//ef_map.IniciaMapa (level_txt,HEIGHT,WIDTH);
		
		if (data.current_level == null){
			Reader.IniciaLevel(ref_level,testLevel,HEIGHT,WIDTH);
			data.current_level = ref_level;
			level_string = "TestLevel";
		}
		else{
			ref_level = data.current_level;
			level_string = data.level_string;
		}
		
		
		foreach (BlockSettings block in ref_level.gridDeIngredientes){
			factory.CreateBlock(Support.GridToWorld(block.GridPosition,BLOCK_SIZE,STARTING_POSITION), block.id, ref blocks);
		}
		
		// Deactivate objects based on file
		List<string> objects_to_deactivate = new List<string>();
		
		//RuleCreator.GetComponent<RuleCreatorContainer>().SetAnd(ref_level.and);
		if (ref_level.lock_and) objects_to_deactivate.Add ("Gui_Container_and");
		if (ref_level.lock_or)  objects_to_deactivate.Add ("Gui_Container_or");
		if (ref_level.lock_not) objects_to_deactivate.Add ("Gui_Container_not");
		if (ref_level.lock_teo) objects_to_deactivate.Add ("Gui_Container_teo");
		if (ref_level.lock_imp) objects_to_deactivate.Add ("Gui_Container_Imp");
		if (ref_level.lock_rule_selector) objects_to_deactivate.Add ("Gui_Rule_Selector");
		if (ref_level.lock_switcher_button) objects_to_deactivate.Add ("Gui_Button_Switch");
		
		BroadcastMessage ("LockInterface",objects_to_deactivate,SendMessageOptions.DontRequireReceiver);
//		for (int i = 0; i < transform.childCount; i++){
//			
//			GameObject child = 	transform.GetChild(i).gameObject;
//			bool active = child.activeSelf;
//			if (!active) child.SetActive(true);
//			child.SendMessage("LockInterface",objects_to_deactivate,SendMessageOptions.DontRequireReceiver);
//			if (!active) child.SetActive (false);	
//			//InterfaceComponent Interface = transform.GetChild(i).GetComponent<InterfaceComponent>();
//			//if (Interface != null) Interface.Lock(names);
//		}
//				
		if (data.current_level_out != null) hiScore = data.current_level_out.top_score;
				
		targetScore		= ref_level.targetScore;
		trophyScore1	= ref_level.trophyScore1;
		trophyScore2	= ref_level.trophyScore2;
		burnLimit		= ref_level.burnLimit;
		lostLimit		= ref_level.lostLimit;
		//requestLost		= ref_level.requestLost;
		
		if (data != null && data.current_level_tutorial != null){
			Instantiate (data.current_level_tutorial, Vector3.zero,Quaternion.identity);
		}
	}

	private void InitRules(){

		// adding default rule with no effect
		ImplicationExpressionComposite implication0 = new ImplicationExpressionComposite ();
		Rule empty = new Rule ("No Rule",implication0);
		rules.Add (empty);

		Reader.LoadRules (RuleCheker, rule_txt);
		current_rule = rules[rule_pointer];
	}

	private void InitRequests()
	{
		//float offset = BLOCK_SIZE * 0.2f;
		
		//ResetRequestAddTimer();
		//ResetRequestRenewTimer();

//		Vector2 position = AREA_STARTING_POSITION;
//		//position.x += request_space;
//
//		foreach (int id in ref_level.listaDePedidos) {
//			factory.CreateRequest(position,id, ref requests);
//			position.x += request_space;
//		}
		
		ReorganizeRequests();
	}
	
	void InitTimers(){
		level_timer = Level_Time_Limit;
	}

	
	
	void InitWidgets(){
		
		
		timer_scale_x = timer.transform.localScale.x;
		timer_pos_x = timer.transform.position.x;
		
		widget_score_target.text = targetScore.ToString("0000");
	}

	// UPDATE --------------------------------------------------------------------------------------------------------

	void Update () 
	{
		if(STATE != GameState.END)
		{
			if (STATE == GameState.LOGIC) 	LogicState();	
			else 							PlayerActions();

			if (SeekAndDestroy()) ChangeState(GameState.PHYSIC);
//			DestroyBlocks();

			TickTimers ();
			RenewRequests();
			DrawWidgets();
			
			CheckCompleteConditions();
		}
	}
	
	void FixedUpdate()
	{
		if (STATE == GameState.PHYSIC) {
			PhysicState();
		} 
	}


	void ChangeState (int state)
	{
		STATE = state;
		first_cycle = true;
	}

	void DestroyBlocks ()
	{
		if (blocks_to_destroy.Count > 0){
			for (int i = 0; i < blocks_to_destroy.Count; i++)
			{
				if (BLOCK_RENEW){
					BlockSettings n_block = new BlockSettings();
					n_block.GridPosition = Support.WorldToGrid(blocks_to_destroy[i].transform.position,BLOCK_SIZE,STARTING_POSITION);

					//= new BlockSettings(to_destroy[i].transform.position.x, to_destroy[i].transform.position.y, to_destroy[i].transform.position.z);
					new_blocks.Add(n_block);
					//CreateBlock(blocks[i].transform.position.x, STARTING_POSITION.y + y_pos * BLOCK_SIZE, true)	
				}
	
				GameObject.Destroy(blocks_to_destroy[i]);
				blocks.Remove(blocks_to_destroy[i]);
			}

			blocks_to_destroy.Clear();
			
			//ChangeState(GameState.PHYSIC);
		}
	}
	
	void DestroyRequests(){
		if (requests_to_destroy.Count > 0){
			for (int i = 0; i < requests_to_destroy.Count; i++)
			{
				GameObject.Destroy(requests_to_destroy[i]);
				requests.Remove(requests_to_destroy[i]);
			}
			
			requests_to_destroy.Clear();

		}
		
		//ReorganizeRequests();
	
	}
	
	public void ReorganizeRequests()
	{
		for (int i=0;i<requests.Count;i++) 
		{
			Vector2 target = new Vector2(AREA_STARTING_POSITION.x + request_space * i,AREA_STARTING_POSITION.y);
			//requests[i].transform.position = AREA_STARTING_POSITION;
			//requests[i].transform.Translate(i*request_space,0,0);
			requests[i].SendMessage("MoveTo",target);
		}
	}

	private bool SeekAndDestroy()
	{
		bool destroyed = false;
		// seek and destroy blocks
		foreach (GameObject block_game_object in blocks) {
			
			Block block_comp = block_game_object.GetComponent<Block> ();
			if (block_comp.Color_Index == destroy){
				destroyed = true;
				blocks_to_destroy.Add (block_game_object);
			}
		}
		
		// seek and destroy requests
		foreach (GameObject request in requests) {
			
			Food.Request request_comp = request.GetComponent<Request> ();
			if (request_comp.Color_Index == destroy)
				requests_to_destroy.Add (request);
		}	
		
		return destroyed;
		
	}
	
	public void RenewBlocks()
	{
		if (new_blocks.Count > 0)
		{
			foreach (BlockSettings block in new_blocks) {
				
				Vector3 block_world_position = Support.GridToWorld(new Vector2 (block.GridPosition.x, block.GridPosition.y + HEIGHT + 1),BLOCK_SIZE,STARTING_POSITION);
				factory.CreateBlock(block_world_position, ref_level.GetRandomID(block.id), ref blocks);
			}
			
			new_blocks.Clear();
			
		}
	}


//	private int				request_max				= 5;		// maximum number of requests on the list

	void ResetRequestAddTimer(){
		request_add_timer = request_add_time + Random.Range (-request_variation,request_variation);
		
	}
	
//	void ResetRequestRenewTimer(){
//		request_renew_timer = Random.Range (Mathf.Clamp(request_renew_time-request_renew_var,0,1000),request_renew_time+request_renew_var);
//	}
	
//	void AddRequestToBuffer(){
//	
//		if (new_requests.Count <= request_renew_buffer){
//			new_requests.Add (ref_level.listaDePedidos[Random.Range (0,ref_level.listaDePedidos.Count)]);
//			//Debug.Log ("Add to buffer (" + new_requests.Count + ")");
//		}
//		
//		ResetRequestAddTimer();
//		
//	}
	
	void TickTimers(){
		if (!stop_time){
			//request_renew_timer -= Time.fixedDeltaTime;
			request_add_timer -= Time.fixedDeltaTime;
			level_timer -= Time.deltaTime;
		}
	}
	
	public void RenewRequests(){
	
		
		if (REQUEST_RENEW == true && !stop_time){
					
			if (request_add_timer <0){// requests.Count - requests_to_destroy.Count <= 0){
			
				//Debug.Log ("Renew requests");
				
				DestroyRequests();
				
				Vector2 rr_pos = (Vector2)AREA_STARTING_POSITION + new Vector2(request_max_area,0);			
				
				int requests_to_add = request_max - requests.Count;
				//int new_requests_size = new_requests.Count;
				//int remove = 0;
				//Debug.Log ("new requests size " + new_requests_size);
				
				int id = ref_level.listaDePedidos[Random.Range (0,ref_level.listaDePedidos.Count)];

				GameObject request = factory.CreateRequest(rr_pos,id, ref requests);
				request.transform.Translate(new Vector3(request_space+1,0,0));
					
				if (requests.Count > request_max){
					MoveRequestToLimbo(0);
				}
					
				
				
				ReorganizeRequests();		
				//ResetRequestRenewTimer();
				ResetRequestAddTimer();
				
				//request_renew_buffer += (int)Random.Range (-request_renew_var,+request_renew_var);
				//Debug.Log ("BUFFER = " + request_renew_buffer);
				
			}
		}
	
	}
	
	void MoveRequestToLimbo(int ind){
		GameObject request = requests[ind];
		requests.RemoveAt (ind);
		request.SendMessage("MoveToLimbo");
		//request.SendMessage("",new Vector2(AREA_STARTING_POSITION.x - 10,AREA_STARTING_POSITION.y));
		//requests_to_destroy.Add (request);
		
		requestLost++;
	}
	
	void ForceNewRequests(int[] id){
	
		Vector2 rr_pos = (Vector2)AREA_STARTING_POSITION + new Vector2(request_max_area,0);		
		
		int add = 0;
		int pos = 0;
		foreach (int i in id){
			if (requests.Count + add < request_max){
				GameObject request = Factory.GetInstance().CreateRequest(rr_pos,i, ref requests);
				request.transform.Translate(new Vector3(request_space*pos++,0,0));
				//request.transform.Translate(new Vector3(request_space*i,0,0));
				add++;
			}
			else return;
		}

		ReorganizeRequests();
	
	}


	// LOGIC STATE ----------------------------------------------------------------------------------------------------

	private void LogicState(){
		//Debug.Log ("ENTER LOGIC");
		bool match = ScanMatches();
		//Debug.Log ("Match = " + match);
		if (match) ChangeState 	(GameState.PHYSIC);	// if there is a match, go to PHYSIC state
		else ChangeState 		(GameState.PLAYER);	// if there is NOT a match, go to PLAYER state
	}

	private bool ScanMatches(){

		bool match = false;

		if(current_rule != null)
		{
			foreach (GameObject game_block in blocks)
			{
				MatchFinder(new List<GameObject>(), 0, game_block,ref match);
			}
			if(current_rule.IDResult != 0 && match)
			{
					//Debug.Log("Play: Mach_SFX");
					Sound.PlaySFX("Mach_SFX");
			}
			else if(current_rule.IDResult == 0 && match)
			{
				//Debug.Log("Play: Burn_SFX");
				Sound.PlaySFX("Burn_SFX");
			}
		}
		return match;
	}


	void MatchFinder (List<GameObject> list, int direcao, GameObject block,ref bool match)
	{	
		List<GameObject> new_list = new List<GameObject>();

		foreach (GameObject n_block in list)
		{
			new_list.Add(n_block);
		}

		new_list.Add (block);

		if (direcao == 1) direcao = 3;
		if (direcao == 2) direcao = 4;
		if (direcao == 3) direcao = 1;
		if (direcao == 4) direcao = 2;
		
		for (int i=1; i<=4; i++) 
		{	
			if(direcao != i)
			{
				Vector3 pos_aux = 	  new Vector3(block.transform.position.x +BLOCK_SIZE ,	block.transform.position.y, block.transform.position.z);				// RIGHT
				if (i == 1) pos_aux = new Vector3(block.transform.position.x +BLOCK_SIZE ,	block.transform.position.y, block.transform.position.z);
				if (i == 2) pos_aux = new Vector3(block.transform.position.x ,	block.transform.position.y +BLOCK_SIZE, block.transform.position.z);
				if (i == 3) pos_aux = new Vector3(block.transform.position.x -BLOCK_SIZE ,	block.transform.position.y, block.transform.position.z);
				if (i == 4) pos_aux = new Vector3(block.transform.position.x  ,	block.transform.position.y-BLOCK_SIZE, block.transform.position.z);

				GameObject colliding_block = Support.Intersect(new string[1] {"Block"},pos_aux);

				if (new_list.Count < current_rule.Size)
				{
					if ((colliding_block != null) && (!new_list.Contains(colliding_block)))
					{
						MatchFinder(new_list, direcao, colliding_block,ref match);

					}

				}
				else if (new_list.Count >= current_rule.Size)
				{

					Context cont = new Context();
					List<GameObject> context_list = new List<GameObject>();

					foreach (GameObject block_in_context in new_list)
					{
						context_list.Add (block_in_context);
					}

					cont.block_settings_array = context_list;

					if (current_rule.Expression.run(cont) != null)
					{


						for (int k = 0; k < new_list.Count; k++)
						{
							if (k == new_list.Count/2){ 

								int result = current_rule.IDResult;

								new_list[k].GetComponent<Block>().ChangeId(result);
							}
							else{
								new_list[k].GetComponent<Block>().ChangeId(destroy);

							}
							
						}
						score += 10;
						Report.MatchAction(level_string,current_rule.IDResult);
						match = true;
					}

				}
				
			}
		}

	}

	// PHYSICS STATE ------------------------------------------------------------------------------
	
	void PhysicState()
	{
		// TURN ON THE PHYSICS
		if (first_cycle)
		{					
			//Debug.Log ("ENTER PHYSIC");

//			SeekAndDestroy ();
			DestroyBlocks ();
									
			RenewBlocks();

			SetBlockKInematic(false);
			WakeBlocks();

			first_cycle = false;
			
			PhysicsTimerReset();

		}
		if (physics_sleep_timer <= 0f) {

			// CHECK IF SLEEPING
			bool all_sleeping = true;
					
			//Continue only if all block are sleeping and not moving
//			foreach (GameObject block in blocks) {
//					
//					if (!block.rigidbody.IsSleeping()){// () || block.GetComponent<Block>().IsMoving()){	
//						all_sleeping = false;
//						break;
//					}
//			}

			// TURN OFF THE PHYSICS
			if (!AnyBlockAwake()) {
			
				SetBlockKInematic (true);
				RoundPositions ();
				ChangeState (GameState.LOGIC);
				PhysicsTimerReset();
			}
			
			else physics_sleep_timer = physics_sleep_time;

		}

		else physics_sleep_timer -= Time.fixedDeltaTime;
	}
	
	bool AnyBlockAwake(){
		foreach (GameObject block in blocks) {
			
			if (!block.rigidbody.IsSleeping()){// () || block.GetComponent<Block>().IsMoving()){	
				return true;
			}
		}
		
		return false;
		
	}
	
	void PhysicsTimerReset(){
		physics_sleep_timer = physics_sleep_time;
	}

	void WakeBlocks()
	{
		foreach (GameObject block in blocks)
		{
			block.rigidbody.WakeUp(); 
			block.rigidbody.AddForce(Vector3.zero);
		}
	}


	void SetBlockKInematic(bool kinematic)
	{
		foreach (GameObject block in blocks)
		{
			
			block.rigidbody.isKinematic = kinematic;
			
		}
	}

	void RoundPositions()
	{
		foreach (GameObject block in blocks)
		{
			block.transform.position = new Vector3(Mathf.Floor(block.transform.position.x) + (BLOCK_SIZE * 0.5f),// + (STARTING_POSITION.x - Mathf.Floor(STARTING_POSITION.x)), 
			                                       Mathf.Floor(block.transform.position.y) + (BLOCK_SIZE * 0.5f),// + (STARTING_POSITION.y - Mathf.Floor(STARTING_POSITION.y)),
			                                       Z_POS);
			//block.transform.position += STARTING_POSITION;
		}
	}

// PLAYER STATE ------------------------------------------------------------------------------

	void PlayerActions(){
	
		if (first_cycle_player){ 
			grid_action = false;
			first_cycle_player = false;
			//Debug.Log("ENTER PLAYER");
			}

		int action_executed = 0;

		if (Input.GetMouseButtonDown(0)) 
		   TouchSelect();
		   
		else if (Input.GetMouseButtonUp(0))
		{
			if (ReleaseSelect())
				action_executed = PlayerAction();	
		}
	
		if (grid_action && !AnyBlockMoving ()){ // GRID ACTION[
				
				ChangeState(GameState.LOGIC);
				first_cycle_player = true;
		}
		
		if (selection_timer_running)
			 TickSelectionTimer();

	}
	
	void TickSelectionTimer(){
	
		selection_timer += Time.deltaTime;
		
		if (selection_timer >= selection_timer_max){
			//Vector2 mouse_pos = WorldMousePosition();
			GameObject.FindGameObjectWithTag("Follower").SendMessage("Appear",selected_objects[first].GetComponent<SpriteRenderer>().sprite);
			Sound.PlaySFX("Select_SFX");
			//Debug.Log("Switch Action");
			
			ResetSelectionTimer();
		}
		
	}
	
	void ResetSelectionTimer(){
		selection_timer_running = false;
		selection_timer = 0;	
	}
	
	bool AnyBlockMoving(){
		foreach (GameObject block in blocks){
			if (block.GetComponent<Block>().IsMoving ())	return true;
		}
		return false;
	}

	Vector3 WorldMousePosition()
	{
		Vector3 mouse_position = camera.ScreenToWorldPoint(Input.mousePosition);
		mouse_position.z = Z_POS;
		return mouse_position;
	}
	
	bool TouchSelect(){
	
		GameObject selected = Support.Intersect(new string[] {"Block","Recipe"}, WorldMousePosition());
		
		if (selected == null) return false;
		
		Select (first,selected);
		//Debug.Log (selected.tag);
		//Debug.Log("Play: Select_SFX");
		
		
		if (selected.tag == "Recipe")
			GameObject.FindGameObjectWithTag("Trash").SendMessage("Show");
				
		
		selection_timer_running = true;
		
		return true;
	}
	
	bool ReleaseSelect(){
	
		
		
		if (selected_objects[first] == null) 
			return false;
			
		if (!selection_timer_running) 
			return DragObject();
		
		return DragSwitch();
		
	
	}
	
	bool DragObject(){
		
		GameObject selected = Support.Intersect(new string[] {"Block","Request","Slot","Recipe","Trash"}, WorldMousePosition());
		
		//GameObject.FindGameObjectWithTag("Follower").SendMessage ("MoveTo", selected_objects[first].transform.position);
		
		GameObject.FindGameObjectWithTag("Follower").SendMessage ("Disappear");
		
		GameObject trash = (GameObject.FindGameObjectWithTag("Trash"));
		if (trash!= null && trash.GetComponent<TrashCan>().lerp >= 1) trash.SendMessage("Hide");
		
		if (selected == null){
			UnSelect (first);
			return false;
		}
		
		if (selected_objects[first].tag == "Recipe" && selected.tag == "Trash"){
			RemoveRecipeAction();
			return false;
		}
		
		
		
		Select (second,selected);
		
		ResetSelectionTimer();
		
		
		return true;
	
	}
	
	bool DragSwitch(){
	
		Vector2 mouse_pos = WorldMousePosition();
		
		Vector2 selected_pos = selected_objects[first].transform.position;
		
		Vector2 delta = selected_pos - mouse_pos;
		
		if 		(delta.x > 0 && delta.x > Mathf.Abs (delta.y))
					selected_objects[second] = Support.Intersect(new string[] {"Block","Request","Slot","Recipe"}, selected_pos - Vector2.right);
		
		else if (delta.x < 0 && delta.x  < -1*(Mathf.Abs (delta.y)))
		         	selected_objects[second] = Support.Intersect(new string[] {"Block","Request","Slot","Recipe"}, selected_pos + Vector2.right);
		
		else if (delta.y > 0 && delta.y > Mathf.Abs (delta.x))
			selected_objects[second] = Support.Intersect(new string[] {"Block","Request","Slot","Recipe"}, selected_pos - Vector2.up);
		
		else if (delta.y < 0 && delta.y  < -1*(Mathf.Abs (delta.x)))
			selected_objects[second] = Support.Intersect(new string[] {"Block","Request","Slot","Recipe"}, selected_pos + Vector2.up);
		
		
		
		if (selected_objects[second] != null){
			SwitchAction();
			
			
		}
		
		UnSelectAll();
		ResetSelectionTimer();
		
		return false;
	
	}
	


	int PlayerAction()
	{

		if ((selected_objects[first] != null) && (selected_objects[second] != null)){

			// Check if selected objects are not the same
			if (selected_objects[first] == selected_objects[second])
			{
				UnSelectAll();
				return 0;
				
			}

			int selection_type = SelectionType (selected_objects);

			if (selection_type == 1 && STATE == GameState.PLAYER && !grid_action) // BLOCK/BLOCK selection
			{
				SwitchAction();
								
				UnSelectAll();
				return 1; 
			}

			else if (selection_type == 2) // BLOCK/REQUEST selection
			{
				
				if (RequestAction() == true){
					UnSelectAll ();
					return 2;
				}
				UnSelectAll ();
				return 0;
			}
			else if (selection_type == 3) // BLOCK or RECIPE/SLOT selection
			{
				SetSlotAction();
				UnSelectAll ();
				return 3;
			}
			else // INVALID selection
			{
				UnSelectAll ();
				return 0;
			}


		}

		return 0;
	}
	
	void RemoveRecipeAction(){
	
		selected_objects[first].SendMessage("Remove");
	
	}

	void SetSlotAction(){

		if (selected_objects [first].tag == "Block" && selected_objects[second].GetComponent<Slot>().accept_ingredients) {
			Block block = selected_objects[first].GetComponent<Block> ();
			selected_objects[second].GetComponent<Slot>().SetExpression(block.Color_Index);
		}

		else if (selected_objects [first].tag == "Recipe" && selected_objects[second].GetComponent<Slot>().accept_recipes) {
			Recipe recipe = selected_objects[first].GetComponent<Recipe> ();
			selected_objects[second].GetComponent<Slot>().SetExpression(recipe.ID, recipe.expression);
		}
		
		//Block block = selected_objects[first].GetComponent<Block> ();
		
		//if (block != null){


	}
	
	void SwitchAction(){
		if (STATE != GameState.PLAYER) return;
		
	
		if (Support.isAdjacent(selected_objects[first],selected_objects[second],BLOCK_SIZE))
		{
			Support.SmoothSwitch(selected_objects[first],selected_objects[second]);
			Sound.PlaySFX ("Slide_SFX");
			grid_action = true;
			UnSelectAll();

		}
	}

	bool RequestAction(){

		//Debug.Log ("request Action");

		Block block 	= selected_objects [first].GetComponent<Block> ();
		Request request = selected_objects [second].GetComponent<Request> ();

		if ((block != null) && (request != null))
		if (block.Color_Index == request.Color_Index)
		//if (request.Color_Index != destroy)
		{
			block.ChangeId(destroy);
			request.ChangeId(destroy);
			//Debug.Log("Play: Done_SFX");
			Sound.PlaySFX("Done_SFX");
			score += 50;
			return true;
		}
		
		return false;
							
	}


	int SelectionType(GameObject[] objects)
	{
		if (objects[first].tag == "Block")

			if (objects[second].tag == "Block")
				return 1;	// BLOCK AND BLOCK
			else if (objects[second].tag == "Request")
				return 2;	// BLOCK AND REQUEST
			else if (objects[second].tag == "Slot")
				return 3;	// BLOCK AND Slot
		if (objects [first].tag == "Recipe"){
				if (objects [second].tag == "Slot")
					return 3; 	// RECIPE AND SLOT
				
			}

		return 0;					// INVALID						
	}

	void Select(int index, GameObject selected) 
	{
		if (selected != null){
		
			UnSelect(index);		
						
			Report.Selection(level_string, selected);
			
			index = Mathf.Clamp (index, first, second);
			
			selected.SendMessage("Select",SendMessageOptions.DontRequireReceiver);
			
			selected_objects [index] = selected;
			
		}
	

	}
	
	


	void UnSelect(int index)
	{
		index = Mathf.Clamp (index, 0, 1);
		
		if (selected_objects [index]!= null ) selected_objects [index].SendMessage("Unselect",SendMessageOptions.DontRequireReceiver);

//		if (selected_objects [index].tag == "Block")
//			selected_objects [index].GetComponent<Block> ().SwitchSelector (false);

		selected_objects [index] = null;
	}

	void UnSelectAll()
	{
		UnSelect (first);
		UnSelect (second);
	}

	public void SwitchRule(GameObject rule)
	{
		if (STATE == GameState.PLAYER) 
		{
			if( rule != null){
				current_rule = rule.GetComponent<RuleButon>().RULE;
				//Report.ActivateRule(level_string, current_rule);
				}
			else
				current_rule = null;

			ruleList.setActiveRule(rule);
			ChangeState (GameState.LOGIC);
		}
	}

	public int GetRulesCount()
	{
		return rules.Count;
	}

	public Rule GetRule(int id)
	{
		return rules[id];
	}

	


	
	void CheckCompleteConditions()
	{
		if ( CheckLevelDefeat() ) CompleteLevel(false);
		else if ( CheckLevelVictory() ) CompleteLevel(true);
	}

	bool CheckLevelDefeat()
	{
	
		//return false;
		
		if (level_timer < 0 && (score < targetScore)){
			
			CompleteLevel(false);
			return true;
		}
		
		return false;
	}
	
	bool CheckLevelVictory(){
	
		//return false;
	
		//bool win;
		if (level_timer < 0 && (score >= targetScore))
		{
			CompleteLevel(true);
			return true;
		} 
		return false;
	}
	
	void CompleteLevel(bool victory)
	{
		STATE = GameState.END;
		
		Report.RestartLevel(victory);

		GAME_PAUSESCREEN = GameObject.FindGameObjectWithTag ("Game_PauseScreen");
		
		if(GAME_PAUSESCREEN == null)	Debug.Log ("Error - Pause Screen not found");
		else 							GAME_PAUSESCREEN.GetComponent<InterfaceComponent>().Deactivate();
		
		if (GAME_ENDSCREEN == null) 	Debug.Log ("Error - End Screen not found");
		else 							ShowEndScreen(victory);
		
		if (data == null) return;
				
		if (victory)data.UnlockNextLevel();
		
		data.SaveGame ();
	}
	
	void ShowEndScreen(bool win)
	{
		//GAME_ENDSCREEN = (GameObject)Instantiate(GAME_ENDSCREEN,GAME_ENDSCREEN.transform.position, GAME_ENDSCREEN.transform.rotation);
		GAME_ENDSCREEN.SetActive(true);
//		GAME_ENDSCREEN.GetComponent<InterfaceComponent>().Activate();

		bool[] check = new bool[6];
		
		if (data.current_level_out != null && data.current_level != null){

			if (score > hiScore){
				data.current_level_out.top_score = score;
			}
			
			if (win){
				if(score >= trophyScore1){
					check [0] = true; 
					data.current_level_out.trophy_one = true;
				}
				else check [0] = false;
				
				if(score >= trophyScore2){
					check [3] = true; 
					data.current_level_out.golden_trophy_one = true;
				}
				else check [3] = false;
				
				int _burned = 0;
				
				foreach(GameObject block in blocks)
					if(block.GetComponent<Block>().Color_Index == 0)
						_burned++;
				if (_burned < burnLimit){
					check [1] = true; 
					data.current_level_out.trophy_two = true;
				}
				
				else check [1] = false;
				
				if (_burned <= 0){
					check [4] = true; 
					data.current_level_out.golden_trophy_two = true;
				}
				else check [4] = false;
				
				if(requestLost < lostLimit){
					check [2] = true; 
					data.current_level_out.trophy_three = true;
				}
				else check [2] = false;
				
				if(requestLost == 0){
					check [5] = true;
					data.current_level_out.trophy_three = true;
				}
					else check [5] = false;
			}
			
		}
		
		else {
			check[0] = true;
			check[1] = true;
			check[2] = true;
			check[3] = true;
			check[4] = true;
			check[5] = true;

		}

		GAME_ENDSCREEN.GetComponentInChildren<EndGameControler>().Run(win,check, score > hiScore? true : false );
		Support.interactionEnable = false;
		//Application.LoadLevel (data.Last_Scene);
	}
	
	// WIDGETS ========================================================================================================
	
	void DrawWidgets(){
	
		// Draw Timer
		if (level_timer > 0){
			
			timer.transform.localScale = new Vector3(Mathf.Lerp(timer_scale_x,0, 1f - level_timer / Level_Time_Limit),
			                                         timer.transform.localScale.y,
			                                         timer.transform.localScale.z);
			
			                                
			float norm = level_timer / Level_Time_Limit;
			
			//timer.GetComponentInChildren<Renderer>().material.color = new Color(1f - level_timer / Level_Time_Limit,level_timer / Level_Time_Limit,0f);
			
			timer.GetComponentInChildren<Renderer>().material.color = Color.Lerp (RedColor,GreenColor,norm);
//			timer.transform.position = new Vector3(Mathf.Lerp(timer_pos_x, timer_pos_x - (timer_pos_x*(timer_scale_x/2)),1f - level_time / Level_Time_Limit),
//			                                       timer.transform.position.y,
//			                                       timer.transform.position.z);
			                                       

		}
		
		if (score != last_score){
			widget_score.text = score.ToString("0000");
			last_score = score;
		}
		
	}
	
	// PUBLIC FUNCTIONS =======================================================================================================
	
	public void StopTime(bool stop){
		stop_time = stop;
	}

	public void StopInteraction(bool stop){
		Support.interactionEnable = !stop;
	}
	
	public void EndLevel(bool victory){
		CompleteLevel(victory);
	}
	
	public void AddRequests(int[] id){
		ForceNewRequests(id);
	}
	
	public void AddRule(Rule _rule)
	{
		
		if (ruleList.GetComponent<RuleList>().CanAdd()){
			
			Report.CreateRule(_rule);
			//Debug.Log(_rule.Expression.ToString());
			SwitchRule (ruleList.AddRule(_rule));
		}
	}
	public void ForceRule(Rule _rule)
	{
		
		if (ruleList.GetComponent<RuleList>().CanAdd()){

			ruleList.AddRule(_rule);
			ruleList.setActiveRule(null);
		}
	}

}
