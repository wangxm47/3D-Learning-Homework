using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class add_btn : MonoBehaviour {

    public Button add;
    void Start()
    {
        add = this.GetComponent<Button>();
        add.onClick.AddListener(click);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void click()
    {
        GameObject.FindObjectOfType<Slider>().value += 10;
    }
}
