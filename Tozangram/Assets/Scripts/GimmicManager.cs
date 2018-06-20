using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicManager : MonoBehaviour {

    Rigidbody2D rb;
    Transform tf;
    GameManager gm;
    float defaultWater;
    [SerializeField] private float waterValue;
    [SerializeField] private float movingValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = transform;
    }

    void Start () {
        gm = GameManager.instance;
        defaultWater = rb.drag;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            rb.drag = waterValue;
        }
        else if (collision.CompareTag("Hole"))
        {
            gm.ReStart();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Moving"))
        {
            switch(collision.name)
            {
                case "up":
                    rb.AddForce(tf.up * movingValue);
                    break;
                case "down":
                    rb.AddForce(tf.up * -movingValue);
                    break;
                case "left":
                    rb.AddForce(tf.right * -movingValue);
                    break;
                case "right":
                    rb.AddForce(tf.right * movingValue);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            rb.drag = defaultWater;
        }
    }
}
