using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene0 : MonoBehaviour
{
    [Header("垃圾预制体")]
    public GameObject rubbishPrefab;

    [Header("垃圾出现的点的坐标"),SerializeField]
    private Transform[] point;

    [Header("“垃圾”物体列表")]
    public RubbishList list;

    //生成物体的最大数量
    private int capaticy = 16;
    //点的数量
    private int childCount;

    [Header("同屏物体的最大个数")]
    public int maxScreenNum;

    //同屏物体当前的个数
    public static int nowScreenNum = 0;

    //已经消灭物体个数
    public static int  totalNum = 0;


    static int rubbishIndex = 0;

    [Header("生成物体的时间间隔")]
    public float deltaTime = 0.8f;

    private float lastTime;


    private void Awake()
    {
        //子物体的数量
        childCount = transform.childCount;

        point = new Transform[childCount];

        //获取子物体的坐标
        for(int i = 0; i < childCount; i++)
        {
            point[i] = transform.GetChild(i);
           // Debug.Log(i);
        }
    }
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //每隔deltiTime秒生成一个垃圾物体，按照list的数组的顺序
        if (nowScreenNum < maxScreenNum && (Time.time -lastTime) > deltaTime && totalNum < capaticy)
        {

            lastTime = Time.time;
            nowScreenNum++;
            int index = Random.Range(0, childCount);
            var newRubbish = Instantiate(rubbishPrefab, point[index].position, Quaternion.Euler(0, 0, 0), transform);

            //速度随着生成个数加快
            newRubbish.gameObject.GetComponent<SpinningAndBoucing>().speed += totalNum / 5f;
            newRubbish.GetComponent<SpriteRenderer>().sprite = list.rubbishList[rubbishIndex].image;
            rubbishIndex = (rubbishIndex + 1) % 16;
        }
    }
}
