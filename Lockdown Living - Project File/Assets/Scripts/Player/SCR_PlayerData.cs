using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_PlayerData : MonoBehaviour
{
    public float playerHappiness = 100f;
    private float decreaseSpeed = 2.5f;

    private int stars = 0;

    [SerializeField] private Slider happinessSlider;
    [SerializeField] private Image happinessBar;

    [SerializeField] private TextMeshProUGUI starCount;
    [SerializeField] private GameObject star;

    [SerializeField] private Image reactionSprite;

    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject ReadyPanel;
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject PausedPanel;

    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite mehSprite;
    [SerializeField] private Sprite sadSprite;

    private bool isReady = false;

    private int gScene;

    [SerializeField] private GameObject readyButton;
    [SerializeField] private GameObject resumeButton;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectStar;
    [SerializeField] private AudioClip loseStar;

    #region Getters and Setters
    public int GetScene() => gScene;

    public float GetPlayerHappiness() => playerHappiness;
    public void SetPlayerHappiness(float points) => playerHappiness = points;

    public int GetStars() => stars;
    public void SetStars(int newStarAmount) => stars = newStarAmount;
    
    public bool GetIsReady() => isReady;
    public void SetIsReady(bool ready) => isReady = ready;

    public GameObject GetMainPanel() => MainPanel;
    public GameObject GetReadyPanel() => ReadyPanel;
    public GameObject GetPauseMenuPanel() => PauseMenuPanel;
    public GameObject GetPausedPanel() => PausedPanel;

    public GameObject GetReadyButton() => readyButton;
    public GameObject GetResumeButton() => resumeButton;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void BeginGame(int scene)
    {
        gScene = scene;

        switch (scene)
        {
            case 1:
                MainPanel.SetActive(true);
                gameObject.GetComponent<SCR_PlayerController>().SetCanThrow(false);
                happinessSlider.value = playerHappiness; 
                happinessBar.color = Color.green;
                reactionSprite.sprite = happySprite;
                break;
            case 2:
                MainPanel.SetActive(true);
                gameObject.GetComponent<SCR_PlayerController>().SetCanThrow(true);
                StartCoroutine(LoseHappiness());
                break;
            case 4:
                MainPanel.SetActive(true);
                gameObject.GetComponent<SCR_PlayerController>().SetCanThrow(false);
                break;
        }
    }

    public void AddHappiness(float points)
    {
        playerHappiness += points;
        happinessSlider.value = playerHappiness;
        CheckHappiness();
    }

    private void CheckHappiness()
    {
        switch (gScene)
        {
            case 1:
                if(playerHappiness % 50 == 0)
                {
                    AddStar();
                }
                break;
            case 2:

                if (playerHappiness > 70)
                {
                    happinessBar.color = Color.green;
                    reactionSprite.sprite = happySprite;
                }
                else if (playerHappiness > 40)
                {
                    happinessBar.color = Color.yellow;
                    reactionSprite.sprite = mehSprite;
                }
                else
                {
                    happinessBar.color = Color.red;
                    reactionSprite.sprite = sadSprite;
                }
                break;
        }
    }

    public void AddStar()
    {
        stars++;
        starCount.SetText(" = " + stars.ToString());
        star.GetComponent<Animator>().SetTrigger("AddStar");
        audioSource.clip = collectStar;
        audioSource.volume = 0.3f;
        audioSource.Play();
    }

    public void RemoveStar()
    {
        if(stars > 0)
        {
            stars--;
            starCount.SetText(" = " + stars.ToString());
            star.GetComponent<Animator>().SetTrigger("LoseStar");
            audioSource.clip = loseStar;
            audioSource.volume = 1f;
            audioSource.Play();
        }        
    }

    IEnumerator LoseHappiness()
    {
        int count = 0;
        for (; ; )
        {
            count++;
            if (playerHappiness > 0)
            {
                playerHappiness -= decreaseSpeed;
                happinessSlider.value = playerHappiness;
                CheckHappiness();
                if(playerHappiness > 100 && (count % 2 == 0))
                {
                    AddStar();
                }
            }
            else
            {
                if(stars > 0)
                RemoveStar();
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
