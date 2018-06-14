using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterFlameManager : MonoBehaviour {

    Collider2D col2D;
    PlayerControl pc;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D winterMaterial;
    private float speed;
    [SerializeField] private float moveRate;

    private void Awake()
    {
        col2D = GameObject.FindWithTag("Player").GetComponent<CapsuleCollider2D>();
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Start()
    {
        speed = pc.speed;
    }

    public void Disabled()
    {
        col2D.sharedMaterial = normalMaterial;
        pc.speed = speed;
    }

	public void Enabled()
    {
        col2D.sharedMaterial = winterMaterial;
        pc.speed = speed * moveRate;
    }

}
