using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkPlayer : PlayerBase {

	//sets the death explosion prefab
	public GameObject ExplosionPrefab;
	public GameObject RespawnPrefab;

	public Slider m_HealthSlider; 
	public Image m_FillHealthSlider;

	public Slider m_WeaponSlider;
	public Image m_FillWeaponSlider;

	public Color m_FullHealthColor = Color.green;
	public Color m_ZeroHealthColor = Color.red;

	public Text m_PlayerName;

	bool isAlive = true;
	Vector3 position;
	Quaternion rotation;
	float posLerpSmooth = 10f;
	float rotLerpSmooth = 10f;

	private int playerID;
	private SpriteRenderer m_playerColor;

	public GameObject HUDCanvas;

	private float respawnTime = 3f;

	//sets the max health as a read only variable
	private float m_MaxHealth = 100f;
	public float MaxHealth
	{
		get
		{
			return m_MaxHealth;
		}
	}


	//sets up the health variables for networking goodness
	float m_Health;
	public float Health
	{
		get
		{
			return m_Health;
		}
		set
		{
			m_Health = Health;
		}
	}


	Color m_PlayerColor;
	public Color PlayerColor
	{
		get
		{
			return m_PlayerColor;
		}
		
	}

	//playerID = GetComponentInParent<Player> ().GetComponent<PhotonView>().viewID;

	// Use this for initialization
	void Start () 
	{
		//playerID = GetComponentInParent<Player> ().GetComponent<PhotonView>().viewID;
		playerID = PhotonView.viewID;
		m_Health = MaxHealth;
		m_PlayerName.text = PhotonView.owner.name;

		m_playerColor = gameObject.GetComponentInChildren<SpriteRenderer> ();
		m_playerColor.color = new Color(Random.value, Random.value, Random.value);

		//Health = GetComponent<


		//Debug.Log (playerID);

		if (PhotonView.isMine) {
			GetComponent<MoveOnAxisInput> ().enabled = true;
			GetComponent<ShootOnAxisInput> ().enabled = true;
		} 
		else 
		{
			StartCoroutine("Alive");
		}
	}

	void Update()
	{
		//Debug.Log ("HP of " + PhotonView.owner + " is " + Health);
		SetHealthUI ();
				
	}




	public void setColor(Color color)
	{
		color = m_playerColor.color;
//		
//		
//		m_PlayerColor = m_playerColor.color;

	}


	void OnPhotonInstantiate( PhotonMessageInfo info )
	{
		//This method gets called right after a GameObject is created through PhotonNetwork.Instantiate
		//The fifth parameter in PhotonNetwork.instantiate sets the instantiationData and every client
		//can access them through the PhotonView. In our case we use this to send which team the ship
		//belongs to. This methodology is very useful to send data that only has to be sent once.
		
		if( PhotonView.isMine == false )
		{
			setColor( (Color)PhotonView.instantiationData[ 0 ] );
		}
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) 
		{

			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			//stream.SendNext(rigidbody2D.velocity);

			stream.SendNext( m_Health );

		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();

			float oldHealth = m_Health;
			m_Health = (float)stream.ReceiveNext();
		}
	}


	IEnumerator Alive()
	{
		while(isAlive)
		{
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * posLerpSmooth);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotLerpSmooth);


			yield return null;
		}
	}

	
	/// <summary>
	/// Modifies the health. Neg numbers for -dmg and Pos numbers for +health
	/// </summary>
	/// <param name="damage">Damage.</param>

	public void ModifyHealth( float damage )
	{

		//this is so we dont over heal ourselves past our max health
		if (m_Health + damage > MaxHealth)
		{
			m_Health = MaxHealth;
		}
		else
		{
			m_Health += damage;
		}

		if (m_Health <= 0)
		{
			//set Health to 0
			m_Health = 0;
			//despawn player()
			PhotonView.RPC ("PlayerIsVisible", PhotonTargets.All, new object[] {false});


			if (PhotonView.isMine)
			{
				GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ().respawnTimer = respawnTime;
			}
		}

	}

	private void SetHealthUI()
	{
		m_HealthSlider.value = Health;
		m_FillHealthSlider.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, Health / MaxHealth);
		//Debug.Log (Color.green);
	}


	[PunRPC]
	private void PlayerIsVisible(bool visible)
	{
		//Disable all the sprites
		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
		for( int i = 0; i < renderers.Length; ++i )
		{
			renderers[ i ].enabled = visible;
		}

		//disable the collider and move/shoot scripts and the UI
		GetComponent<CircleCollider2D> ().enabled = visible;
		GetComponent<ShootOnAxisInput> ().enabled = visible;
		GetComponent<MoveOnAxisInput> ().enabled = visible;
		HUDCanvas.SetActive (visible);

		CreateHitFx(ExplosionPrefab);
	}


	/// <summary>
	/// Creates the hit fx. This creates the explosion prefab on death
	/// </summary>
	void CreateHitFx(GameObject FXPrefab)
	{
		Instantiate( FXPrefab, transform.position, transform.rotation );
		//maybe start a coroutine on destory here for the partial object. 
		//should really be doing object pooling but hey dont think this is really resource intensive
	}





	[PunRPC]
	public void RespawnPlayer(Vector3 position, Quaternion rotation, int playerID )
	{

		if (GetComponent<PhotonView>().isMine && (GetComponent<PhotonView>().viewID == playerID))
		{
			
			//Debug.Log("I get here");
			m_Health = 100;
			
			transform.position = position;
			transform.rotation = rotation;



			bool visible = true;

			//Disable all the sprites
			SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
			for( int i = 0; i < renderers.Length; ++i )
			{
				renderers[ i ].enabled = visible;
			}
			
			//disable the collider and move/shoot scripts and the UI
			GetComponent<CircleCollider2D> ().enabled = visible;
			GetComponent<ShootOnAxisInput> ().enabled = visible;
			GetComponent<MoveOnAxisInput> ().enabled = visible;
			HUDCanvas.SetActive (visible);
			
			
		}
		else
		{

			GameObject[] playerObjects = GameObject.FindGameObjectsWithTag( "Player" );
			
			for( int i = 0; i < playerObjects.Length; ++i )
			{
				if (playerObjects[ i ].GetComponent<NetworkPlayer>().GetComponent<PhotonView>().viewID == playerID)
				{
					SpriteRenderer[] renderers = playerObjects[i].GetComponentsInChildren<SpriteRenderer>();
					
					for( int k = 0; k < renderers.Length; ++k )
					{
						renderers[ k ].enabled = true;
					}
					//Debug.Log("Updating Score");
					playerObjects[i].GetComponent<NetworkPlayer>().GetComponent<CircleCollider2D> ().enabled = true;
					playerObjects[i].GetComponent<NetworkPlayer>().m_Health = playerObjects[i].GetComponent<NetworkPlayer>().MaxHealth;

					bool visible = true;
					HUDCanvas.SetActive (visible);


				}
			}
		}

		//create the respawn partiales
		Instantiate( RespawnPrefab, position, rotation );
	}













}
