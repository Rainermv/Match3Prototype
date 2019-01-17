using UnityEngine;
using System.Collections;

namespace Food{
	public class OutGameButtonMovable : OutGameButton {
	
		public Vector2 move_target;
		public Vector2 starting_position;
		public float move_speed = 1.0f;
		
		private float lerp = 0.0f;
		
		
		void Start(){
			InitMovable();
		}

		new void Update(){
			Move();
		}
		
		public void InitMovable(){			// call when instantiated
		
			move_target = transform.position;
			starting_position = transform.position;
			
		}
	
		public void SetTarget(Vector2 new_target){
		
			starting_position = move_target;
			move_target += new_target;
			lerp = 0f;
		
		}
		
		public void Move(){			// call on Update
		
			//if (Vector2.Distance(transform.position,move_target) > move_speed){
				
				//Vector2 move_vector = Vector2.
				transform.position = Vector2.Lerp(starting_position,move_target,lerp);
				lerp+= 0.05f;
				//Mathf.Clamp(lerp,0f,1f);
			//}
			//else lerp = 0f;
		
		}
		
	}
}
