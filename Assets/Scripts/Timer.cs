using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // attributes
    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] float starterTime;
    float remaningTime;
    int minutes;
    int secondes;

    // methods
    private void Start()
    {
        remaningTime = starterTime;
        TimerText.color = Color.white;

    }



    void Update()
    {
        IncrementTimer();
        TimerDisplay();
    }

    public void IncrementTimer()
    {
        if (remaningTime > 0)
            remaningTime -= Time.deltaTime;
        minutes = Mathf.FloorToInt(remaningTime / 60);
        secondes = Mathf.FloorToInt(remaningTime % 60);

        

    }

    public void TimerDisplay()
    {

        if (remaningTime < 0)
        {
            remaningTime = 0;
            TimerText.color = Color.red;
        }
            
        
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, secondes);
    }








}
