using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;

namespace Game.Stage
{
	public class SceneInitializer : MonoBehaviour
	{
		private Position stairPosition;
		private Position playerPosition;

		public GameObject _player;

		private ItemGenerate itemGenerate;
		private EnemyGenerateManager enemyGenerateManager;
		private StageGenerator stageGenerator;

		[SerializeField] private GameObject floorPrefab;
		[SerializeField] private List<GameObject> wallList;
		[SerializeField] private GameObject roomPrefab;
		[SerializeField] private GameObject stairPrefab;
		[SerializeField] private Transform allInstantTransform;

		public int[,] map { private set; get; }= new int[StageData.MAP_SIZE_X, StageData.MAP_SIZE_Y];

		private void Awake()
        {
			itemGenerate = GetComponent<ItemGenerate>();
			enemyGenerateManager = FindObjectOfType<EnemyGenerateManager>();
			stageGenerator = GetComponent<StageGenerator>();
		}
		//�������ꂽ�}�b�v��Instance
		private void GenerateMap()
		{

			var parent = allInstantTransform.transform;

			for (int y = 0; y < StageData.MAP_SIZE_Y; y++)
			{
				for (int x = 0; x < StageData.MAP_SIZE_X; x++)
				{
					//������
					if (map[x, y] == 1)
					{
						Instantiate(floorPrefab, new Vector3(x* StageData.adjustStageSize, 0, y* StageData.adjustStageSize), new Quaternion(), parent);
					}
					//��������
					else if (map[x, y] == 2)
					{
						if (stairPosition.X == x && stairPosition.Y == y) Instantiate(stairPrefab, new Vector3(stairPosition.X * StageData.adjustStageSize, 0, stairPosition.Y * StageData.adjustStageSize), new Quaternion(), parent);
						else
						{
							Instantiate(roomPrefab, new Vector3(x * StageData.adjustStageSize, 0, y * StageData.adjustStageSize), new Quaternion(), parent);
						}
					}
					//�ǐ���
					else
					{
						if (RogueUtils.RandomJadge(0.1f))
						{
							var wallPrefab = wallList[Random.Range(1, wallList.Count)];
							Instantiate(wallPrefab, new Vector3(x * StageData.adjustStageSize, 1f, y * StageData.adjustStageSize), new Quaternion(), parent);
						}
						else
						{
							Instantiate(wallList[0], new Vector3(x * StageData.adjustStageSize, 0, y * StageData.adjustStageSize), new Quaternion(), parent);
						}
						
					}
				}
			}

		}

		//�v���C���[�𐶐�
		private void SponePlayer()
		{
			do
			{
				var x = RogueUtils.GetRandomInt(0, StageData.MAP_SIZE_X - 1);
				var y = RogueUtils.GetRandomInt(0, StageData.MAP_SIZE_Y - 1);
				playerPosition = new Position(x, y);
			} while (map[playerPosition.X, playerPosition.Y] != 2);

			_player.transform.position = new Vector3(playerPosition.X* StageData.adjustStageSize, 0, playerPosition.Y* StageData.adjustStageSize);
		}
		//�K�i�̈ʒu���擾
		private void CreateStairPos()
		{
			do
			{
				var x = RogueUtils.GetRandomInt(0, StageData.MAP_SIZE_X - 1);
				var y = RogueUtils.GetRandomInt(0, StageData.MAP_SIZE_Y - 1);
				stairPosition = new Position(x, y);
			} while (map[stairPosition.X, stairPosition.Y] != 2&&map[playerPosition.X,playerPosition.Y]!= map[stairPosition.X, stairPosition.Y]);

		}

		//�啔�������������̑I��
		private void ChooseStage() 
		{
			if (RogueUtils.RandomJadge(0.9f))
			{
				map = stageGenerator.GenerateMap(StageData.MAX_ROOM_NUMBER);
			}
			else 
			{
				map = new LargeLoomGenerate().GenerateLargeLoom();
			}
		}

		//�_���W�����̍Đ���
		public void ReloadStage() 
		{
			ChooseStage();
			SponePlayer();
			CreateStairPos();

			stageGenerator.CanFloorDown(stairPosition, playerPosition.X,playerPosition.Y, map);
			if (stageGenerator.CanDownFloor == false) {
				Debug.Log("�Đ���");
				stageGenerator.ResetCheckData();
				ReloadStage();
				return; 
			}

			stageGenerator.ResetCheckData();
			GenerateMap();
			itemGenerate.GenerateItem(map, playerPosition,allInstantTransform.transform);
			enemyGenerateManager.FirstGenerateEnemy(map);
		}

		//�_���W�����̔j��
		public void DestroyStage() 
        {
			foreach (Transform c in allInstantTransform.transform) 
			{
				GameObject.Destroy(c.gameObject);
			}
        }
    }
}