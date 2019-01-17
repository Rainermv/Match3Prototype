using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Food;

public class Factory {

	public GameObject block_ref;
	public GameObject request_ref;

	private float BLOCK_SIZE;
	private float Z_POS;

	private static Factory instance;

	private Factory()
	{

	}

	public static Factory GetInstance(){
		if (Factory.instance == null)
				instance = new Factory ();
		return instance;
	}

	public void InitFactory(float _B_SIZE, float _Z_POS, GameObject _block_ref, GameObject _request_ref )
	{

		instance.BLOCK_SIZE = _B_SIZE;
		instance.Z_POS = _Z_POS;
		
		instance.block_ref = _block_ref;
		instance.request_ref = _request_ref;
	}




	public GameObject CreateBlock(Vector3 position, int id, ref List<GameObject> blocks){
		
		GameObject n_block = (GameObject)GameObject.Instantiate (block_ref, new Vector3 (position.x,position.y,Z_POS), Quaternion.identity);
		
		n_block.transform.localScale = new Vector3(BLOCK_SIZE,BLOCK_SIZE,0.3f);
		//n_block.SetActive (false);
		n_block.GetComponent<Block> ().ChangeId (id);
		blocks.Add (n_block);
		
		return n_block;
		
	}

	public GameObject CreateRequest(Vector3 position, int id, ref List<GameObject> requests)
	{
		GameObject n_request = (GameObject)GameObject.Instantiate (request_ref, new Vector3 (position.x+0.3f,position.y,Z_POS), Quaternion.identity);
		
		//n_request.transform.localScale = new Vector3(BLOCK_SIZE,BLOCK_SIZE,0.3f);
		//n_block.SetActive (false);
		n_request.GetComponent<Request> ().ChangeId (id);
		requests.Add (n_request);
		
		return n_request;
	}
	



}
