using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Camera raycastCamera;
    
    public TextMeshProUGUI coinsUI;
    
    public int coinCount;

    void Start()
    {
        coinCount = 10;
        
        coinsUI.text = "Coins: " + coinCount;
    }

    void Update()
    {
        coinsUI.text = "Coins: " + coinCount;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.name == "Enemy" || hitInfo.transform.name == "Enemy(Clone)")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hitInfo.collider.GetComponent<EnemyDemo>().health--;
                    Debug.Log(hitInfo.collider.GetComponent<EnemyDemo>().health);

                    if (hitInfo.collider.GetComponent<EnemyDemo>().health <= 0)
                    {
                        Destroy(hitInfo.collider.gameObject);
                        coinCount += hitInfo.collider.GetComponent<EnemyDemo>().coinValue;
                        //Debug.Log("Got him! Coin count is " + coinCount);
                    }
                }
            }

        }
    }
}
