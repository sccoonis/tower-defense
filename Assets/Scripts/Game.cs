using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public Transform NavmeshEnemy;
    public int numEnemiesToSpawn = 1;
    public Transform[] waypointList;

    public TextMeshProUGUI coinsText;
    
    public GameObject Tower;

    public AudioClip noise;

    public GameObject startButton;

    private int treasury = 10;

    private int killTarget;
    
    private bool waveActive = false;
    
    //-----------------------------------------------------------------------------
    private void Start()
    {
        coinsText.text = "Coins: " + treasury;
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

        if (killTarget <= 0)
        {
            waveActive = false;
        }
        
        if (waveActive)
        {
            startButton.SetActive(false);
        }
        else
        {
            startButton.SetActive(true);
        }
        
    }

    //-----------------------------------------------------------------------------
    public void OnEnemyDied(NavmeshEnemy deadEnemy)
    {
        deadEnemy.OnEnemyDied -= OnEnemyDied;
        treasury += deadEnemy.coinReward;
        coinsText.text = $"Coins: {treasury}";
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = noise;
        audioSource.Play();
        killTarget--;
    }
    
    void PlaceTower(Vector3 position)
    {
        Instantiate(Tower, position, Quaternion.identity, transform);
    }
    
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    IEnumerator Spawn()
    {
        waveActive = true;
        
        int enemiesSpawned = 0;
        killTarget = 0;
        
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
            killTarget++;

            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnWave()
    {
        numEnemiesToSpawn++;
        StartCoroutine(Spawn());
        //StopCoroutine(Spawn());
    }
}
