using UnityEngine;
using UnityEngine.UI;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	
	public ParticleSystem muzzleFlash;
	public GameObject impactPrefab;
	
	Animator anim;

	GameObject[] impacts;
	int currentImpact = 0;
	int maxImpacts = 5;
	bool shooting = false;
	float damage = 15f;
	int patrons=15;
	bool reload = false;
	int count=0;
	Ray shotray;
	GameObject gameob;
	bool kill;
	//private CharacterController _characterController;
	//float walk = 0f;
	
	
	// Use this for initialization
	void Start () {
		//_characterController = GetComponentInParent <CharacterController>();
		impacts = new GameObject[maxImpacts];
		for(int i = 0; i < maxImpacts; i++)
		impacts[i] = (GameObject)Instantiate(impactPrefab);
		
		anim = GetComponentInChildren<Animator> ();
	}
	
	
	IEnumerator MyMethod(float wait) {
		//Debug.Log("Before Waiting 2 seconds");
		reload=true;
		print(Time.time);
		yield return new WaitForSeconds(wait);
		print(Time.time);
		reload=false;
		//Debug.Log("After Waiting 2 Seconds");
	}
	
	
	// Update is called once per frame
	void Update () {

		//if (Input.GetButtonUp(mouse))
		//if (patrons ==0)
		//{
		//	StartCoroutine ("MyMethod");
		//	patrons=1;
		
		//}
		//else{






		if (Input.GetMouseButton(0) && !Input.GetKey (KeyCode.LeftShift)) 
			//while (Input.GetKeyDown(KeyCode.Mouse1) && !Input.GetKey (KeyCode.LeftShift))
		{

			if (patrons==0)
			{
				anim.SetTrigger("Reloaded");
				patrons=15;
				
				StartCoroutine(MyMethod(1f));
				if (Input.GetButtonDown("Fire1"))
				{
				Input.GetButtonUp("Fire1");
					StartCoroutine(MyMethod(1f));
				}
				//return ;
			}
			else{
				if (!reload)
				{
					if (count==7)
					{
						StartCoroutine(MyMethod(0.7f));
						count=0;
					}
					else
					{
						muzzleFlash.Play();
						anim.SetTrigger ("Fire");
						shooting = true;
						patrons=patrons-1;
						//StartCoroutine(MyMethod(0.25f));
						count++;
					}
				}
			}
		}
	}
	//}
	
	//}
	
	void FixedUpdate()
	{
		if(shooting)
		{
			shooting = false;

			shotray.origin=transform.position;
			shotray.direction=transform.forward;
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hitinfo;
			if (	Physics.Raycast(ray,out hitinfo))
			{
				Debug.Log ("we hit"+hitinfo.collider.name);
				impacts[currentImpact].transform.position = hitinfo.point;
				impacts[currentImpact].GetComponent<ParticleSystem>().Play ();
				
				if(++currentImpact >= maxImpacts)
					currentImpact = 0;
				if(hitinfo.transform.tag == "Player")
				{
					hitinfo.transform.GetComponent<PhotonView>().RPC ("GetShot", PhotonTargets.All, damage, PhotonNetwork.player.name);
					StartCoroutine(MyMethod(0.2f));

				}
				int lm;
				
				//cycle impact effects

			}
		}
	}
}