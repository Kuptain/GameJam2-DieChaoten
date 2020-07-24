using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class MouseSensitivitz : MonoBehaviour
{
    public float lookSpeed = 0.00001f;
    private CinemachineFreeLook freeLookComponent;
    float xAxis, yAxis;


    public void Start()
    {
        freeLookComponent = GetComponent<CinemachineFreeLook>();
        xAxis = freeLookComponent.m_XAxis.m_MaxSpeed;
        yAxis = freeLookComponent.m_YAxis.m_MaxSpeed;
        lookSpeed = PlayerPrefs.GetFloat("sensitivity", 50);

    }


    // Update is called once per frame
    void Update()
    {
        //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
        //Vector2 lookMovement = context.ReadValue<Vector2>().normalized;
        //lookMovement.y = InvertY ? -lookMovement.y : lookMovement.y;

        // This is because X axis is only contains between -180 and 180 instead of 0 and 1 like the Y axis
        //lookMovement.x = lookMovement.x * 180f;

        //Ajust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
        freeLookComponent.m_XAxis.m_MaxSpeed = xAxis * lookSpeed * Time.deltaTime;
        freeLookComponent.m_YAxis.m_MaxSpeed = yAxis * lookSpeed * Time.deltaTime;
    }

}
