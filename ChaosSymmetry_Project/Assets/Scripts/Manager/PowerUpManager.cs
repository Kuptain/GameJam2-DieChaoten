using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    // Factor wird im code multiplieziert (kann entweder = 1 oder = Data sein), Data ist der Speicherwert für Powerup
    [HideInInspector] public float higherJumpFactor, higherJumpData, longerFloatData, longerFreezeData;
    [HideInInspector] public string currentPowerUp;

    PlayerShoot shootScript;

    public string[] powerUps = { "higherJump", "longerFloat", "longerFreeze", "secondClusterFreeze" };
    public List<string> currentPowerUps = new List<string>();
    public string[] consumables = { "placePlatform" };
    public string currentConsumable;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        higherJumpFactor = 1;
        higherJumpData = 1.5f;
        longerFloatData = 70f;
        longerFreezeData = 8;
        shootScript = ObjectManager.instance.player.GetComponent<PlayerShoot>();
        currentConsumable = "";
    }

    // Update is called once per frame
    void Update()
    {
        GivePowerUps();
        LosePowerUp();
        ShowCurrentPowerUps();
        UseConsumable();
    }

    void GivePowerUps()
    {
        if(currentPowerUps.Count > 0)
        {
            foreach (string powerUp in currentPowerUps)
            {
                if(powerUp != null)
                {
                    if (powerUp == "higherJump")
                    {
                        higherJumpFactor = higherJumpData;
                    }
                    if (powerUp == "longerFloat")
                    {
                        PlayerManager.instance.maxFloatFuel = longerFloatData;
                    }
                    if (powerUp == "longerFreeze")
                    {
                        shootScript.meltingTime = longerFreezeData;
                    }
                    if (powerUp == "secondClusterFreeze")
                    {

                    }
                }
            }
        }

        /*if (currentPowerUp != null)
        {
            if (currentPowerUp == "higherJump")
            {
                higherJumpFactor = higherJumpData;
                shootScript.meltingTime = 5;
                PlayerManager.instance.maxFloatFuel = 40f;
            }
            if (currentPowerUp == "longerFloat")
            {
                PlayerManager.instance.maxFloatFuel = longerFloatData;
                shootScript.meltingTime = 5;
                higherJumpFactor = 1;
            }
            if (currentPowerUp == "longerFreeze")
            {
                higherJumpFactor = 1;
                PlayerManager.instance.maxFloatFuel = 40f;
                shootScript.meltingTime = longerFreezeData;
            }
            if(currentPowerUp == "secondClusterFreeze")
            {
                higherJumpFactor = 1;
                shootScript.meltingTime = 5;
                PlayerManager.instance.maxFloatFuel = 40f;
            }
        }*/
        else
        {
            higherJumpFactor = 1;
            shootScript.meltingTime = 5;
            PlayerManager.instance.maxFloatFuel = 40f;
        }
    }

    void LosePowerUp()
    {
        if(currentPowerUps.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
          
                GameObject droppedPowerup1 = Instantiate(ObjectManager.instance.powerUp, ObjectManager.instance.player.transform.position +
                                                                                         ObjectManager.instance.player.transform.forward * 2, Quaternion.identity) as GameObject;
                droppedPowerup1.transform.GetChild(0).GetComponent<RandomPowerUp>().isDropped = true;
                droppedPowerup1.transform.GetChild(0).GetComponent<RandomPowerUp>().thisPowerUp = currentPowerUps[0];
             

            

                //UIManager.instance.currentPowerupOne.GetComponent<Text>().text = "None";
                print("power1");
                if(currentPowerUps[0] == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }
                currentPowerUps.RemoveAt(0);
                //currentPowerUps = new List<string>();
                print(currentPowerUps.Count);
                
            }
            else if (currentPowerUps.Count >= 2 && Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameObject droppedPowerup2 = Instantiate(ObjectManager.instance.powerUp, ObjectManager.instance.player.transform.position + 
                                                                                         ObjectManager.instance.player.transform.forward * 2, Quaternion.identity) as GameObject;
                droppedPowerup2.transform.GetChild(0).GetComponent<RandomPowerUp>().isDropped = true;
                droppedPowerup2.transform.GetChild(0).GetComponent<RandomPowerUp>().thisPowerUp = currentPowerUps[1];
                //UIManager.instance.currentPowerupTwo.GetComponent<Text>().text = "None";
                if (currentPowerUps[1] == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }
                currentPowerUps.RemoveAt(1);
                print("power2");
            
            }
            else if (currentPowerUps.Count == 3 && Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameObject droppedPowerup3 = Instantiate(ObjectManager.instance.powerUp, ObjectManager.instance.player.transform.position +
                                                                                         ObjectManager.instance.player.transform.forward * 2, Quaternion.identity) as GameObject;
                droppedPowerup3.transform.GetChild(0).GetComponent<RandomPowerUp>().isDropped = true;
                droppedPowerup3.transform.GetChild(0).GetComponent<RandomPowerUp>().thisPowerUp = currentPowerUps[2];
                //UIManager.instance.currentPowerupThree.GetComponent<Text>().text = "None";

                if (currentPowerUps[2] == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }
                currentPowerUps.RemoveAt(2);
                print("power3");
         
            }
        }        
    }

    void ShowCurrentPowerUps()
    {
        if ((currentPowerUps.Count == 0))
        {
            UIManager.instance.currentPowerupOne.GetComponent<Text>().text = "None";
            UIManager.instance.currentPowerupTwo.GetComponent<Text>().text = "None";
            UIManager.instance.currentPowerupThree.GetComponent<Text>().text = "None";
        }
        if (currentPowerUps.Count == 1)
        {
            UIManager.instance.currentPowerupOne.GetComponent<Text>().text = currentPowerUps[0];
            UIManager.instance.currentPowerupTwo.GetComponent<Text>().text = "None";
            UIManager.instance.currentPowerupThree.GetComponent<Text>().text = "None";
        }
        else if (currentPowerUps.Count == 2)
        {
            UIManager.instance.currentPowerupThree.GetComponent<Text>().text = "None";
            UIManager.instance.currentPowerupTwo.GetComponent<Text>().text = currentPowerUps[1];
        }
        else if (currentPowerUps.Count == 3)
        {
            UIManager.instance.currentPowerupThree.GetComponent<Text>().text = currentPowerUps[2];
        }
        
    }

    void UseConsumable()
    {
        if(currentConsumable == "placePlatform")
        {
            UIManager.instance.consumable.GetComponent<Text>().text = "";
            currentConsumable = "";
        }
    }
}
