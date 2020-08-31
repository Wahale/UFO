using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public Transform obj;
    public float speed;
    public bool kuda;

    private Transform targetPoint;
    private int currentPoint;

    void Start()
    {
        currentPoint = 0;
        targetPoint = points[currentPoint];
    }

    
    void Update()
    {
        if (obj.position == targetPoint.position)
        {
            currentPoint++;

            //if (currentPoint >= points.Length)
            //    currentPoint--;

            if (currentPoint >= points.Length)
                currentPoint = 0;

            targetPoint = points[currentPoint];
        }

        obj.position = Vector3.MoveTowards(obj.position, targetPoint.position, speed * Time.deltaTime);
    }
}
