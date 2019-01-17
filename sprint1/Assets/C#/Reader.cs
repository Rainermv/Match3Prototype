using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Reader 
{

	public static void LoadRules(List<Rule> Rules,TextAsset content)
	{
		using (StringReader reader = new StringReader(content.text))
		{
			//Debug.Log ("#Arquivo OK");
			//Leitura---------------------------------------
			string line;
			while ((line = reader.ReadLine()) != null) 
			{
				// Do something with the line.
				string[] parts = line.Split (' ');
				if(parts[0] == "#RULE" || parts[0] == "#Rule")
				{
					List<Expression> list_exp = new List<Expression>();

					for (int i=3; i<parts.Length; i++) 
					{
						if(parts[i] == "+")
							{
								AndExpressionComposite aux_and = new AndExpressionComposite();
								if(parts[i-1] != ")") 
									{
										Number numb_aux = new Number(); 
										numb_aux.value = System.Convert.ToInt32(parts[i-1]);
										aux_and.left = numb_aux;
									}
								if(parts[i+1] != "(") 
									{
										Number numb_aux = new Number(); 
										numb_aux.value = System.Convert.ToInt32(parts[i+1]);
										aux_and.right = numb_aux;
									}
								list_exp.Add(aux_and);
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
								list_exp.Add(aux_or);
							}

						if(parts[i] == "=")
							{
								ImplicationExpressionComposite aux_imp = new ImplicationExpressionComposite();
								if(parts[i-1] != ")") 
									{
										Number numb_aux = new Number(); 
										numb_aux.value = System.Convert.ToInt32(parts[i-1]);
										aux_imp.left = numb_aux;
									}
								list_exp.Add(aux_imp);
							}
					}


					int Index_list = 0;
					for (int i=3; i<parts.Length; i++) 
						{
							if(parts[i] == "+")
								{
									AndExpressionComposite aux_and = (AndExpressionComposite)list_exp[Index_list];
									if(parts[i-1] == ")") 
										{
											aux_and.left = list_exp[Index_list-1];
										}
									if(parts[i+1] == "(") 
										{
											aux_and.right = list_exp[Index_list+1];
										}
									Index_list++;
								}
							
							if(parts[i] == "|")
								{
									OrExpressionComposite aux_or = new OrExpressionComposite();
									if(parts[i-1] == ")") 
										{
											aux_or.left = list_exp[Index_list-1];
										}
									if(parts[i+1] == "(") 
										{
											aux_or.right = list_exp[Index_list+1];
										}
									Index_list++;
								}

							if(parts[i] == "=")
								{
									ImplicationExpressionComposite aux_imp = (ImplicationExpressionComposite)list_exp[Index_list]; 
									if(parts[i-1] == ")") 
										{
											aux_imp.left = list_exp[Index_list-1];
										}
									Number res_aux = new Number();
									int bla = System.Convert.ToInt32(parts[i+1]);
									res_aux.value = bla;
									aux_imp.right = res_aux;
									Index_list++;
								}
						}
					Rule aux_rule = new Rule(parts[1],(ImplicationExpressionComposite)list_exp[list_exp.Count-1], System.Convert.ToInt32(parts[2]));
					Rules.Add(aux_rule);
				}
			}
		}
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
					if((parts[i] == "#R") || (parts[i] == "#r"))
					{
						map.isRandom = true;
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
}
