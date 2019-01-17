using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

public class SplashScreenBehaviour : MonoBehaviour {

	public enum Type {NoEffect,FadeIn,FadeOut,DoubleFade,Video};

	[System.Serializable]
	public class Screens 
	{ 
		public Type Efect;
		public int FadeInTimer;
		public int FadeOutTimer;
		public int StaticTimer;

		public Sprite Image;
		public AudioClip Sounds;
		public AnimationClip Videos;
	}

	public Color BackGroundColor;
	public List<Screens> screens;
	
	private float timer;
	private int imagem;
	private int estado;

	SpriteRenderer ThisSprite;
	AudioSource ThisAudio;
	Camera MainCamera;

	// Use this for initialization
	void Start ()
	{
		MainCamera = gameObject.GetComponent<Camera>();
		ThisSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
		ThisAudio = gameObject.GetComponentInChildren<AudioSource>();

		MainCamera.backgroundColor = BackGroundColor;
		estado = 0;
		imagem = -1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(estado)
		{
		case 0: // Mudar imagem
			if(ThisAudio.isPlaying) ThisAudio.Stop();
			imagem++;
			if(screens.Count-1 >= imagem)
			{
				timer = 0;

				if(screens[imagem].Efect == Type.Video)
				{
					//TODO
				}
				else
					ThisSprite.sprite = screens[imagem].Image;
				if(screens[imagem].Sounds != null)
				{
					ThisAudio.clip = screens[imagem].Sounds;
					ThisAudio.Play();
				}



				if(screens[imagem].Efect == Type.Video)
					estado = 1;
				else if(screens[imagem].Efect == Type.FadeIn || screens[imagem].Efect == Type.DoubleFade)
				{
					estado = 2;
					ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,0.0f);
				}
				else if(screens[imagem].Efect == Type.FadeOut)
				{
					ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,255.0f);
					estado = 3;
				}
				else if(screens[imagem].Efect == Type.NoEffect)
				{
					ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,255.0f);
					estado = 4;
				}
			}

			else
			{
				Application.LoadLevel(Application.loadedLevel + 1);
			}
			break;
		case 1: //Video
			//TODO
			estado = 0;
			break;

		case 2: // FadeIn
			timer += Time.deltaTime;
			ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,((255/screens[imagem].FadeInTimer) * timer)/255);
			if(timer >= screens[imagem].FadeInTimer)
			{
				ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,255.0f);
				timer = 0;
				estado = 4;
			}
			break;

		case 3: //FadeOut
			timer += Time.deltaTime;
			ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b, 1f-(((255/screens[imagem].FadeOutTimer) * timer)/255));
			if(timer >= screens[imagem].FadeOutTimer)
			{
				ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,0f);
				timer = 0;
				estado = 0;
			}
			break;

		case 4: // Espera
			timer += Time.deltaTime;
			if(timer >= screens[imagem].StaticTimer)
			{
				timer = 0;
				if(screens[imagem].Efect == Type.FadeIn || screens[imagem].Efect == Type.NoEffect)
					estado = 0;
				else if(screens[imagem].Efect == Type.FadeOut || screens[imagem].Efect == Type.DoubleFade)
				{
					ThisSprite.color = new Color(ThisSprite.color.r,ThisSprite.color.g,ThisSprite.color.b,255.0f);
					estado = 3;
				}
			}
			break;
		}
	}
}
