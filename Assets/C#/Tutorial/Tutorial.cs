using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

[System.Serializable]
public class TutorialCard
{ 
	public Sprite Image;
}

public class Tutorial : MonoBehaviour {

	public enum slider
	{
		min = -1,
		med = 0,
		max = 1
	}

	private TutorialCard current;
	private int count;
	private bool triger;
	private float timer;
	private SpriteRenderer ThisSpriteRenderer;

	public GameObject Button;
	public slider Vertical;
	public slider Horizontal;

	public List<TutorialCard> Cards;

	// Use this for initialization
	void Start () 
	{
		timer = 0;
		ThisSpriteRenderer = this.GetComponent<SpriteRenderer>();

		Button.AddComponent<BoxCollider2D>();

		current = null;
		triger = true;
		count = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(triger && count >= Cards.Count)
			TutorialOut();
		if(triger && count < Cards.Count && timer <= 0)
		{
			timer = 0.5f;
			current = Cards[count];
			ThisSpriteRenderer.sprite = current.Image;
			count ++;

			SetButton();
		}
		else //Tutorial is ended
		{
			timer -= Time.deltaTime; 
		}
		triger = false;
	}

	void SetButton()
	{
		float x,y;
		x = current.Image.bounds.size.x/2f * (float)Horizontal;
		y = current.Image.bounds.size.y/2f * (float)Vertical;
		Button.transform.position = this.transform.position;
		Button.transform.Translate(new Vector3(x,y,0));
	}

	void TutorialOut()
	{
		GameObject.Destroy(gameObject);
	}

	public void CallTirger()
	{
		triger = true;
	}
}
