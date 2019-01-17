using UnityEngine;
using System.Collections;

public class EndGameControler : MonoBehaviour {

	public GameObject next_level_button;
	public GameObject hi_score_obj;
	public Sprite win_spr;
	public Sprite lose_spr;
	public GameObject result;
	public GameObject[] stars;
	public Sprite throphy;
	public Sprite goldThrophy;
	
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public void Run(bool win, bool[] check, bool hi_score)
	{
		//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlayBGS("Stop");
		if(win)
		{	
			if (next_level_button != null) next_level_button.SetActive(true);
			//GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Win_SFX");
			result.GetComponent<SpriteRenderer>().sprite = win_spr;
			//Debug.Log("Play: Win_SFX");
			GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Win_SFX");
		}
		else
		{
			if (next_level_button != null) next_level_button.SetActive(false);
			result.GetComponent<SpriteRenderer>().sprite = lose_spr;
			//Debug.Log("Play: Lose_SFX");
			GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundControler>().PlaySFX("Lose_SFX");
		}
		
		result.SendMessage("Init",1);
		
//		// testing
//		for(int i=0; i<3 ; i++){
//			stars[i].GetComponent<SpriteRenderer>().sprite = throphy;
//			stars[i].SetActive(true);
//			stars[i].SendMessage("Init",1f+(((float)i+1f)/2f));
//		}
//		
		for(int i=0; i<3 ; i++)
			if(win && check[i])
			{
				if(check[(i+3)])
					stars[i].GetComponent<SpriteRenderer>().sprite = goldThrophy;
				else stars[i].GetComponent<SpriteRenderer>().sprite = throphy;
	
				//stars[i].SetActive(true);// Animation()
				stars[i].SendMessage("Init",1f+(((float)i+1f)/2f));
				//Debug.Log("Play: Rumbber_SFX");
				
			}
		if (hi_score){
			
			hi_score_obj.SendMessage ("Init",1.2);
		}
	}
}
