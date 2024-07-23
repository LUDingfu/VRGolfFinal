using UnityEngine;
using Random = UnityEngine.Random;

public class GolfBallController : MonoBehaviour
{
    [SerializeField] private float speedFactor = 1.0f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private Transform holeTransform;
    [SerializeField] private DataSaver dataSaver;
    [SerializeField] private Renderer ballRenderer;
    
    [Header("Probabilities")]
    [Tooltip("Chance of ball being sunk into the hole.")]
    [Range(0,100)][SerializeField] private int correctPercentage;
    [Tooltip("If miss, chance of ball to have a wrong angle. " +
             "The lower this chance, the more likely it will overshoot or undershoot")]
    [Range(0,100)] [SerializeField] private int wrongAngleChance;
    [Tooltip("Ratio of the ball to undershoot or overshoot. " +
             "If it misses, but the angle is correct, " +
             "this determines if it overshoot or undershoots")]
    [Range(0,100)] [SerializeField] private int undershootToOvershootRatio;
    
    [Header("Parameters")]   
    [SerializeField] private float undershootDistance;
    [SerializeField] private float overshootDistance;
    [SerializeField] private int wrongAngleDistance;
    [SerializeField] private ShotResult currentShotResult;
    [SerializeField] private float fireDelay = 10.0f; 
    [SerializeField] private float resetDelay = 10.0f;

    private new Rigidbody rigidbody;
    private bool hasFired;
    private bool directionFromGolfToHoleLock;
    private Vector3 targetDirectionNormalized;
    private bool wrongAngleLock;
    private Vector3 wrongAngleVector;
    private Vector3 wrongAngleVectorNormalised;
    private Collider col;
    private Vector3 startPosition;
    private float timer;
    private float speedToStop=0.01f;
    public float actualSpeed { get; private set; }

    private bool hasFallen;

    private enum ShotResult { IntoHole, WrongAngle, Undershoot, Overshoot }
    public bool holeDetectionTrigger;
    
    void Start()
    {
        startPosition = transform.position;
        targetDirectionNormalized = Vector3.zero;
        wrongAngleVector = Vector3.zero;
        rigidbody = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        ComputeShotType();
        // Debug.Log(currentShotResult);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireDelay && !hasFired)
        {
            hasFired = true;
            timer = 0.0f;
        }   
        MoveBall(); 
    }

    public void Reset()
    {
        hasFired = false;
        transform.position = startPosition;
        rigidbody.velocity = Vector3.zero; 
        hasFallen = false;
        directionFromGolfToHoleLock = false;
        wrongAngleLock = false;
        col.isTrigger = false; 
    }

    public void Fall(bool state)
    {
        col.isTrigger = state;
        rigidbody.velocity += Vector3.down * gravity;
        hasFallen = true;
        // start timer to reset ball position
        timer += Time.deltaTime;
        if (timer >= resetDelay)
        {
            Reset();
            timer = 0.0f;
        }   
    }

    private void MoveBall()
    {
        if (!hasFired || hasFallen) return;
        Vector3 targetDirectionVector = (holeTransform.position - transform.position);
        float targetDirectionVectorMagnitude = targetDirectionVector.magnitude;
        if (!directionFromGolfToHoleLock)
        {
            targetDirectionNormalized = targetDirectionVector.normalized;
            directionFromGolfToHoleLock = true;
        }
        
        if (!wrongAngleLock)
        { 
            Vector3 randomAngle = GiveWrongAngle();
            wrongAngleVector = holeTransform.position + randomAngle * wrongAngleDistance;
            wrongAngleVectorNormalised = (wrongAngleVector - transform.position).normalized;
            wrongAngleLock = true;
        }

        switch (currentShotResult)
        {
            case ShotResult.IntoHole:
                holeDetectionTrigger = true;
                SetActualSpeed(targetDirectionNormalized, targetDirectionVectorMagnitude, speedFactor);
                if (rigidbody.velocity.magnitude < speedToStop) speedFactor = 0;
                break;
            
            case ShotResult.WrongAngle:
                float targetSpeed = (wrongAngleVector - transform.position).magnitude;
                SetActualSpeed(wrongAngleVectorNormalised, targetSpeed, speedFactor);
                break;
            
            case ShotResult.Undershoot:
                float actualDistanceUndershoot = targetDirectionVectorMagnitude - undershootDistance;
                SetActualSpeed(targetDirectionNormalized,actualDistanceUndershoot,speedFactor);
                break;
            
            case ShotResult.Overshoot:
                float actualDistanceOvershoot = (targetDirectionVector + overshootDistance * targetDirectionNormalized).magnitude;
                SetActualSpeed(targetDirectionNormalized,actualDistanceOvershoot,speedFactor);
                break;
            
        }
    }

    private void ComputeShotType()
    {
        int roll = Random.Range(0, 100);
        if (roll < correctPercentage) currentShotResult = ShotResult.IntoHole;
        else
        {
            roll = Random.Range(0, 100);
            if (roll < wrongAngleChance)
            {
                currentShotResult = ShotResult.WrongAngle;
            }
            else
            {
                roll = Random.Range(0, 100);
                currentShotResult = roll < undershootToOvershootRatio ? ShotResult.Overshoot : ShotResult.Undershoot;
            }
        }
    }

    private Vector3 GiveWrongAngle()
    {
        float xCheck = -1;
        Vector3 direction = Vector3.zero;
        while (xCheck < 0.5f)
        {
            direction = Random.insideUnitSphere;
            xCheck = direction.x;
        }
        direction = new Vector3(direction.x, 0f, direction.z).normalized;
        return direction;
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

