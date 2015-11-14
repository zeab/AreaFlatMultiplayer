using UnityEngine;
using System.Collections;

public class DestroyAfterSomeTime : MonoBehaviour {

	private float expireTime = 3f;			//this is the timed live of the projectile
	
	// Use this for initialization
	void Start () 
	{
		StartCoroutine (DestroyMyselfAfterSomeTime ());
	}

	IEnumerator DestroyMyselfAfterSomeTime()
	{
		yield return new WaitForSeconds (expireTime);
		Destroy (transform.gameObject);
	}//END DestoryMyselfAfterSomeTime()
}
