using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Game.Stage;
using Game.players;
using System.Collections.Generic;
using Game.enemys;

namespace Game.Manager
{
	public class EnemyGenerateManager : MonoBehaviour
	{
		[SerializeField] List<GameObject> enemyPrefab = new List<GameObject>();
		[SerializeField] GameObject target;
		[SerializeField] PlayerHP hp;
		[SerializeField] private const int GenMax = 6;
		[SerializeField] private float generateSpan = 20.0f;
		[SerializeField] private Transform allInstantTransform;
		[SerializeField] private SceneInitializer sceneInitializer;
		private float time = 0;
		private int enemyNum;
		public int currentGenNum { private set; get; } = 0;

		private FloorManager floorManager;

		private void Start()
		{
			floorManager = FindObjectOfType<FloorManager>();
			EnemyGenerateObservable();

			for (int prefabNum = 0; prefabNum < enemyPrefab.Count; prefabNum++) 
			{
				enemyPrefab[prefabNum].GetComponent<EnemyController>().SetEnemyFloorData();
			}
			
		}
		private void Update()
		{
			time += Time.deltaTime;
		}

		private void EnemyGenerateObservable() 
		{
			this.UpdateAsObservable()
				.Where(_ => time > generateSpan)
				.Where(_ => currentGenNum < GenMax)
				.Subscribe(_ =>
				{
					var enemyPos = EnemyGeneratePos(sceneInitializer.map);

					if (floorManager.currentFloor > 100)
					{
						enemyNum = Random.Range(0, enemyPrefab.Count-1);
					}
					else 
					{
						enemyNum = ChooseEnemy();
					}

					var obj = GameObject.Instantiate(enemyPrefab[enemyNum],enemyPos,new Quaternion(),allInstantTransform.transform);
					//ˆË‘¶‰ğÁ‚Ì‚½‚ß
					obj?.OnDestroyAsObservable()
					.Subscribe(_ =>
					{
						currentGenNum--;
						Debug.Log("“G‚Ì€–SŒŸ’m"+currentGenNum);
					})
					.AddTo(this);
					obj.GetComponent<IMove>().SetTarget(target,hp);
					currentGenNum++;
					time = 0;

				}).AddTo(this);
		}


		//Enemy‚Ì¶¬ˆÊ’u‚ğ•Ô‚·
		private Vector3 EnemyGeneratePos(int[,] map) 
		{

			Position enemyPos;
			do
			{
				var x = RogueUtils.GetRandomInt(0, StageData.MAP_SIZE_X - 1);
				var y = RogueUtils.GetRandomInt(0, StageData.MAP_SIZE_Y - 1);
				enemyPos = new Position(x, y);
			} while (map[enemyPos.X, enemyPos.Y] != 2);

			Vector3 enemyInstancePos = new Vector3(enemyPos.X * StageData.adjustStageSize, 0, enemyPos.Y * StageData.adjustStageSize);

			return enemyInstancePos;
		}

		//‰Šú¶¬
		public void FirstGenerateEnemy(int[,] map)
		{
			int firstGenNum = Random.Range(GenMax - 2, GenMax);

			for (int i = 0; i < firstGenNum; i++)
			{
				var enemyPos = EnemyGeneratePos(map);

				    if (floorManager.currentFloor >100)
					{
						enemyNum = Random.Range(0, enemyPrefab.Count - 1);
					}
					else
					{
						enemyNum = ChooseEnemy();
					}

				var enemyObj = GameObject.Instantiate(enemyPrefab[enemyNum], enemyPos, new Quaternion(),allInstantTransform.transform);
				enemyObj.OnDestroyAsObservable()
					.Subscribe(_ =>
					{
						currentGenNum--;
					})
					.AddTo(this);

				enemyObj.GetComponent<IMove>().SetTarget(target, hp);
				currentGenNum++;

			}
		}

		private int ChooseEnemy() 
		{
			List<int> enemyFloorData = new List<int>();

			for (int arrayNum = 0; arrayNum < enemyPrefab.Count; arrayNum++) 
			{
				if (enemyPrefab[arrayNum].GetComponent<EnemyController>()?.enemyFloorArray[floorManager.currentFloor] == 1) 
				{
					Debug.Log("enemy‘I‚ñ‚¾‚æ");
					enemyFloorData.Add(arrayNum);
				}
			
			}

			var enemyGenerateNum = enemyFloorData[Random.Range(0, enemyFloorData.Count)];

			return enemyGenerateNum;
		}

	}

}