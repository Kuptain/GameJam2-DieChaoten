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
    public GameObject particleDestroy;
    public GameObject particleFreeze;

    public static PlayerManager instance;
    private GameObject player;

    public float floatFuel;
    public float maxFloatFuel = 40f;
    public float collideJumpForce = 50f;
    public bool isGrounded;
    public bool canJump = true;
    public int lives = 3;

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
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetFuelLevel();
    }

    public void SetFuelLevel()
    {
        //UI
        //fuelSlider.maxValue = maxFlpoatFuel;
        //fuelSlider.value = floatFuel;
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForEndOfFrame();

        UIManager.instance.heart.enabled = false;
        UIManager.instance.lives.SetActive(false);

        respawnPanelDown.GetComponent<Animation>().Play();
        respawnPanelUp.GetComponent<Animation>().Play();

        yield return new WaitForSeconds(1f);
        ObjectManager.instance.player.GetComponent<ThirdPersonController>().isSafe = false;
        player.transform.position = LevelGeneration.instance.checkPointOne.transform.position + new Vector3(0, 1.5f, 0);
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.25f);

        respawnPanelDown.gameObject.SetActive(false);
        respawnPanelUp.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("gameMode") == 0)
        {
            UIManager.instance.heart.enabled = true;
            UIManager.instance.lives.SetActive(true);
        }

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
