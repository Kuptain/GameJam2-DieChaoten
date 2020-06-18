using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RandomPowerUp : MonoBehaviour
{
    public string thisPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        thisPowerUp = PowerUpManager.instance.powerUps[Random.Range(0, PowerUpManager.instance.powerUps.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PowerUpManager.instance.currentPowerUp = thisPowerUp;
            UIManager.instance.currentPowerup.GetComponent<Text>().text = thisPowerUp;
            if(thisPowerUp == "longerFreeze")
            {
                UIManager.instance.currentFreezeTime = PowerUpManager.instance.longerFreezeData;
                UIManager.instance.freezetimer = PowerUpManager.instance.longerFreezeData;
            }
            else
            {
                UIManager.instance.currentFreezeTime = 5;
                UIManager.instance.freezetimer = 5;
            }
            Destroy(this.gameObject);
        }
    }
}
