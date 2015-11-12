using UnityEngine;
using System.Collections;

public class WeaponMover : MonoBehaviour {

	public float speed;

	// Use this for initialization
	//void Start () {
	//	GetComponent<Rigidbody2D> ().velocity = (transform.up * speed);
	//}

	
	private Transform trans;
	
	void Awake() {
		this.useGUILayout = false;
		this.trans = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		//var moveAmt = speed * Time.deltaTime;
		
		//var pos = this.trans.position;
		//pos.y += moveAmt;
		//this.trans.position = pos;

		transform.position += transform.up*speed*Time.deltaTime;

	}

}
