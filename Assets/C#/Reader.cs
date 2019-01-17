using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Food{
	public class Reader 
	{
	
		public static void LoadRules(List<RuleString> Rules,TextAsset content)
		{
			using (StringReader reader = new StringReader(content.text))
			{
				//Debug.Log ("#Arquivo OK");
				//Leitura---------------------------------------
				string line;
				while ((line = reader.ReadLine()) != null) 
				{
					string[] parts = line.Split ('/');
					if(parts[0] == "#RULE" || parts[0] == "#Rule" || parts[0] == "#R" || parts[0] == "#r")
						{
							RuleString aux_rule = new RuleString();
							aux_rule.name = parts[1];
							string[] parts2 = parts[2].Split ('=');
							aux_rule.left = parts2[0];
							aux_rule.right = parts2[1];
							Rules.Add(aux_rule);
						}
				}
			}
		}
	
		public static Expression ImterpretaString(string line,List<Rule> Rules)
		{
			Expression aux_exp = null;
			string[] parts = line.Split (' ');
	
			if(parts[0] == "#RULE" || parts[0] == "#Rule" || parts[0] == "#R" || parts[0] == "#r")
			{
				for (int i=2; i<parts.Length; i++) 
				{
					if(parts[i] == "+")
					{
						AndExpressionComposite aux_and = new AndExpressionComposite();
						if(parts[i-1].Length == 1)
						{
							Number numb_aux = new Number(); 
							numb_aux.value = System.Convert.ToInt32(parts[i-1]);
							aux_and.left = numb_aux;
						}
						else
						{
							foreach(Rule rule in Rules)
							{
								if(parts[i-1] == rule.Name)
								{
									aux_and.left = rule.Expression;
									break;
								}
							}
						}
						if(parts[i+1].Length == 1) 
						{
							Number numb_aux = new Number(); 
							numb_aux.value = System.Convert.ToInt32(parts[i+1]);
							aux_and.right = numb_aux;
						}
						else
						{
							foreach(Rule rule in Rules)
							{
								if(parts[i+1] == rule.Name)
								{
									aux_and.right = rule.Expression;
									break;
								}
							}
						}
						aux_exp=aux_and;
					}
					
					if(parts[i] == "|")
					{
						OrExpressionComposite aux_or = new OrExpressionComposite();
						if(parts[i-1] != ")") 
						{
							Number numb_aux = new Number(); 
							numb_aux.value = System.Convert.ToInt32(parts[i-1]);
							aux_or.left = numb_aux;
						}
						if(parts[i+1] != "(") 
						{
							Number numb_aux = new Number(); 
							numb_aux.value = System.Convert.ToInt32(parts[i+1]);
							aux_or.right = numb_aux;
						}
						aux_exp=aux_or;
					}
					
					if(parts[i] == "=")
					{
						ImplicationExpressionComposite aux_imp = new ImplicationExpressionComposite();
						if(parts[i-1].Length == 1)
						{
							Number numb_aux = new Number(); 
							numb_aux.value = System.Convert.ToInt32(parts[i-1]);
							aux_imp.left = numb_aux;
						}
						else
						{
							foreach(Rule rule in Rules)
							{
								if(parts[i-1] == rule.Name)
								{
									aux_imp.left = rule.Expression;
									break;
								}
							}
						}
						if(parts[i+1].Length >= 1) 
						{
							Number numb_aux = new Number(); 
							numb_aux.value = System.Convert.ToInt32(parts[i+1]);
							aux_imp.right = numb_aux;
						}
						aux_exp=aux_imp;
					}
				}
			}
		return aux_exp;
	}
	
	
		public static void IniciaLevel (Level map,TextAsset content , int Altura , int Largura)
		{	
			// Read every line in the file.
			
			using (StringReader reader = new StringReader(content.text))
			{
				//Leitura---------------------------------------
				string line;
				int y = 0;
				while ((line = reader.ReadLine()) != null) 
				{
					// Do something with the line.
					string[] parts = line.Split (' ');
					for (int i=0; i<parts.Length; i++) 
					{
						if (parts[i] == "//"){
							break;
						}
						if((parts[i] == "#R") || (parts[i] == "#r"))
						{
							map.isRandom = true;
						}
						if((parts[i] == "score") || (parts[i] == "Score"))
						{
							int aux = System.Convert.ToInt32(parts[i+1]);
							map.targetScore = aux;
						}
						if((parts[i] == "tr_score1") || (parts[i] == "Tr_score1"))
						{
							if (map.targetScore >= 0){
								double aux = System.Convert.ToDouble(parts[i+1]);
								map.trophyScore1 = (int)aux * map.targetScore;
								}
							else {
								int aux = System.Convert.ToInt32(parts[i+1]);
								map.trophyScore1 = aux;
							}
						}
						if((parts[i] == "tr_score2") || (parts[i] == "Tr_score2"))
						{
							double aux = System.Convert.ToDouble(parts[i+1]);
							if (map.targetScore > 0)
								map.trophyScore2 = (int)aux * map.targetScore;
						}
						if((parts[i] == "tr_burn") || (parts[i] == "Tr_burn"))
						{
							int aux = System.Convert.ToInt32(parts[i+1]);
							map.burnLimit = aux;
						}
						if((parts[i] == "tr_lost") || (parts[i] == "Tr_lost"))
						{
							int aux = System.Convert.ToInt32(parts[i+1]);
							map.lostLimit = aux;
						}
						if(parts[i] == "#lock_and")
						{
							map.lock_and = true;
						}
						if(parts[i] == "#lock_or")
						{
							map.lock_or = true;
						}
						if(parts[i] == "#lock_not")
						{
							map.lock_not = true;
						}
						if(parts[i] == "#lock_teo")
						{
							map.lock_teo = true;
						}
						if(parts[i] == "#lock_imp")
						{
							map.lock_imp = true;
						}
						if(parts[i] == "#lock_switcher")
						{
							map.lock_switcher_button = true;
						}
						if(parts[i] == "#lock_selector")
						{
							map.lock_rule_selector = true;
						}
						if((parts[i] == "I") || (parts[i] == "i"))
						{
							int aux = System.Convert.ToInt32(parts[i+1]);
							if(!map.tiposDeIngredientes.Contains(aux))
								map.tiposDeIngredientes.Add(aux);
						}
						else if((parts[i] == "P") || (parts[i] == "p"))
						{
							int aux = System.Convert.ToInt32(parts[i+1]);
							map.listaDePedidos.Add(aux);
						}
						else if(((parts[i] == "G") || (parts[i] == "g")) && ( y < Altura) && (!map.isRandom))
						{
							for(int x=0;x<Largura;x++)
							{
								if (x >= parts.Length-1) break;
								
								int aux = System.Convert.ToInt32(parts[i+x+1]);
								
								BlockSettings block = new BlockSettings();
								block.GridPosition = new Vector2 (x,Altura-y);
								block.id = aux;
								map.gridDeIngredientes.Add(block);
							}
							y++;
						}
					}
				}
				
				if(map.isRandom)
				{
					map.gridDeIngredientes.Clear();
					for(int y_ = 0; y_ < Altura; y_++)
						for(int x=0; x<Largura;x++)
						{
							BlockSettings block = new BlockSettings();
							int aux = (map.tiposDeIngredientes[Random.Range(0,map.tiposDeIngredientes.Count)]);
							block.GridPosition = new Vector2 (x,y_);
							block.id = aux;
							map.gridDeIngredientes.Add(block);
						}
				}
			}
		}
		
		public static Level LoadLevel (TextAsset content , int Altura , int Largura){
		
			Level map = new Level();
			
			IniciaLevel (map,content,Altura,Largura);
			
			return map;

		}
	}
}
