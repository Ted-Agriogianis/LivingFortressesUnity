using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	public GameObject player;
	public Vector2 cameraPosition;
	// Use this for initialization
	void Start () {
        cameraPosition = player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        cameraPosition.x = Mathf.Lerp(cameraPosition.x, player.transform.position.x, 3 * Time.fixedDeltaTime);
		cameraPosition.y = Mathf.Lerp(cameraPosition.y, player.transform.position.y, 3 * Time.fixedDeltaTime);
		transform.position = new Vector3 (cameraPosition.x, cameraPosition.y, -10);
	}
}
