using System.Collections.Generic;
using UnityEngine;

using Game.enemys;


namespace Inventories.Enemy
{
    [RequireComponent(typeof(EnemyStatus))]
    [RequireComponent(typeof(ILevelProvider))]
    public class DropTable : MonoBehaviour
    {
        //�e�[�u���Ɗm��
        [SerializeField] private List<PickableItem> table = new List<PickableItem>();
        [SerializeField] private List<float> random = new List<float>();

        //���x��
        private ILevelProvider level;

        private void Start()
        {
            level = GetComponent<ILevelProvider>();
        }



        //1�����o��
        public void Drop()
        {
            //�w��񐔒��I �o����L�����Z��
            for(int i = 0; i < table.Count; i++)
            {
                if(Random.value + level.Value/100 <= random[i])
                {
                    //�C���X�^���X���ƃ��x���̏�����
                    //�������Ɍ��̃v���n�u�̉�]�ɂ��A���W�̓h���b�v�ӏ��Ƀv���n�u�̍��W�����Z
                    GameObject itemObject = Instantiate(table[i].gameObject, gameObject.transform.position + table[i].transform.position, table[i].transform.rotation);

                    //���x����ݒ肵�ďI��
                    PickableItem pItem = itemObject.GetComponent<PickableItem>();
                    pItem.unityItemStack.ChangeCastableLevel(level.Value);
                    break;
                }
            }
        }


    }
}
