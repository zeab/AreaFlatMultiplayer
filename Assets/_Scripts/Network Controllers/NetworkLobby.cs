﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkLobby : Photon.MonoBehaviour {

	public Text connectionStatus;

	public Text playerName;

	private string roomName = "MyRoom";

	void Start ()
	{
		playerName.text = PlayerPrefs.GetString("Player Name");

	}

	// Update is called once per frame
	void Update () {
		connectionStatus.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}

	public void onCreateGameClick()
	{
		Application.LoadLevel ("level_01");

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.maxPlayers = 4;

		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);


	}



}





