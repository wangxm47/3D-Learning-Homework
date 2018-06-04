using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

    public float speed = 1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float translationX = Input.GetAxis("Horizontal") * speed;
        float translationZ = Input.GetAxis("Vertical") * speed;
        translationX *= Time.deltaTime;
        translationZ *= Time.deltaTime;
        this.gameObject.transform.Translate(translationX, 0, 0);
        this.gameObject.transform.Translate(0, 0, translationZ);
        float mousX = Input.GetAxis("Mouse X") * 2 * speed;//得到鼠标移动距离  
        this.transform.Rotate(new Vector3(0, mousX, 0));//旋转 
    }
}
