using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	Player m_Player;
	public Player Player
	{
		get
		{
			return Helper.GetCachedComponent<Player>( gameObject, ref m_Player );
		}
	}

	NetworkPlayer m_NetworkPlayer;
	public NetworkPlayer NetworkPlayer
	{
		get
		{
			return Helper.GetCachedComponent<NetworkPlayer>( gameObject, ref m_NetworkPlayer );
		}
	}


	MoveOnAxisInput m_MoveOnAxisInput;
	public MoveOnAxisInput MoveOnAxisInput
	{
		get
		{
			return Helper.GetCachedComponent<MoveOnAxisInput>( gameObject, ref m_MoveOnAxisInput );
		}
	}

	ShootOnAxisInput m_ShootOnAxisInput;
	public ShootOnAxisInput ShootOnAxisInput
	{
		get
		{
			return Helper.GetCachedComponent<ShootOnAxisInput>( gameObject, ref m_ShootOnAxisInput );
		}
	}

	PhotonView m_View;
	public PhotonView PhotonView
	{
		get
		{
			return Helper.GetCachedComponent<PhotonView>( gameObject, ref m_View );
		}
	}
}
