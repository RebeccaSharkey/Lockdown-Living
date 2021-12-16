using UnityEngine;
using TMPro;

public class SCR_PickUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;

    private void OnTriggerEnter(Collider other)
    {
        hint.text = "Pick up the pillow by walking over it.";
    }
}
