using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject followTarget;
    public float offset = -10f;
    public bool culltest; //Temp

    void Awake()
    {
        followTarget = FindObjectOfType<Player>().gameObject;
    }

    void Update()
    {
        if (followTarget != null)
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, offset);
        if(culltest)
        {
            ToggleInvsibleLayer();
            culltest = false;
        }
    }

    public void ToggleInvsibleLayer()
    {
        GetComponent<Camera>().cullingMask += 1 << 10;
    }
    
}
