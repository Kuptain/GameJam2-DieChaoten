using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour
{
    public bool lightUp;
    bool shining;
    Light lightComp;
    public float fadeSpeed;
    Color startColor;
    Color endColor;
    ObjectManager objectManager;
    ParticleSystem barbieParticle;
    bool doOnce;

    // Start is called before the first frame update
    void Start()
    {
        lightComp = GetComponent<Light>();
        lightUp = false;
        fadeSpeed = 3f;
        objectManager = ObjectManager.instance;
        barbieParticle = objectManager.barbieParticle;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.C))
        {
            lightUp = true;
            //startColor = new Color(objectManager.glowMat.color.r, objectManager.glowMat.color.g, objectManager.glowMat.color.b, 0);
            //endColor = new Color(objectManager.glowMat.color.r, objectManager.glowMat.color.g, objectManager.glowMat.color.b, 40);
        }*/

        if (lightUp)
        {
            if (doOnce == false)
            {
                Instantiate(barbieParticle, transform.position, Quaternion.Euler(-90, 0, 0));
                doOnce = true;
            }
            if (lightComp.intensity <= 29 && shining == false)
            {
                lightComp.intensity = Mathf.Lerp(lightComp.intensity, 30, Time.deltaTime * fadeSpeed);

            }

            if (lightComp.intensity > 29 && shining == false)
            {
                shining = true;
            }

            if (shining)
            {
                lightComp.intensity = Mathf.Lerp(lightComp.intensity, 0, Time.deltaTime * fadeSpeed);
            }

            if (lightComp.intensity <= 1 && shining == true)
            {
                lightUp = false;
                lightComp.intensity = 0;
                shining = false;
                doOnce = false;
            }
        }
        /*
        if (lightUp)
        {
            if (objectManager.glowMat.color.a <= 29 && shining == false)
            {
                //glowMat.color = new Color(glowMat.color.r, glowMat.color.g, glowMat.color.b, Mathf.Lerp(glowMat.color.a, 30, Time.deltaTime * fadeSpeed));
                objectManager.glowMat.color = Color.Lerp(objectManager.glowMat.color, endColor, Time.deltaTime * fadeSpeed);
            }

            if (objectManager.glowMat.color.a > 29 && shining == false)
            {
                shining = true;
                print("hhhhh");
            }

            if (shining)
            {
                //glowMat.color = new Color(glowMat.color.r, glowMat.color.g, glowMat.color.b, Mathf.Lerp(glowMat.color.a, 0, Time.deltaTime * fadeSpeed));
                objectManager.glowMat.color = Color.Lerp(objectManager.glowMat.color, startColor, Time.deltaTime * fadeSpeed);
            }

            if (objectManager.glowMat.color.a <= 1 && shining == true)
            {
                lightUp = false;
                //glowMat.color = new Color(glowMat.color.r, glowMat.color.g, glowMat.color.b, 0);
                print("ffff");
                objectManager.glowMat.color = startColor;
                shining = false;
            }
        }*/
    }
}
