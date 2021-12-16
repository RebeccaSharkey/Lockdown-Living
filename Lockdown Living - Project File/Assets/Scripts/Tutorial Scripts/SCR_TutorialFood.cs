using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_TutorialFood : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;
    [SerializeField] private Slider happinessSlider;
    [SerializeField] private Image happinessBar;
    [SerializeField] private Image reactionSprite;
    [SerializeField] private Sprite happy;
    [SerializeField] private GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Yeet");
            happinessSlider.value = 100;
            happinessBar.color = Color.green;
            reactionSprite.sprite = happy;
            hint.text = "Yey all better!";
            Destroy(door);
            Destroy(gameObject);
        }
    }
}
