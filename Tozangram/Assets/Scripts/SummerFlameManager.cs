using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SummerFlameManager : MonoBehaviour {

    [SerializeField] private GameObject crimbableWall;
    private TilemapRenderer crimbableRender;
    private PlatformEffector2D crimbablePlatform;

    private void Start()
    {
        crimbableRender = crimbableWall.GetComponent<TilemapRenderer>();
        crimbablePlatform = crimbableWall.GetComponent<PlatformEffector2D>();
    }

    public void Desabled()
    {
        crimbableRender.enabled = !enabled;
        crimbablePlatform.useSideFriction = false;
    }

    public void Enabled()
    {
        crimbableRender.enabled = enabled;
        crimbablePlatform.useSideFriction = true;
    }
}
