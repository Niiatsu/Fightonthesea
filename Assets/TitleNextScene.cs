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
        // �R���g���[���[�̔C�ӂ̃{�^���������ꂽ�����`�F�b�N
        if (Input.anyKeyDown)
        {
            // �Q�[���V�[���̖��O���w�肵�ăV�[�������[�h����
            SceneManager.LoadScene("GameScene");
        }
    }
}
