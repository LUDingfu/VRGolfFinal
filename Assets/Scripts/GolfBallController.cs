using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GolfBallController : MonoBehaviour
{
    [SerializeField] private float speedFactor = 1.0f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private Hole hole; 
    [SerializeField] private DataSaver dataSaver;
    [SerializeField] private Renderer ballRenderer;
    [SerializeField, Tooltip("Set this to the y value of the golf ball and hole")] 
    private float floorHeight = 0f;
    
    
    [Header("Feedback Groups")]
    [SerializeField] private int feedbackGroup; // 1 = Perfect, 2 = Random, 3 = Adaptive
    [SerializeField] private float possibility; // Used for Group 2 and Group 3
    
    [Header("Parameters")]   
    private new Rigidbody rigidbody;
    private bool hasFired;
    private bool hasSelectedRandomTarget;
    private Vector3 targetDirectionNormalized;
    private Collider collider;
    private Vector3 startPosition;
    private float speedToStop=0.01f;
    private Vector3 targetPosition;
    private int currIdx = 0;
    public float actualSpeed { get; private set; }
    [SerializeField]private bool hasFallen;
    public bool holeDetectionTrigger;
    
    void Start()
    {
        startPosition = transform.position;
        targetDirectionNormalized = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        targetPosition = GetTargetPosition();
    }

    void Update()
    {
        if (!hasFired)  
        {
            hasFired = true;
        }   
        MoveBall(); 
    }

    public void Reset()
    {
        hasFired = false;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        rigidbody.velocity = Vector3.zero; 
        hasFallen = false;
        collider.isTrigger = false; 
        targetPosition = GetTargetPosition();
    }

    private void PrepareNextTrial()
    {
        Vector2 currCoordinate = hole.FinalList[currIdx];
        targetPosition = new Vector3(currCoordinate.x, floorHeight, currCoordinate.y);
        currIdx++;
        if (targetPosition.x == hole.transform.position.x && targetPosition.z == hole.transform.position.z)
        {
            holeDetectionTrigger = true;
        }
    }

    public void Fall(bool state)
    {
        collider.isTrigger = state;
        rigidbody.velocity += Vector3.down * gravity;
        hasFallen = true;
    }

    private void MoveBall()
    {
        if (!hasFired || hasFallen) return;
        
        Vector3 targetDirectionVector = (targetPosition - transform.position);
        float targetDirectionVectorMagnitude = targetDirectionVector.magnitude;
        targetDirectionNormalized = targetDirectionVector.normalized;
        SetActualSpeed(targetDirectionNormalized, targetDirectionVectorMagnitude, speedFactor);
    }

    private Vector3 GetTargetPosition()
    {
        PrepareNextTrial();
        // switch (feedbackGroup)
        // {
        //     case 1: // Perfect group
        //         holeDetectionTrigger = true;
        //         break;
        //     case 2: // Random feedback group
        //         if (targetPosition == hole.transform.position)
        //         {
        //             holeDetectionTrigger = true;
        //         }
        //         break;
        //     case 3: // Adaptive feedback group 
        //         break;
        // }
        return targetPosition;
    }
    
    private Vector3 GetRandomCoordinate()
    {
        Vector2 randomCoordinate = hole.FinalList[0];
        if (randomCoordinate != null)
        {
            return new Vector3(randomCoordinate.x, transform.position.y, randomCoordinate.y);
        }
        return transform.position; // Return current position if no coordinate is chosen
    }

    private Vector3 GetAdaptiveCoordinate()
    {
        // Placeholder for future implementation
        return transform.position;
    }


    private void SetActualSpeed(Vector3 targetDirectionNormalized, float directionVectorMagnitude, float speedFactor)
    {
        actualSpeed = speedFactor * directionVectorMagnitude;
        rigidbody.velocity = targetDirectionNormalized * actualSpeed;
    }

    public void ChangeBallColor(Color newColor)
    {
        if (ballRenderer != null)
        {
            ballRenderer.material.color = newColor;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, targetDirectionNormalized);
        Gizmos.color = Color.red;
    }

    public void Refire()
    {
        Reset();
    }
    
}
