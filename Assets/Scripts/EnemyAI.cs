using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    // Cone info
    //public float radius;

    [Range(0, 360)]
    [SerializeField] public float angle;

    public GameObject playerRef;
    public GameObject playerLife;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public LayerMask collisionLayers;

    public bool canSeePlayer;

    // Player dependent
    [SerializeField] private float probCorrect;
    [SerializeField] private float baseProb;
    [SerializeField] private float rangeDetect;
    [SerializeField] public float rangeCone;
    [SerializeField] public float time;

    // Werewolf stats
    [SerializeField] private float _speed;
    [SerializeField] private float baseSpeed;
    [SerializeField] private int _dmg = 0;
    [SerializeField] private GameObject _enemy;
    private EnemyHP healthPoints;

    private Rigidbody enemyBody;
    private Vector2 walkingDirection;
    public float changeDirectionInterval = 3f;
    private Coroutine moveCoroutine;
    private bool isVelocityStopped = false;

    public enum State
    {
        Neutral,
        Searching,
        Aggro,
        Allied,
        Stunned
    }

    public State CurrentState { get; set; }



    public void ChangeState(State newState)
    {
        CurrentState = newState;
    }

    public void ChangeHuntingParameters(float probability, float detection, float cone, float stunTime)
    {
        probCorrect = probability;
        rangeDetect = detection;
        rangeCone = cone;
        time = stunTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        probCorrect = baseProb;
        _speed = baseSpeed;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerLife = GameObject.Find("Player");
        StartCoroutine(FOVRoutine());


        enemyBody = _enemy.GetComponent<Rigidbody>();
        walkingDirection = Vector2.zero;
        moveCoroutine = StartCoroutine(ChangeDirectionRoutine());

        healthPoints = GetComponent<EnemyHP>();
    }

    private void OnDestroy()
    {
        // Stop the coroutine when the GameObject is destroyed
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }

    #region GizmoWork
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawSphere(transform.position, radius);
        //Debug.Log("IN");

        Gizmos.color = Color.red;
        //Gizmos.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);

        //Cone range
        Gizmos.DrawWireSphere(transform.position, rangeCone);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * rangeCone);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * rangeCone);

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }

        if (CurrentState == State.Neutral)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.magenta;
        }
        Gizmos.DrawWireSphere(transform.position, rangeDetect);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, rangeCone, targetMask);
        Collider[] nearChecks = Physics.OverlapSphere(transform.position, rangeDetect, targetMask);


        #region Cone checks
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    if(CurrentState != State.Stunned)
                    {
                        ChangeState(State.Aggro);
                    }
                    
                }
                else
                {
                    canSeePlayer = false;
                }

            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
        #endregion

        #region NearChecks
        if (nearChecks.Length != 0 && rangeChecks.Length == 0 && CurrentState!= State.Stunned)
        {
            ChangeState(State.Searching);
        } else if(nearChecks.Length == 0)
        {
            ChangeState(State.Neutral);
        }

        #endregion

    }

    private IEnumerator ChangeDirectionRoutine()
    {
        Debug.Log("Started this thing");
        while (true)
        {
            if (CurrentState != State.Aggro)
            {
                _speed = baseSpeed;
                if (CurrentState == State.Searching)
                {
                    probCorrect =+ 0.3f;
                }
                else
                {
                    probCorrect = baseProb;
                }

                if (Random.value < probCorrect)
                {
                    FacePlayer();
                }
                else
                {
                    MoveInRandomDirection();
                }
            }else if (CurrentState == State.Aggro)
            {
                _speed += 5f;
                FacePlayer();
            } else if(CurrentState == State.Stunned) {
                _speed = 0;
            }


            //Console.Log(CurrentState);
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }

    private void FacePlayer()
    {
        transform.LookAt(playerRef.transform);
        enemyBody.velocity = transform.forward * _speed;
    }

    private void MoveInRandomDirection()
    {
        // Generate a random direction
        Vector3 randomDirection = Random.insideUnitSphere.normalized;

        // Ignore the y-axis to move only on the X-Z plane
        randomDirection.y = 0f;
        transform.LookAt(randomDirection);

        // Apply movement in the random direction
        enemyBody.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is in the specified collision layers
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            Debug.Log("We had a collision");
            if (collision.gameObject == playerLife)
            {
                AttackAttempt(_dmg);
                StopAndResume(time);

            }
            else
            {
                // Change direction to opposite
                Vector3 newDirection = -enemyBody.velocity;
                newDirection.y = 0f;
                transform.LookAt(newDirection.normalized);
                enemyBody.velocity = transform.forward * _speed;
            }
            
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoints.TakeDamage(damage);
        StopAndResume(time + 1f);
    }

    public void Heal(int amount)
    {
        healthPoints.Heal(amount);
    }

    private IEnumerator StopVelocityForDelay(float delay)
    {
        Debug.Log("Stopping");
        ChangeState(State.Stunned);
        Vector3 newDirection = -enemyBody.velocity;
        enemyBody.velocity = newDirection * _speed/10;
        yield return new WaitForSeconds(0.3f);

        enemyBody.velocity = Vector3.zero;

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Restore the original velocity
        ChangeState(State.Aggro);
        isVelocityStopped = false;
    }

    // Example usage: Call this method to stop and resume velocity
    public void StopAndResume(float stopDuration)
    {
        if (!isVelocityStopped)
        {
            StartCoroutine(StopVelocityForDelay(stopDuration));
        }
    }


    public void AttackAttempt(int damage)
    {
        Debug.Log("Attacking the player");
        playerLife.GetComponent<HealthPoints>().TakeDamage(damage);
        
    }

}
