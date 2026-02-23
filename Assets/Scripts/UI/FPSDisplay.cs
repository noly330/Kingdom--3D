using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime = 0f;
    private float coldTime = 0.5f;

    void Awake()
    {
        fpsText = GetComponent<Text>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        coldTime -= Time.deltaTime;
        if (coldTime <= 0f)
        {
            coldTime = 0.5f;
            fpsText.text = $"FPS:{Mathf.Ceil(fps)}";

            if (fps < 30)
                fpsText.color = Color.red;
            else if (fps < 60)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.green;
        }

    }
}
