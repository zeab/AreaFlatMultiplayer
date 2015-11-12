using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShootOnAxisInput : PlayerBase
{
	public bool useKeyboard;

	public GameObject shot;
	public Transform shotSpawn;

	public string horizontalAxis = "HorizontalAim";
	public string verticalAxis = "VerticalAim";

	public float shootDelay = 0.1f;

	private bool canShoot = true;

	//Aiming the weapon
	private Vector3 mouse_pos;
	private Vector3 object_pos;
	private Vector3 currentPosition;
	private Vector3 moveToward;
	private float angle;

	int m_LastProjectileId;

	List<ProjectileBase> m_Projectiles = new List<ProjectileBase>();

	float rotLerpSmooth = 10f;

	void ResetShot ()
	{
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (useKeyboard == true)
		{
			// Point the cannon at the mouse.
			mouse_pos = Input.mousePosition;
			mouse_pos.z = 0.0f; 
			object_pos = Camera.main.WorldToScreenPoint (transform.position);
			mouse_pos.x = mouse_pos.x - object_pos.x;
			mouse_pos.y = mouse_pos.y - object_pos.y;
			angle = Mathf.Atan2 (mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
			
			Vector3 rotationVector = new Vector3 (0, 0, angle - 90);	
			transform.rotation = Quaternion.Euler (rotationVector);
		
		
			if(Input.GetButton("Fire1") && canShoot)
			{
				GetComponent<PhotonView>().RPC( "OnShoot", PhotonTargets.All, new object[] { shotSpawn.position, shotSpawn.rotation, m_LastProjectileId});
				
				canShoot = false;
				Invoke("ResetShot",shootDelay);
			}
		
		}
		else 
		{
			Vector3 shootDirection = Vector3.right * Input.GetAxis (horizontalAxis)+ Vector3.up*Input.GetAxis(verticalAxis);
			
			var angle = Mathf.Atan2(Input.GetAxis(verticalAxis), Input.GetAxis(horizontalAxis)) * Mathf.Rad2Deg;
			
			if(shootDirection.sqrMagnitude > 0.2f && shootDirection.sqrMagnitude < 0.9f)
			{
				//transform.rotation = Quaternion.Euler(0, 0, angle - 90);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * rotLerpSmooth);
			}
			
			
			if(shootDirection.sqrMagnitude > 0.9f)
			{
				
				
				//transform.rotation = Quaternion.Euler(0, 0, angle - 90);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * rotLerpSmooth);
				
				//transform.rotation = Quaternion.LookRotation(shootDirection, Vector3.forward);
				//Debug.Log(transform.rotation);
				//transform.rotation = Quaternion.AngleAxis(shootDirection.z, Vector3.forward);
				
				if(canShoot)
				{
					//PoolBoss.SpawnOutsidePool("BasicShot", shotSpawn.transform.position, shotSpawn.transform.rotation); 
					//Instantiate(shot,shotSpawn.position,shotSpawn.rotation);
					//PhotonNetwork.Instantiate("BasicShot",shotSpawn.position,shotSpawn.rotation, 0);
					
					
					GetComponent<PhotonView>().RPC( "OnShoot", PhotonTargets.All, new object[] { shotSpawn.position, shotSpawn.rotation, m_LastProjectileId});
					
					//GetComponent<PhotonView>().RPC("SpawnProjectile", PhotonTargets.All, new object[] {launchPosition, launchRotation, playerID, GetComponentInParent<ChangeWeapon>().selectedWeaponNumber});
					
					
					
					canShoot = false;
					Invoke("ResetShot",shootDelay);
				}
			}
		}
	}








	[PunRPC]
	public void OnShoot( Vector3 position, Quaternion rotation, int projectileId, PhotonMessageInfo info )
	{
		double timestamp = PhotonNetwork.time;
		
		if( info != null )
		{
			timestamp = info.timestamp;
		}
		
		CreateProjectile( position, rotation, timestamp, projectileId );
	}


	public void CreateProjectile( Vector3 position, Quaternion rotation, double createTime, int projectileId )
	{
		//m_LastShootTime = Time.realtimeSinceStartup;

		//Debug.Log (projectileId);

		//Vector3 myPosition = transform.position + new Vector3(0, 1, 0);

		GameObject newProjectileObject = (GameObject)Instantiate( Resources.Load<GameObject>( "BasicShot" ), shotSpawn.position, rotation);
		newProjectileObject.name = "ZZZ_" + newProjectileObject.name;
		
		ProjectileBase newProjectile = newProjectileObject.GetComponent<ProjectileBase>();
		
		newProjectile.SetCreationTime( createTime );
		newProjectile.SetStartPosition( position );
		newProjectile.SetProjectileId( projectileId );
		
		newProjectile.Owner = GetComponent<Player>();
		
		m_Projectiles.Add( newProjectile );
	}






}
