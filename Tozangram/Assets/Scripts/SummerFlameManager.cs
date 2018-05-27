using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SummerFlameManager : MonoBehaviour {

    [SerializeField] private GameObject crimbableWall;
    private TilemapRenderer crimbableRender;

    private void Start()
    {
        crimbableRender = crimbableWall.GetComponent<TilemapRenderer>();
    }

    public void Desabled()
    {
        crimbableRender.enabled = !enabled;
    }

    public void Enabled()
    {
        crimbableRender.enabled = enabled;
    }
}
