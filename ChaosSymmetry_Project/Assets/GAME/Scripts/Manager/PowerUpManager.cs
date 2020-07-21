using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    // Factor wird im code multiplieziert (kann entweder = 1 oder = Data sein), Data ist der Speicherwert für Powerup
    //[HideInInspector] 
    public float higherJumpFactor, higherJumpData, longerFloatData, longerFreezeData;
    [HideInInspector] public string currentPowerUp;

    PlayerShoot shootScript;

    public string[] powerUps = { "higherJump", "longerFloat", "longerFreeze", "secondClusterFreeze", "betterSloMo" };
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
        higherJumpData = 1.1f;
        longerFloatData = 200f;
        longerFreezeData = 10;
        shootScript = ObjectManager.instance.player.GetComponent<PlayerShoot>();
        currentConsumable = "";
    }

    // Update is called once per frame
    void Update()
    {
        GivePowerUps();
        LosePowerUp();
        //ShowCurrentPowerUps();
        ShowConsumableCharges();
    }

    void GivePowerUps()
    {
        if (currentPowerUps.Count > 0)
        {
            foreach (string powerUp in currentPowerUps)
            {
                if (powerUp != null)
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
                    if (powerUp == "betterSloMo")
                    {
                        CubeManager.instance.slowmoValue = 0.04f;
                        CubeManager.instance.currentSlowmo = 0.04f;
                    }
                    if (powerUp == "secondClusterFreeze")
                    {
                        currentPowerUp = "secondClusterFreeze";
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
            PlayerManager.instance.maxFloatFuel = 125f;
            CubeManager.instance.currentSlowmo = 0.25f;
            CubeManager.instance.slowmoValue = 0.15f;
            currentPowerUp = "";
        }
    }

    void LosePowerUp()
    {
        if (currentPowerUps.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AudioManager.instance.powerUp.PlayOneShot(AudioManager.instance.putDown, 1);
                GameObject droppedPowerup1 = Instantiate(ObjectManager.instance.powerUp, ObjectManager.instance.player.transform.position +
                                                                                         ObjectManager.instance.player.transform.forward * 2 + Vector3.up, Quaternion.identity) as GameObject;
                droppedPowerup1.transform.GetChild(0).GetComponent<RandomPowerUp>().isDropped = true;
                droppedPowerup1.transform.GetChild(0).GetComponent<RandomPowerUp>().thisPowerUp = currentPowerUps[0];

                if (currentPowerUps[0] == "secondClusterFreeze")
                {
                    currentPowerUp = "";
                }
                if (currentPowerUps[0] == "higherJump")
                {
                    higherJumpFactor = 1;
                }
                if (currentPowerUps[0] == "longerFloat")
                {
                    PlayerManager.instance.maxFloatFuel = 40f;
                }
                if (currentPowerUps[0] == "betterSloMo")
                {
                    CubeManager.instance.slowmoValue = 0.6f;
                }
                //UIManager.instance.currentPowerupOne.GetComponent<Text>().text = "None";
                if (currentPowerUps[0] == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }
                currentPowerUps.RemoveAt(0);
                //currentPowerUps = new List<string>();

            }
            else if (currentPowerUps.Count >= 2 && Input.GetKeyDown(KeyCode.Alpha2))
            {
                AudioManager.instance.powerUp.PlayOneShot(AudioManager.instance.putDown, 1);

                GameObject droppedPowerup2 = Instantiate(ObjectManager.instance.powerUp, ObjectManager.instance.player.transform.position +
                                                                                         ObjectManager.instance.player.transform.forward * 2 + Vector3.up, Quaternion.identity) as GameObject;
                droppedPowerup2.transform.GetChild(0).GetComponent<RandomPowerUp>().isDropped = true;
                droppedPowerup2.transform.GetChild(0).GetComponent<RandomPowerUp>().thisPowerUp = currentPowerUps[1];
                //UIManager.instance.currentPowerupTwo.GetComponent<Text>().text = "None";

                if (currentPowerUps[1] == "secondClusterFreeze")
                {
                    currentPowerUp = "";
                }
                if (currentPowerUps[1] == "higherJump")
                {
                    higherJumpFactor = 1;
                }
                if (currentPowerUps[1] == "longerFloat")
                {
                    PlayerManager.instance.maxFloatFuel = 40f;
                }
                if (currentPowerUps[1] == "betterSloMo")
                {
                    CubeManager.instance.slowmoValue = 0.6f;
                }
                if (currentPowerUps[1] == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }
                currentPowerUps.RemoveAt(1);

            }
            else if (currentPowerUps.Count == 3 && Input.GetKeyDown(KeyCode.Alpha3))
            {
                AudioManager.instance.powerUp.PlayOneShot(AudioManager.instance.putDown, 1);

                GameObject droppedPowerup3 = Instantiate(ObjectManager.instance.powerUp, ObjectManager.instance.player.transform.position +
                                                                                         ObjectManager.instance.player.transform.forward * 2 + Vector3.up, Quaternion.identity) as GameObject;
                droppedPowerup3.transform.GetChild(0).GetComponent<RandomPowerUp>().isDropped = true;
                droppedPowerup3.transform.GetChild(0).GetComponent<RandomPowerUp>().thisPowerUp = currentPowerUps[2];
                //UIManager.instance.currentPowerupThree.GetComponent<Text>().text = "None";
                if (currentPowerUps[2] == "secondClusterFreeze")
                {
                    currentPowerUp = "";
                }
                if (currentPowerUps[2] == "higherJump")
                {
                    higherJumpFactor = 1;
                }
                if (currentPowerUps[2] == "longerFloat")
                {
                    PlayerManager.instance.maxFloatFuel = 40f;
                }
                if (currentPowerUps[2] == "betterSloMo")
                {
                    CubeManager.instance.slowmoValue = 0.6f;
                }
                if (currentPowerUps[2] == "longerFreeze")
                {
                    UIManager.instance.currentFreezeTime = 5;
                    UIManager.instance.freezetimer = 5;
                }
                currentPowerUps.RemoveAt(2);

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

    void ShowConsumableCharges()
    {
        if (shootScript.cubeSpawnCharges > 0)
        {
            UIManager.instance.consumable.GetComponent<Text>().text = "placePlatform";
            UIManager.instance.consumableCharges.GetComponent<Text>().text = shootScript.cubeSpawnCharges.ToString();
        }
        else
        {
            UIManager.instance.consumable.GetComponent<Text>().text = "";
            UIManager.instance.consumableCharges.GetComponent<Text>().text = "";
            currentConsumable = "";
        }
    }
}
