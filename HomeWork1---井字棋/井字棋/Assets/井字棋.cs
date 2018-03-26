using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 井字棋 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("游戏开始！");
        Reset();
	}

    // Update is called once per frame
    float bu_width = 60;
    float bu_height = 60;
    float height = Screen.height * 0.5f - 200;
    float width = Screen.width * 0.5f - 200;
    private int[,] chess = new int[3, 3];//0为空，1为O，2为X
    private bool turn = true;//true为O,false为X
    int num = 0;

    private void Reset()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int k = 0; k < 3; k++)
            {
                chess[i, k] = 0;
            }
        }
        turn = true;
        num = 0;
    }

    private int Check()
    {
        // 横向连线    
        for (int i = 0; i < 3; ++i)
        {
            if (chess[i, 0] != 0 && chess[i, 0] == chess[i, 1] && chess[i, 1] == chess[i, 2])
            {
                return chess[i, 0];
            }
        }
        //纵向连线    
        for (int j = 0; j < 3; ++j)
        {
            if (chess[0, j] != 0 && chess[0, j] == chess[1, j] && chess[1, j] == chess[2, j])
            {
                return chess[0, j];
            }
        }
        //斜向连线    
        if (chess[1, 1] != 0 &&
            chess[0, 0] == chess[1, 1] && chess[1, 1] == chess[2, 2] ||
            chess[0, 2] == chess[1, 1] && chess[1, 1] == chess[2, 0])
        {
            return chess[1, 1];
        }
        return 0;
    }

    private void OnGUI()
    {
        GUIStyle title = new GUIStyle();
        GUIStyle msg = new GUIStyle();
        title.fontSize = 50;
        title.fontStyle = FontStyle.Bold;
        msg.fontSize = 50;
        msg.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(width+125, 0, 2 * bu_width, bu_height), "井字棋",title);
        
        if(GUI.Button(new Rect(width + 150, 380, 100, 50), "重新开始"))
        {
            Reset();
            return;
        }
        int result = Check();
        //Debug.Log(result);
        if (result == 1)
        {
            GUI.Label(new Rect(width + 150, 80, 100, 50), "O赢!", msg);
        }
        else if (result == 2)
        {
            GUI.Label(new Rect(width + 150, 80, 100, 50), "X赢!", msg);
        }
        else if (num == 9)
        {
            GUI.Label(new Rect(width + 150, 80, 100, 50), "平局!", msg);
        }
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                if(chess[i,k] == 1)
                {
                    GUI.Button(new Rect(width + 110 + i * bu_width, height+110 + k * bu_height, bu_width, bu_height), "O");
                }
                else if(chess[i, k] == 2)
                {
                    GUI.Button(new Rect(width + 110 + i * bu_width, height+110 + k * bu_height, bu_width, bu_height), "X");
                }
                else
                {
                    if(GUI.Button(new Rect(width + 110 + i * bu_width, height +110+ k * bu_height, bu_width, bu_height), ""))
                    {
                        if (result == 0)
                        {
                            if (turn == true)
                            {
                                chess[i, k] = 1;
                                turn = false;
                                num++;
                            }
                            else
                            {
                                chess[i, k] = 2;
                                turn = true;
                                num++;
                            }
                        }
                    }
                }
            }
        }
    }
}
