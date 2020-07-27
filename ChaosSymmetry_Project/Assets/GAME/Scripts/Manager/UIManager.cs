using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [HideInInspector] public GameObject slomo, consumable, consumableCharges, currentPowerupOne, currentPowerupTwo, currentPowerupThree, slowmoScreen;
    [HideInInspector] public GameObject mainMenuCanvas, pauseCanvas, ingameCanvas, gameOverCanvas;
    [SerializeField] GameObject freezeBalken, freezeBalkenGoal, pauseConsumableCharges, freezeBalkenBG, fuelbalken, fuelbalkenGoal, fuelbalkenBG;
    [SerializeField] GameObject secondfreezeBalken, secondfreezeBalkenGoal, secondfreezeBalkenBG, camFreeLook, slider;
    [SerializeField] Sprite floatSprite, jumpSprite, freezeSprite, secondSprite, platformSprite, slomoSprite, consumableSprite;
    [SerializeField] Image powerUpImageOne, powerUpImageTwo, powerUpImageThree, consumableImage;
    [SerializeField] Image pausePowerUpImageOne, pausePowerUpImageTwo, pausePowerUpImageThree, pauseConsumableImage;
    public GameObject uiPrefab, lives, livespause, currentScore, highScore;
    public GameObject freezeTime, overScore, overHighscore;
    public Image heart, heartpause;
    public bool showMenu, jumped;
    public int normalScore, endlessScore;

    Text powerDescOne, powerDescTwo, powerDescThree, consDesc;
    public bool paused, gameStarted;

    PowerUpManager powerUpManager;
    GameObject player;
    GameObject cineMach;
    [HideInInspector] public float freezetimer, currentFreezeTime, secondCurrentFreezeTime;

    Vector3 balkenStartPos, balkenEndPos, secondbalkenStartPos, secondbalkenEndPos, fuelbalkenStartPos, fuelbalkenEndPos;
    Quaternion balkenStartRot, balkenEndRot, secondbalkenStartRot, secondbalkenEndRot, fuelbalkenStartRot, fuelbalkenEndRot;

    float balkenTime = 0;
    float secondbalkenTime = 0;
    float fuelbalkenTime = 0;
    float fuelSave = 0;

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
        slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensitivity", 1);
        normalScore = 0;
        endlessScore = 0;

        if (PlayerPrefs.GetInt("levelRestarted") == 1)
        {
            PlayerPrefs.SetInt("showMenu", 1);
        }
        else
        {
            PlayerPrefs.SetInt("showMenu", 0);
        }

        mainMenuCanvas = uiPrefab.transform.GetChild(1).gameObject;
        pauseCanvas = uiPrefab.transform.GetChild(2).gameObject;
        ingameCanvas = uiPrefab.transform.GetChild(0).gameObject;
        gameOverCanvas = uiPrefab.transform.GetChild(3).gameObject;
        gameOverCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        powerDescOne = pauseCanvas.transform.GetChild(5).GetComponent<Text>();
        powerDescTwo = pauseCanvas.transform.GetChild(6).GetComponent<Text>();
        powerDescThree = pauseCanvas.transform.GetChild(7).GetComponent<Text>();
        consDesc = pauseCanvas.transform.GetChild(8).GetComponent<Text>();

        powerUpManager = PowerUpManager.instance;
        cineMach = GameObject.Find("CM FreeLook1");
        cineMach.GetComponent<CinemachineFreeLook>().enabled = false;
        gameStarted = false;
        balkenStartPos = freezeBalken.transform.position;
        balkenStartRot = freezeBalken.transform.rotation;
        balkenEndPos = freezeBalkenGoal.transform.position;
        balkenEndRot = freezeBalkenGoal.transform.rotation;
        fuelbalkenStartPos = fuelbalken.transform.position;
        fuelbalkenStartRot = fuelbalken.transform.rotation;
        fuelbalkenEndPos = fuelbalkenGoal.transform.position;
        fuelbalkenEndRot = fuelbalkenGoal.transform.rotation;
        secondbalkenStartPos = secondfreezeBalken.transform.position;
        secondbalkenStartRot = secondfreezeBalken.transform.rotation;
        secondbalkenEndPos = secondfreezeBalkenGoal.transform.position;
        secondbalkenEndRot = secondfreezeBalkenGoal.transform.rotation;

        slomo = ingameCanvas.transform.GetChild(1).GetChild(2).gameObject;
        slowmoScreen = ingameCanvas.transform.GetChild(3).gameObject;

        freezeTime = uiPrefab.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        player = ObjectManager.instance.player;
        currentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime;
        secondCurrentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime;
        freezetimer = currentFreezeTime;
        currentPowerupOne = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        currentPowerupTwo = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        currentPowerupThree = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(2).gameObject;
        consumable = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(3).gameObject;
        consumableCharges = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(4).gameObject;
        mainMenuCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => StartGame());
        mainMenuCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => StartEndlessMode());
        mainMenuCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitGame());
        mainMenuCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().onValueChanged.AddListener((value) => { ToggleTutorial(); });
        gameOverCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => RestartGame());
        gameOverCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => OpenMainMenu());
        gameOverCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitGame());
        pauseCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ResumeGame());
        pauseCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => OpenMainMenu());
        pauseCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitGame());
        pauseCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().onValueChanged.AddListener((value) => { ToggleTutorial(); });
        slider.GetComponent<Slider>().onValueChanged.AddListener((value) => { ChangeSensitivity(); });
        Camera.main.transform.parent.GetComponent<CameraController>().enabled = false;

        // 1 is on, 0 is off
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            pauseCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
            mainMenuCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
        else if (PlayerPrefs.GetInt("tutorial") == 1)
        {
            pauseCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            mainMenuCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        }

        // showmnenu 0 =  true, 1 is no menu
        if (PlayerPrefs.GetInt("showMenu") == 0)
        {
            ingameCanvas.SetActive(false);
            player.GetComponent<ThirdPersonController>().enabled = false;
            Camera.main.transform.parent.GetComponent<CameraController>().enabled = false;
            player.GetComponent<PlayerShoot>().enabled = false;
            Cursor.visible = true;
            mainMenuCanvas.SetActive(true);
        }
        else
        {
            mainMenuCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
            player.GetComponent<ThirdPersonController>().enabled = true;
            cineMach.GetComponent<CinemachineFreeLook>().enabled = true;
            ingameCanvas.SetActive(true);
            player.GetComponent<PlayerShoot>().enabled = true;
        }
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        paused = false;
        //ingameCanvas.SetActive(true);
        //Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
        //player.GetComponent<ThirdPersonController>().enabled = true;
        //player.GetComponent<PlayerShoot>().enabled = true;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetInt("normalHighScore", 0);

        ShowSlomo();
        ShowFreezeTime();
        ShowFuel();
        ShowCurrentPowerups();
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false && mainMenuCanvas.activeSelf == false && gameOverCanvas.activeSelf == false)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused && mainMenuCanvas.activeSelf == false && gameOverCanvas.activeSelf == false)
        {
            ResumeGame();
        }

        if (ingameCanvas.activeSelf == true)
        {
            if (PlayerPrefs.GetInt("gameMode") == 1)
            {
                heart.enabled = false;
                lives.gameObject.SetActive(false);
            }
            else
            {
                lives.GetComponent<Text>().text = PlayerManager.instance.lives.ToString();
            }
        }
    }

    void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        Cursor.visible = false;
        cineMach.GetComponent<CinemachineFreeLook>().enabled = true;
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<ThirdPersonController>().enabled = true;
        Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
        PlayerPrefs.SetInt("gameMode", 0);
        paused = false;
        gameStarted = true;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cineMach.GetComponent<CinemachineFreeLook>().enabled = false;
        ingameCanvas.SetActive(false);
        Camera.main.transform.parent.GetComponent<CameraController>().enabled = false;
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<PlayerShoot>().enabled = false;
        pauseCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        if (PlayerPrefs.GetInt("gameMode") == 1)
        {
            heartpause.enabled = false;
            livespause.gameObject.SetActive(false);
            currentScore.GetComponent<Text>().text = "Islands reached: " + endlessScore.ToString();
            highScore.GetComponent<Text>().text = "High Score: " + PlayerPrefs.GetInt("endlessHighScore").ToString();
        }
        else
        {
            livespause.GetComponent<Text>().text = PlayerManager.instance.lives.ToString();
            currentScore.GetComponent<Text>().text = "Islands reached: " + normalScore.ToString();
            highScore.GetComponent<Text>().text = "High Score: " + PlayerPrefs.GetInt("normalHighScore").ToString();
        }

        ShowCurrentPowerupsPause();
        ShowPowerUpDescription();

        // 1 is on, 0 is off
        if (PlayerPrefs.GetInt("tutorial") == 0)
        {
            pauseCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
        else if (PlayerPrefs.GetInt("tutorial") == 1)
        {
            pauseCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(true);

        }
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        paused = false;
        cineMach.GetComponent<CinemachineFreeLook>().enabled = true;
        ingameCanvas.SetActive(true);
        //Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ShowPowerUpDescription()
    {
        if (powerUpManager.currentPowerUps.Count > 0)
        {
            //"longerFloat", "longerFreeze", "secondClusterFreeze", "betterSloMo"
            if (powerUpManager.currentPowerUps[0] == "higherJump")
            {
                powerDescOne.text = "Makes you jump higher!";
            }
            else if (powerUpManager.currentPowerUps[0] == "longerFreeze")
            {
                powerDescOne.text = "Platforms remain frozen for longer.";
            }
            else if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
            {
                powerDescOne.text = "Allows you to freeze two clusters at the same time.";
            }
            else if (powerUpManager.currentPowerUps[0] == "betterSloMo")
            {
                powerDescOne.text = "Slowmotion slows time even more!";
            }
            else
            {
                powerDescOne.text = "";
            }

            if (powerUpManager.currentPowerUps.Count > 1)
            {
                if (powerUpManager.currentPowerUps[0] == "higherJump")
                {
                    powerDescOne.text = "Makes you jump higher!";
                }
                else if (powerUpManager.currentPowerUps[0] == "longerFreeze")
                {
                    powerDescOne.text = "Platforms remain frozen for longer.";
                }
                else if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
                {
                    powerDescOne.text = "Allows you to freeze two clusters at the same time.";
                }
                else if (powerUpManager.currentPowerUps[0] == "betterSloMo")
                {
                    powerDescOne.text = "Slowmotion slows time even more!";
                }
                else
                {
                    powerDescOne.text = "";
                }

                if (powerUpManager.currentPowerUps[1] == "higherJump")
                {
                    powerDescTwo.text = "Makes you jump higher!";
                }
                else if (powerUpManager.currentPowerUps[1] == "longerFreeze")
                {
                    powerDescTwo.text = "Platforms remain frozen for longer.";
                }
                else if (powerUpManager.currentPowerUps[1] == "secondClusterFreeze")
                {
                    powerDescTwo.text = "Allows you to freeze two clusters at the same time.";
                }
                else if (powerUpManager.currentPowerUps[1] == "betterSloMo")
                {
                    powerDescTwo.text = "Slowmotion slows time even more!";
                }
                else
                {
                    powerDescTwo.text = "";
                }
            }

            if (powerUpManager.currentPowerUps.Count > 2)
            {
                if (powerUpManager.currentPowerUps[0] == "higherJump")
                {
                    powerDescOne.text = "Makes you jump higher!";
                }
                else if (powerUpManager.currentPowerUps[0] == "longerFreeze")
                {
                    powerDescOne.text = "Platforms remain frozen for longer.";
                }
                else if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
                {
                    powerDescOne.text = "Allows you to freeze two clusters at the same time.";
                }
                else if (powerUpManager.currentPowerUps[0] == "betterSloMo")
                {
                    powerDescOne.text = "Slowmotion slows time even more!";
                }
                else
                {
                    powerDescOne.text = "";
                }

                if (powerUpManager.currentPowerUps[1] == "higherJump")
                {
                    powerDescTwo.text = "Makes you jump higher!";
                }
                else if (powerUpManager.currentPowerUps[1] == "longerFreeze")
                {
                    powerDescTwo.text = "Platforms remain frozen for longer.";
                }
                else if (powerUpManager.currentPowerUps[1] == "secondClusterFreeze")
                {
                    powerDescTwo.text = "Allows you to freeze two clusters at the same time.";
                }
                else if (powerUpManager.currentPowerUps[1] == "betterSloMo")
                {
                    powerDescTwo.text = "Slowmotion slows time even more!";
                }
                else
                {
                    powerDescTwo.text = "";
                }

                if (powerUpManager.currentPowerUps[2] == "higherJump")
                {
                    powerDescThree.text = "Makes you jump higher!";
                }
                else if (powerUpManager.currentPowerUps[2] == "longerFreeze")
                {
                    powerDescThree.text = "Platforms remain frozen for longer.";
                }
                else if (powerUpManager.currentPowerUps[2] == "secondClusterFreeze")
                {
                    powerDescThree.text = "Allows you to freeze two clusters at the same time.";
                }
                else if (powerUpManager.currentPowerUps[2] == "betterSloMo")
                {
                    powerDescThree.text = "Slowmotion slows time even more!";
                }
                else
                {
                    powerDescThree.text = "";
                }
            }
        }

        if (player.GetComponent<PlayerShoot>().cubeSpawnCharges > 0)
        {
            consDesc.text = "Hold Q to find a spot for a platform, then release Q to place it. ";
        }
        else
        {
            consDesc.text = "";
        }
    }

    void RestartGame()
    {
        // levelrestarted = 1 means that no mainmenu should appear
        PlayerPrefs.SetInt("levelRestarted", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void QuitGame()
    {
        PlayerPrefs.SetInt("levelRestarted", 0);
        Application.Quit();
    }

    void ChangeSensitivity()
    {
        camFreeLook.GetComponent<MouseSensitivitz>().lookSpeed = slider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("sensitivity", slider.GetComponent<Slider>().value);
    }

    void StartEndlessMode()
    {
        mainMenuCanvas.SetActive(false);
        Cursor.visible = false;
        PlayerPrefs.SetInt("gameMode", 1);
        cineMach.GetComponent<CinemachineFreeLook>().enabled = true;
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<ThirdPersonController>().enabled = true;
        Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
        paused = false;
        gameStarted = true;
    }

    void OpenMainMenu()
    {        
        PlayerPrefs.SetInt("levelRestarted", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ShowFuel()
    {
        if (jumped)
        {
            fuelbalken.gameObject.SetActive(true);
            fuelbalkenBG.gameObject.SetActive(true);
            if (PlayerManager.instance.floatFuel > 0 && fuelSave != PlayerManager.instance.floatFuel)
            {
                fuelbalkenTime = Time.deltaTime / (PlayerManager.instance.maxFloatFuel / 70);
                fuelbalken.transform.position = Vector3.Slerp(fuelbalken.transform.position, fuelbalkenEndPos, fuelbalkenTime);
                fuelbalken.transform.rotation = Quaternion.Lerp(fuelbalken.transform.rotation, fuelbalkenEndRot, fuelbalkenTime);
            }
            fuelSave = PlayerManager.instance.floatFuel;
            if (PlayerManager.instance.floatFuel <= 0)
            {

                fuelbalken.transform.position = fuelbalkenStartPos;
                fuelbalken.transform.rotation = fuelbalkenStartRot;
                fuelbalken.SetActive(false);
                fuelbalkenBG.SetActive(false);
                fuelbalkenTime = 0;
                jumped = false;
            }
        }
        else
        {
            fuelbalken.gameObject.SetActive(false);
            fuelbalkenBG.gameObject.SetActive(false);
        }

        if (PlayerManager.instance.isGrounded)
        {
            fuelbalken.transform.position = fuelbalkenStartPos;
            fuelbalken.transform.rotation = fuelbalkenStartRot;
            fuelbalken.SetActive(false);
            fuelbalkenBG.SetActive(false);
            fuelbalkenTime = 0;
            jumped = false;
        }
    }

    void ShowCurrentPowerups()
    {
        if (powerUpManager.currentPowerUps.Count == 0)
        {
            powerUpImageOne.gameObject.SetActive(false);
            powerUpImageTwo.gameObject.SetActive(false);
            powerUpImageThree.gameObject.SetActive(false);
        }
        else if (powerUpManager.currentPowerUps.Count == 1)
        {
            powerUpImageOne.gameObject.SetActive(true);
            powerUpImageTwo.gameObject.SetActive(false);
            powerUpImageThree.gameObject.SetActive(false);
            if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
            {
                powerUpImageOne.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "higherJump")
            {
                powerUpImageOne.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFloat")
            {
                powerUpImageOne.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "betterSloMo")
            {
                powerUpImageOne.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFreeze")
            {
                powerUpImageOne.sprite = freezeSprite;
            }
        }
        if (powerUpManager.currentPowerUps.Count == 2)
        {
            powerUpImageThree.gameObject.SetActive(false);
            powerUpImageOne.gameObject.SetActive(true);
            powerUpImageTwo.gameObject.SetActive(true);
            if (powerUpManager.currentPowerUps[1] == "secondClusterFreeze")
            {
                powerUpImageTwo.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "higherJump")
            {
                powerUpImageTwo.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "longerFloat")
            {
                powerUpImageTwo.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "betterSloMo")
            {
                powerUpImageTwo.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "longerFreeze")
            {
                powerUpImageTwo.sprite = freezeSprite;
            }
        }
        if (powerUpManager.currentPowerUps.Count == 3)
        {
            powerUpImageThree.gameObject.SetActive(true);
            powerUpImageOne.gameObject.SetActive(true);
            powerUpImageTwo.gameObject.SetActive(true);
            if (powerUpManager.currentPowerUps[2] == "secondClusterFreeze")
            {
                powerUpImageThree.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "higherJump")
            {
                powerUpImageThree.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "longerFloat")
            {
                powerUpImageThree.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "betterSloMo")
            {
                powerUpImageThree.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "longerFreeze")
            {
                powerUpImageThree.sprite = freezeSprite;
            }
        }

        if (player.GetComponent<PlayerShoot>().cubeSpawnCharges > 0)
        {
            consumableImage.gameObject.SetActive(true);
            consumableImage.sprite = consumableSprite;
            consumableCharges.GetComponent<Text>().text = player.GetComponent<PlayerShoot>().cubeSpawnCharges.ToString();
        }
        else
        {
            consumableImage.gameObject.SetActive(false);
            consumableCharges.GetComponent<Text>().text = "";
        }
    }

    void ShowCurrentPowerupsPause()
    {
        if (powerUpManager.currentPowerUps.Count == 0)
        {
            pausePowerUpImageOne.gameObject.SetActive(false);
            pausePowerUpImageTwo.gameObject.SetActive(false);
            pausePowerUpImageThree.gameObject.SetActive(false);
        }
        else if (powerUpManager.currentPowerUps.Count == 1)
        {
            pausePowerUpImageOne.gameObject.SetActive(true);
            pausePowerUpImageTwo.gameObject.SetActive(false);
            pausePowerUpImageThree.gameObject.SetActive(false);
            if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
            {
                pausePowerUpImageOne.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "higherJump")
            {
                pausePowerUpImageOne.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFloat")
            {
                pausePowerUpImageOne.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "betterSloMo")
            {
                pausePowerUpImageOne.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFreeze")
            {
                pausePowerUpImageOne.sprite = freezeSprite;
            }
        }
        if (powerUpManager.currentPowerUps.Count == 2)
        {
            pausePowerUpImageThree.gameObject.SetActive(false);
            pausePowerUpImageOne.gameObject.SetActive(true);
            pausePowerUpImageTwo.gameObject.SetActive(true);
            if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
            {
                pausePowerUpImageOne.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "higherJump")
            {
                pausePowerUpImageOne.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFloat")
            {
                pausePowerUpImageOne.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "betterSloMo")
            {
                pausePowerUpImageOne.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFreeze")
            {
                pausePowerUpImageOne.sprite = freezeSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "secondClusterFreeze")
            {
                pausePowerUpImageTwo.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "higherJump")
            {
                pausePowerUpImageTwo.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "longerFloat")
            {
                pausePowerUpImageTwo.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "betterSloMo")
            {
                pausePowerUpImageTwo.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "longerFreeze")
            {
                pausePowerUpImageTwo.sprite = freezeSprite;
            }
        }
        if (powerUpManager.currentPowerUps.Count == 3)
        {
            pausePowerUpImageThree.gameObject.SetActive(true);
            pausePowerUpImageOne.gameObject.SetActive(true);
            pausePowerUpImageTwo.gameObject.SetActive(true);
            if (powerUpManager.currentPowerUps[0] == "secondClusterFreeze")
            {
                pausePowerUpImageOne.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "higherJump")
            {
                pausePowerUpImageOne.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFloat")
            {
                pausePowerUpImageOne.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "betterSloMo")
            {
                pausePowerUpImageOne.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[0] == "longerFreeze")
            {
                pausePowerUpImageOne.sprite = freezeSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "secondClusterFreeze")
            {
                pausePowerUpImageTwo.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "higherJump")
            {
                pausePowerUpImageTwo.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "longerFloat")
            {
                pausePowerUpImageTwo.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "betterSloMo")
            {
                pausePowerUpImageTwo.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[1] == "longerFreeze")
            {
                pausePowerUpImageTwo.sprite = freezeSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "secondClusterFreeze")
            {
                pausePowerUpImageThree.sprite = secondSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "higherJump")
            {
                pausePowerUpImageThree.sprite = jumpSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "longerFloat")
            {
                pausePowerUpImageThree.sprite = floatSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "betterSloMo")
            {
                pausePowerUpImageThree.sprite = slomoSprite;
            }
            if (powerUpManager.currentPowerUps[2] == "longerFreeze")
            {
                pausePowerUpImageThree.sprite = freezeSprite;
            }
        }

        if (player.GetComponent<PlayerShoot>().cubeSpawnCharges > 0)
        {
            pauseConsumableImage.gameObject.SetActive(true);
            pauseConsumableImage.sprite = consumableSprite;
            pauseConsumableCharges.GetComponent<Text>().text = player.GetComponent<PlayerShoot>().cubeSpawnCharges.ToString();
        }
        else
        {
            pauseConsumableImage.gameObject.SetActive(false);
            pauseConsumableCharges.GetComponent<Text>().text = "";
        }
    }

    void ShowSlomo()
    {
        if (PlayerManager.instance.slowMode == true)
        {
            //slomo.SetActive(true);
            slowmoScreen.SetActive(true);

        }
        else
        {
            //slomo.SetActive(false);
            slowmoScreen.SetActive(false);
        }


    }

    void ShowFreezeTime()
    {
        if (player.GetComponent<PlayerShoot>().frozenCluster != null)
        {
            freezeBalken.SetActive(true);
            freezeBalkenBG.SetActive(true);           
            if (currentFreezeTime >= 0)
            {
                balkenTime = Time.deltaTime / freezetimer;
                freezeBalken.transform.position = Vector3.Slerp(freezeBalken.transform.position, balkenEndPos, balkenTime);
                freezeBalken.transform.rotation = Quaternion.Lerp(freezeBalken.transform.rotation, balkenEndRot, balkenTime);
                currentFreezeTime -= Time.deltaTime;
            }
            else
            {
                freezeBalken.transform.position = balkenStartPos;
                freezeBalken.transform.rotation = balkenStartRot;
                freezeBalken.SetActive(false);
                currentFreezeTime = freezetimer;
                freezeBalkenBG.SetActive(false);
                balkenTime = 0;
            }
        }
        else
        {
            freezeBalken.SetActive(false);
            freezeBalkenBG.SetActive(false);

        }

        if (player.GetComponent<PlayerShoot>().secondFrozenCluster != null)
        {
            secondfreezeBalken.SetActive(true);
            secondfreezeBalkenBG.SetActive(true);

            if (secondCurrentFreezeTime >= 0)
            {
                secondbalkenTime = Time.deltaTime / freezetimer;
                secondfreezeBalken.transform.position = Vector3.Slerp(secondfreezeBalken.transform.position, secondbalkenEndPos, secondbalkenTime);
                secondfreezeBalken.transform.rotation = Quaternion.Lerp(secondfreezeBalken.transform.rotation, secondbalkenEndRot, secondbalkenTime);
                secondCurrentFreezeTime -= Time.deltaTime;
            }
            else
            {
                secondfreezeBalken.transform.position = secondbalkenStartPos;
                secondfreezeBalken.transform.rotation = secondbalkenStartRot;
                secondfreezeBalken.SetActive(false);
                secondCurrentFreezeTime = freezetimer;
                freezeBalkenBG.SetActive(false);
                secondCurrentFreezeTime = freezetimer;
                secondbalkenTime = 0;
            }
        }
        else
        {
            secondfreezeBalken.SetActive(false);
            secondfreezeBalkenBG.SetActive(false);

        } 
    }

    void ToggleTutorial()
    {
        // 1 is on, 0 is off
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            PlayerPrefs.SetInt("tutorial", 1);
        }
        else if (PlayerPrefs.GetInt("tutorial") == 1)
        {
            PlayerPrefs.SetInt("tutorial", 0);
        }
    }
}
