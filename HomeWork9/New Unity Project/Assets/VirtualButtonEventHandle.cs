using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualButtonEventHandle : MonoBehaviour ,IVirtualButtonEventHandler{

    public GameObject vb;
    public Animator ani;
	// Use this for initialization
	void Start () {
        VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        //Debug.Log(vbb);
        if (vbb)
        {
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        if(ani.GetInteger("state") == 1)
        {
            ani.SetInteger("state", 0);
        }
        else
        {
            ani.SetInteger("state", 1);
        }
        Debug.Log("pressed");
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("released");
    }
}
