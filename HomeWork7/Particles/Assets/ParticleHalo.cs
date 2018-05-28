using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHalo : MonoBehaviour {
    private ParticleSystem system;  // 粒子系统
    private ParticleSystem.Particle[] arr;  // 粒子数组
    private Circle[] circle; // 极坐标数组
    public Gradient colorGradient;

    public int count = 10000;       // 粒子数量  
    public float size = 0.1f;      // 粒子大小  
    public float minRadius = 16f;  // 最小半径  
    public float maxRadius = 20f; // 最大半径  
    public bool clockwise = true;   // 顺时针|逆时针  
    public float speed = 2f;        // 速度  
    public float pingPong = 0.02f;  // 游离范围
    private int tier = 10;  // 速度差分层数
                            // Use this for initialization
    void Start () {
        arr = new ParticleSystem.Particle[count];
        circle = new Circle[count];

        system = this.GetComponent<ParticleSystem>();
        var main = system.main;
        main.startSpeed = 0;
        main.startSize = size;
        main.startColor = Color.white;
        main.loop = false;
        main.maxParticles = count;
        system.Emit(count);               // 发射粒子  
        system.GetParticles(arr);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.9f;
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1.0f; colorKeys[1].color = Color.white;
        colorGradient.SetKeys(colorKeys, alphaKeys);

        ParticleInit();   // 初始化各粒子位置
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < count; i++)
        {
            if (clockwise)  // 顺时针旋转  
                circle[i].angle -= (i % tier + 1) * (speed / circle[i].radius / tier);
            else            // 逆时针旋转  
                circle[i].angle += (i % tier + 1) * (speed / circle[i].radius / tier);

            // 保证angle在0~360度  
            circle[i].angle = (360.0f + circle[i].angle) % 360.0f;
            float theta = circle[i].angle / 180 * Mathf.PI;

            arr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));

            // 粒子在半径方向上游离  
            circle[i].time += Time.deltaTime;
            circle[i].radius += Mathf.PingPong(circle[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;

            arr[i].startColor = colorGradient.Evaluate(circle[i].angle / 360.0f);
        }

        system.SetParticles(arr, arr.Length);
    }

    void ParticleInit()
    {
        for (int i = 0; i < count; ++i)
        {   // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);

            // 随机每个粒子的角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;

            // 随机每个粒子的游离起始时间  
            float time = Random.Range(0.0f, 360.0f);

            circle[i] = new Circle(radius, angle, time);

            arr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
        }

        system.SetParticles(arr, arr.Length);
    }
}
