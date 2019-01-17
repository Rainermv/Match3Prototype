using UnityEngine;
using System.Collections;

public class TrashCan : MonoBehaviour {
	
	public Vector2 starting_position;
	public Vector2 final_position;
	
	float direction = 0;
	
	public float lerp = 0f;
	
	public void Show(){
		
		direction = 0.2f;
		lerp = 0;
		
		//Debug.Log ("SHOW TRASH");
		
//		showing = true;
//		hiding = false;
		
	}
	
	public void Hide(){
	
		direction = -0.2f;
		lerp = 1f;
		
		//Debug.Log ("HIDE TRASH");
	
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (direction != 0 && lerp <= 1f && lerp >= 0){
			transform.position = Vector2.Lerp (starting_position,final_position,lerp);
			
			lerp += direction;
			
			if (lerp >= 1f || lerp <= 0)	direction = 0;
		}
	
	}
	
	
}
