using UnityEngine;
using System.Collections;

public class TutorialMove : MonoBehaviour {

	public Vector3  Move;
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Move * Time.deltaTime);
	
	}
}
