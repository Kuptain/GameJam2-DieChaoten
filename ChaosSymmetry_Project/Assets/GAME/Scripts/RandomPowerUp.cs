using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RandomPowerUp : MonoBehaviour
{
    [HideInInspector] public bool isDropped = false;
    [SerializeField] TMP_Text textMeshPref;
    [SerializeField] Sprite longerFreeze;
    [SerializeField] Sprite secondClusterFreeze;
    [SerializeField] Sprite longerFloat;
    [SerializeField] Sprite higherJump;
    [SerializeField] Sprite betterSloMo;

    TMP_Text textMesh;

    GameObject player;
    public string thisPowerUp;
    public bool consumable;
    public bool isHovered = true;

    // Start is called before the first frame update
    void Start()
    {
        /*int i = Random.Range(1, 11);

        if (i <= 3)
        {
            thisPowerUp = PowerUpManager.instance.consumables[Random.Range(0, PowerUpManager.instance.consumables.Length)];
            consumable = true;
        }*/

        if (isDropped == false)
        {
            CheckDoubles();
        }

        player = ObjectManager.instance.player;
        textMesh = Instantiate(textMeshPref, transform) as TMP_Text;
        textMesh.GetComponent<PowerUpText>().type = thisPowerUp;
        textMesh.GetComponent<PowerUpText>().consumable = consumable;
        textMesh.GetComponent<PowerUpText>().powerUp = this.gameObject;
        CheckType();

    }

    void CheckDoubles()
    {
        thisPowerUp = PowerUpManager.instance.powerUps[Random.Range(0, PowerUpManager.instance.powerUps.Length)];       
        
        for(int i = 0; i < PowerUpManager.instance.currentPowerUps.Count; i++)
        {
            if(thisPowerUp == PowerUpManager.instance.currentPowerUps[i])
            {
                CheckDoubles();
            }
        }   
        
    }

    void Update()
    {
        transform.parent.LookAt(Camera.main.transform.position);

        if (isHovered)
        {
            textMesh.GetComponent<PowerUpText>().fadeMode = 1;
        }
        else
        {
            textMesh.GetComponent<PowerUpText>().fadeMode = 2;

        }
    }

    void CheckType()
    {
        if (thisPowerUp == "longerFloat")
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = longerFloat;
            textMesh.text = "Extra Float Fuel";

        }
        if (thisPowerUp == "higherJump")
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = higherJump;
            textMesh.text = "Higher Jumps";

        }
        if (thisPowerUp == "secondClusterFreeze")
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = secondClusterFreeze;
            textMesh.text = "Freeze A Second Cluster";

        }
        if (thisPowerUp == "betterSloMo")
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = betterSloMo;
            textMesh.text = "Stronger Slowmotion";

        }
        if (thisPowerUp == "longerFreeze")
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = longerFreeze;
            textMesh.text = "Freeze Cluster Longer";

        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<ThirdPersonController>() != null)
        {
            if (PowerUpManager.instance.currentPowerUps.Count < 3)
            {
                PowerUpManager.instance.currentPowerUps.Add(thisPowerUp);                AudioManager.instance.powerUp.PlayOneShot(AudioManager.instance.pickUp, 1);
                if (TutorialManager.instance.currentHint == "")
                {
                    TutorialManager.instance.ChangeType("power");
                    TutorialManager.instance.StartCoroutine(TutorialManager.instance.DisableAfter(5f));
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

            /*else if (consumable == true && PowerUpManager.instance.currentConsumable == "")
            {
                PowerUpManager.instance.currentConsumable = thisPowerUp;
                UIManager.instance.consumable.GetComponent<Text>().text = thisPowerUp;
                Destroy(this.gameObject.transform.parent.gameObject);

            }*/
        }
    }
    private void OnDestroy()
    {
        Destroy(textMesh);
    }
}
