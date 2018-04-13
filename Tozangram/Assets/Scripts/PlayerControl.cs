using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    Rigidbody2D rb;
    Transform tf;
    [SerializeField] ContactFilter2D filter2d;
    [SerializeField] private float jumpValue;   // ジャンプ力
    [SerializeField] private float speed;       // 移動速度
    [SerializeField] private float sprintRate;  // ダッシュ時の倍率
    private bool sprint;        // ダッシュ状態かどうか
    private bool isTouched;     // 地面と接触しているか

    void Start()
    {
        // コンポーネントをキャッシュ
        rb = GetComponent<Rigidbody2D>();
        tf = transform;
    }

    void Update()
    {
        // キー入力を取得
        GetKey();
    }

    /// <summary>
    /// キー入力を取得しキャラクターの行動に反映する関数
    /// </summary>
    private void GetKey()
    {
        // スペースが押された時、ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // 左右の矢印キーが押されている間、移動
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Input.GetAxisRaw("Horizontal"));
        }

        // シフトが押されている間、ダッシュ状態
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
    }

    /// <summary>
    /// ジャンプする関数
    /// </summary>
    private void Jump()
    {
        isTouched = rb.IsTouching(filter2d);
        if (isTouched)
        {
            rb.velocity = tf.up * jumpValue;
        }
    }

    /// <summary>
    /// 移動する関数
    /// </summary>
    /// <param name="value">左右の矢印キーからの入力値</param>
    private void Move(float value)
    {
        float moveValue = speed;
        if (sprint)
        {
            moveValue *= sprintRate;
        }
        rb.velocity = new Vector2(value * moveValue, rb.velocity.y);
    }
}
