﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SummerFlameManager : MonoBehaviour {

    [SerializeField] private GameObject crimbableWall;
    private TilemapRenderer crimbableRender;

    private void Awake()
    {
        crimbableRender = crimbableWall.GetComponent<TilemapRenderer>();
    }

    public void Disabled()
    {
        crimbableRender.enabled = !enabled;
    }

    public void Enabled()
    {
        crimbableRender.enabled = enabled;
    }
}
