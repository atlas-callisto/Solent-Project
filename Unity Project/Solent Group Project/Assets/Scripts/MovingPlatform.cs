using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public bool isEnabled = false;

    private bool moveTowardspointB = true;
    private SpriteRenderer mySpriteRenderer;
    void Start()
    {
        transform.position = pointA.transform.position;
        this.gameObject.SetActive(GameManager.myGameManager.moonsEyeMonacle);
    }

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
