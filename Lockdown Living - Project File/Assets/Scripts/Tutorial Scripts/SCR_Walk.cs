using UnityEngine;
using TMPro;

public class SCR_Walk : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;

    private void OnTriggerEnter(Collider other)
    {
        hint.text = "Use WASD or the left analog stick to move.";
    }
}
