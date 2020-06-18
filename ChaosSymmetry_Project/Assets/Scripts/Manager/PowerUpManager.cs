using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    // Factor wird im code multiplieziert (kann entweder = 1 oder = Data sein), Data ist der Speicherwert für Powerup
    [HideInInspector] public float higherJumpFactor, higherJumpData, longerFloatData, longerFreezeData;
    [HideInInspector] public string currentPowerUp;

    PlayerShoot shootScript;

    public string[] powerUps = { "higherJump", "longerFloat", "longerFreeze" };

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
    }

    // Update is called once per frame
    void Update()
    {
        GivePowerUps();
    }

    void GivePowerUps()
    {
        if (currentPowerUp != null)
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
        }
        else
        {
            higherJumpFactor = 1;
            shootScript.meltingTime = 5;
            PlayerManager.instance.maxFloatFuel = 40f;
        }
    }
}
