using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, ISceneController, IUserAction
{
    public SSActionManager actionManager { get; set; }
    public int round = 0;//轮数
    public int score = 0;//分数
    public Text Score;//分数文本
    public Text Round;//轮数文本
    public Text Time;//倒计时文本
    public Text Text;//结束文本
    public Text Lost;//未击中文本
    public int game = 0;//记录游戏进行情况
    public int num = 0;//每轮的飞碟数量
    public int lost = 0;//未击中
    GameObject disk;
    GameObject explosion;
    GameObject plane;
    public int StartTimes = 3; //准备时间
    // Use this for initialization
    void Awake()
    //创建导演实例并载入资源
    {
        SSDirector director = SSDirector.getInstance();
        DiskFactory DF = DiskFactory.getInstance();
        DF.sceneControler = this;
        director.setFPS(60);
        director.currentScenceController = this;
        director.currentScenceController.LoadResources();
    }
    void Start()
    {
        round = 1;
    }
    public void LoadResources()
    {
        explosion = Instantiate(Resources.Load("prefabs/Explosion"), new Vector3(0, 0, -20), Quaternion.identity) as GameObject;
        plane = Instantiate(Resources.Load("prefabs/Plane"), new Vector3(0, 0, 2), Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        lost = num - score;
        Score.text = "Score:" + score.ToString();
        Round.text = "Round:" + round.ToString();
        Lost.text = "Lost:" + lost.ToString();
        if (Input.GetMouseButtonDown(0) && game == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Disk")
                {
                    explosion.transform.position = hit.collider.gameObject.transform.position;
                    explosion.GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
                    explosion.GetComponent<ParticleSystem>().Play();
                    hit.collider.gameObject.SetActive(false);
                    score += 1;
                }
            }
        }
        if (game == 2)
        {
            GameOver();
        }
        if (game == 4)
        {
            Win();
        }
    }
    public IEnumerator waitForOneSecond()
    {
        while (StartTimes >= 0 && game == 3)
        {
            Time.text = StartTimes.ToString();
            yield return new WaitForSeconds(1);
            StartTimes--;
        }
        Time.text = "";
        game = 1;
    }
    public void GameOver()
    {
        Text.text = "Game Over!";
    }
    public void Win()
    {
        Text.text = "You   Win!";
    }
    public void StartGame()
    {
        num = 0;
        if (game == 0)
        {
            game = 3;//进入倒计时状态
            StartCoroutine(waitForOneSecond());
        }
    }
    public void ReStart()
    {
        SceneManager.LoadScene("task1");
    }
    public void ShowDetail()
    {
        GUI.Label(new Rect(220, 50, 350, 250), "每20分通过一关，总有三关,每一关超过15个未击中，便失败");
    }
}

