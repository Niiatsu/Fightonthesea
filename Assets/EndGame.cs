using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameWhenOnePlayerLeft : MonoBehaviour
{
    //メインカメラを取得
    public Camera mainCamera;
    public float zoomSpeed = 3.0f;
    public float zoomDistance = 4.0f;

    //プレイヤーとカメラの初期相対距離
    private Vector3 PtoCdistance;

    //カメラがイージングしたか
    private bool isEasing = false;

    //時間管理用変数
    float waitTime = 0.0f;

    //テキストメッシュプロ用変数
    public GameObject WinnerText;

    // 対象のタグを指定
    public string targetTag = "Player";

    [SerializeField] GameObject GameBGMmanager;
    BGMmanager bgmScript;

    void Start()
    {
        bgmScript = GameBGMmanager.GetComponent<BGMmanager>();

        Time.timeScale = 1.0f;

        // カメラとプレイヤーの初期距離を計算
        if (mainCamera != null && GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            GameObject firstPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
            PtoCdistance = mainCamera.transform.position - firstPlayer.transform.position;
        }

        WinnerText.SetActive(false);
    }

    void zoomPlayer(GameObject player)
    {
        // プレイヤーを中心にする位置を計算
        Vector3 targetPosition = player.transform.position + PtoCdistance.normalized * zoomDistance;
        // カメラを移動
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * zoomSpeed);

        isEasing = true;
    }

    //時間待機用メソッド
    bool doWait(float inTime)
    {
        //myTimeをフレームタイムで更新していく
        waitTime += Time.deltaTime;
        //myTimeがinTimeを超えるなら
        if (waitTime > inTime)
        {
            //myTimeを0クリア
            waitTime = 0.0f;
            //時間が経過したことを示すtrueを返す
            return true;
        }
        //時間が経過していないことを示すfalseを返す
        return false;
    }

    void Update()
    {
        // タグ付きオブジェクトをすべて取得
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);

        // アクティブなオブジェクトをカウント
        int activeCount = 0;

        foreach (GameObject obj in players)
        {
            if (obj.activeInHierarchy) // アクティブかどうかを確認
            {
                activeCount++;
            }
        }

        //生存者が1人になったら、そのプレイヤーにカメラをズームする
        if (activeCount == 1)
        {
            zoomPlayer(players[0]);
            WinnerText.SetActive(true);

            // コントローラーの任意のボタンが押されたかをチェック
            if (Input.anyKeyDown)
            {
                // ゲームシーンの名前を指定してシーンをロードする
                SceneManager.LoadScene("Title");
            }

           
        }

        //イージングをする、または誰も表示されていなければゲームを終了
        if(isEasing == true && doWait(1f))
        {
            Time.timeScale = 0;
            bgmScript.PlayVictryBGM();
        }

        if(activeCount == 0)
        {
            Time.timeScale = 0;

            // コントローラーの任意のボタンが押されたかをチェック
            if (Input.anyKeyDown)
            {
                // ゲームシーンの名前を指定してシーンをロードする
                SceneManager.LoadScene("Title");
            }
        }
    }

}
