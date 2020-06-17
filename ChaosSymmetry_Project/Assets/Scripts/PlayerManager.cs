using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Slider fuelSlider;
    public static PlayerManager instance;

    public float floatFuel;
    public float maxFloatFuel = 40f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        
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
}
