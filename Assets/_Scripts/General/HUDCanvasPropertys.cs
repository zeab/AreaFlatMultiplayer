using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDCanvasPropertys : PlayerBase {

	public Text Health;
	public Text Score;
	public Text MatchTime;
	public Text Weapon;

	public GameObject myPlayer;

	private NetworkManager m_NetworkManager;

	private double m_StartTime;
	private int m_SecondsPerTurn;


	private NetworkPlayer playerObject;
	private float playerMaxHP;

	int m_turn;
	public int turn 
	{
		get 
		{
			return m_turn;
		}
	}


	void Start()
	{
		m_NetworkManager = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ();
	}


	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (myPlayer.GetComponent<NetworkPlayer>().Health);


		m_StartTime = m_NetworkManager.StartTime;
		m_SecondsPerTurn = m_NetworkManager.SecondsPerTurn;

		double elapsedTime = (PhotonNetwork.time - m_StartTime);
		double remainingTime = m_SecondsPerTurn - (elapsedTime % m_SecondsPerTurn);
		m_turn = (int)(elapsedTime / m_SecondsPerTurn);

		//PhotonNetwork.LoadLevel( map.Name );
		MatchTime.text = (string.Format("Match Time: {0:0.0}", remainingTime));

//		double elapsedTime = (PhotonNetwork.time - m_StartTime);
//		MatchTime.text = elapsedTime.ToString();
			//string.Format ("elapsed: {0:0.000}", elapsedTime.ToString());


		Health.text = "Health: " + myPlayer.GetComponent<NetworkPlayer>().Health.ToString();

	
		//Debug.Log(GetComponent<NetworkPlayer>().Health.ToString());
		//Debug.Log (this.playerObject.MaxHealth);
		//Debug.Log ("HP of " + NetworkPlayer.GetComponent<PhotonView>().owner + " is " + Health);

	}
}
