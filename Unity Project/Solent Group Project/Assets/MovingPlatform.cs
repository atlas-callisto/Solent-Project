using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public bool moveTowardspointB = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = pointA.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMovement();
    }

    private void PlatformMovement()
    {
        if (transform.position == pointA.transform.position)
        {
            moveTowardspointB = true;            
        }
        if (transform.position == pointB.transform.position)
        {
            moveTowardspointB = false;
        }

        if (moveTowardspointB)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(pointA.position, pointB.position);
    }
}
