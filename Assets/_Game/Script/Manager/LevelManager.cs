using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject level;
    [HideInInspector] public Level curLevel;
    public GameObject obstacleLoop;

    public void SpawnLevel()
    {
        GameObject newLevel = Instantiate(level, transform.position, Quaternion.identity);
        curLevel = newLevel.GetComponent<Level>();
        obstacleLoop.gameObject.SetActive(true);
    }

    public void DespawmLevel()
    {
        if (curLevel != null)
        {
            obstacleLoop.gameObject.SetActive(false);
            Destroy(curLevel.gameObject);
        }
    }
}