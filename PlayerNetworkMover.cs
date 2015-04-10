using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour 
{
	public delegate void Respawn(float time);
	public event Respawn RespawnMe;
	public delegate void SendMessage(string MessageOverlay);
	public event SendMessage SendNetworkMessage;
	[SerializeField] Animator anim;
	[SerializeField] Animator animpers;
	GameObject gameob;
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
	float health = 100f;
	bool aim = false;
	bool sprint = false;
	float speed = 0f;
	bool initialLoad = true;
	int kill=0;

	void Start () {
		rotation=transform.rotation;
		//gameob=GameObject.Find("health");
		//gameob.SetActive(false);

		//gameob.GetComponent<Image>().enabled=false;
		//animpers = GetComponentInChildren<Animator> ();
		//anim = GetComponentInChildren<Animator> ();
		//if (!photonView.isMine)

		if(photonView.isMine)
		{
			//gameob.SetActive(true);
			//gameob.GetComponent<Image>().enabled=true;
			GetComponent<PlayerShooting>().enabled = true;
			transform.Find("FirstPersonCharacter/GunCamera/MARMO3/Sights").gameObject.layer = 11;
			transform.Find("FirstPersonCharacter/GunCamera/MARMO3/default").gameObject.layer = 11;
			transform.Find("FirstPersonCharacter/GunCamera/MARMO3/default/Cube").gameObject.layer = 11;
			transform.Find("GameObject/GameObject/char_robotGuard/char_robotGuard_body").gameObject.layer = 12;
			transform.Find("GameObject/GameObject/char_robotGuard/char_robotGuard_skeleton/char_robotGuard_Hips/char_robotGuard_Spine/char_robotGuard_Neck/char_robotGuard_Head/char_robotGuard_helmet").gameObject.layer = 12;
			rotation=transform.rotation;
			rigidbody.useGravity = true;
			(GetComponent("FirstPersonController") as MonoBehaviour).enabled = true;
			//GetComponent<CharacterController>().enabled=true;
			GetComponentInChildren<AudioListener>().enabled=true;
			GetComponent<AudioSource>().enabled=true;

			//GetComponentInChildren<AudioListener>().enabled=true;
			//foreach(SimpleMouseRotator rot in GetComponentsInChildren<SimpleMouseRotator>())
				//rot.enabled = true;
			foreach(Camera cam in GetComponentsInChildren<Camera>())
			{
				cam.enabled = true;
				//return;
			}
			GameObject.Find("music").gameObject.GetComponent<AudioSource>().enabled=true;
			//foreach(Renderer mesh in GetComponentsInChildren<MeshRenderer>())
				//mesh.enabled = true;
			
			transform.Find("FirstPersonCharacter/GunCamera/MARMO3").gameObject.layer = 11;
		}
		else{
			StartCoroutine("UpdateData");
		}
	}
	IEnumerator UpdateData () 
	{
		if(initialLoad)
		{
			initialLoad = false;
			transform.position = position;
			transform.rotation = rotation;
		}
		
		while(true)
		{
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			anim.SetBool("Aim", aim);
			anim.SetBool ("Sprint", sprint);
			animpers.SetFloat("speed",speed);
			yield return null;
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(health);
			stream.SendNext(anim.GetBool ("Aim"));
			stream.SendNext(anim.GetBool ("Sprint"));
			stream.SendNext(animpers.GetFloat("speed"));
		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
			health = (float)stream.ReceiveNext();
			aim = (bool)stream.ReceiveNext();
			sprint = (bool)stream.ReceiveNext();
			speed = (float)stream.ReceiveNext();
		}
	}
	void FixedUpdate()
	{

	}
	
	[RPC]
	public void GetShot(float damage, string enemyName)
	{
		health -= damage;
		gameob=GameObject.Find("health");
		gameob.GetComponent<Image>().enabled=true;
		Image image =gameob.GetComponent<Image>();
		image.fillAmount = health/100;
		Debug.Log(image.fillAmount);
		if (health <= 0 && photonView.isMine){
			PhotonNetwork.Destroy (gameObject);
			if(SendNetworkMessage != null)
				SendNetworkMessage(PhotonNetwork.player.name + " was killed by " + enemyName);
			kill++;

			if(RespawnMe != null)
				RespawnMe(3f);
			if(photonView.isMine)
			{
				GUI.Label(new Rect(10,10,100,20),"Frags = "+kill);
			}

		}

	}
}