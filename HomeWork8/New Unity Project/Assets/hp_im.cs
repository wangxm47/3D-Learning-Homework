using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp_im : MonoBehaviour {

    //主摄像机对象
    private Camera cam;

    //NPC模型高度
    float npcHeight;
    //血条贴图
    public Texture2D blood;
    //血条背景贴图
    public Texture2D blood_back;
    //默认NPC血值
    private int HP = 100;
    // Use this for initialization
    void Start()
    {
        //得到摄像机对象
        cam = Camera.main;

        //注解1
        //得到模型原始高度
        float size_y = this.gameObject.GetComponent<Collider>().bounds.size.y;
        //得到模型缩放比例
        float scal_y = transform.localScale.y;
        //它们的乘积就是高度
        npcHeight = (size_y * scal_y);

    }

    void Update()
    {
        //保持NPC一直面朝MainCamera
        transform.LookAt(cam.transform);
    }

    private void OnGUI()
    {
        //得到NPC头顶在3D世界中的坐标
        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight/2, transform.position.z);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 position = cam.WorldToScreenPoint(worldPosition);
        //得到真实NPC头顶的2D坐标
        position = new Vector2(position.x, Screen.height - position.y);
        //注解2
        //计算出血条的宽高
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood));
        Debug.Log(bloodSize.ToString());
        //通过血值计算红色血条显示区域
        int blood_width = blood.width * HP / 100;
        //先绘制血条背景
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodSize.x*1.1f, bloodSize.y/5), blood_back);
        //再绘制血条
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, blood_width*1.1f, bloodSize.y/5), blood);
        if(GUI.Button(new Rect(200, 50, 50, 50), "+") && HP < 100)
        {
            HP += 10;
        }
        if(GUI.Button(new Rect(200, 120, 50, 50), "-") && HP > 0)
        {
            HP -= 10;
        }
    }
}
