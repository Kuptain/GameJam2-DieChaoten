using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [HideInInspector] public GameObject slomo, consumable, currentPowerupOne, currentPowerupTwo, currentPowerupThree, slowmoScreen;
    [HideInInspector] public GameObject mainMenuCanvas, pauseCanvas, ingameCanvas, gameOverCanvas;
    public GameObject uiPrefab;
    public GameObject freezeTime;
    public bool showMenu;

    bool paused;

    GameObject player;
    [HideInInspector] public float freezetimer, currentFreezeTime, secondCurrentFreezeTime;

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

        if(PlayerPrefs.GetInt("levelRestarted") == 1)
        {
            PlayerPrefs.SetInt("showMenu", 1);
        }

        mainMenuCanvas = uiPrefab.transform.GetChild(1).gameObject;
        pauseCanvas = uiPrefab.transform.GetChild(2).gameObject;
        ingameCanvas = uiPrefab.transform.GetChild(0).gameObject;
        gameOverCanvas = uiPrefab.transform.GetChild(3).gameObject;
        gameOverCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        slomo = ingameCanvas.transform.GetChild(1).GetChild(2).gameObject;
        slowmoScreen = ingameCanvas.transform.GetChild(3).gameObject;

        //freezeTime = uiPrefab.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        player = ObjectManager.instance.player;
        currentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime;
        secondCurrentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime;
        freezetimer = currentFreezeTime;
        currentPowerupOne = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
        currentPowerupTwo = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        currentPowerupThree = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(2).gameObject;
        consumable = ingameCanvas.transform.GetChild(1).GetChild(1).GetChild(3).gameObject;
        mainMenuCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => StartGame());
        mainMenuCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => StartEndlessMode());
        mainMenuCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitGame());
        gameOverCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => RestartGame());
        gameOverCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => OpenMainMenu());
        gameOverCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitGame());
        pauseCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => ResumeGame());
        pauseCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => OpenMainMenu());
        pauseCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => QuitGame());


        // showmnenu 0 =  true, 1 is no menu
        if (PlayerPrefs.GetInt("showMenu") == 0)
        {
            ingameCanvas.SetActive(false);
            player.GetComponent<ThirdPersonController>().enabled = false;
            player.GetComponent<PlayerShoot>().enabled = false;
            Cursor.visible = true;
        }
        else
        {
            mainMenuCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        else if(Input.GetKeyDown(KeyCode.Escape) && paused && mainMenuCanvas.activeSelf == false && gameOverCanvas.activeSelf == false)
        {
            ResumeGame();
        }
    }

    void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        Cursor.visible = false;
        pauseCanvas.SetActive(false);
        ingameCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
        PlayerPrefs.SetInt("gameMode", 0);
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ingameCanvas.SetActive(false);
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<PlayerShoot>().enabled = false;
        pauseCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        paused = false;
        ingameCanvas.SetActive(true);
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        print("hhh");
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
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<ThirdPersonController>().enabled = true;
        PlayerPrefs.SetInt("gameMode", 1);
        player.GetComponent<PlayerShoot>().enabled = true;
    }

    void OpenMainMenu()
    {
        PlayerPrefs.SetInt("levelRestarted", 0);
        mainMenuCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<PlayerShoot>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
        if (player.GetComponent<PlayerShoot>().frozenCluster != null || player.GetComponent<PlayerShoot>().secondFrozenCluster != null)
        {
            freezeTime.SetActive(true);
            //TimeSpan interval = TimeSpan.FromSeconds(currentFreezeTime);
            //string timeInterval = interval.ToString();
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = timeInterval;
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = (((Mathf.Floor(currentFreezeTime / 60f)) % 60).ToString("00")) + ":" + (Mathf.Floor(currentFreezeTime % 60f).ToString("00")); ;
            if (currentFreezeTime >= 0)
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
                //freezeTime.transform.GetChild(1).GetComponent<Text>().text = ((Mathf.Floor(secondCurrentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((secondCurrentFreezeTime * 100f) % 100).ToString("00")));
                secondCurrentFreezeTime -= Time.deltaTime;
            }
            else
            {

                //freezeTime.transform.GetChild(1).GetComponent<Text>().text = "";
                //freezeTime.transform.GetChild(1).gameObject.SetActive(false);

            }
        }
        else
        {
            freezeTime.SetActive(false);
            currentFreezeTime = freezetimer;
            secondCurrentFreezeTime = freezetimer;
        }
    }
}
