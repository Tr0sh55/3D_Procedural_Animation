﻿using System.Collections;
using UnityEngine;

public class SpiderMovementController : MonoBehaviour
{
    public Transform[] legPoints;
    [SerializeField] private float maxDist = 0.5f;
    [SerializeField] public float speed = 5.0f;
    [SerializeField] public float stepDuration = 0.5f;
    [SerializeField] private float stepHeight = 0.2f;

    private Vector3[] nextStepPositions;
    private float[] stepStartTime;
    private bool isMovingSetA = true;
    private float someMinimumHeightAboveGround = 2.5f;

    void Start()
    {
        nextStepPositions = new Vector3[legPoints.Length];
        stepStartTime = new float[legPoints.Length];
        for (int i = 0; i < legPoints.Length; i++)
        {
            nextStepPositions[i] = legPoints[i].position;
            stepStartTime[i] = Time.time;
        }
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal);
        transform.Translate(movement, Space.World);

        MoveLegs(movement);
    }

    void MoveLegs(Vector3 movement)
    {
        CheckAndSwitchMovingSets();
    
        // Update the next step position with the body movement
        for (int i = 0; i < legPoints.Length; i++)
        {
            nextStepPositions[i] += movement;

            Vector3 raycastStartPosition = nextStepPositions[i] + (Vector3.up * 0.5f);
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(raycastStartPosition, Vector3.down * hit.distance, Color.red);
                nextStepPositions[i] = hit.point;
            }
        }

        for (int i = 0; i < legPoints.Length; i++)
        {
            // Determine if the current leg is part of the set that should be moving
            bool isCurrentLegMovingSet = (i % 2 == 0) ? isMovingSetA : !isMovingSetA;

            if (movement != Vector3.zero)
            {
                if (isCurrentLegMovingSet)
                {
                    float progress = (Time.time - stepStartTime[i]) / stepDuration;
                    if (progress < 1.0f)
                    {
                        Vector3 stepPositionWithHeight = nextStepPositions[i] + Vector3.up * (stepHeight * Mathf.Sin(progress * Mathf.PI)); // Add height for stepping effect
                        legPoints[i].position = Vector3.Lerp(legPoints[i].position, stepPositionWithHeight, progress);
                    }
                    else
                    {
                        stepStartTime[i] = Time.time;
                    }
                }
            }
        }

        AdjustBodyPositionAndRotation();
    }
    
    void AdjustBodyPositionAndRotation()
    {
        Vector3 averageLegPosition = Vector3.zero;
        for (int i = 0; i < legPoints.Length; i++)
        {
            averageLegPosition += legPoints[i].position;
        }
        averageLegPosition /= legPoints.Length;

        // Adjust the body's height based on the average position of the legs plus an offset for visual consistency
        float newHeight = averageLegPosition.y + someMinimumHeightAboveGround;
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);

        // Optionally, adjust the body's rotation based on the terrain's slope or leg positions
        // This part is more complex and might require additional logic to smoothly handle rotations
    }
    
    void CheckAndSwitchMovingSets()
    {
        bool allLegsInSetHaveFinishedMoving = true;
        for (int i = 0; i < nextStepPositions.Length; i++)
        {
            bool isCurrentLegMovingSet = (i % 2 == 0) ? isMovingSetA : !isMovingSetA;
            if (isCurrentLegMovingSet)
            {
                float progress = (Time.time - stepStartTime[i]) / stepDuration;
                if (progress < 1.0f)
                {
                    allLegsInSetHaveFinishedMoving = false;
                    break;
                }
            }
        }

        if (allLegsInSetHaveFinishedMoving)
        {
            isMovingSetA = !isMovingSetA;
        }
    }
    
    void OnDrawGizmos()
    {
        if(nextStepPositions != null)
        {
            for (int i = 0; i < nextStepPositions.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(nextStepPositions[i], 0.1f);
            }
        }
    }
}