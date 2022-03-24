using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followedTransform;
    private Transform leftEdge, rightEdge;
    private float screenWidth;

    private void Awake()
    {
        leftEdge = transform.GetChild(0);
        rightEdge = transform.GetChild(1);
        screenWidth = rightEdge.position.x - leftEdge.position.x - 0.1f;
    }



    void Update()
    {
        if (followedTransform.position.x < leftEdge.position.x)
        {
            transform.Translate(-screenWidth, 0, 0);
        }
        if(followedTransform.position.x > rightEdge.position.x)
        {
            transform.Translate(screenWidth, 0, 0);
        }
    }
}
