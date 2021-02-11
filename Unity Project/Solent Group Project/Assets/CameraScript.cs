using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject followTarget;
    public float offset = -10f;
    // Start is called before the first frame update
    void Awake()
    {
        followTarget = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, offset);
    }
}
