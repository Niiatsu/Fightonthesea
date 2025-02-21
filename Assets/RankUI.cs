using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    //�e�v���C���[�����Ǘ�����N���X
    [System.Serializable]
    public class PlayerData
    {
        public GameObject playerObject;         //�v���C���[
        public TextMeshProUGUI rankText;        //�����N�\���p��UI�e�L�X�g
        public bool isFinished = false;         //�v���C���[����\���ɂȂ�����
        public int finishFrameCount = 0;     //�v���C���[����\���ɂȂ����t���[����
    }

    //�S�Ẵv���C���[�̃��X�g
    public List<PlayerData> players = new List<PlayerData>();

    // Start is called before the first frame update
    void Start()
    {

    }

    //�v���C���[����\���ɂȂ������𔻒肵�A���̃t���[���J�E���g���L�^
    private void CheckPlayerStatus()
    {
        foreach(var player in players)
        {
            if(!player.isFinished && !player.playerObject.activeSelf)
            {
                player.isFinished = true;
                player.finishFrameCount = Time.frameCount;  //��\���ɂȂ����t���[�����L�^
            }

        }
    }

    //���ʂ��X�V���A�Ή�����e�L�X�g�ɕ\������
    private void UpdateRanking()
    {
        //��\���ɂȂ����v���C���[���t���[���J�E���g���Ƀ\�[�g����
        var rankedPlayer = players
            .Where(p => p.isFinished)           //��\���ɂȂ����v���C���[�̂�
            .OrderBy(p => p.finishFrameCount)   //�����Ń\�[�g����
            .ToList();

        ResetRankText();
        DisplayRanking(rankedPlayer);

    }

    //���ʃe�L�X�g�����Z�b�g
    private void ResetRankText()
    {
        foreach(var player in players)
        {
            if(player.rankText != null)
            {
                player.rankText.text = "";
            }
        }
    }

    //�\�[�g���ꂽ��񂩂珇�ʂ����߁A�e�L�X�g�ɔ���
    private void DisplayRanking(List<PlayerData> rankedplayers)
    {
        int beginRank = 4;              //�����̏��ʁi4�ʂ���j
        int displayRank = beginRank;    //�\�����鏇��
        int lasttimeFrameCount = -1;    //�O��̃t���[���J�E���g

        for(int i=0; i < rankedplayers.Count; i++)
        {
            //���݂̃v���C���[�̃t���[���J�E���g
            int beginFrameCount = rankedplayers[i].finishFrameCount;

            //�t���[���J�E���g���قȂ�ꍇ�̂ݏ��ʂ��X�V
            if(beginFrameCount!=lasttimeFrameCount)
            {
                displayRank = beginRank;    //�\�����鏇�ʂ�ύX����
            }

            //���ʂ��e�L�X�g�ɕ\��
            if (i > 0 && beginFrameCount == lasttimeFrameCount)
            {
                //����
                rankedplayers[i].rankText.text = $"{displayRank}";
            }
            else
            {
                rankedplayers[i].rankText.text = $"{displayRank}";
            }

            //���̏��ʂɐi��
            beginRank--;
            //���݂̃t���[���J�E���g������̂��߂ɋL�^
            lasttimeFrameCount = beginFrameCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerStatus();    //�v���C���[����\���ɂȂ��Ă��邩����
        UpdateRanking();        //���ʂ��X�V

    }

}
