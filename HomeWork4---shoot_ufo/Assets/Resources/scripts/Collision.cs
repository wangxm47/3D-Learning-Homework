using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public SceneController sceneController;
    private void Start()
    {
       sceneController = (SceneController)SSDirector.getInstance().currentScenceController;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Disk")
        {
            other.gameObject.SetActive(false);
            sceneController.explosion.transform.position = other.gameObject.transform.position;
            sceneController.explosion.GetComponent<Renderer>().material = other.gameObject.gameObject.GetComponent<Renderer>().material;
            sceneController.explosion.GetComponent<ParticleSystem>().Play();//触地爆炸
            sceneController.factory.cacheDisk(other.gameObject);//回收
        }
    }
}