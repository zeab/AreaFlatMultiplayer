using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDCanvasPropertys : PlayerBase {

	public Text Health;
	public Text Score;
	public Text MatchTime;
	public Text Weapon;


	private NetworkPlayer playerObject;
	private float playerMaxHP;

	// Use this for initialization
	void Start () {
		//playerMaxHP = NetworkPlayer.Health;
		//playerObject = this;
	}

	// Update is called once per frame
	void Update () {
	
		//Health.text = "Health: " + playerMaxHP.ToString();
		//Debug.Log(GetComponent<NetworkPlayer>().Health.ToString());
		//Debug.Log (this.playerObject.MaxHealth);
		//Debug.Log ("HP of " + NetworkPlayer.GetComponent<PhotonView>().owner + " is " + Health);

	}
}
