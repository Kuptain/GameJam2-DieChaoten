using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject uiPrefab;
    [HideInInspector] public GameObject slomo, currentPowerup, freezeTime;
    GameObject player;
    float freezetimer, currentFreezeTime;

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
        slomo = uiPrefab.transform.GetChild(0).GetChild(3).gameObject;
        freezeTime = uiPrefab.transform.GetChild(0).GetChild(1).gameObject;
        player = ObjectManager.instance.player;
        currentFreezeTime = player.GetComponent<PlayerShoot>().meltingTime;
        freezetimer = currentFreezeTime;
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
        }
        else
            slomo.SetActive(false);
    }

    void ShowFreezeTime()
    {
        if(player.GetComponent<PlayerShoot>().frozenCluster != null)
        {
            freezeTime.SetActive(true);
            //TimeSpan interval = TimeSpan.FromSeconds(currentFreezeTime);
            //string timeInterval = interval.ToString();
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = timeInterval;
            //freezeTime.transform.GetChild(0).GetComponent<Text>().text = (((Mathf.Floor(currentFreezeTime / 60f)) % 60).ToString("00")) + ":" + (Mathf.Floor(currentFreezeTime % 60f).ToString("00")); ;
            freezeTime.transform.GetChild(0).GetComponent<Text>().text = ((Mathf.Floor(currentFreezeTime % 60f).ToString("00")) + ":" + (Mathf.Floor((currentFreezeTime * 100f) % 100).ToString("00")));
            currentFreezeTime -= Time.deltaTime;
        }
        else
        {
            freezeTime.SetActive(false);
            currentFreezeTime = freezetimer;
        }
    }
}
