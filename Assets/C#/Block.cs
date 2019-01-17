using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	private int color_index = 0;
	private int c_max;
	private SpriteSheetManager sprites;

	private Renderer selector;
	
	private Vector3 starting_position;
	private Vector3 target_position;
	private bool moving = false;
	private float lerp = 0f;

	public int Color_Index{
		get{return color_index;}
		set{ color_index = value;}
	}

	void Awake()
	{
		//selector = transform.GetChild (0).GetComponent<Renderer> ();
		sprites = SpriteSheetManager.GetInstance ();
		//c_max = sprites.IngredientsSpriteSheet.Length +1;
		//SwitchSelector(false);
		Unselect ();
		
		starting_position = transform.position;
		target_position = transform.position;

	}

	// Use this for initialization
	void Start () {

	}
	
	void FixedUpdate(){
		Move ();
	}

	public void ChangeId(int id){
		Color_Index = id;
		//ChangeColor ();
		if (id >= 0)
			ChangeSprite ();
		else if (id == -1)
			ChangeColor(Color.gray);
	}

	private void ChangeSprite()
	{
		GetComponent<SpriteRenderer>().sprite = sprites.GetIngredientSprite (Color_Index);
		
	}
	
	private void ChangeColor(Color _color){
		renderer.material.color = _color;
	}
	
	public void MoveTo(Vector3 position){
	
		if (!moving){
			starting_position = transform.position;
			target_position = position;
			lerp = 0f;
			moving = true;
		}
	}
	
	public bool IsMoving(){
		
		return (moving);
	}
	
	public void Move(){
		if (moving){
			collider.enabled = false;
			transform.position = Vector3.Lerp(starting_position,target_position,lerp);
			lerp += 0.1f;
			Mathf.Clamp (lerp,0f,1f);
			
			if (lerp >= 1f) FinishMoving ();
			
		}
		
	}
	
	void FinishMoving(){
		transform.position = target_position;
		moving = false;
		collider.enabled = true;
		rigidbody.Sleep();
	}



	public void Select(){
		renderer.material.color = Color.gray;
	}

	public void Unselect(){
		renderer.material.color = Color.white;
	}

}
