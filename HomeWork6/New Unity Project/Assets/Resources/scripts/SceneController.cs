using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, ISceneController, IUserAction
{
    public GameObject role;
    public Text FinalText;
    public Text GameText;
    public int game = 0;
    public ScoreRecorder scoreRecorder;
    int CoolTimes = 3;
    void Awake()
    //创建导演实例并载入资源  
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentScenceController = this;
        director.currentScenceController.LoadResources();
    }
    public void LoadResources()  //载入资源  
    {
        role = Instantiate(Resources.Load("Prefabs/role")) as GameObject;
        if(role == null)
        {
            Debug.Log("null");
        }
        Instantiate(Resources.Load("Prefabs/start"), new Vector3(2.5f, 0, 2f), Quaternion.identity);
        for(int i = 0; i < 3; i++)
        {
            Instantiate(Resources.Load("Prefabs/maze"), new Vector3(-4 * i, 0, 0), Quaternion.identity);
            Instantiate(Resources.Load("Prefabs/maze"), new Vector3(-4 * i, 0, 4), Quaternion.identity);
            Instantiate(Resources.Load("Prefabs/Zombie"), new Vector3(-1.6f - 4 * i, 0.5f,-1.6f), Quaternion.identity);
            Instantiate(Resources.Load("Prefabs/Zombie"), new Vector3(-1.6f - 4 * i, 0.5f, -1.6f + 4 ), Quaternion.identity);
        }
    }
    // Use this for initialization
    void Start()
    {
        game = 2;
        RoleTrigger.gameOver += GameOver;
        StartCoroutine(waitForOneSecond());
    }

    // Update is called once per frame
    void Update()
    {
        if (role.transform.position.y < -10)
        {
            GameOver();
        }
    }
    void GameOver()
    {
        game = 0;
        FinalText.text = "Game Over!!!";
    }
    public void ShowDetail()
    {
        GUIStyle fontstyle1 = new GUIStyle();
        fontstyle1.fontSize = 20;
        fontstyle1.normal.textColor = new Color(255, 255, 255);
        GUI.Label(new Rect(220, 50, 500, 500), "摆脱丧尸即可得分，被触碰游戏结束！", fontstyle1);
    }
    public void ReStart()
    {
        SceneManager.LoadScene("Task1");
    }
    public IEnumerator waitForOneSecond()
    {
        while (CoolTimes >= 0 && game == 2)
        {
            GameText.text = CoolTimes.ToString();
            print("还剩" + CoolTimes);
            yield return new WaitForSeconds(1);
            CoolTimes--;
        }
        GameText.text = "";
        game = 1;//游戏开始  
    }
}
