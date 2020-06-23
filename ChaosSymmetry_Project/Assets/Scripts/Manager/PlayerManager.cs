using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Slider fuelSlider;
    [SerializeField] Image respawnPanelUp;
    [SerializeField] Image respawnPanelDown;
    [HideInInspector] public bool isRespawning = false;

    public static PlayerManager instance;
    private GameObject player;

    public float floatFuel;
    public float maxFloatFuel = 40f;
    public float collideJumpForce = 50f;
    public bool isGrounded;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        player = ObjectManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        SetFuelLevel();
    }

    public void SetFuelLevel()
    {
        //UI
        fuelSlider.maxValue = maxFloatFuel;
        fuelSlider.value = floatFuel;
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForEndOfFrame();

        respawnPanelDown.GetComponent<Animation>().Play();
        respawnPanelUp.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(1.5f);

        respawnPanelDown.gameObject.SetActive(false);
        respawnPanelUp.gameObject.SetActive(false);

        player.transform.position = LevelGeneration.instance.checkPointOne.transform.position + new Vector3(0, 2, 0);
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.5f);
        isRespawning = false;


    }


    public void Respawn()
    {
        isRespawning = true;
       
        respawnPanelDown.gameObject.SetActive(true);
        respawnPanelUp.gameObject.SetActive(true);

        StartCoroutine(RespawnTimer());
      
    }
}
