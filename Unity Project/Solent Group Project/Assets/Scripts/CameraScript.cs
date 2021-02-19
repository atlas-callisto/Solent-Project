using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject followTarget;
    private float offset = -10f;

    void Awake()
    {
        followTarget = FindObjectOfType<Player>().gameObject;
    }

    void Update()
    {
        if (followTarget != null)
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, offset);        
    }

    public void ToggleInvsibleLayer()
    {
        Debug.Log("yo");
        GetComponent<Camera>().cullingMask += 1 << 10;
    }    
}
