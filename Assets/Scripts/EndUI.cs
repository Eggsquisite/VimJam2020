using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public GameObject endUI;
    public Text secondsSurvived;

    private float timer = 0.0f;
    private string niceTime;
    private bool gameEnd = false;

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
            int minutes = Mathf.FloorToInt(Time.time / 60F);
            int seconds = Mathf.FloorToInt(Time.time - minutes * 60);
            niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    void EnableUI()
    {
        gameEnd = true;
        endUI.SetActive(true);
        secondsSurvived.text = niceTime;
    }
}
