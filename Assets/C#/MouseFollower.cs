using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour {

	public float speed = 0.1F;
	
	private Camera cam;
	private SpriteRenderer spr_renderer = null;
	
	private string state = "invisible";
	
	private Vector2 initial_position;
	private Vector2 final_position;
	
	float lerp = 0.0f;
	
	void Awake(){
		
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		spr_renderer = GetComponent<SpriteRenderer>();
	}
	
	// Use this for initialization
	void Start () {
		Disappear ();
	
		
	}
	
	public void Appear(Sprite sprite){
		state = "following";
		
		Vector2 mouse_pos =  cam.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3 (mouse_pos.x,mouse_pos.y,0);
		
		spr_renderer.enabled = true;
		spr_renderer.sprite = sprite;
	}

	public void Disappear(){
		state = "invisible";
		
		spr_renderer.sprite = null;
		spr_renderer.enabled = false;
	}
	
	void MoveTo(Vector2 position)
	{
		state = "returning";
		
		initial_position = transform.position;
		final_position = position;
		lerp = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
//			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
//			transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
//		}
		
		if (state == "following"){
			Vector2 mouse_pos =  cam.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3 (mouse_pos.x,mouse_pos.y,0);
		}
		else if (state == "returning"){
			transform.position = Vector2.Lerp(initial_position, final_position, lerp);
			lerp += 0.0001f;
			
			if (lerp >= 1.0f) Disappear ();
		
		}
		
		
	}
}
