using System.Collections.Generic;
using UnityEngine;
namespace Game.Stage
{
	public class ItemGenerate : MonoBehaviour
	{
		[SerializeField] private List<GameObject> ItemList;

		private const float itemGenerateProbability = 0.015f;
		/// <summary>
		/// フロアにアイテムを確立で部屋に生成する。
		/// </summary>
		/// <param name="map">フロアの情報</param>

		public void GenerateItem(int[,] map,Position playerPos,Transform allInstantTransform)
		{
			for (int y = 0; y < StageData.MAP_SIZE_Y; y++)
			{
				for (int x = 0; x < StageData.MAP_SIZE_X; x++)
				{
					if (playerPos.X == x && playerPos.Y == y) continue;

						if (map[x, y] == 2)
						{
							if (RogueUtils.RandomJadge(itemGenerateProbability))
							{
								GameObject item = ItemList[Random.Range(0, ItemList.Count)];
								Debug.Log(item);
								Instantiate(item, new Vector3(x * StageData.adjustStageSize, item.transform.position.y, y * StageData.adjustStageSize), item.transform.rotation, allInstantTransform.transform);
							}
						}
				}
			}


		}


	}
}