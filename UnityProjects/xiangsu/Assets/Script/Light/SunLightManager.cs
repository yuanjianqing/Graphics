using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SunLightManager : MonoBehaviour
{

    Transform trans;
    private void Awake()
    {
        trans = GetComponent<Transform>();
    }

    public void SetTime(int time)
    {
#pragma warning disable CS0618 // 类型或成员已过时
        switch (time)
        {
            case 0:
                trans.rotation = Quaternion.EulerAngles(20, 20, 0);
                break;
            case 1:
                trans.rotation = Quaternion.EulerAngles(-5, -3, 0);
                break;
            case 2:
                trans.rotation = Quaternion.EulerAngles(-25, 35, 0);
                break;
            case 3:
                trans.rotation = Quaternion.EulerAngles(-85, 35, 0);
                break;
            case 4:
                trans.rotation = Quaternion.EulerAngles(-90, 0, 0);
                break;
            default:
                print("Incorrect.");
                break;
        }
#pragma warning restore CS0618 // 类型或成员已过时
    }

}
