using UnityEngine;
using System.Collections;

namespace Food{

	public class Request : MonoBehaviour {

		private int color_index = 0;
		private int c_max = 6;

	//	private bool destroy =  false;

		public int Color_Index{
			get{return color_index;}
			set{ color_index = Mathf.Clamp(value,-2,c_max+1);}
		}

		void Awake()
		{
		
		}

		// Use this for initialization
		void Start () {

		}

		public void ChangeId(int id){
			Color_Index = id;
			ChangeColor ();
			}

		private void ChangeColor(){
			switch (color_index) {
			case -1:
				renderer.material.color = Color.black;	
				break;

			case 0:
				renderer.material.color = Color.gray;	
				break;

			case 1:
				renderer.material.color = Color.blue;
				break;

			case 2:
				renderer.material.color = Color.green;
				break;

			case 3:
				renderer.material.color = Color.yellow;
				break;

			case 4:
				renderer.material.color = Color.red;
				break;

			case 5:
				renderer.material.color = new Color(0.2f,0.4f,0.8f);
				break;

			case 6:
				renderer.material.color = Color.magenta;
				break;
			
			default:
				renderer.material.color = Color.magenta;			
				break;
			}


		}

		// Update is called once per frame
		void Update () {

				
		}
		void FixedUpdate(){
			//transform.position = new Vector3(x_pos,transform.position.y,transform.position.z);
	}

}
}
