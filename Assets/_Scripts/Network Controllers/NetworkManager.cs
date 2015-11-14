using ExitGames.Client.Photon;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

	public string playerPrefabName = "Player";

	private int m_MainHUDCanvas ;

	private GameObject m_myPlayer;
	public GameObject myPlayer
	{
		get
		{
			return m_myPlayer;
		}
	}
	
	SpawnSpot [] spawnSpots;

	//Timer Variables
	//public int SecondsPerTurn = 300;  
	//public double StartTime;						//Holds the start time
	private bool startRoundWhenTimeIsSynced;        // used in an edge-case when we wanted to set a start time but don't know it yet.
	private const string StartTimeKey = "st";		//holds the string for the shorten key name

	public float respawnTimer;						//Holds the respawn timers for the player

	double m_StartTime;
	public double StartTime
	{
		get
		{
			return m_StartTime;
		}
	}
	
	int m_SecondsPerTurn = 300;
	public int SecondsPerTurn
	{
		get
		{
			return m_SecondsPerTurn;
		}
	}

	//Holds if if round is over var
	bool m_isRoundOver;
	public bool isRoundOver
	{
		get
		{
			return m_isRoundOver;
		}
		set
		{
			m_isRoundOver = isRoundOver;
		}
	}

	void Awake()
	{
		PhotonNetwork.automaticallySyncScene = true;
	}

	// Use this for initialization
	void Start () 
	{
		//grabs all the spawn spots on the map
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
		m_isRoundOver = false;
		m_MainHUDCanvas = GameObject.Find ("MainHUDCanvas").GetComponent<HUDCanvasPropertys> ().turn;
	}
	
	private void StartRoundNow()
	{
		// in some cases, when you enter a room, the server time is not available immediately.
		// time should be 0.0f but to make sure we detect it correctly, check for a very low value.
		if ((PhotonNetwork.time) < 0.0001f)
		{
			// we can only start the round when the time is available. let's check that in Update()
			startRoundWhenTimeIsSynced = true;
			return;
		}
		startRoundWhenTimeIsSynced = false;

		//Sets up the custom room property to hold the time
		ExitGames.Client.Photon.Hashtable startTimeProp = new Hashtable();  // only use ExitGames.Client.Photon.Hashtable for Photon
		startTimeProp[StartTimeKey] = PhotonNetwork.time;
		PhotonNetwork.room.SetCustomProperties(startTimeProp);              // implement OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged) to get this change everywhere
	}

	void Update()
	{
		m_MainHUDCanvas = GameObject.Find ("MainHUDCanvas").GetComponent<HUDCanvasPropertys> ().turn;
		if (m_MainHUDCanvas == 1)
		{
			isRoundOver = true;
			//Application.LoadLevel ("lobby");
			//Debug.Log("Yay");

			//disable control for all players


			//display a splash screen of the scores and who won
			//timer for like to 10 seconds
			//have teh master client load the next level...yes?
		}

		//Debug.Log (m_isRoundOver);
	
		if (startRoundWhenTimeIsSynced)
		{
			this.StartRoundNow();   // the "time is known" check is done inside the method.
		}

		SpawnSpot mySpwanSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];

		if (respawnTimer > 0) 
		{
			respawnTimer -= Time.deltaTime;
			if(respawnTimer <= 0){
				//Respawn Player
				//Play respawn poof
				m_myPlayer.GetComponent<PhotonView>().RPC ("RespawnPlayer", PhotonTargets.All, mySpwanSpot.transform.position , mySpwanSpot.transform.rotation, m_myPlayer.GetComponent<PhotonView>().viewID);
				
				//respawn the playere here...
				
				//SEND RPC CALL TO TURN BACK ON THE PLAYER STATS
				//myPlayerGO.GetComponent<PhotonView>().RPC( "RespawnPlayer", PhotonTargets.All, mySpwanSpot.transform.position , mySpwanSpot.transform.rotation, myPlayerGO.GetComponent<PhotonView>().viewID );
				//RespawnPlayer(myPlayerGO);
			}
		}
	}
	
	void OnJoinedRoom()
	{
		SetRoundStartTime ();
		SpawnMyPlayer (Team.None);
	}

	void SpawnMyPlayer(Team team)
	{
		object[] instantiationData = new object[] { (int)team } ;

		SpawnSpot mySpwanSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];

		m_myPlayer = PhotonNetwork.Instantiate (playerPrefabName, mySpwanSpot.transform.position, mySpwanSpot.transform.rotation, 0, instantiationData);

		NetworkPlayer newPlayer = m_myPlayer.GetComponent<NetworkPlayer> ();
		newPlayer.SetTeam (team);

		//m_myPlayer.SetTeam (team);
		//and set HUDCancas to this player object...?
		GameObject HUD = GameObject.Find("MainHUDCanvas");
		HUD.GetComponent<HUDCanvasPropertys> ().myPlayer = m_myPlayer;
	
		//sets the player photon view owner name to the player prefab setting
		PhotonNetwork.playerName = PlayerPrefs.GetString("Player Name");

	}



	protected void SetRoundStartTime()
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.StartRoundNow();
		}
		else
		{
			// as the creator of the room sets the start time after entering the room, we may enter a room that has no timer started yet
			Debug.Log("StartTime already set: " + PhotonNetwork.room.customProperties.ContainsKey(StartTimeKey));
		}
	}

	/// <summary>Called by PUN when new properties for the room were set (by any client in the room).</summary>
	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey(StartTimeKey))
		{
			Debug.Log("Start time set");
			m_StartTime = (double)propertiesThatChanged[StartTimeKey];
		}
	}
	
	/// <remarks>
	/// In theory, the client which created the room might crash/close before it sets the start time.
	/// Just to make extremely sure this never happens, a new masterClient will check if it has to
	/// start a new round.
	/// </remarks>
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!PhotonNetwork.room.customProperties.ContainsKey(StartTimeKey))
		{
			Debug.Log("The new master starts a new round, cause we didn't start yet.");
			this.StartRoundNow();
		}
	}

}
