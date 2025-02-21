using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    //各プレイヤー情報を管理するクラス
    [System.Serializable]
    public class PlayerData
    {
        public GameObject playerObject;         //プレイヤー
        public TextMeshProUGUI rankText;        //ランク表示用のUIテキスト
        public bool isFinished = false;         //プレイヤーが非表示になったか
        public int finishFrameCount = 0;     //プレイヤーが非表示になったフレーム数
    }

    //全てのプレイヤーのリスト
    public List<PlayerData> players = new List<PlayerData>();

    // Start is called before the first frame update
    void Start()
    {

    }

    //プレイヤーが非表示になったかを判定し、そのフレームカウントを記録
    private void CheckPlayerStatus()
    {
        foreach(var player in players)
        {
            if(!player.isFinished && !player.playerObject.activeSelf)
            {
                player.isFinished = true;
                player.finishFrameCount = Time.frameCount;  //非表示になったフレームを記録
            }

        }
    }

    //順位を更新し、対応するテキストに表示する
    private void UpdateRanking()
    {
        //非表示になったプレイヤーをフレームカウント順にソートする
        var rankedPlayer = players
            .Where(p => p.isFinished)           //非表示になったプレイヤーのみ
            .OrderBy(p => p.finishFrameCount)   //昇順でソートする
            .ToList();

        ResetRankText();
        DisplayRanking(rankedPlayer);

    }

    //順位テキストをリセット
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

    //ソートされた情報から順位を決め、テキストに判定
    private void DisplayRanking(List<PlayerData> rankedplayers)
    {
        int beginRank = 4;              //初期の順位（4位から）
        int displayRank = beginRank;    //表示する順位
        int lasttimeFrameCount = -1;    //前回のフレームカウント

        for(int i=0; i < rankedplayers.Count; i++)
        {
            //現在のプレイヤーのフレームカウント
            int beginFrameCount = rankedplayers[i].finishFrameCount;

            //フレームカウントが異なる場合のみ順位を更新
            if(beginFrameCount!=lasttimeFrameCount)
            {
                displayRank = beginRank;    //表示する順位を変更する
            }

            //順位をテキストに表示
            if (i > 0 && beginFrameCount == lasttimeFrameCount)
            {
                //同着
                rankedplayers[i].rankText.text = $"{displayRank}";
            }
            else
            {
                rankedplayers[i].rankText.text = $"{displayRank}";
            }

            //次の順位に進む
            beginRank--;
            //現在のフレームカウントを次回のために記録
            lasttimeFrameCount = beginFrameCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerStatus();    //プレイヤーが非表示になっているか判定
        UpdateRanking();        //順位を更新

    }

}
