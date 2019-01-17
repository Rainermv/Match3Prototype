using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

//namespace Food{
public enum Action
{
	Evento, // Passa com um evento predefinido
	Timer, // Passa com o tempo
	GenericTouch,// Passa se algum toque for dado na tela
}
public enum Response
{
	Next,// Jump to next
	Exit, // Clear window
	End // Finsh the level
}

[System.Serializable]
public class TutorialTriger
{
	public Action action;
	public Response responses;
	public int ID;
	public string data;
	public bool generic;
}

public class TutorialManager : MonoBehaviour 
{
	public static TutorialManager GetInstance()
	{
		GameObject instance_obj = GameObject.FindGameObjectWithTag ("TutorialManager");
		if (instance_obj != null)
			return instance_obj.GetComponent<TutorialManager> ();	
		else
			return null;
	}

	public static void CheckTrigers(Action _action,string data,int value)
	{
		GameObject instance_obj = GameObject.FindGameObjectWithTag ("TutorialManager");
		if (instance_obj != null) instance_obj.GetComponent<TutorialManager> ().Check_Triger(_action,data,value);
		//TutorialManager.GetInstance().GetComponent<TutorialManager>().Check_Triger(string data, int value);
	}

	public List<GameObject> Layers;
	//public List<TutorialTriger> Trigers;
	public int count;
	//int trigercount;
	bool flag;
	float timercount;

	// Use this for initialization
	void Start () 
	{
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("StopTime",true);
		this.gameObject.tag = "TutorialManager";
		Layers = new List<GameObject> ();
		count = -1;
		flag = false;
		timercount = 0;
		//trigercount = 0;
		TutorialLayer[] chieldList = GetComponentsInChildren<TutorialLayer>();
		foreach (TutorialLayer chield in chieldList) 
		{
			Layers.Add(chield.gameObject);
		}
		foreach (GameObject element in Layers) 
		{
			element.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(count+1 < Layers.Count)
			if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.action == Action.Timer)
			{
				timercount += Time.deltaTime;
				if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.ID <= timercount)
				{
					Check_Triger (Action.Timer,null, Layers[count+1].GetComponent<TutorialLayer>().Trigers.ID);
					timercount = 0;
				}
			}
			else if (Input.GetMouseButtonUp (0))
				if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.action == Action.GenericTouch)
					{
						Check_Triger (Action.GenericTouch,null,0);
					}
	}

	public void Next()
	{
		if (count < Layers.Count-1) 
		{
			if (flag) 
				Layers [count].SetActive (false);
			count ++;
			Layers [count].SetActive (true);
			flag = true;
		}
	}
	public void Exit()
	{
		if (count < Layers.Count) 
		{
			if (flag) 
			{
				Layers [count].SetActive (false);
				flag = false;
			}
		}
	}
	public void End()
	{
		Exit ();
		GameObject.FindGameObjectWithTag ("MainCamera").SendMessage ("EndLevel",true);
	}
	
//	public void CheckTriger(Action _action, int _ID)
//	{
//		if (count < Layers.Count) 
//		{
//				if ((Layers[count+1].GetComponent<TutorialLayer>().Trigers.action == _action) && (Layers[count+1].GetComponent<TutorialLayer>().Trigers.ID == _ID || Layers[count+1].GetComponent<TutorialLayer>().Trigers.generic == true))
//			{
//					if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.Next)
//					Next ();
//				else
//					Exit ();
//			}
//		}
//	}
	public void Check_Triger(Action _action, string _Data, int ID)
	{
		if (count < Layers.Count-1) 
		{
			if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.action == _action)
			    {
					if(_action == Action.GenericTouch || _action == Action.Timer)
					{
					if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.Next)
						Next ();
					else if(Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.Exit)
						Exit ();
					else if(Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.End)
						End ();
					}
					else if((Layers[count+1].GetComponent<TutorialLayer>().Trigers.data == _Data) && ((Layers[count+1].GetComponent<TutorialLayer>().Trigers.ID == ID) || (Layers[count+1].GetComponent<TutorialLayer>().Trigers.generic == true)))
					{
						if (Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.Next)
							Next ();
						else if(Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.Exit)
							Exit ();
						else if(Layers[count+1].GetComponent<TutorialLayer>().Trigers.responses == Response.End)
							End ();
					}
				}
		}
	}

	// Acrion list "All" "" ""
	public void LayerRun(string _action)
	{
		Layers [count].GetComponent<TutorialLayer> ().Run (_action);
	}
}
//}