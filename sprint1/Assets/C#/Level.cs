using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BlockSettings
{

	private Vector2 position;
    public Vector2 GridPosition
    {
		get{return position;}
		set{int x = (int)value.x;
			int y = (int)value.y;
			position.x = x;
			position.y = y;}
    }
	public int id;
}

public class Level
{
	//Vars

	public bool isRandom = false;

	private List<int> TiposDeIngredientes = new List<int>();
	public List<int> tiposDeIngredientes {
				get{ return TiposDeIngredientes;}
		}

	private List<BlockSettings> GridDeIngredientes = new List<BlockSettings>();
	public List<BlockSettings> gridDeIngredientes{
		get{return GridDeIngredientes;}
	}

	private List<int> ListaDePedidos = new List<int>();
	public List<int> listaDePedidos{
		get{return ListaDePedidos;}
	}
}