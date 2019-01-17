using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GUIObject {

	protected bool _active;
	public string label;
	public Rect rectangle;


	public GUIObject (){
	}

	public GUIObject(string Label, Rect Rectangle)
	{
		_active = true;
		label = Label;
		rectangle = Rectangle;
	
	}
	public abstract string Draw(Rect parent);

	public bool Active{
		get{ return _active;}
	}

	public abstract void SetActive(bool active);
}

// BOX OBJECT -----------------------------------------------------------------------------------------------------------

public class BoxGUIObject : GUIObject{

	public List<GUIObject> children =  new List<GUIObject>();

	public BoxGUIObject (){
		}
	
	public BoxGUIObject(string Label, Rect Rectangle)
	{
		SetActive(true);
		label = Label;
		rectangle = Rectangle;
		
	}

	public override void SetActive (bool active)
	{
		_active = active;

		foreach (GUIObject child in children) {
			child.SetActive (active);
		}
	}



	public void AddChild(GUIObject child)
	{
		children.Add (child);

	}

	public override string Draw(Rect parent){

		Rect draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y, rectangle.width, rectangle.height);

		// draw self
		if (Active)	GUI.Box (draw_rectangle, label);

		//float offset = 0;

		// draw children
		foreach (GUIObject child in children) {
			draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y, rectangle.width, rectangle.height);
			
			if (child.Draw (draw_rectangle) != null)
					return child.label;

			//offset += + child.rectangle.y + child.rectangle.height ;
		}

		return null;

	}
}

public class AlignedBoxGUIObject : BoxGUIObject{

	public AlignedBoxGUIObject(string Label, Rect Rectangle)
	{
		SetActive(true);
		label = Label;
		rectangle = Rectangle;
		
	}

	public override string Draw(Rect parent){
		
		Rect draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y, rectangle.width, rectangle.height);
		
		// draw self
		if (Active)	GUI.Box (draw_rectangle, label);
		
		float offset = 0;
		
		// draw children
		foreach (GUIObject child in children) {
			draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y + offset, offset, rectangle.height);
			
			if (child.Draw (draw_rectangle) != null)
				return child.label;
			
			offset += + child.rectangle.y + child.rectangle.height ;
		}
		
		return null;
		
	}

}

// BUTTON OBJECT -----------------------------------------------------------------------------------------------------------

public class ButtonGUIObject : GUIObject
{

	public ButtonGUIObject(string Label, Rect Rectangle)
	{
		SetActive(true);
		label = Label;
		rectangle = Rectangle;
		
	}

	public override void SetActive (bool active)
	{
		_active = active;

	}

		public override string Draw(Rect parent){

			Rect draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y, rectangle.width, rectangle.height);

			if (Active && GUI.Button (draw_rectangle, label))
						return label;

			return null;

		}

}

// TEXTURE OBJECT -----------------------------------------------------------------------------------------------------------

public class TextureGUIObject : GUIObject{

	public Texture _texture;

	public TextureGUIObject(Texture texture, Rect Rectangle)
	{
		SetActive(true);
		_texture = texture;
		rectangle = Rectangle;
		
	}

	public override void SetActive (bool active)
			{
				_active = active;
				
			}

	public override string Draw(Rect parent){
		
		Rect draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y, rectangle.width, rectangle.height);
		
		if ((Active) && (_texture != null)){
			GUI.DrawTexture(draw_rectangle,_texture);
		}
		
		return null;	
	}



}

// CONTAINER OBJECT -----------------------------------------------------------------------------------------------------------

public class ContainerGUIObject : GUIObject{
	
	public GameObject _contain;
	private bool instantiated;
	
	public ContainerGUIObject(GameObject contain, Rect Rectangle)
	{
		SetActive(true);
		instantiated = false;
		_contain = contain;
		rectangle = Rectangle;
		
	}
	
	public override void SetActive (bool active)
	{
		_active = active;
		
	}
	
	public override string Draw(Rect parent){
		
		Rect draw_rectangle = new Rect (parent.x + rectangle.x, parent.y + rectangle.y, rectangle.width, rectangle.height);

		if (!instantiated) {
			Vector2 position = Camera.main.ScreenToWorldPoint (new Vector2 (draw_rectangle.x + draw_rectangle.width/2, Camera.main.pixelHeight - draw_rectangle.y - draw_rectangle.height/2));
			_contain = (GameObject)GameObject.Instantiate (_contain, position, Quaternion.identity);
			instantiated = true;
		}
		
		if ((Active) && (_contain != null)){
			GUI.Box(draw_rectangle, Active.ToString());

		}

		if (_contain != null)
			_contain.SetActive (Active);
		
		return null;	
	}
	
	
	
}