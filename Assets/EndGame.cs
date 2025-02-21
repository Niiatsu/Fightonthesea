using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameWhenOnePlayerLeft : MonoBehaviour
{
    //���C���J�������擾
    public Camera mainCamera;
    public float zoomSpeed = 3.0f;
    public float zoomDistance = 4.0f;

    //�v���C���[�ƃJ�����̏������΋���
    private Vector3 PtoCdistance;

    //�J�������C�[�W���O������
    private bool isEasing = false;

    //���ԊǗ��p�ϐ�
    float waitTime = 0.0f;

    //�e�L�X�g���b�V���v���p�ϐ�
    public GameObject WinnerText;

    // �Ώۂ̃^�O���w��
    public string targetTag = "Player";

    [SerializeField] GameObject GameBGMmanager;
    BGMmanager bgmScript;

    void Start()
    {
        bgmScript = GameBGMmanager.GetComponent<BGMmanager>();

        Time.timeScale = 1.0f;

        // �J�����ƃv���C���[�̏����������v�Z
        if (mainCamera != null && GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            GameObject firstPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
            PtoCdistance = mainCamera.transform.position - firstPlayer.transform.position;
        }

        WinnerText.SetActive(false);
    }

    void zoomPlayer(GameObject player)
    {
        // �v���C���[�𒆐S�ɂ���ʒu���v�Z
        Vector3 targetPosition = player.transform.position + PtoCdistance.normalized * zoomDistance;
        // �J�������ړ�
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * zoomSpeed);

        isEasing = true;
    }

    //���ԑҋ@�p���\�b�h
    bool doWait(float inTime)
    {
        //myTime���t���[���^�C���ōX�V���Ă���
        waitTime += Time.deltaTime;
        //myTime��inTime�𒴂���Ȃ�
        if (waitTime > inTime)
        {
            //myTime��0�N���A
            waitTime = 0.0f;
            //���Ԃ��o�߂������Ƃ�����true��Ԃ�
            return true;
        }
        //���Ԃ��o�߂��Ă��Ȃ����Ƃ�����false��Ԃ�
        return false;
    }

    void Update()
    {
        // �^�O�t���I�u�W�F�N�g�����ׂĎ擾
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);

        // �A�N�e�B�u�ȃI�u�W�F�N�g���J�E���g
        int activeCount = 0;

        foreach (GameObject obj in players)
        {
            if (obj.activeInHierarchy) // �A�N�e�B�u���ǂ������m�F
            {
                activeCount++;
            }
        }

        //�����҂�1�l�ɂȂ�����A���̃v���C���[�ɃJ�������Y�[������
        if (activeCount == 1)
        {
            zoomPlayer(players[0]);
            WinnerText.SetActive(true);

            // �R���g���[���[�̔C�ӂ̃{�^���������ꂽ�����`�F�b�N
            if (Input.anyKeyDown)
            {
                // �Q�[���V�[���̖��O���w�肵�ăV�[�������[�h����
                SceneManager.LoadScene("Title");
            }

           
        }

        //�C�[�W���O������A�܂��͒N���\������Ă��Ȃ���΃Q�[�����I��
        if(isEasing == true && doWait(1f))
        {
            Time.timeScale = 0;
            bgmScript.PlayVictryBGM();
        }

        if(activeCount == 0)
        {
            Time.timeScale = 0;

            // �R���g���[���[�̔C�ӂ̃{�^���������ꂽ�����`�F�b�N
            if (Input.anyKeyDown)
            {
                // �Q�[���V�[���̖��O���w�肵�ăV�[�������[�h����
                SceneManager.LoadScene("Title");
            }
        }
    }

}
