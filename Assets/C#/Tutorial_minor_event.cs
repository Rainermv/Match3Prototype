using UnityEngine;
using System.Collections;

public class Tutorial_minor_event : MonoBehaviour {

	public int [] RequestsID;
	public enum minor_event
	{
		createRecipe,
		createRules
	}
	public minor_event type;

	// Use this for initialization
	void Start () 
	{
		if(type == minor_event.createRecipe)
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Main> ().AddRequests (RequestsID);
		else if(type == minor_event.createRules)
		{
			AndExpressionComposite andaux1 = new AndExpressionComposite();
			Number valueaux11 = new Number();
			valueaux11.value = 8;
			andaux1.left = valueaux11;
			Number valueaux12 = new Number();
			valueaux12.value = 9;
			andaux1.right = valueaux12;

			ImplicationExpressionComposite expaux1 = new ImplicationExpressionComposite();
			expaux1.left = andaux1;
			Number valueaux13 = new Number();
			valueaux13.value = 10;
			expaux1.right = valueaux13;

			Rule aux1 = new Rule("exemplo1",expaux1);
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Main> ().ForceRule(aux1);


			AndExpressionComposite andaux2 = new AndExpressionComposite();
			Number valueaux21 = new Number();
			valueaux21.value = 12;
			andaux2.left = valueaux21;
			Number valueaux22 = new Number();
			valueaux22.value = 14;
			andaux2.right = valueaux22;
			
			ImplicationExpressionComposite expaux2 = new ImplicationExpressionComposite();
			expaux2.left = andaux2;
			Number valueaux23 = new Number();
			valueaux23.value = 0;
			expaux2.right = valueaux23;
			
			Rule aux2 = new Rule("exemplo1",expaux2);
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Main> ().ForceRule(aux2);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{

	}
}
