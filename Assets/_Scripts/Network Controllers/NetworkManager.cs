using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {

	//private string VERSION = "v0.0.1";
	public string roomName = "MyRoom";

	public string playerPrefabName = "Player";

	private GameObject myPlayer;

	//public Transform spawnSpot;
	SpawnSpot [] spawnSpots;

	// Use this for initialization
	void Start () 
	{
		//PhotonNetwork.ConnectUsingSettings (VERSION);
		//PhotonNetwork.connectionStateDetailed.ToString ()

		//grabs all the spawn spots on the map
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
	}
	

	void OnJoinedRoom()
	{
		SpawnMyPlayer ();
	}

	void SpawnMyPlayer()
	{
		SpawnSpot mySpwanSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];

		myPlayer = PhotonNetwork.Instantiate (playerPrefabName, mySpwanSpot.transform.position, mySpwanSpot.transform.rotation, 0);

		//sets the player photon view owner name to the player prefab setting
		PhotonNetwork.playerName = PlayerPrefs.GetString("Player Name");
	}

	protected void SetRoundStartTime()
	{
		ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable();
		newProperties.Add( RoomProperty.StartTime, PhotonNetwork.time );

		PhotonNetwork.room.SetCustomProperties( newProperties );

	}


}
