using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    // Cone info
    public float radius;

    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    // Player dependent
    [SerializeField] private float probCorrect;
    [SerializeField] private float  rangeDetect;
    [SerializeField] private float rangeCone;
    [SerializeField] private float timeHidden;

    // Werewolf stats
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _dmg = 0f;
    [SerializeField] private int _health = 10;

    public enum State
    {
        Neutral,
        Searching,
        Aggro,
        Allied
    }

    public State CurrentState { get; set; }



    public void ChangeState(State newState)
    {
        CurrentState = newState;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawSphere(transform.position, radius);
        Debug.Log("IN");

        Gizmos.color = Color.red;
        //Gizmos.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * radius);

        if (canSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


   
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

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
                    ChangeState(State.Aggro);
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
    }
}
