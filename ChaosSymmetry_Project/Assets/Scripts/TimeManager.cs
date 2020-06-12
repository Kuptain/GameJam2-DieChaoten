
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLenght = 4f;

    public float fastFactor = 2f;
    public float fastLength = 4f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoSlowmotion();
        }

        Time.timeScale += (1f / slowdownLenght) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (Input.GetKeyDown(KeyCode.F))
        {
            DoFastforward();
        }


    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void DoFastforward()
    {
        Time.timeScale = fastFactor;
    }
}
