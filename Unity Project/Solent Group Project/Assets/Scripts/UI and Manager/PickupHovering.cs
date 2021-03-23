using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHovering : MonoBehaviour
{
    public float VerticalMovementSpeed;

    private float TravelledDistance;
    public float MaxTravelDistance;

    public bool IsReturning;

    private void Start()
    {
        // Sets default values incase people forget to assign them in the inspector
        if(VerticalMovementSpeed == 0)
        {
            VerticalMovementSpeed = 0.5f;
        }
        if(MaxTravelDistance == 0)
        {
            MaxTravelDistance = 35;
        }
    }

    void Update()
    {
        // Up and down wobbling
        if (TravelledDistance <= MaxTravelDistance && !IsReturning)
        {
            transform.Translate(0, VerticalMovementSpeed * Time.deltaTime, 0);
            TravelledDistance += VerticalMovementSpeed;
        }
        else if (TravelledDistance >= MaxTravelDistance)
        {
            IsReturning = true;
        }
        if (IsReturning)
        {
            transform.Translate(0, -VerticalMovementSpeed * Time.deltaTime, 0);
            TravelledDistance += -VerticalMovementSpeed;

            if (TravelledDistance <= -MaxTravelDistance)
            {
                IsReturning = false;
            }
        }
    }
}
