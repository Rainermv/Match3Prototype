using UnityEngine;
using System.Collections;

public class EndLevelScreenComponent : MonoBehaviour {

	float timer;
	bool start = false;
	
	SpriteRenderer rend;
	
	// Use this for initialization
	void Awake () {
		rend = GetComponent<SpriteRenderer>();

	}
	
	void Start(){
		rend.enabled = false;
	}
	
	public void Init(float time){
		timer = time;
		start = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (start){
		
			timer -= Time.deltaTime;
			
			if (timer <= 0){
				rend.enabled = true;
				//Debug.Log ("Play Stamp SFX");
				GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Rumbber_SFX");
				
				start = false;
			}
		
		
		}
	
	}
}
