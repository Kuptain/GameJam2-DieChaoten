using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource slomoOn;
    public AudioSource slomoOff;
    public AudioSource landing;
    public GameObject soundParent;

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
        soundParent = GameObject.FindGameObjectWithTag("SoundParent");
        slomoOff = soundParent.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        slomoOn = soundParent.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        landing = soundParent.transform.GetChild(2).gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
