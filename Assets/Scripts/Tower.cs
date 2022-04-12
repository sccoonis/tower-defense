using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 15f;

    public float rof = 1f;
    public float reload = 0f;

    public Transform barrel;
    public LineRenderer lr;
    
    private Transform target;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    
    void Update()
    {
        if (target == null)
        {
            if (lr.enabled)
            {
                lr.enabled = false;
            }
            return;
        }

        if (reload <= 0f)
        {
            Shoot();
            reload = 1f / rof;
        }

        reload -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closest = Mathf.Infinity;

        GameObject closestEnemy = null;
        
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closest)
            {
                closest = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && closest <= range)
        {
            target = closestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        if (!lr.enabled)
        {
            lr.enabled = true;
        }
        
        lr.SetPosition(0, barrel.position);
        lr.SetPosition(1, target.position);


        target.GetComponent<NavmeshEnemy>().health--;
        if (target.GetComponent<NavmeshEnemy>().health <= 0)
        {
            // GetComponent<Game>().OnEnemyDied(target.GetComponent<NavmeshEnemy>());
            
            Destroy(target.gameObject);
        }
        Vector3 newHealthAmount = new Vector3(target.GetComponent<NavmeshEnemy>().health/100f , target.GetComponent<NavmeshEnemy>().healthBar.localScale.y, target.GetComponent<NavmeshEnemy>().healthBar.localScale.z);
        target.GetComponent<NavmeshEnemy>().healthBar.localScale = newHealthAmount;

    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
