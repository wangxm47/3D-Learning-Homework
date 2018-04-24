using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics_move : SSAction
{
    int force_times = 1;
    Vector3 force;//力  
    float startX;//起始位置  
    public SceneController sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;
    // Use this for initialization
    public override void Start()
    {
        startX = 3 - Random.value * 6;//使发射位置随机在（-3,3）
        this.transform.position = new Vector3(startX, 0, -15);
        force = new Vector3(200 - Random.value * 400, 500, 500 * sceneControler.round);
    }
    public static physics_move GetSSAction()
    {
        physics_move action = ScriptableObject.CreateInstance<physics_move>();
        return action;
    }
    public override void FixedUpdate()
    {
        if (!this.destroy)
        {
            if (force_times > 0)
            {
                gameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameobject.GetComponent<Rigidbody>().AddForce(force);
                force_times--;
                this.destroy = true;
            }
        }
    }
    public override void Update() { }
}