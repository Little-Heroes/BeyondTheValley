using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBomb : MonoBehaviour
{

    bool hasLanded;
    public Vector3 landPosition;
    bool positionIsCool = false;
    int index = 0;
    public float radius;
    public float appleRadius;
    public float speed;
    public Vector3 start;
    public float startTime;

    // Use this for initialization
    void Start()
    {
        while (!positionIsCool)
        {
            index++;
            Vector3 position = radius * Random.insideUnitCircle;
            if (Physics2D.OverlapCircle(position, appleRadius))
            {
                continue;
            }
            else
            {
                landPosition = position;
                positionIsCool = true;
            }


            if (index >= 100)
            {
                positionIsCool = true;
            }
        }
        start = transform.position;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLanded)
        {
            float fracComplete = (Time.time - startTime) / speed;
            transform.position = Vector3.Slerp(start, landPosition, fracComplete);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
