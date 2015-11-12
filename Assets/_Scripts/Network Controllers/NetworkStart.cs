using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkStart : Photon.MonoBehaviour {

	private string VERSION = "v0.0.1";

	public Text connectionStatus;



	void Start()
	{
		//connectionStatus = gameObject.GetComponent<Text>();
	}


	//Start button. This starts the connection
	public void mouseClick()
	{
		PhotonNetwork.ConnectUsingSettings (VERSION);
	}


	void Update()
	{
		//updates the connection string status text to the current status
		connectionStatus.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}


	void OnJoinedLobby()
	{
		//join the lobby once your joined the phonton lobby and connected succeffuslly
		Application.LoadLevel ("Lobby");
	}

	//PhotonNetwork.playerName = PlayerPrefs.GetString("Pirate", "Guest" + Random.Range(1, 9999));
	//PlayerPrefs.SetString ("playerName", PhotonNetwork.playerName);
}
