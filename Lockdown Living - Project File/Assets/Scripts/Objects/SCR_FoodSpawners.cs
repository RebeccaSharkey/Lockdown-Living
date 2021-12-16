using System.Collections;
using UnityEngine;

public class SCR_FoodSpawners : MonoBehaviour
{
    [SerializeField] private GameObject[] food;

    private void Awake()
    {
        spawnFood();
    }

    public void spawnFood()
    {
        GameObject foodObject = Instantiate(food[Random.Range(0, food.Length)], transform.position, Quaternion.identity);
        foodObject.GetComponent<SCR_Food>().spawner = gameObject;
    }

    public void BeginSpawning(float time)
    {
        StartCoroutine(StartSpawn(time));
    }

    public IEnumerator StartSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        spawnFood();
    }
}
