using UnityEngine;
using System.Collections;

public class rotateImage : MonoBehaviour {
    public float speed = 1.5f;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(new Vector3(0, 0, speed));
        if (gameObject.transform.rotation.z > 360) {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, gameObject.transform.rotation.w);
        }
	}
}
