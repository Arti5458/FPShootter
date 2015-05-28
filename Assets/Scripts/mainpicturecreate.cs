using UnityEngine;
using System.Collections;

public class mainpicturecreate : MonoBehaviour {

	private int pRounded;
	public GUIStyle Main;
	public GUIStyle BackGround;
	

	
	void OnGUI () {
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", Main);
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", BackGround);
	}
}
