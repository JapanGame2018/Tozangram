using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterFlameManager : MonoBehaviour {

    Collider2D collider;
    PlayerControl pc;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D winterMaterial;
    private float speed;
    [SerializeField] private float moveRate;

    private void Awake()
    {
        collider = GameObject.FindWithTag("Player").GetComponent<CircleCollider2D>();
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Start()
    {
        speed = pc.speed;
    }

    public void Disabled()
    {
        collider.sharedMaterial = normalMaterial;
        pc.speed = speed;
    }

	public void Enabled()
    {
        collider.sharedMaterial = winterMaterial;
        pc.speed = speed * moveRate;
    }

}
