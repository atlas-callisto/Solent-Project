using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    // This script was just taken from one of my solo prototypes.
    // Sorry it's a pile of s**t.

    public float ScrollSpeed;

    public int RespawnOffset;
    public int DespawnOffset;

    private Vector3 SpawnPosition;

    public GameObject BackgroundGameObject;

    private void Start()
    {
        // Spawn position of scrolling background
        SpawnPosition = new Vector3(RespawnOffset, transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(-ScrollSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x < DespawnOffset)
        {
            Instantiate(BackgroundGameObject).transform.SetPositionAndRotation(SpawnPosition, Quaternion.Euler(0, 0, 0));

            //Expire
            Destroy(gameObject);
        }
    }
}
