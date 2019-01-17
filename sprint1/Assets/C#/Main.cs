using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

public class Main : MonoBehaviour {

	// Struct that defines the state of the gameplay
	struct GameState{
		public static int 	PLAYER 	= 0;	// the game is waiting for the player action
		public static int 	PHYSIC 	= 1;	// the game is executing the game physics
		public static int 	LOGIC 	= 2;	// the game is testing the game rules
	}

	Level ref_level = new Level();

	// RULES
	List <Rule> rules = new List<Rule> ();
	private int rule_pointer = 0;
	private Rule current_rule = null;

	// STATES
	private int STATE = GameState.PHYSIC;	// current state of the game
	private bool first_cycle = true;		// true if the first cicle of the current state is running

	// LISTS
	private List<GameObject> blocks = 					new List<GameObject>();			// all the blocks in the scene
	private List<GameObject> matches = 					new List<GameObject>();			// matches made in the current cycle
	private List<BlockSettings> new_blocks = 			new List<BlockSettings>();	// settings of new blocks that must be created (if BLOCK_RENEW)
	private List<GameObject> blocks_to_destroy = 		new List<GameObject>();
	private List<GameObject> requests_to_destroy = 		new List<GameObject>();
	private List<GameObject> requests = 				new List<GameObject> ();		// requests in the game

	// UNITY EDITOR	
	private GameObject GAME_AREA;			// reference to the "game area" object (TAG = "Game_Area")
	private GameObject REQUEST_AREA;		// reference to the "request_area" object (TAG = "Request_Area")
	private Vector3 STARTING_POSITION;		// top left position of the game area (defined by tag "Game_Start")
	private Vector3 AREA_STARTING_POSITION; // left position of the request area (defined by tag "Request_Start")
	private int WIDTH = 10;					// width (in blocks) of the game area
	private int HEIGHT = 8;					// height (in blocks) of the game area
	// public
	public TextAsset rule_txt;				//reference to the game "rules" configuration
	public TextAsset level_txt;				//reference to the game levels
	public GameObject block_ref;			// reference to the "block" object
	public GameObject request_ref;			// reference to the "request" object
	
	// selection variables
	private GameObject[] selected_objects = new GameObject[2];
	const int first = 0;
	const int second = 1;
	//private GameObject selected_second = null;

	// CONSTANTS

	// SCALE AND POSITIONING
	private const float 	BLOCK_SIZE = 1.0f; 	// scale of the blocks (WARNING, DO NOT MESS WITH THIS SHIT!!!)
	private const float 	Z_POS = 0;			// Z position of the blocks

	// GAME CONFIGURATION
	private const int 		MATCH_COUNT = 3;	// number that defines a match
	private const bool 		BLOCK_RENEW = true;	// define if the blocks will be renewed when destroyed
	//private bool Interpolate = false;

	// PHYSICS
	private float sleep_timer;
	private const float TMAX = 1.0f;

	// OTHER
	const int destroy = -1;

	// INITIALIZATION ----------------------------------------------------------------------------------------------------

	void Awake(){

		//GAME_AREA.transform.position = new Vector3 (0f, 0f, 1f);

		if (GAME_AREA != null) {
			WIDTH =  (int)(GAME_AREA.transform.localScale.x  / BLOCK_SIZE);
			HEIGHT = (int)(GAME_AREA.transform.localScale.y  / BLOCK_SIZE);
				}

		GAME_AREA = GameObject.FindGameObjectWithTag ("Game_Area");

		STARTING_POSITION = GameObject.FindGameObjectWithTag ("Game_Start").transform.position;
		STARTING_POSITION.z = Z_POS;

		REQUEST_AREA = GameObject.FindGameObjectWithTag ("Request_Area");

		AREA_STARTING_POSITION = GameObject.FindGameObjectWithTag ("Request_Start").transform.position;
		AREA_STARTING_POSITION.z = Z_POS;

		sleep_timer = TMAX;
	
	}

	// Use this for initialization
	void Start () {

		InitRules ();

		InitMap ();
		InitBlocks ();
		InitRequests ();

	}

	private void InitMap(){

		Reader.IniciaLevel (ref_level,level_txt,HEIGHT,WIDTH);
		//ef_map.IniciaMapa (level_txt,HEIGHT,WIDTH);

	}

	private void InitBlocks(){

		foreach (BlockSettings block in ref_level.gridDeIngredientes){
			CreateBlock(GridToWorld(block.GridPosition), block.id);
		}
	}

	private void InitRules(){

		// RULE 0 (empty)

		ImplicationExpressionComposite implication0 = new ImplicationExpressionComposite ();
		Rule empty = new Rule ("No Rule",implication0, 0);
		rules.Add (empty);

		// RULE 1

		Number number1 = new Number ();
		number1.value = 1;
		
		Number number2 = new Number ();
		number2.value = 2;
		
		Number number3 = new Number ();
		number3.value = 3;
		
		Number number4 = new Number ();
		number4.value = 4;
		
		AndExpressionComposite and1 = new AndExpressionComposite ();
		and1.left = number1;
		and1.right = number2;
		
		AndExpressionComposite and2 = new AndExpressionComposite ();
		and2.left = and1;
		and2.right = number3;
		
		ImplicationExpressionComposite implication1 = new ImplicationExpressionComposite ();
		implication1.left = and2;
		implication1.right = number4;

		Rule rule1 = new Rule ("B+G+Y -> R",implication1, 3);
		rules.Add (rule1);

		// RULE 1

		Number number5 = new Number ();
		number5.value = 5;

		Number number6 = new Number ();
		number6.value = 6;
		
		AndExpressionComposite and3 = new AndExpressionComposite ();
		and3.left = number4;
		and3.right = number5;
					
		ImplicationExpressionComposite implication2 = new ImplicationExpressionComposite ();
		implication2.left = and3;
		implication2.right = number6;
		
		Rule rule2 = new Rule ("R+LB -> M",implication2, 2);
		rules.Add (rule2);

		current_rule = rule1;//rules[rule_pointer];
//

	}

	private void InitRequests()
	{
		float offset = BLOCK_SIZE * 0.5f;

		Vector2 position = AREA_STARTING_POSITION;
		position.x += offset;

		foreach (int id in ref_level.listaDePedidos) {
			CreateRequest(position,id);
			position.x += BLOCK_SIZE;
		}
	}

	private GameObject CreateRequest(Vector3 position, int id)
	{
		GameObject n_request = (GameObject)GameObject.Instantiate (request_ref, new Vector3 (position.x,position.y,Z_POS), Quaternion.identity);
		
		n_request.transform.localScale = new Vector3(BLOCK_SIZE,BLOCK_SIZE,0.3f);
		//n_block.SetActive (false);
		n_request.GetComponent<Request> ().ChangeId (id);
		requests.Add (n_request);
		
		return n_request;
	}


	private GameObject CreateBlock(Vector3 position, int id){

		GameObject n_block = (GameObject)GameObject.Instantiate (block_ref, new Vector3 (position.x,position.y,Z_POS), Quaternion.identity);

		n_block.transform.localScale = new Vector3(BLOCK_SIZE,BLOCK_SIZE,0.3f);
		//n_block.SetActive (false);
		n_block.GetComponent<Block> ().ChangeId (id);
		blocks.Add (n_block);

		return n_block;
	
	}

	private void RenewBlocks()
	{
		if (new_blocks.Count > 0)
		{
			foreach (BlockSettings block in new_blocks) {
			
				Vector3 block_world_position = GridToWorld(new Vector2 (block.GridPosition.x, block.GridPosition.y + HEIGHT));
				//new_blocks[i] = new Vector3 (new_blocks[i].position.x, new_blocks[i].position.y + HEIGHT, new_blocks[i].z);
				CreateBlock(block_world_position, IdStrategy(block.id));
			}

			new_blocks.Clear();

		}
	}

	private Vector2 WorldToGrid(Vector3 world){

		Vector2 grid = new Vector3 (Mathf.Floor(world.x / BLOCK_SIZE - (BLOCK_SIZE * 0.5f) - STARTING_POSITION.x),
		                            Mathf.Floor(world.y / BLOCK_SIZE - (BLOCK_SIZE * 0.5f) - STARTING_POSITION.y));
		return grid;
	}

	private Vector3 GridToWorld(Vector2 grid){
		
		Vector3 world = new Vector3 (grid.x * BLOCK_SIZE + (BLOCK_SIZE * 0.5f) + STARTING_POSITION.x,
		                             grid.y * BLOCK_SIZE + (BLOCK_SIZE * 0.5f) + STARTING_POSITION.y,
		                            Z_POS);
		return world;
	}

	int IdStrategy(int id){

		if (ref_level == null) {
			Debug.Log ("level is null");
			return 0;
		}
		
		int total = ref_level.tiposDeIngredientes.Count;
		return ref_level.tiposDeIngredientes[Random.Range (0, total)];
	}

	// UPDATE --------------------------------------------------------------------------------------------------------

	// Update is called once per frame
	void Update () {

		RenewBlocks();

		if (STATE == GameState.LOGIC) {
			LogicState();
			
		}
		else if (STATE == GameState.PLAYER){
			PlayerState();
		}

		SeekAndDestroy ();
		DestroyBlocks ();
		//DestroyMatches ();
		
	}

	void FixedUpdate(){
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
					n_block.GridPosition = WorldToGrid(blocks_to_destroy[i].transform.position);

					//= new BlockSettings(to_destroy[i].transform.position.x, to_destroy[i].transform.position.y, to_destroy[i].transform.position.z);
					new_blocks.Add(n_block);
					//CreateBlock(blocks[i].transform.position.x, STARTING_POSITION.y + y_pos * BLOCK_SIZE, true)	
				}
	
				GameObject.Destroy(blocks_to_destroy[i]);
				blocks.Remove(blocks_to_destroy[i]);
			}

			blocks_to_destroy.Clear();
		}
	}



	// LOGIC STATE ----------------------------------------------------------------------------------------------------

	private void LogicState(){
		//bool match = Matches ();
		Debug.Log ("ENTER LOGIC");

		bool match = ScanMatches();


		Debug.Log ("Matches = " + match);

		if (match) ChangeState 	(GameState.PHYSIC);	// if there is a match, go to PHYSIC state
		else ChangeState 		(GameState.PLAYER);	// if there is NOT a match, go to PLAYER state
	}



	private bool ScanMatches(){



		bool match = false;

		foreach (GameObject game_block in blocks)
		{
			MatchFinder(new List<GameObject>(), 0, game_block,ref match);
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

				GameObject colliding_block = Intersect(new string[1] {"Block"},pos_aux);

				if (new_list.Count < current_rule.Size)
				{
					if ((colliding_block != null) && (!matches.Contains(colliding_block)))
					{
						//BlockSettings n_block = new BlockSettings();
						//n_block.GridPosition = WorldToGrid(colliding_block.transform.position);

						MatchFinder(new_list, direcao, colliding_block,ref match);

					}

				}
				else if (new_list.Count >= current_rule.Size)
				{
					//GameObject derp = block_ref;
					//derp.GetComponent<Block>().ChangeId(3);
					//new_list.Add(derp);

					Context cont = new Context();
					List<GameObject> context_list = new List<GameObject>();

					foreach (GameObject block_in_context in new_list)
					{
						context_list.Add (block_in_context);
					}
					//context_list = new_list;

					cont.block_settings_array = context_list;

					if (current_rule.Expression.run(cont) != null)
					{
						//Debug.Log ("new_list = " + new_list.Count);
						//return new_list;

						for (int k = 0; k < new_list.Count; k++)
						{
							//if (k == 0){
							if (k == new_list.Count/2){ 
								//Number result = (Number)CURRENT_EXPRESSION.right;
								int result = current_rule.IDResult;

								new_list[k].GetComponent<Block>().ChangeId(result);
							}
							else{
								new_list[k].GetComponent<Block>().ChangeId(destroy);
								//to_destroy.Add(new_list[k]);
							}

							match = true;
						}
					}

				}
				
			}
		}

	}


	private void SeekAndDestroy()
	{
		// seek and destroy blocks
		foreach (GameObject block_game_object in blocks) {

			Block block_comp = block_game_object.GetComponent<Block> ();
			if (block_comp.Color_Index == destroy)
				blocks_to_destroy.Add (block_game_object);
		}

		// seek and destroy requests
		foreach (GameObject request in requests) {
			
			Food.Request request_comp = request.GetComponent<Request> ();
			if (request_comp.Color_Index == destroy);
				requests_to_destroy.Add (request);
		}	

			    
	}



	GameObject Intersect(string[] tags, Vector3 position){

		Collider[] colliding_array = Physics.OverlapSphere(position, BLOCK_SIZE/100);
		if (colliding_array.Length >= 1)
		{
			for (int c = 0; c < colliding_array.Length; c++){
				
				if (colliding_array[c] != null)
					foreach (string tag in tags)
						if (colliding_array[c].tag == tag)
							return colliding_array[c].gameObject;
					
			}
		}

		return null;

	}


	// PHYSICS STATE ------------------------------------------------------------------------------

	void PhysicState()
	{
		// TURN ON THE PHYSICS
		if (first_cycle)
		{					
			Debug.Log ("ENTER PHYSIC");
			//SetBlockGravity(true);
			SetBlockKInematic(false);
			WakeBlocks();
			//SetBlockRigidBody(true);
			first_cycle = false;
			//Debug.Log("NEW TURN ---------------------------------------------------------");

			//AddForceBlocks(new Vector3(0f,5.0f,0f));


		}
		if (sleep_timer <= 0f) {

			// CHECK IF SLEEPING
			bool sleeping = true;
			foreach (GameObject block in blocks) {
			if (!block.rigidbody.IsSleeping ())
					sleeping = false;
			}

			// TURN OFF THE PHYSICS
			if (sleeping) {
					//SetBlockGravity(false);
					//SetSleepBlocks(true);
				SetBlockKInematic (true);
					//SetBlockRigidBody(false);
				RoundPositions ();
				ChangeState (GameState.LOGIC);
				sleep_timer = TMAX;
			}

		}

		else sleep_timer -= Time.fixedDeltaTime;
	}

	void AddForceBlocks(Vector3 force)
	{
		foreach (GameObject block in blocks)
						block.rigidbody.AddForce (force);
	}

	void SetSleepBlocks(bool sleep)
	{
		foreach (GameObject block in blocks)
		{
			if (sleep)block.rigidbody.Sleep(); 
			if (!sleep)block.rigidbody.WakeUp(); 
		}
	}

	void WakeBlocks()
	{
		foreach (GameObject block in blocks)
		{
			block.rigidbody.WakeUp(); 
			block.rigidbody.AddForce(Vector3.zero);
		}
	}
	void SetBlockGravity(bool gravity)
	{
		foreach (GameObject block in blocks)
		{

				block.rigidbody.useGravity = gravity;

				if (!gravity)
					block.rigidbody.velocity = new Vector3(0,0,0);
		}
	}

	void SetBlockKInematic(bool kinematic)
	{
		foreach (GameObject block in blocks)
		{
			
			block.rigidbody.isKinematic = kinematic;
			
		}
	}

	void SetBlockRigidBody(bool rigid_body)
	{
		foreach (GameObject block in blocks)
		{
			
			block.rigidbody.detectCollisions = rigid_body;

		}
	}

	void RoundPositions()
	{
		foreach (GameObject block in blocks)
		{
			//block.GetComponent<Block> ().X = Mathf.Floor(block.transform.position.x)+ (BLOCK_SIZE * 0.5f);
			//block.GetComponent<Block> ().Y = Mathf.Floor(block.transform.position.y)+ (BLOCK_SIZE * 0.5f);
			//int aux = (int)block.transform.position.y;
			block.transform.position = new Vector3(Mathf.Floor(block.transform.position.x) + (BLOCK_SIZE * 0.5f), 
			                                       Mathf.Floor(block.transform.position.y) + (BLOCK_SIZE * 0.5f),
			                                       Z_POS);
		}
	}

	// PLAYER STATE ------------------------------------------------------------------------------

	void PlayerState(){
		
		if (first_cycle)
		{
			Debug.Log ("ENTER PLAYER");

			selected_objects[first] = null;
			selected_objects[second] = null;
			first_cycle = false;
			
		}

		int action_executed = 0;

		if (Input.GetMouseButtonDown(0))
		{
			action_executed = PlayerAction();
		}

		if (action_executed == 1) // GRID ACTION
			ChangeState(GameState.LOGIC);
		if (action_executed == 2) // REQUEST
			ChangeState(GameState.PHYSIC);

	}

	Vector3 WorldMousePosition()
	{
		Vector3 mouse_position = camera.ScreenToWorldPoint(Input.mousePosition);
		mouse_position.z = Z_POS;
		return mouse_position;
	}

	int PlayerAction()
	{
		GameObject selected = Intersect(new string[2] {"Block","Request"}, WorldMousePosition());

		if (selected == null)
			return 0;
		
		if (selected_objects[first] == null){
			Select (first,selected);
		}
		
		else
		{
			Select (second,selected);
		}

		if (selected_objects[first] == selected_objects[second])
		{
			UnSelectAll();
		}

		///


		if ((selected_objects[first] != null) && (selected_objects[second] != null)){

			int selection_type = SelectionType (selected_objects);

			if (selection_type == 1) // BLOCK/BLOCK selection
			{
				SwitchAction();
				UnSelectAll();
				return 1; 
			}

			if (selection_type == 2) // BLOCK/REQUEST selection
			{
				RequestAction();
				UnSelectAll ();
				return 2;
			}
			if (selection_type == 0) // INVALID selection
			{
				UnSelectAll ();
			}


		}

		return 0;
	}

	void SwitchAction(){
		if (isAdjacent(selected_objects[first],selected_objects[second]))
		{
			Switch(selected_objects[first],selected_objects[second]);

		}
	}

	void RequestAction(){

		Debug.Log ("request Action");

		Block block 	= selected_objects [first].GetComponent<Block> ();
		Request request = selected_objects [second].GetComponent<Request> ();

		if ((block != null) && (request != null))
		if (block.Color_Index == request.Color_Index)
		{
			block.ChangeId(destroy);
			request.ChangeId(destroy);
		}
						
	}


	int SelectionType(GameObject[] objects)
	{
		if (objects[first].tag == "Block")

			if (objects[second].tag == "Block")
						return 1;	// BLOCK AND BLOCK
			else if (objects[second].tag == "Request")
						return 2;	// BLOCK AND REQUEST
		return 0;					// INVALID						
	}

	void Select(int index, GameObject selected) 
	{
		index = Mathf.Clamp (index, first, second);

		if (selected.tag == "Block") {
			Block block = selected.GetComponent<Block> ();
			if (block != null) {
				block.SwitchSelector (true);
				selected_objects [index] = selected;

				Debug.Log("Selected a " + selected);
				
			}
		} else if (selected.tag == "Request") {
			{

				//if (selected.GetComponent<Request> () != null) {
					selected_objects [index] = selected;

					Debug.Log("Selected a " + selected); 

				//}
			}

		

		}
	}

	void UnSelect(int index)
	{
		index = Mathf.Clamp (index, 0, 1);

		if (selected_objects [index].tag == "Block")
			selected_objects [index].GetComponent<Block> ().SwitchSelector (false);
		selected_objects [index] = null;

	}

	void UnSelectAll()
	{
		UnSelect (first);
		UnSelect (second);
	}

	bool isAdjacent(GameObject block1, GameObject block2){
		
		Vector3 pos1 = block1.transform.position;
		Vector3 pos2 = block2.transform.position;

		//if ((pos1.x + BLOCK_SIZE *1.5 <= pos2.x) && (Vector3.Distance(pos1,pos2) <= BLOCK_SIZE*1.5))	return true;
		//if ((pos1.x - BLOCK_SIZE *1.5 >= pos2.x) && (Vector3.Distance(pos1,pos2) <= BLOCK_SIZE*1.5))	return true;
		
		//if ((Vector3.Distance(pos1,pos2) <= BLOCK_SIZE*1.5) && (pos1.y + BLOCK_SIZE*1.5 <= pos2.y))	return true;
		//if ((Vector3.Distance(pos1,pos2) <= BLOCK_SIZE*1.5) && (pos1.y - BLOCK_SIZE*1.5 == pos2.y))	return true;

		if (Vector3.Distance (pos1, pos2) <= BLOCK_SIZE*1.01f)
						return true;
		Debug.Log (Vector3.Distance (pos1, pos2));
		
		return false;
	}

	void Switch(GameObject first, GameObject second){

		first.collider.enabled = false;
		second.collider.enabled = false;

		Vector3 aux = first.transform.position;
		first.transform.position = second.transform.position;
		second.transform.position = aux;

		first.collider.enabled = true;
		second.collider.enabled = true;
	}

	// GUI FUNCIONS /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnGUI () {

		int GUI_WIDTH = 120;
		int GUI_HEIGHT = 200;

		int border = 10;

		int GUI_X = Screen.width - GUI_WIDTH - border;
		int GUI_Y = 10;

		// Make a background box
		GUI.Box(new Rect(GUI_X,GUI_Y,GUI_WIDTH,GUI_HEIGHT), "Rule: " + current_rule.Name);

		GUI_Y += 40;

		int offset = 10;
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(GUI_X + offset/2 ,GUI_Y,GUI_WIDTH-offset,20), rules[0].Name)) {
			SwitchRule(0);
			ChangeState (GameState.LOGIC);
		}

		GUI_Y += 30;
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(GUI_X + offset/2 ,GUI_Y,GUI_WIDTH-offset,20), rules[1].Name)) {
			SwitchRule(1);
			ChangeState (GameState.LOGIC);
		}

		GUI_Y += 30;
		
		// Make the second button.
		if(GUI.Button(new Rect(GUI_X+ offset/2,GUI_Y,GUI_WIDTH-offset,20), rules[2].Name)) {
			SwitchRule(2);
			//ChangeState (GameState.LOGIC);
		}

		current_rule = rules[rule_pointer];
	}


	void SwitchRule(int rule){
		if (STATE == GameState.PLAYER) {
						rule_pointer = rule;
						ChangeState (GameState.LOGIC);
				}
	}

}
