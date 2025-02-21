using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //���鑬��
    public float runSpeed = 7.5f;
    //�W�����v�̍���
    public float jumpHigh = 5.0f;
    //�G���΂���
    public float KnockbackForce = 2.5f;
    // �n�ʂɐڒn���Ă��邩�ǂ���(�W�����v�p)
    private bool isGrounded;
    //�U�������ǂ���
    private bool isAttacking = false;

    //���W�b�g�{�f�B
    private Rigidbody rb;
    //�X�t�B�A�R���W����
    private SphereCollider sphereCol;
    //�A�j���[�^�[
    private Animator animator;

    //�ړ������̌�����ۑ����邽�߂̕ϐ�
    private Vector3 direction;

    //�����̃R���g���[���[��ڑ����邽�߂̕ϐ�
    [SerializeField] private int padNo = 0; 

    // Start is called before the first frame update
    void Start()
    {
        //Rigitbody�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();
        //SphereCollider�̃R���|�[�l���g�̎擾
        sphereCol = GetComponent<SphereCollider>();
        //�ŏ��͔�\���ɂ��Ă���
        sphereCol.enabled = false;
        //Animator�̃R���|�[�l���g�̎擾
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

        //�U��
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

        //�U�����͈ړ���W�����v�𖳌�������
        if (isAttacking == true) return;

        Vector2 lstickValue = pad.leftStick.ReadValue();
        float vert = lstickValue.y;
        float horiz = lstickValue.x;

        direction = new Vector3(horiz, 0, vert);

        if (lstickValue.magnitude > 0.1f)
        {
            // �I�u�W�F�N�g�̑O�����ړ������Ɍ�����
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += direction.normalized * runSpeed * Time.deltaTime;
            //lstickValue = transform.TransformDirection(lstickValue);

            Vector3 movement = new Vector3(horiz, 0.0f, vert);

            // �L�����N�^�[�������Ă��邩���m�F���AAnimator�̃p�����[�^�[���X�V
            animator.SetBool("Run", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }

        //�W�����v
        // "Jump"���͂�������A�n�ʂɐڒn���Ă���ꍇ�ɃW�����v����
        if (pad.buttonSouth.isPressed && isGrounded)
        {
            JumpAction();
        }

        
        

    }

    //�W�����v
    private void JumpAction()
    {
        // ������ɗ͂�������
        rb.AddForce(Vector3.up * jumpHigh, ForceMode.Impulse);
        isGrounded = false;
    }

    //�A�^�b�N
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

    //�I�u�W�F�N�g�ƐڐG�����u�ԂɌĂяo��
    private void OnTriggerEnter(Collider other)
    {
        //�����ȊO��Player�^�O�����I�u�W�F�N�g�ɓ��������Ƃ�
        if (other.CompareTag("Player") && other.gameObject != this.gameObject)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //�v���C���[�G�ւ̕������v�Z
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

                //���ɔ�΂����߂ɁAY��������������
                //knockbackDirection.y = 0.5f;

                //�͂������Č��ɔ�΂�
                rb.AddForce(knockbackDirection * KnockbackForce, ForceMode.Impulse);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�����ɐڐG�������ɑޏꂳ����B
        if (collision.gameObject.CompareTag("DeathTrigger"))
        {
            gameObject.SetActive(false);
        }

        //�n�ʂɒ����Ă���Ƃ���A�{�^���������ƁA�W�����v����
        // �^�O�uGround�v�̃I�u�W�F�N�g�ɐG�ꂽ��n�ʂƔ��肷��
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }

}
