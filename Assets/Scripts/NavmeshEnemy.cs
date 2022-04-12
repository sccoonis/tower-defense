using UnityEngine;
using UnityEngine.AI;

public class NavmeshEnemy : MonoBehaviour
{
public int health = 3;
    public int coinReward = 2;
    public float lookAheadDistance = 2f;

    public Transform[] WaypointList { get; set; }

    public Transform healthBar;

    public delegate void EnemyDied(NavmeshEnemy deadEnemy);
    public event EnemyDied OnEnemyDied;

    private NavMeshAgent myAgent;
    private Vector3 targetPosition;
    private int index = 0;

    //-----------------------------------------------------------------------------
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();

        //Place our enemy at the start point
        Vector3 startingPosition = GetNavmeshPosition(WaypointList[0].transform.position);
        myAgent.Warp(startingPosition);

        transform.LookAt(GetNavmeshPosition(WaypointList[1].transform.position));
        TargetNextWaypoint();
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        Vector3 position2D = transform.position;
        position2D.z = targetPosition.z;

        Vector3 directionToTarget = targetPosition - position2D;
        Vector3 newPosition = position2D + directionToTarget.normalized * lookAheadDistance;
        Vector3 newDirectionToTarget = targetPosition - newPosition;

        // Check if we passed our target
        if (Vector3.Dot(directionToTarget, newDirectionToTarget) < 0f)
        {
            // Debug.Break();
            TargetNextWaypoint();
        }

        UpdateTestDamage();
    }

    //-----------------------------------------------------------------------------
    void UpdateTestDamage()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray pickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pickRay, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.transform == transform)
                {
                    health -= 1;
                    if (health <= 0)
                    {
                        OnEnemyDied?.Invoke(this);
                        Destroy(gameObject);
                    }
                    Vector3 newHealthAmount = new Vector3(health/100f , healthBar.localScale.y, healthBar.localScale.z);
                    healthBar.localScale = newHealthAmount;
                }
            }
        }
    }

    //-----------------------------------------------------------------------------
    Vector3 GetNavmeshPosition(Vector3 samplePosition)
    {
        NavMesh.SamplePosition(samplePosition, out NavMeshHit hitInfo, 100, -1);
        return hitInfo.position;
    }

    //-----------------------------------------------------------------------------
    private void TargetNextWaypoint()
    {
        if (index < WaypointList.Length - 1)
        {
            index += 1;
            targetPosition = GetNavmeshPosition(WaypointList[index].transform.position);
            myAgent.SetDestination(targetPosition);

            // Debug.Log($"Set Destination to point {index}");
        }
    }

}
