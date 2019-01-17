using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Teste: MonoBehaviour
	{

		public TextAsset rule_txt;
		public GameObject block_ref;
		List <Rule> Rules = new List<Rule> ();

		public int current_rule = 0;
		public int[] valor;

		public void Start()
		{
			Reader.LoadRules(Rules,rule_txt);
//			Debug.Log ("Rules.Count: " + Rules.Count);
//			for (int i =0; i< Rules.Count; i++) 
//				{
//					Debug.Log(Rules[i].Name + " : " + Rules[i].Expression.ToString ());
//				}

			Context con = new Context ();
			List<GameObject> blocks = new List<GameObject> ();

			for (int i =0; i<Rules[current_rule].Size; i++) 
			{
				GameObject block_aux = new GameObject();
				block_aux.AddComponent<Block>();
				block_aux.GetComponent<Block>().Color_Index = valor[i];
				blocks.Add (block_aux);
			}

			con.block_settings_array = blocks;

			Debug.Log(Rules[current_rule].Name + " : " + Rules[current_rule].Expression.ToString ());
			for (int i =0; i<Rules[current_rule].Size; i++) 
			{
				Debug.Log("Context["+i+"]: "+blocks[i].GetComponent<Block>().Color_Index);
			}
			Number I =  (Number)Rules[current_rule].Expression.run (con);
			if(I != null)Debug.Log(I.ToString ());else Debug.Log("Falha");
		}
	}
}

