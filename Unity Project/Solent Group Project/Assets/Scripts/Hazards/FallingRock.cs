using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float lifeDuration = 5f;

    public GameObject rockRubble1;
    public GameObject rockRubble2;

    public int minNumberOfRubbles;
    public int maxNumberOfRubbles;

    public float rubbleLifeDuration;
    Transform currentTransform;

    void Start()
    {
        Destroy(this.gameObject, lifeDuration);
    }
    private void Update()
    {
        currentTransform = this.gameObject.transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.TakeDamage(damage);
        }

        // So that rocks break when touching the ground,
        // you will need to remove this when doing animations in the future
        if(collision.gameObject.tag == "RockCatcher")
        {
            int numberOfRubbles = Random.Range(minNumberOfRubbles, maxNumberOfRubbles + 1);
            if (numberOfRubbles <= 0) return;
            for (int i = 0; i < numberOfRubbles ; i++)
            {
                float rockYOffset = UnityEngine.Random.Range(-1f, 1f);
                float rockXOffset = UnityEngine.Random.Range(-0.8f, 0.2f); // Spawn boulder with this Offset on top of players x axis.
                Vector3 boulderRotation = new Vector3(0, 0, Random.Range(0, 360));
                int j = Random.Range(0, 2);
                if (j == 0)
                {
                    var rubble = Instantiate(rockRubble1, transform.position + new Vector3(rockXOffset, rockYOffset),
                        Quaternion.Euler(boulderRotation));
                    Destroy(rubble, rubbleLifeDuration);
                }
                else
                {
                    var rubble = Instantiate(rockRubble2 , transform.position + new Vector3(rockXOffset, rockYOffset),
                        Quaternion.Euler(boulderRotation));
                    Destroy(rubble, rubbleLifeDuration);
                }
            }
            Destroy(gameObject);
        }
    }
}
