using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RandomPowerUp : MonoBehaviour
{
    [HideInInspector] public bool isDropped = false;

    GameObject player;
    public string thisPowerUp;
    public bool consumable;

    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(1, 11);

        if(i <= 3)
        {
            thisPowerUp = PowerUpManager.instance.consumables[Random.Range(0, PowerUpManager.instance.consumables.Length)];
            print("comnsumable");
            consumable = true;
        }
        else if (isDropped == false)
        {
            thisPowerUp = PowerUpManager.instance.powerUps[Random.Range(0, PowerUpManager.instance.powerUps.Length)];

        }
        player = ObjectManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.LookAt(player.transform.position);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<ThirdPersonController>() != null)
        {

            if (PowerUpManager.instance.currentPowerUps.Count < 3)
            {
                PowerUpManager.instance.currentPowerUps.Add(thisPowerUp);

                if (TutorialManager.instance.currentHint == "")
                {
                    TutorialManager.instance.ChangeType("power");
                    TutorialManager.instance.StartCoroutine(TutorialManager.instance.DisableAfter(3f));


                }

                if (PowerUpManager.instance.currentPowerUps.Count == 1)
                {
                    UIManager.instance.currentPowerupOne.GetComponent<Text>().text = thisPowerUp;
                }
                else if (PowerUpManager.instance.currentPowerUps.Count == 2)
                {
                    UIManager.instance.currentPowerupTwo.GetComponent<Text>().text = thisPowerUp;
                }
                else if (PowerUpManager.instance.currentPowerUps.Count == 3)
                {
                    UIManager.instance.currentPowerupThree.GetComponent<Text>().text = thisPowerUp;
                }
                /*PowerUpManager.instance.currentPowerUp = thisPowerUp;
                UIManager.instance.currentPowerup.GetComponent<Text>().text = thisPowerUp;*/

                if (thisPowerUp == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = PowerUpManager.instance.longerFreezeData;
                    UIManager.instance.freezetimer = PowerUpManager.instance.longerFreezeData;
                }
                else
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }


                Destroy(this.gameObject.transform.parent.gameObject);
            }

            else if (consumable == true && PowerUpManager.instance.currentConsumable == "")
            {
                PowerUpManager.instance.currentConsumable = thisPowerUp;
                UIManager.instance.consumable.GetComponent<Text>().text = thisPowerUp;
                Destroy(this.gameObject.transform.parent.gameObject);

            }
        }
    }
}
