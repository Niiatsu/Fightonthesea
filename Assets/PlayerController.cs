using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //走る速さ
    public float runSpeed = 7.5f;
    //ジャンプの高さ
    public float jumpHigh = 5.0f;
    //敵を飛ばす力
    public float KnockbackForce = 2.5f;
    // 地面に接地しているかどうか(ジャンプ用)
    private bool isGrounded;
    //攻撃中かどうか
    private bool isAttacking = false;

    //リジットボディ
    private Rigidbody rb;
    //スフィアコリジョン
    private SphereCollider sphereCol;
    //アニメーター
    private Animator animator;

    //移動方向の向きを保存するための変数
    private Vector3 direction;

    //複数のコントローラーを接続するための変数
    [SerializeField] private int padNo = 0; 

    // Start is called before the first frame update
    void Start()
    {
        //Rigitbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        //SphereColliderのコンポーネントの取得
        sphereCol = GetComponent<SphereCollider>();
        //最初は非表示にしておく
        sphereCol.enabled = false;
        //Animatorのコンポーネントの取得
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count <= padNo)
        {
            return;
        }
        var pad = Gamepad.all[padNo];

        //攻撃
        if(pad.buttonWest.isPressed && isGrounded)
        {
            Attack();
            sphereCol.enabled = true;
            
            //Invoke("ColliderReset", 1.5f);
        }
        else
        {
            AttackEnd();
            sphereCol.enabled = false;
        }

        //攻撃中は移動やジャンプを無効化する
        if (isAttacking == true) return;

        Vector2 lstickValue = pad.leftStick.ReadValue();
        float vert = lstickValue.y;
        float horiz = lstickValue.x;

        direction = new Vector3(horiz, 0, vert);

        if (lstickValue.magnitude > 0.1f)
        {
            // オブジェクトの前方を移動方向に向ける
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += direction.normalized * runSpeed * Time.deltaTime;
            //lstickValue = transform.TransformDirection(lstickValue);

            Vector3 movement = new Vector3(horiz, 0.0f, vert);

            // キャラクターが動いているかを確認し、Animatorのパラメーターを更新
            animator.SetBool("Run", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }

        //ジャンプ
        // "Jump"入力が押され、地面に接地している場合にジャンプする
        if (pad.buttonSouth.isPressed && isGrounded)
        {
            JumpAction();
        }

        
        

    }

    //ジャンプ
    private void JumpAction()
    {
        // 上方向に力を加える
        rb.AddForce(Vector3.up * jumpHigh, ForceMode.Impulse);
        isGrounded = false;
    }

    //アタック
    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("BadgerPawAttack", true);
    }

    private void AttackEnd()
    {
        isAttacking = false;
        animator.SetBool("BadgerPawAttack", false);
        animator.SetBool("Idle", true);
    }

    //オブジェクトと接触した瞬間に呼び出す
    private void OnTriggerEnter(Collider other)
    {
        //自分以外のPlayerタグを持つオブジェクトに当たったとき
        if (other.CompareTag("Player") && other.gameObject != this.gameObject)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //プレイヤー敵への方向を計算
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

                //後ろに飛ばすために、Y成分を少し調整
                //knockbackDirection.y = 0.5f;

                //力を加えて後ろに飛ばす
                rb.AddForce(knockbackDirection * KnockbackForce, ForceMode.Impulse);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーが水に接触した時に退場させる。
        if (collision.gameObject.CompareTag("DeathTrigger"))
        {
            gameObject.SetActive(false);
        }

        //地面に着いているときにAボタンを押すと、ジャンプする
        // タグ「Ground」のオブジェクトに触れたら地面と判定する
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

}
