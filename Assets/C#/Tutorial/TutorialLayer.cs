using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

public class TutorialLayer : TutorialComponent
{
	public List<GameObject> Components;
	public bool TimerRuning;
	public bool InteractionRuning;
	public TutorialTriger Trigers;
	// Use this for initialization
	
	private GameObject main_ref;
	
	void Awake(){
	
	main_ref = GameObject.FindGameObjectWithTag ("MainCamera");
	
	}
	
	void Start () 
	{
		Components = new List<GameObject> ();
		TutorialComponent[] chieldList = GetComponentsInChildren<TutorialComponent>();

		foreach (TutorialComponent chield in chieldList) 
		{
			if(chield.gameObject != this.gameObject)
			Components.Add(chield.gameObject);
		}
		foreach (GameObject element in Components) 
		{
			element.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnEnable()
	{	
		if(!TimerRuning && main_ref != null)
			main_ref.SendMessage ("StopTime",true);
		if(!InteractionRuning && main_ref != null)
			main_ref.SendMessage ("StopInteraction",true);
	}

	void OnDisable()
	{
		if (main_ref != null)
			main_ref.SendMessage ("StopTime",false);
		if (main_ref != null)
			main_ref.SendMessage ("StopInteraction",false);
	}

	public void Efect(string efect)
	{

	}

	public void Run(string action)
	{
		switch (action) 
		{
		default:
			break;
		case"All":
				foreach(GameObject chield in Components)
					chield.SetActive(true);
			break;
		case"":
				break;
		}
	}
}
