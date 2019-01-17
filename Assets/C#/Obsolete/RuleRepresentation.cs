using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food{
	public class RuleRepresentation : MonoBehaviour {
	
		private List<GameObject> components = new List<GameObject>();
	
		public bool show_background = true;
	
		private Renderer background_render;
	
		public List<GameObject> Components{
			get{ return components;}
		}
	
		public void AddComponent(GameObject new_comp){
			//components.Add(new_comp);
		}
	
		public void SetComponent(GameObject new_comp, int index){
			components[index] = new_comp;
		}
	
		void Awake () {
			background_render = GetComponent<Renderer> ();
		}
	
		// Use this for initialization
		void Start () {
	
			for (int i = 0; i < transform.childCount; i++) {
				AddComponent(transform.GetChild(i).gameObject);
			}
	
			background_render.enabled = show_background;
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	
		void OnMouseDown(){
		}
	}
}
