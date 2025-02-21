using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GFmanager : MonoBehaviour
{
    public float downDistance = 8f;          // Y�������ɉ����鋗��
    public float moveSpeed = 2f;             // �ړ����x
    public float beforeDownTime = 2.5f;        // ������O�̑ҋ@����
    public float downTime = 4f;              // ���������܂܂̎���
    public float intervalTime = 5f;          // ���̑I�o�܂ł̊Ԋu����
    public Color selectedColor = Color.green;  // �I�o���ꂽ�I�u�W�F�N�g�̐F

    private List<Transform> groundObjects;   // Ground�^�O�̃I�u�W�F�N�g���i�[���郊�X�g
    private Dictionary<Transform, Coroutine> activeCoroutines = new Dictionary<Transform, Coroutine>();
    private Color originalColor;             // ���̐F��ێ�����ϐ�

    void Start()
    {
        // Ground�^�O���t�����I�u�W�F�N�g���擾
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Ground");
        groundObjects = new List<Transform>();

        // �I�u�W�F�N�g��Transform�����X�g�ɒǉ�
        foreach (GameObject obj in objects)
        {
            groundObjects.Add(obj.transform);
        }

        // �R���[�`���œ�����J��Ԃ�
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
            // �����_����1�̃I�u�W�F�N�g��I��
            int selectedIndex = Random.Range(0, groundObjects.Count);
            Transform selectedObject = groundObjects[selectedIndex];

            // �I�o���ꂽ�I�u�W�F�N�g�̌��̐F���擾
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.EnableKeyword("_EMISSION");
            }

            // ������O�ɑҋ@
            await UniTask.WaitForSeconds(beforeDownTime);

            // �I�΂ꂽ�I�u�W�F�N�g�ȊO��������R���[�`�����J�n
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

            // ��莞�ԑҋ@
            await UniTask.WaitForSeconds(downTime);

            // �I�u�W�F�N�g�����̈ʒu�ɖ߂��A�I�o���ꂽ�I�u�W�F�N�g�̐F�����ɖ߂�
            foreach (Transform obj in otherObjects)
            {
                if (activeCoroutines.ContainsKey(obj) && activeCoroutines[obj] != null)
                {
                    StopCoroutine(activeCoroutines[obj]);
                }
                activeCoroutines[obj] = StartCoroutine(MoveObjectY(obj, downDistance));
            }

            // �I�o���ꂽ�I�u�W�F�N�g�̐F�����ɖ߂�
            if (renderer != null)
            {
                renderer.material.DisableKeyword("_EMISSION");
            }

            // ���̑I�o�܂őҋ@
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