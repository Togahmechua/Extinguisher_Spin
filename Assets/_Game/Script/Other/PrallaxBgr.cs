using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrallaxBgr : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField]
    Vector2 targetPos, startPos;
/*
    private Vector2 targetPos = new Vector2(-34.36f, 0f);
    private Vector2 startPos = new Vector2(34.34f, 0f);*/


    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
        {
            transform.position = startPos;
        }
    }
}
