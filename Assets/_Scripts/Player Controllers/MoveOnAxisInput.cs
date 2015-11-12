using UnityEngine;
using System.Collections;

public class MoveOnAxisInput : MonoBehaviour
{
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public float speed = 1.0f;

//	Vector3 position;
//	Quaternion rotation;
//	float lerpSmooth = 5f;
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += (Vector3.right*Input.GetAxis(horizontalAxis) + Vector3.up*Input.GetAxis(verticalAxis)).normalized*speed*Time.deltaTime;


//		transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * lerpSmooth);


	}
}
