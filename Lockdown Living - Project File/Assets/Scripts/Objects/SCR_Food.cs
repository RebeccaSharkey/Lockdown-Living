using UnityEngine;

public class SCR_Food : MonoBehaviour
{
    [SerializeField] private float oBuff = 0;
    public GameObject currentPlayer = null;
    public GameObject spawner;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.transform.root.gameObject;
            if (currentPlayer != null)
            {
                currentPlayer.GetComponent<SCR_PlayerData>().AddHappiness(oBuff);
                spawner.GetComponent<SCR_FoodSpawners>().BeginSpawning((float)Random.Range(30, 51));
                Destroy(gameObject);
            }
        }
    }
}
