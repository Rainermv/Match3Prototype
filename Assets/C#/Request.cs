using UnityEngine;
using System.Collections;

namespace Food{

	public class Request : MonoBehaviour {

		private int color_index = 0;
		private int c_max = 6;
		private SpriteSheetManager sprites;
		
		public Sprite check_true;
		public Sprite check_false;
				
		bool awake = false;
		private float speed = 0.1f;
		private Vector2 target;

		public int Color_Index{
			get{return color_index;}
			set{ color_index = value;}
		}

		void Awake()
		{
			sprites = SpriteSheetManager.GetInstance ();
			c_max = sprites.IngredientsSpriteSheet.Length +1;
		
		}

		// Use this for initialization
		void Start () {

		}

		public void ChangeId(int id){
			Color_Index = id;
			ChangeSprite ();
		}
		
		private void ChangeSprite()
		{	if (Color_Index >= 0){
					GetComponent<SpriteRenderer> ().sprite = sprites.GetIngredientSprite (Color_Index);
					return;
			}
			
			this.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
			
			if 		(color_index == -1) this.GetComponentsInChildren<SpriteRenderer>()[1].sprite = check_true;
			else if (color_index == -2) this.GetComponentsInChildren<SpriteRenderer>()[1].sprite = check_false;
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		
		void FixedUpdate(){
			if (awake) Move();
		}
		
		public void MoveTo(Vector2 move){
			target = move;
			awake = true;
		}
		
		public void MoveToLimbo(){
			Destroy(gameObject,3);
			color_index = -2;
			ChangeSprite();
			MoveTo(new Vector2(transform.position.x - 10,transform.position.y));
		}
		
		void Move(){
			if (Vector2.Distance(transform.position,target) > speed)
				transform.Translate (new Vector3(-speed,0,0));
			else{
				awake = false;
				transform.position = target	;		
			} 
		}
		

	}
}
