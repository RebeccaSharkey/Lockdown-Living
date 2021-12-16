using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_Eat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;
    [SerializeField] private Slider happinessSlider;
    [SerializeField] private Image happinessBar;
    [SerializeField] private Image reactionSprite;
    [SerializeField] private Sprite meh;

    private void OnTriggerEnter(Collider other)
    {
        happinessSlider.value = 80;
        happinessBar.color = Color.yellow;
        reactionSprite.sprite = meh;
        hint.text = "You've grown unhappy with this long tutorial :( Maybe that doughnut will cheer you up?";
    }
}
