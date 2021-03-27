using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject followTarget;
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

    public void SetupFollowTarget()
    {
        followTarget = FindObjectOfType<Player>().gameObject;
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
