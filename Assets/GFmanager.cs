using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GFmanager : MonoBehaviour
{
    public float downDistance = 8f;          // Y軸方向に下げる距離
    public float moveSpeed = 2f;             // 移動速度
    public float beforeDownTime = 2.5f;        // 下げる前の待機時間
    public float downTime = 4f;              // 下がったままの時間
    public float intervalTime = 5f;          // 次の選出までの間隔時間
    public Color selectedColor = Color.green;  // 選出されたオブジェクトの色

    private List<Transform> groundObjects;   // Groundタグのオブジェクトを格納するリスト
    private Dictionary<Transform, Coroutine> activeCoroutines = new Dictionary<Transform, Coroutine>();
    private Color originalColor;             // 元の色を保持する変数

    void Start()
    {
        // Groundタグが付いたオブジェクトを取得
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Ground");
        groundObjects = new List<Transform>();

        // オブジェクトのTransformをリストに追加
        foreach (GameObject obj in objects)
        {
            groundObjects.Add(obj.transform);
        }

        // コルーチンで動作を繰り返す
        RandomSelectionRoutine().Forget();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    async UniTask RandomSelectionRoutine()
    {
        while (true)
        {
            // ランダムに1つのオブジェクトを選択
            int selectedIndex = Random.Range(0, groundObjects.Count);
            Transform selectedObject = groundObjects[selectedIndex];

            // 選出されたオブジェクトの元の色を取得
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.EnableKeyword("_EMISSION");
            }

            // 下げる前に待機
            await UniTask.WaitForSeconds(beforeDownTime);

            // 選ばれたオブジェクト以外を下げるコルーチンを開始
            List<Transform> otherObjects = new List<Transform>(groundObjects);
            otherObjects.Remove(selectedObject);

            foreach (Transform obj in otherObjects)
            {
                if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                {
                    StopCoroutine(activeCoroutines[obj]);
                }
                activeCoroutines[obj] = StartCoroutine(MoveObjectY(obj, -downDistance));
            }

            // 一定時間待機
            await UniTask.WaitForSeconds(downTime);

            // オブジェクトを元の位置に戻し、選出されたオブジェクトの色も元に戻す
            foreach (Transform obj in otherObjects)
            {
                if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                {
                    StopCoroutine(activeCoroutines[obj]);
                }
                activeCoroutines[obj] = StartCoroutine(MoveObjectY(obj, downDistance));
            }

            // 選出されたオブジェクトの色を元に戻す
            if (renderer != null)
            {
                renderer.material.DisableKeyword("_EMISSION");
            }

            // 次の選出まで待機
            await UniTask.WaitForSeconds(intervalTime);
        }
    }

    IEnumerator MoveObjectY(Transform obj, float distance)
    {
        Vector3 startPos = obj.position;
        Vector3 endPos = startPos + new Vector3(0, distance, 0);

        float elapsedTime = 0;
        while (elapsedTime < moveSpeed)
        {
            obj.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.position = endPos;
    }
}