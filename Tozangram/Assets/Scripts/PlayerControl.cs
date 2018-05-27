using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Rigidbody2D rb;
    Transform tf;
    SpriteRenderer sr;
    public GameManager gm;
    [SerializeField] public ContactFilter2D filter2d;
    [SerializeField] private float jumpValue;   // ジャンプ力
    [SerializeField] private float speed;       // 移動速度
    [SerializeField] private float sprintRate;  // ダッシュ時の倍率
    [SerializeField] private float jumpRate;    // 2段ジャンプの倍率
    [SerializeField] private float downForce; // 2段ジャンプ時の落下速度
    private float gravityScale;
    private bool sprint;        // ダッシュ状態かどうか
    private bool canDouble;     // 2段ジャンプ可能か
    public bool isTouched;     // 地面と接触しているか
    public bool crimbable;      // 壁登れるか

    void Start()
    {
        // コンポーネントをキャッシュ
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        tf = transform;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        gravityScale = rb.gravityScale;
    }

    void Update()
    {
        // キー入力を取得
        if (gm.state == STATE.GAME)
        {
            GetKey();
        }
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

        // スペースを離した時に上昇中なら下降
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            Fall();
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

        // 昇降が可能な時、上下に移動
        if (crimbable && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            Crimb(Input.GetAxisRaw("Vertical"));
        }
    }

    /// <summary>
    /// ジャンプする関数
    /// </summary>
    private void Jump()
    {
        // 地面と接触しているかどうか
        isTouched = rb.IsTouching(filter2d);

        // 地面と接触しているならジャンプ
        if (isTouched)
        {
            rb.velocity = tf.up * jumpValue;
            canDouble = false;
        }

        // ２段ジャンプ可能なら2段ジャンプ
        if (canDouble)
        {
            StartCoroutine(DoubleJump());
        }

        // 秋フレームで一回目のジャンプの時に2段ジャンプ可能にする
        if (gm.season == SEASON.AUTUMN && isTouched)
        {
            canDouble = true;
        }
    }

    /// <summary>
    /// 2段ジャンプの関数
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoubleJump()
    {
        rb.velocity = tf.up * jumpValue * jumpRate;
        canDouble = false;
        while (!rb.IsTouching(filter2d))
        {
            rb.AddForce(tf.up * -downForce);
            yield return null;
        }
        yield break;
    }

    /// <summary>
    /// スペースが離されたタイミングで降下する関数
    /// </summary>
    private void Fall()
    {
        // y成分を0に
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }

    /// <summary>
    /// 移動する関数
    /// </summary>
    /// <param name="value">左右の矢印キーからの入力値</param>
    private void Move(float value)
    {
        float moveValue = speed;

        // スプリント中なら速度を上げる
        if (sprint)
        {
            moveValue *= sprintRate;
        }

        // 左右に入力
        rb.velocity = new Vector2(value * moveValue, rb.velocity.y);

        // 移動方向にキャラクターを向ける
        if (value > 0)
        {
            sr.flipX = false;
        }
        else if (value < 0)
        {
            sr.flipX = true;
        }
    }

    /// <summary>
    /// キャラを上下に移動する関数
    /// </summary>
    /// <param name="value">上下の矢印キーからの入力値</param>
    private void Crimb(float value)
    {
        tf.Translate(0, value * 0.1f, 0);
    }

    public void ActiveGravity(bool active)
    {
        if (active)
        {
            rb.gravityScale = gravityScale;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }
}
