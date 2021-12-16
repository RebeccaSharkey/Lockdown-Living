using UnityEngine;
using TMPro;

public class SCR_EnemyTut : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;

    private void OnTriggerEnter(Collider other)
    {
        hint.text = "Oh no! An Enemy! Quick press Q or Right Trigger on controller to attack.";
    }
}
