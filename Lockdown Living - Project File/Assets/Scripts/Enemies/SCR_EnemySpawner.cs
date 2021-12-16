using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private SCR_EnemyManaager eManager;
    private bool empty = true;

    private void Awake()
    {
        eManager = transform.root.gameObject.GetComponent<SCR_EnemyManaager>();
    }

    public void SpawnEnemy()
    {
        if(empty)
        {
            GameObject placeHolder = Instantiate(enemy, transform.position, Quaternion.identity);
            placeHolder.GetComponent<SCR_EnemyController>().SetEManager(eManager);
            eManager.AddEnemy(placeHolder);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        empty = false;
    }

    private void OnTriggerStay(Collider other)
    {
        empty = false;
    }

    private void OnTriggerExit(Collider other)
    {
        empty = true;
    }

}
