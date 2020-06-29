using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    GameObject player;
    public string thisPowerUp;
    public bool consumable;

    [SerializeField] TMP_Text textMeshPref;
    public bool isHovered = true;
    TMP_Text textMesh;

    // Start is called before the first frame update
    void Start()
    {
        consumable = true;
        thisPowerUp = "placePlatform";
        player = ObjectManager.instance.player;
        textMesh = Instantiate(textMeshPref, transform) as TMP_Text;
        textMesh.GetComponent<PowerUpText>().type = thisPowerUp;
        textMesh.GetComponent<PowerUpText>().consumable = consumable;
        textMesh.GetComponent<PowerUpText>().powerUp = this.gameObject;

        textMesh.text = thisPowerUp;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.LookAt(player.transform.position);

        if (isHovered)
        {
            textMesh.GetComponent<PowerUpText>().fadeMode = 1;
        }
        else
        {
            textMesh.GetComponent<PowerUpText>().fadeMode = 2;

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<ThirdPersonController>() != null)
        {
            //PowerUpManager.instance.currentConsumable = thisPowerUp;
            player.GetComponent<PlayerShoot>().cubeSpawnCharges += 1;
            UIManager.instance.consumable.GetComponent<Text>().text = thisPowerUp;
            Destroy(this.gameObject.transform.parent.gameObject);

        }
    }
    private void OnDestroy()
    {
        Destroy(textMesh);
    }
}
