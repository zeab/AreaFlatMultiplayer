using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkPlayer : PlayerBase {

	//sets the death explosion prefab
	public GameObject ExplosionPrefab;

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

	public float MaxHealth = 100f;
	//sets up the health variables for networking goodness
	float m_Health;
	public float Health
	{
		get
		{
			return m_Health;
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






//	public void OnProjectileHit( ProjectileBase projectile )
//	{
//		DealDamage( 10, projectile.Owner );
//	}

	//	public void DealDamage( float damage, NetworkPlayer damageDealer )

	public void DealDamage( float damage )
	{
		m_Health -= damage;


		//Debug.Log (m_Health);

		//Debug.Log ("HP of " + GetComponent<PhotonView>().owner + " is " + Health);
		//OnHealthChanged( damageDealer );
	}

	private void SetHealthUI()
	{
		m_HealthSlider.value = Health;
		m_FillHealthSlider.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, Health / MaxHealth);
		//Debug.Log (Color.green);
	}









}
