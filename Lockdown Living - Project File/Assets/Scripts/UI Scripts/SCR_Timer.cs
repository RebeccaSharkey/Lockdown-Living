using UnityEngine;
using TMPro;

public class SCR_Timer : MonoBehaviour
{
    public static SCR_Timer gTimer;

    [SerializeField] float timer = 720f;
    [SerializeField] private TextMeshProUGUI[] tTimer = new TextMeshProUGUI[2];
    [SerializeField] private GameObject[] iTimer = new GameObject[2];
    private int useTimer = 0;

    void Start()
    {
        if(SCR_GameManager.gameManager.GetPlayerAmount() == 2)
        {
            useTimer = 0;
        }
        else
        {
            useTimer = 1;
        }

        int minutes = (int)(timer % 60);
        int hours = (int)((timer / 60) % 60);

        string timeString = string.Format("{00:00}:{01:00}", hours, minutes);

        tTimer[useTimer].text = timeString;
    }

    void Update()
    {
        if(SCR_GameManager.gameManager.GetScene() == 1 || SCR_GameManager.gameManager.GetScene() == 2 || SCR_GameManager.gameManager.GetScene() == 4)
        {
            if (SCR_GameManager.gameManager.GetPlaying())
            {
                timer += Time.deltaTime * 2f;

                int minutes = (int)(timer % 60);
                int hours = (int)((timer / 60) % 60);

                string timeString = string.Format("{00:00}:{01:00}", hours, minutes);

                tTimer[useTimer].text = timeString;

                if (timeString == "14:59")
                {
                    SCR_GameManager.gameManager.Pause(true, -1);
                }
                else if (timeString == "19:59")
                {
                    SCR_GameManager.gameManager.Pause(true, -1);
                }
                else if (timeString == "20:10")
                {
                    SCR_GameManager.gameManager.Pause(true, -1);
                }
            }
        }        
    }

    public void ToggleTimer()
    {        
        if(iTimer[useTimer].activeInHierarchy)
        {
            iTimer[useTimer].SetActive(false);
        }
        else
        {
            iTimer[useTimer].SetActive(true);
        }
    }

    public void ToggleTimer(bool toggle)
    {
        if (iTimer[useTimer].activeInHierarchy != toggle)
        {
            iTimer[useTimer].SetActive(toggle);
        }
    }

    public void SetTimer(float time)
    {
        timer = time;
    }
}
