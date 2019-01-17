using System;
using UnityEngine;
using System.Collections.Generic;

public class Context
	{
		public List<GameObject> block_settings_array;
		//public int index;
	}

//-----------------------------------------------------------------------------------------------------------------------------

public abstract class Expression			
	{
		public abstract Expression run(Context c);
		public abstract void size(ref int size);
	}

//-----------------------------------------------------------------------------------------------------------------------------

public class Number: Expression
	{
		//public BlockSettings value;
		public int value;

		public override Expression run(Context c)
			{
				for (int i =0; i< c.block_settings_array.Count; i++) 
				{
					// if current block id is equal to value
					if (c.block_settings_array [i].GetComponent<Block>().Color_Index == value)
					{
						c.block_settings_array.Remove(c.block_settings_array[i]);
						return this;
					}
				}
				return null;
			}

		public override void size(ref int size)
			{
				size++;
			}

		public override string ToString()
			{
				return value.ToString ();
			}
	}

//-----------------------------------------------------------------------------------------------------------------------------

public abstract class ExpressionComposite: Expression
	{
		public Expression left;
		public Expression right;
	}

//-----------------------------------------------------------------------------------------------------------------------------

public class ImplicationExpressionComposite: ExpressionComposite
	{

		public override Expression run(Context c)
			{
				if (left != null)		
					if (left.run (c) != null)
						if (right != null)
							return right;
				return null;
			}
		public override void size(ref int size)
			{
				if (left != null)		
					left.size(ref size);
			}

		public override string ToString()
			{
				return left.ToString() + " = " + right.ToString();
			}
	}

//-----------------------------------------------------------------------------------------------------------------------------

public class AndExpressionComposite: ExpressionComposite
	{
			
		public override Expression run(Context c)
			{
				if ((left.run (c) != null) && (right.run (c) != null))
					return this;
				return null;
			}

		public override void size(ref int size)
			{
				left.size(ref size);
				right.size(ref size);
			}

		public override string ToString()
			{
				return left.ToString() + " + " + right.ToString();
			}

	}

//-----------------------------------------------------------------------------------------------------------------------------

public class OrExpressionComposite: ExpressionComposite
	{
		
		public override Expression run(Context c)
			{
				if (left.run (c) != null)
						return this;
				else if (right.run (c) != null)
						return this;
		
				return null;
			}

		public override void size(ref int size)
			{
				left.size(ref size);
			}

		public override string ToString()
			{
				return left.ToString() + " | " + right.ToString();
			}
		
	}



