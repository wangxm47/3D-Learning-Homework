using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class round1
{
    public static float size = 0.8f;
    public static Color color = Color.blue;
}
public class round2
{
    public static float size = 0.6f;
    public static Color color = Color.yellow;
}
public class round3
{
    public static float size = 0.4f;
    public static Color color = Color.black;
}
public class DiskFactory : MonoBehaviour
{
    private static DiskFactory _instance;
    public SceneController sceneControler { get; set; }
    public List<GameObject> used;
    public List<GameObject> cache;
    // Use this for initialization

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = Singleton<DiskFactory>.Instance;//单实例化  
            _instance.used = new List<GameObject>();
            _instance.cache = new List<GameObject>();
        }
    }
    public void Start()
    {
        sceneControler = (SceneController)SSDirector.getInstance().currentScenceController;             //引入场记  
        sceneControler.factory = this;//设置场记的工厂  
    }

    public GameObject getDisk(int round)
    {
        if ( sceneControler.score == round * 20)
        //每轮总共发射30个，如果得分达到一定要求进入下一轮，否则GameOver
        {
            sceneControler.round++;
            sceneControler.lost = 0;
        }
        else if (sceneControler.lost == 15)
        {
            sceneControler.game = 2;//游戏结束
        }
        if(sceneControler.round == 4)
        {
            sceneControler.game = 4;
        }
        GameObject newDisk;
        if (cache.Count == 0)
        {
            if (sceneControler.isphysics)
            {
                newDisk = GameObject.Instantiate(Resources.Load("prefabs/Disk"), new Vector3(0, 0, -10), Quaternion.identity) as GameObject;
            }
            else
            {
                newDisk = GameObject.Instantiate(Resources.Load("prefabs/Disk1"), new Vector3(0, 0, -10), Quaternion.identity) as GameObject;
            }
        }
        else
        {
            newDisk = cache[0];
            //newDisk.SetActive(true);
            cache.Remove(cache[0]);
        }
        switch (round)
        //根据轮数制定飞碟的颜色和大小
        {
            case 1:
                newDisk.transform.localScale = new Vector3(round1.size, round1.size, round1.size);
                newDisk.GetComponent<Renderer>().material.color = round1.color;
                break;
            case 2:
                newDisk.transform.localScale = new Vector3(round2.size, round2.size, round2.size);
                newDisk.GetComponent<Renderer>().material.color = round2.color;
                break;
            case 3:
                newDisk.transform.localScale = new Vector3(round3.size, round3.size, round3.size);
                newDisk.GetComponent<Renderer>().material.color = round3.color;
                break;
        }
        used.Add(newDisk);
        return newDisk;
    }

    public void cacheDisk(GameObject disk1)
    {
        for (int i = 0; i < used.Count; i++)
        {
            if (used[i] == disk1)
            {
                used.Remove(disk1);
                disk1.SetActive(true);//被鼠标击中的disk设置为false，所以这里全部激活一遍
                cache.Add(disk1);
            }
        }
    }
}
