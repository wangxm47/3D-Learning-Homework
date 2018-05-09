using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    private float pos_X, pos_Z;
    public float speed = 0.5f;
    private float dis = 0;
    private bool flag = true;
    public int state = 0;
    public GameObject role;
    int n = 0;
    public SceneController sceneController;
    // Use this for initialization
    void Start()
    {
        sceneController = (SceneController)SSDirector.getInstance().currentScenceController;
        role = sceneController.role;
        pos_X = this.transform.position.x;
        pos_Z = this.transform.position.z;
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        if (state == 0)
        {
            patrol();
        }
        else if (state == 1)
        {
            chase(role);
        }
    }

    void patrol()
    {
        if (flag)
        {
            switch (n)
            {
                case 0:
                    pos_Z += 3.2f;
                    break;
                case 1:
                    pos_X += 3.2f;
                    break;
                case 2:
                    pos_Z -= 3.2f;
                    break;
                case 3:
                    pos_X -= 3.2f;
                    break;
            }
            flag = false;
        }
        speed = 0.5f;
        this.transform.LookAt(new Vector3(pos_X, 0.5f, pos_Z));
        dis = Vector3.Distance(transform.position, new Vector3(pos_X, 0.5f, pos_Z));
        if (dis > 0.1)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(pos_X, 0.5f, pos_Z), speed * Time.deltaTime);
        }
        else
        {
            n++;
            n %= 4;
            flag = true;
            this.transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    void chase(GameObject role)
    {
        speed = 1.0f * (1 + sceneController.scoreRecorder.Score / 5);
        transform.position = Vector3.MoveTowards(this.transform.position, role.transform.position, 0.5f * speed * Time.deltaTime);
        this.transform.LookAt(role.transform.position);
    }
}
