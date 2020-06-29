﻿using System;
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
    [SerializeField] GameObject freezeBalken, freezeBalkenGoal;
    public GameObject uiPrefab;
    public GameObject freezeTime;
    public bool showMenu;

    Text powerDescOne, powerDescTwo, powerDescThree, consDesc;
    public bool paused, gameStarted;

    PowerUpManager powerUpManager;
    GameObject player;
    GameObject cineMach;
    [HideInInspector] public float freezetimer, currentFreezeTime, secondCurrentFreezeTime;

    Vector3 balkenStartPos, balkenEndPos;
    Quaternion balkenStartRot, balkenEndRot;

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
        if (showMenu)
        {
            PlayerPrefs.SetInt("showMenu", 0);
        }
        else
        {
            PlayerPrefs.SetInt("showMenu", 1);
        }

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
        ShowSlomo();
        ShowFreezeTime();
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false && mainMenuCanvas.activeSelf == false && gameOverCanvas.activeSelf == false)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused && mainMenuCanvas.activeSelf == false && gameOverCanvas.activeSelf == false)
        {
            ResumeGame();
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
        Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
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
            consDesc.text = "Hold Q to find a spot for a platform, then release Q to place it. " + player.GetComponent<PlayerShoot>().cubeSpawnCharges.ToString() + " charge(s) left.";
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

    void StartEndlessMode()
    {
        mainMenuCanvas.SetActive(false);
        Cursor.visible = false;
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        cineMach.GetComponent<CinemachineFreeLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.transform.parent.GetComponent<CameraController>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;
        PlayerPrefs.SetInt("gameMode", 1);
        player.GetComponent<PlayerShoot>().enabled = true;
        gameStarted = true;
        paused = false;
    }

    void OpenMainMenu()
    {
        /* PlayerPrefs.SetInt("levelRestarted", 0);
         mainMenuCanvas.SetActive(true);
         gameOverCanvas.SetActive(false);
         pauseCanvas.SetActive(false);
         Camera.main.transform.parent.GetComponent<CameraController>().enabled = false;
         player.GetComponent<ThirdPersonController>().enabled = false;
         player.GetComponent<PlayerShoot>().enabled = false;
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         // 1 is on, 0 is off
         if (PlayerPrefs.GetInt("tutorial") == 0)
         {
             mainMenuCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
         }
         else if (PlayerPrefs.GetInt("tutorial") == 1)
         {
             mainMenuCanvas.transform.GetChild(4).gameObject.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
         }*/
        PlayerPrefs.SetInt("levelRestarted", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ShowSlomo()
    {
        if (CubeManager.instance.slowMode == true)
        {
            slomo.SetActive(true);
            slowmoScreen.SetActive(true);

        }
        else
        {
            slomo.SetActive(false);
            slowmoScreen.SetActive(false);
        }


    }

    void ShowFreezeTime()
    {
        if (player.GetComponent<PlayerShoot>().frozenCluster != null)
        {
            freezeBalken.SetActive(true);
            //TimeSpan interval = TimeSpan.FromSeconds(currentFreezeTime);
            //string timeInterval = interval.ToString();
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = timeInterval;
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = (((Mathf.Floor(currentFreezeTime / 60f)) % 60).ToString("00")) + ":" + (Mathf.Floor(currentFreezeTime % 60f).ToString("00")); ;
            /*if (currentFreezeTime >= 0)
            {
                freezeTime.transform.GetChild(0).GetComponent<Text>().text = ((Mathf.Floor(currentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((currentFreezeTime * 100f) % 100).ToString("00")));
                currentFreezeTime -= Time.deltaTime;
            }
            else
            {
                freezeTime.transform.GetChild(0).GetComponent<Text>().text = "";
            }
            if (player.GetComponent<PlayerShoot>().secondFrozenCluster != null)
            {
                freezeTime.transform.GetChild(1).GetComponent<Text>().text = ((Mathf.Floor(secondCurrentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((secondCurrentFreezeTime * 100f) % 100).ToString("00")));
                secondCurrentFreezeTime -= Time.deltaTime;
            }
            else
            {

                freezeTime.transform.GetChild(1).GetComponent<Text>().text = "";
                freezeTime.transform.GetChild(1).gameObject.SetActive(false);

            }*/
            print("dddd");
            if (currentFreezeTime >= 0)
            {
                //freezeTime.transform.GetChild(0).GetComponent<Text>().text = ((Mathf.Floor(currentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((currentFreezeTime * 100f) % 100).ToString("00")));
                freezeBalken.transform.position = Vector3.Slerp(freezeBalken.transform.position, balkenEndPos, currentFreezeTime);
                freezeBalken.transform.rotation = Quaternion.Lerp(freezeBalken.transform.rotation, balkenEndRot, currentFreezeTime);
                currentFreezeTime -= Time.deltaTime;
            }
            else
            {
                //freezeTime.transform.GetChild(0).GetComponent<Text>().text = "";
                freezeBalken.SetActive(false); 
                currentFreezeTime = freezetimer;
                secondCurrentFreezeTime = freezetimer;
            }
        }
       /* else
        {
            freezeTime.SetActive(false);
            currentFreezeTime = freezetimer;
            secondCurrentFreezeTime = freezetimer;
        }

        if (secondCurrentFreezeTime >= 0 && player.GetComponent<PlayerShoot>().secondFrozenCluster != null)
        {
            freezeTime.transform.GetChild(1).GetComponent<Text>().text = ((Mathf.Floor(secondCurrentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((secondCurrentFreezeTime * 100f) % 100).ToString("00")));
            secondCurrentFreezeTime -= Time.deltaTime;
        }
        else
        {
            freezeTime.transform.GetChild(1).GetComponent<Text>().text = "";

        }  */      
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