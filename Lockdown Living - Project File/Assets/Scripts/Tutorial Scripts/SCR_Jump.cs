using UnityEngine;
using TMPro;

public class SCR_Jump : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;

    private void OnTriggerEnter(Collider other)
    {
        hint.text = "Press Space or A/X on the controller to jump";
    }
}
