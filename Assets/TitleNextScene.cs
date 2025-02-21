using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // コントローラーの任意のボタンが押されたかをチェック
        if (Input.anyKeyDown)
        {
            // ゲームシーンの名前を指定してシーンをロードする
            SceneManager.LoadScene("GameScene");
        }
    }
}
