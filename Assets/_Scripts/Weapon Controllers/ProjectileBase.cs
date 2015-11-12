using UnityEngine;
using System.Collections;

/// <summary>
/// Defines base functionality that is used in all projectiles. This demo only has one type of projectile, but try to add more yourself :)
/// </summary>
public class ProjectileBase : MonoBehaviour
{
	/// <summary>
	/// How fast is the projectile flying?
	/// </summary>
	public float Speed;

	/// <summary>
	/// How long is the projectile alive until it is destroyed automatically
	/// </summary>
	public float LifeTime;

	/// <summary>
	/// Reference to the hit effect that is played when the projectile hits a ship or the environment
	/// </summary>
	public GameObject HitFX;

	double m_CreationTime;
	Vector3 m_StartPosition;
	int m_ProjectileId;

	Player m_Owner;

	/// <summary>
	/// The owner of this projectile is the ship who fired it
	/// </summary>
	public Player Owner
	{
		get
		{
			return m_Owner;
		}
		set
		{
			m_Owner = value;
		}
	}


	public int ProjectileId
	{
		get
		{
			return m_ProjectileId;
		}
	}

	void Start()
	{
		//Do Physics.SphereCastAll
	}

	public void SetStartPosition( Vector3 position )
	{
		m_StartPosition = position;
	}

	public void SetCreationTime( double time )
	{
		m_CreationTime = time;
	}

	public void SetProjectileId( int id )
	{
		m_ProjectileId = id;
	}

	void Update()
	{
		float timePassed = (float)( PhotonNetwork.time - m_CreationTime );
		transform.position = m_StartPosition + transform.up * Speed * timePassed;

		if( timePassed > LifeTime )
		{
			//Debug.Log(PhotonNetwork.time);
			Destroy( gameObject );
			//Debug.Log("destory by lifetime");
		}

//		if( transform.position.y < 0f )
//		{
//			Destroy( gameObject );
//			CreateHitFx();
//			Debug.Log("Destory by position");
//		}
	}

	void CreateHitFx()
	{
		Instantiate( HitFX, transform.position, transform.rotation );
	}

	public void OnProjectileHit()
	{
		Destroy( gameObject );
		CreateHitFx();
	}

	void OnTriggerEnter2D( Collider2D collision )
	{
		if( collision.GetComponent<Collider2D>().tag == "Player" )
		{
			//playerName = GetComponent<PhotonView>().owner.name;

			OnProjectileHit();

			Player player = collision.GetComponent<Collider2D>().GetComponent<Player>();
			//Debug.Log(player.GetComponent<PhotonView>().owner.name);

			if (collision.GetComponent<PhotonView>().isMine)
			{
				//collision.GetComponent<Player>().GetComponent<PhotonView>().RPC("DealDamage", PhotonTargets.All, damage, myOwnerID);
				//collision.GetComponent<NetworkPlayer>().GetComponent<PhotonView>().RPC("DealDamage", PhotonTargets.All, 10);
				collision.GetComponent<NetworkPlayer>().DealDamage(10);

			}



			//player.OnProjectileHit( this );

		}
	}





//	void OnTriggerExit2D( Collider2D collision )
//	{
//		if( collision.GetComponent<Collider2D>().tag == "Obstacle" )
//		{
//			OnProjectileHit();
//		}
//		else if( collision.GetComponent<Collider2D>().tag == "Player" )
//		{
//			Player player = collision.GetComponent<Collider2D>().GetComponent<Player>();
//
//			if( player.PhotonView.isMine == false )
//			{
//				return;
//			}
//
//			Debug.Log("Hit");
////			if( m_Owner.Team == Team.None || player.Team != m_Owner.Team )
////			{
////				player.ShipCollision.OnProjectileHit( this );
////				OnProjectileHit();
////				m_Owner.ShipShooting.SendProjectileHit( m_ProjectileId );
////			}
//		}
//	}
}