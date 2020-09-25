using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public GameObject endUI;
    public Text secondsSurvived;

    private float timer = 0.0f;
    private int seconds;
    private bool gameEnd;

    private void OnEnable()
    {
        End.OnEnd += EnableUI;
    }

    private void OnDisable()
    {
        End.OnEnd -= EnableUI;
    }


    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            timer += Time.deltaTime;
            seconds = (int)timer % 60;
        }
    }

    void EnableUI()
    {
        gameEnd = true;
        endUI.SetActive(true);
        secondsSurvived.text = seconds.ToString();
    }
}
