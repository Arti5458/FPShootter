using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	int k=1;
	public ScreenFader screenFader;	// Use this for initialization
	void Start () {

	}

	void Update()
		//Если хотите чтобы скрипт роботал при входе в триггер то меняет "OnMouseDown" на "OnTriggerEnter"
	{
         
	}
	IEnumerator MyMethod(float wait) {
		//Debug.Log("Before Waiting 2 seconds");

		yield return new WaitForSeconds(wait);
		Application.LoadLevel(1);
		//Debug.Log("After Waiting 2 Seconds");
	}

	void newgame(){
		screenFader.fadeState = ScreenFader.FadeState.In;
		if (k==1)
		{
			k =0;
			StartCoroutine(MyMethod(0.6f));
		}
		//StartCoroutine(MyMethod(f));	
	}
	void exit(){
		Application.Quit();
	}

	





}
