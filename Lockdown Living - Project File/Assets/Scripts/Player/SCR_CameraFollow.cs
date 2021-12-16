using UnityEngine;

public class SCR_CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.LookAt(player);    
    }
}
