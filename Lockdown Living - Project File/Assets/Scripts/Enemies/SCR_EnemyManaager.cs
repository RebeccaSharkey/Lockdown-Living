using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemyManaager : MonoBehaviour
{

    [SerializeField] public GameObject[] spawners;

    [SerializeField] private int enemyAmount = 4;
    private List<GameObject> enimies = new List<GameObject>();
    private bool canSpawn = true;

    void Update()
    {
        if (SCR_GameManager.gameManager.GetPlaying())
        {
            if (enimies.Count < enemyAmount && canSpawn)
            {
                canSpawn = false;
                StartCoroutine(Spawn());
            }
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        enimies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enimies.Remove(enemy);
    }

    IEnumerator Spawn()
    {
        spawners[Random.Range(0, spawners.Length)].GetComponent<SCR_EnemySpawner>().SpawnEnemy();
        yield return new WaitForSeconds(0.5f);
        canSpawn = true;
    }

}
