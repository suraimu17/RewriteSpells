using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChanger : MonoBehaviour
{
    //Dropdown���i�[����ϐ�
    [SerializeField] private Dropdown dropdown;
    //Cube���i�[����ϐ�
    [SerializeField] private GameObject cube;



    // �I�v�V�������ύX���ꂽ�Ƃ��Ɏ��s���郁�\�b�h
    public void ItemChanger1()
    {
        //Dropdown��Value��0�̂Ƃ��i�A�C�e�����I������Ă���Ƃ��j
        if (dropdown.value == 0)
        {
            cube.GetComponent<Image>().color = new Color(1,1,1,1);


            //�A�C�e���̃A�C�R����\��
        }
        //Dropdown��Value��1�̂Ƃ��i�����C�g�X�y�����I������Ă���Ƃ��j
        else if (dropdown.value == 1)
        {
            cube.GetComponent<Image>().color = new Color(0,0,0,0);

        }
        //Dropdown��Value��2�̂Ƃ��i�����C�g�A�C�e�����I������Ă���Ƃ��j
        else if (dropdown.value == 2)
        {
            cube.GetComponent<Image>().color = new Color(0,0,0,1);

        }
        //Dropdown��Value��3�̂Ƃ��i�|�[�V�������I������Ă���Ƃ��j
        else if (dropdown.value == 3)
        {
            cube.GetComponent<Image>().color = new Color(0,0,0,0);

        }
        else if (dropdown.value == 4)
        {
            cube.GetComponent<Image>().color = new Color(1,1,1,1);

        }

    }
}

