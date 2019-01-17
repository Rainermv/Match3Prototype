using UnityEngine;
using System.Collections;

public class Rule
{
	private string _name;
	private Expression _expression;
	private int _size;
	private int _id_result;

	public string Name {
		get{ return _name;}
		set{ _name = value;}
	}

	public Expression Expression {
				get{ return _expression;}
				set{ _expression = value;}
		}

	public int Size {
		get{ return _size;}
		set{ _size = value;}
	}

	public int IDResult {
		get{ return _id_result;}
		set{ _id_result = value;}
	}

	public Rule(string name, Expression expression)
	{
		Name = name;
		
		Expression = expression;
		Size = 0;
		Expression.size(ref _size);

		if (expression.GetType() == typeof(ImplicationExpressionComposite))
			{
				ImplicationExpressionComposite aux_expression = (ImplicationExpressionComposite)expression;

				Number result = (Number)aux_expression.right;
				
				if (result != null)
					_id_result = result.value;
			}
	}
}