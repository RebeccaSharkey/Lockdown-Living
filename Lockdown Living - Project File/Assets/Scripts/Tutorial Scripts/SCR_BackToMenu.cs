using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_BackToMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("Start Menu");
    }
}
