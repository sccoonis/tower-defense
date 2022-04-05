using System.Collections;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform NavmeshEnemy;
    public int numEnemiesToSpawn = 1;
    public Transform[] waypointList;

    public TextMeshProUGUI coinsText;
    
    public GameObject Tower;

    private int treasury = 10;

    //-----------------------------------------------------------------------------
    IEnumerator Start()
    {
        coinsText.text = "Coins: " + treasury;
        
        int enemiesSpawned = 0;
        while (enemiesSpawned < numEnemiesToSpawn)
        {
            Transform newEnemyTransform = Instantiate(NavmeshEnemy);
            NavmeshEnemy newEnemy = newEnemyTransform.GetComponent<NavmeshEnemy>();
            if (newEnemy)
            {
                newEnemy.OnEnemyDied += OnEnemyDied;
                newEnemy.WaypointList = waypointList;
            }
            enemiesSpawned++;

            yield return new WaitForSeconds(2f);
        }
    }

    private void Update()
    {
        coinsText.text = "Coins: " + treasury;
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.transform.position.z));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                if (hit.transform.tag == "TowerSpot")
                {
                    if (treasury >= 5)
                    {
                        treasury = treasury - 5;
                        hit.transform.gameObject.SetActive(false);
                        PlaceTower(hit.transform.position);
                    }
                }
        }
    }

    //-----------------------------------------------------------------------------
    void OnEnemyDied(NavmeshEnemy deadEnemy)
    {
        deadEnemy.OnEnemyDied -= OnEnemyDied;
        treasury += deadEnemy.coinReward;
        coinsText.text = $"Coins: {treasury}";
    }
    
    void PlaceTower(Vector3 position)
    {
        Instantiate(Tower, position, Quaternion.identity, transform);
    }
}
