using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int maxEnemyCount = 10;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private EnemyCharacter enemyPrefab1;
    [SerializeField] private EnemyCharacter enemyPrefab2;
    [SerializeField] private EnemyCharacter enemyPrefab3;
    [SerializeField] private EnemyCharacter enemyPrefab4;

    [SerializeField] private int level = 1; // difficulté actuelle

    [SerializeField] private PlayerCharacter player;


    private int _enemyKilledCount = 0;
    private float _spawnTimer = 0f;
    private List<EnemyCharacter> _enemyCharacters = new();

    public int EnemyKilledCount => _enemyKilledCount;

    private IEnumerator Start()
    {
        while (true)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= spawnInterval)
            {
                _spawnTimer = 0f;

                if (_enemyCharacters.Count >= maxEnemyCount)
                {
                    yield return null;
                    continue;
                }

                EnemyCharacter prefabToSpawn = PickEnemyPrefab();

                Vector3 spawnPos = new Vector3(
                    Random.Range(-12f, 12f),
                    0f,
                    Random.Range(-12f, 12f)
                );

                var enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, transform);
                enemy.gameObject.SetActive(true);

                _enemyCharacters.Add(enemy);
                enemy.AddDestroyListener(OnEnemyCharacterDestroyed);
            }

            yield return null;
        }
    }

    private EnemyCharacter PickEnemyPrefab()
    {
        float roll = Random.value;

        // ----- DÉBUT : uniquement enemy 1
        if (player.level < 5)
            return enemyPrefab1;

        // ----- NIVEAU MOYEN : enemy 2 arrive
        if (player.level < 10)
        {
            if (roll < 0.7f) return enemyPrefab1;
            return enemyPrefab2;
        }

        // ----- PLUS DUR : enemy 3 arrive
        if (player.level < 15)
        {
            if (roll < 0.4f) return enemyPrefab1;
            if (roll < 0.7f) return enemyPrefab2;
            return enemyPrefab3;
        }

        if (player.level < 20)
        {
            if (roll < 0.2f) return enemyPrefab1;
            if (roll < 0.4f) return enemyPrefab2;
            if (roll < 0.9f) return enemyPrefab3;
            return enemyPrefab4;
        }

        if (player.level < 25)
        {
            if (roll < 0.15f) return enemyPrefab1;
            if (roll < 0.3f) return enemyPrefab2;
            if (roll < 0.75f) return enemyPrefab3;
            return enemyPrefab4;
        }

        
        if (roll < 0.3f) return enemyPrefab2;
        if (roll < 0.6f) return enemyPrefab3;
        return enemyPrefab4;
    }

    private void OnEnemyCharacterDestroyed(EnemyCharacter enemy)
    {
        _enemyCharacters.Remove(enemy);
        _enemyKilledCount++;
    }
}
