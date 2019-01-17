using UnityEngine;
using System.Collections;
using Food;

public class SoundControler : MonoBehaviour {
	
	AudioSource current_BGS;

	public float Volume;
	public AudioClip[] BGS_list;
	public AudioClip[] SFX_list;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(gameObject);
		current_BGS = gameObject.GetComponent<AudioSource>();
		current_BGS.clip = null;
		PlayBGS ("Main_BGS");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(current_BGS.clip != null)
			if (Support.SoundEnable == current_BGS.mute) 
				{
					current_BGS.mute = !Support.SoundEnable;
				}
	}

	public void PlayBGS(string sound)
	{	
		
		AudioClip new_BGS = null;
		switch (sound)
		{
			default :
				new_BGS = null;
				break;
			case "Main_BGS":
				new_BGS = BGS_list[0];
				break;
			case "Credits_BGS":
				new_BGS = BGS_list[1];
				break;
			case "Game_BGS":
				new_BGS = BGS_list[1];
				break;	
		}

		if (current_BGS.clip != new_BGS) 
			{
		if (current_BGS != null) current_BGS.Stop ();
		current_BGS.clip = new_BGS;
		if(current_BGS != null){
			 current_BGS.Play ();
			 current_BGS.volume = Volume;
			}
		}
	}

	public void PlaySFX(string sound)
	{
				if (Support.SoundEnable) {
						//Create an empty game object
						GameObject go = new GameObject ();
						go.transform.position = new Vector3 (0f, 0f, 0f);
						//Create the source
						AudioSource source = go.AddComponent<AudioSource> ();
						AudioClip aux = null;
						switch (sound) {
						case "Button_SFX":
								aux = SFX_list [0];
								break;
						case "Slide_SFX":
								aux = SFX_list [1];
								break;
						case "Select_SFX":
								aux = SFX_list [2];
								break;
						case "Create_SFX":
								aux = SFX_list [3];
								break;
						case "Mach_SFX":
								aux = SFX_list [4];
								break;
						case "Burn_SFX":
								aux = SFX_list [5];
								break;
						case "Done_SFX":
								aux = SFX_list [6];
								break;
						case "Win_SFX":
								aux = SFX_list [7];
								break;
						case "Lose_SFX":
								aux = SFX_list [8];
								break;
						case "Rumbber_SFX":
								aux = SFX_list [9];
								break;
						}
						go.name = "Game_Audio : " + aux.name;
						source.clip = aux;
						source.volume = Volume*0.8f;
						source.pitch = 1;
						source.Play ();
						Destroy (go, aux.length);
				}
		}
}