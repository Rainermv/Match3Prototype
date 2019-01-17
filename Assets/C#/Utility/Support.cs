using UnityEngine;
using System.Collections;

namespace Food{
	public static class Support {

		public static bool interactionEnable = true;
		public static bool SoundEnable = true;

		public static Vector2 WorldToGrid(Vector3 world_position, float BLOCK_SIZE, Vector3 STARTING_POSITION){
			
			Vector2 grid = new Vector3 (Mathf.Floor(world_position.x / BLOCK_SIZE - (BLOCK_SIZE * 0.5f) - STARTING_POSITION.x),
			                            Mathf.Floor(world_position.y / BLOCK_SIZE - (BLOCK_SIZE * 0.5f) - STARTING_POSITION.y));
			return grid;
		}
		
		public static Vector3 GridToWorld(Vector2 grid_position, float BLOCK_SIZE, Vector3 STARTING_POSITION){
			
			Vector3 world = new Vector3 (grid_position.x * BLOCK_SIZE + (BLOCK_SIZE * 0.5f) + STARTING_POSITION.x,
			                             grid_position.y * BLOCK_SIZE + (BLOCK_SIZE * 0.5f) + STARTING_POSITION.y,
			                             STARTING_POSITION.z);
			return world;
		}

		public static GameObject Intersect(string[] tags, Vector3 position)
		{
			if (interactionEnable) 
			{
								Collider[] colliding_array = Physics.OverlapSphere (position, 0.01f);
								if (colliding_array.Length >= 1) {
										for (int c = 0; c < colliding_array.Length; c++) {
					
												if (colliding_array [c] != null)
														foreach (string tag in tags)
																if (colliding_array [c].tag == tag)
																		return colliding_array [c].gameObject;
										}
								}
						}
			return null;
			
		}

		public static void Switch(GameObject first, GameObject second){
			
			first.collider.enabled = false;
			second.collider.enabled = false;

			Vector3 aux = first.transform.position;
			first.transform.position = second.transform.position;
			second.transform.position = aux;
			
			first.collider.enabled = true;
			second.collider.enabled = true;
		}
		
		public static void SmoothSwitch(GameObject first, GameObject second){
		
			first.SendMessage("MoveTo",second.transform.position);
			second.SendMessage("MoveTo",first.transform.position);
		}

		public static bool isAdjacent(GameObject block1, GameObject block2, float BLOCK_SIZE){
			
			Vector3 pos1 = block1.transform.position;
			Vector3 pos2 = block2.transform.position;
			
			if (Vector3.Distance (pos1, pos2) <= BLOCK_SIZE*1.01f)
				return true;
			Debug.Log (Vector3.Distance (pos1, pos2));
			
			return false;
		}

	}
}
