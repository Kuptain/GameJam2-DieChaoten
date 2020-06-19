using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject uiPrefab;
    [HideInInspector] public GameObject slomo, currentPowerup, freezeTime, slowmoScreen;
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
        slomo = uiPrefab.transform.GetChild(0).GetChild(1).GetChild(2).gameObject;
        slowmoScreen = uiPrefab.transform.GetChild(0).GetChild(3).gameObject;

        freezeTime = uiPrefab.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        player = ObjectManager.instance.player;
        currentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime; 
        secondCurrentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime;
        freezetimer = currentFreezeTime;
        currentPowerup = uiPrefab.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        ShowSlomo();
        ShowFreezeTime();
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
        if(player.GetComponent<PlayerShoot>().frozenCluster != null || player.GetComponent<PlayerShoot>().secondFrozenCluster != null)
        {
            freezeTime.SetActive(true); 
            //TimeSpan interval = TimeSpan.FromSeconds(currentFreezeTime);
            //string timeInterval = interval.ToString();
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = timeInterval;
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = (((Mathf.Floor(currentFreezeTime / 60f)) % 60).ToString("00")) + ":" + (Mathf.Floor(currentFreezeTime % 60f).ToString("00")); ;
            if(currentFreezeTime >= 0)
            {
                freezeTime.transform.GetChild(0).GetComponent<Text>().text = ((Mathf.Floor(currentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((currentFreezeTime * 100f) % 100).ToString("00")));
                currentFreezeTime -= Time.deltaTime;
            }
            else
            {
                freezeTime.transform.GetChild(0).GetComponent<Text>().text = "";
            }
            if(player.GetComponent<PlayerShoot>().secondFrozenCluster != null)
            {
                freezeTime.transform.GetChild(1).GetComponent<Text>().text = ((Mathf.Floor(secondCurrentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((secondCurrentFreezeTime * 100f) % 100).ToString("00")));
                secondCurrentFreezeTime -= Time.deltaTime;
            }
            else
            {
                freezeTime.transform.GetChild(1).GetComponent<Text>().text = "";
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
