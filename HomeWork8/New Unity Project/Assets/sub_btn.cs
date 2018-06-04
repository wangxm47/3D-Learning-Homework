using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sub_btn : MonoBehaviour {
    // Use this for initialization
    public Button sub;
    void Start () {
        sub = this.GetComponent<Button>();
        sub.onClick.AddListener(click);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    void click()
    {
        GameObject.FindObjectOfType<Slider>().value -= 10;
    }
}
