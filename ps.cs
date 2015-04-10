using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using System.Collections;

public class ps : MonoBehaviour {
	
	public ParticleSystem muzzleFlash;
	public GameObject impactPrefab;
	

	
	GameObject[] impacts;
	int currentImpact = 0;
	int maxImpacts = 5;
	// Use this for initialization

	void Start () {
		//_characterController = GetComponentInParent <CharacterController>();
		impacts = new GameObject[maxImpacts];
		for(int i = 0; i < maxImpacts; i++)
			impacts[i] = (GameObject)Instantiate(impactPrefab);
		
		//anim = GetComponentInChildren<Animator> ();
	}
	
	IEnumerator MyMethod(float wait) {
		//Debug.Log("Before Waiting 2 seconds");
		//reload=true;
		//print(Time.time);
		yield return new WaitForSeconds(wait);
		//print(Time.time);
		//reload=false;
		//Debug.Log("After Waiting 2 Seconds");
	}
	
	
	// Update is called once per frame
	void Update () {
		
	

	
	if (Input.GetMouseButton(0))
	{
			fire ();
			StartCoroutine(MyMethod(0.5f));

	}
	//}
	
	//}
	}
	
	void fire()
	{


			Debug.Log ("firing");

		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hitinfo;


		if (	Physics.Raycast(ray,out hitinfo))
		{

			Debug.Log ("we hit"+hitinfo.collider.name);
			impacts[currentImpact].transform.position = hitinfo.point;
			impacts[currentImpact].GetComponent<ParticleSystem>().Play ();
			
			if(++currentImpact >= maxImpacts)
				currentImpact = 0;
		}

				
				//cycle impact effects
				
			
		}

}