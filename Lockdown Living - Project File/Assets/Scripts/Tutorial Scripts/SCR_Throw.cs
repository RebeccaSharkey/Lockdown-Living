using UnityEngine;
using TMPro;

public class SCR_Throw : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;

    private void OnTriggerEnter(Collider other)
    {
        hint.text = "The sword does more damage! Throw the pillow by pressing E or X/Square to pick up the sword.";
    }
}
