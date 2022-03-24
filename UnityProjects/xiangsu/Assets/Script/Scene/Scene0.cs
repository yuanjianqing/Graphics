using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene0 : MonoBehaviour
{
    [Header("����Ԥ����")]
    public GameObject rubbishPrefab;

    [Header("�������ֵĵ������"),SerializeField]
    private Transform[] point;

    [Header("�������������б�")]
    public RubbishList list;

    //����������������
    private int capaticy = 16;
    //�������
    private int childCount;

    [Header("ͬ�������������")]
    public int maxScreenNum;

    //ͬ�����嵱ǰ�ĸ���
    public static int nowScreenNum = 0;

    //�Ѿ������������
    public static int  totalNum = 0;


    static int rubbishIndex = 0;

    [Header("���������ʱ����")]
    public float deltaTime = 0.8f;

    private float lastTime;


    private void Awake()
    {
        //�����������
        childCount = transform.childCount;

        point = new Transform[childCount];

        //��ȡ�����������
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
        //ÿ��deltiTime������һ���������壬����list�������˳��
        if (nowScreenNum < maxScreenNum && (Time.time -lastTime) > deltaTime && totalNum < capaticy)
        {

            lastTime = Time.time;
            nowScreenNum++;
            int index = Random.Range(0, childCount);
            var newRubbish = Instantiate(rubbishPrefab, point[index].position, Quaternion.Euler(0, 0, 0), transform);

            //�ٶ��������ɸ����ӿ�
            newRubbish.gameObject.GetComponent<SpinningAndBoucing>().speed += totalNum / 5f;
            newRubbish.GetComponent<SpriteRenderer>().sprite = list.rubbishList[rubbishIndex].image;
            rubbishIndex = (rubbishIndex + 1) % 16;
        }
    }
}
