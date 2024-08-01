using UnityEngine;
using Random = UnityEngine.Random;

public class GolfBallController : MonoBehaviour
{
    [SerializeField] private float speedFactor = 1.0f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private Hole hole; 
    [SerializeField] private DataSaver dataSaver;
    [SerializeField] private Renderer ballRenderer;
    
    [Header("Feedback Groups")]
    [SerializeField] private int feedbackGroup; // 1 = Perfect, 2 = Random, 3 = Adaptive
    [SerializeField] private float possibility; // Used for Group 2 and Group 3
    
    [Header("Parameters")]   
    private new Rigidbody rigidbody;
    private bool hasFired;
    private Vector3 targetDirectionNormalized;
    private Collider collider;
    private Vector3 startPosition;
    private float speedToStop=0.01f;
    public float actualSpeed { get; private set; }
    private bool hasFallen;
    public bool holeDetectionTrigger;
    
    void Start()
    {
        startPosition = transform.position;
        targetDirectionNormalized = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!hasFired)  
        {
            hasFired = true;
            MoveBall(); 
        }   
    }

    public void Reset()
    {
        hasFired = false;
        transform.position = startPosition;
        rigidbody.velocity = Vector3.zero; 
        hasFallen = false;
        collider.isTrigger = false; 
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

        Vector3 targetPosition = Vector3.zero;
            
        switch (feedbackGroup)
        {
            case 1: // Perfect group
                targetPosition = hole.GetPerfectCoordinate();
                holeDetectionTrigger = true;
                if (rigidbody.velocity.magnitude < speedToStop) speedFactor = 0;
                break;
            case 2: // Random feedback group
                targetPosition = GetRandomCoordinate();
                if (targetPosition.x==0&&targetPosition.y==0&&targetPosition.z==0)
                {
                    holeDetectionTrigger = true;
                    if (rigidbody.velocity.magnitude < speedToStop) speedFactor = 0;
                }
                break;
            case 3: // Adaptive feedback group 
                targetPosition = GetAdaptiveCoordinate();
                break;
        }
        
        Vector3 targetDirectionVector = (targetPosition - transform.position);
        float targetDirectionVectorMagnitude = targetDirectionVector.magnitude;
        Vector3 targetDirectionNormalized = targetDirectionVector.normalized;
        SetActualSpeed(targetDirectionNormalized, targetDirectionVectorMagnitude, speedFactor);
    }

    private Vector3 GetRandomCoordinate()
    {
        Hole.Coordinate randomCoordinate = hole.GetRandomCoordinate(possibility);
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
}
