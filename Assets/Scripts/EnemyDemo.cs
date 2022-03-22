using System.Collections.Generic;
using UnityEngine;

public class EnemyDemo : MonoBehaviour
{
    // todo #1 set up properties
    //   health, speed, coin worth
    //   waypoints
    //   delegate event for outside code to subscribe and be notified of enemy death

    public int health = 3;
    public float speed = 3.0f;
    public int coinValue = 3;

    public List<Transform> waypointList;
    
    private int targetWaypointIndex;

    // NOTE! This code should work for any speed value (large or small)
    public delegate void EnemyDied(EnemyDemo deadEnemy);

    public event EnemyDied OnEnemyDied;

    //-----------------------------------------------------------------------------
    void Start()
    {
        // todo #2
        //   Place our enemy at the starting waypoint
        transform.position = waypointList[0].position;
        targetWaypointIndex = 1;
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        // todo #3 Move towards the next waypoint
        Vector3 targetPosition = waypointList[targetWaypointIndex].position;
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        Vector3 newPosition = transform.position;
        newPosition += moveDir * speed * Time.deltaTime;

        transform.position = newPosition;

        /*
        bool enemyDied = false;
        if (enemyDied)
        {
            OnEnemyDied?.Invoke(this);
        }
        */

        // todo #4 Check if destination reaches or passed and change target

        if (Vector3.Distance(transform.position, targetPosition) <= 0.2f)
        {
            TargetNextWaypoint();
        }
    }

    //-----------------------------------------------------------------------------
    private void TargetNextWaypoint()
    {
        if (targetWaypointIndex >= waypointList.Count - 1)
        {
            Debug.Log("Game Over");
            Destroy(gameObject);
            return;
        }
        
        targetWaypointIndex++;
        
    }
}
