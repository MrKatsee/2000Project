using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_EnemySpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    public bool SpawnStart;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count; //적 생성수
        public float delay; //생성 간격시간
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBtwnWaves = 5f;
    private float waveCountdown = 0f;
    
    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        SpawnStart = true;
        if (spawnPoints.Length == 0)
        {
            Debug.Log("Error : No Spawn Points Referenced");
        }
        waveCountdown = timeBtwnWaves;

    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            //Enemy가 아직 있는지 확인
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else if (SpawnStart)
        {
            waveCountdown -= Time.deltaTime;
        }
    }




    void WaveCompleted()
    {
        //Begin new wave
        Debug.Log("WaveCompleted");

        state = SpawnState.COUNTING;
        waveCountdown = timeBtwnWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed All Waves - 반복함");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(_wave.delay);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn Enemy
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}