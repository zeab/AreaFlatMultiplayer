using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loginFieldProperty : Photon.MonoBehaviour {

	public InputField loginField; 

	private string playerName;

	void Start()
	{
	
		if (PlayerPrefs.HasKey("Player Name"))
		{
			loginField.text = PlayerPrefs.GetString("Player Name");
		}
		else
		{		
			//set the login to the player prefab
			playerName = ("Liero" + Random.Range (1, 9999));
			loginField.text = playerName;
			PlayerPrefs.SetString("Player Name", playerName);
		}
	}





	public void onFoucsChange ()
	{
		//PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Liero" + Random.Range(1, 9999));
		PlayerPrefs.SetString ("Player Name", loginField.text);
	}
}
