using UnityEngine;

namespace Game.Stage
{
	public class LargeLoomGenerate : MonoBehaviour
	{

		private int[,] map;

		//‘å•”‰®‚Ìƒ}ƒbƒv¶¬
		public int[,] GenerateLargeLoom()
		{
			map = new int[StageData.MAP_SIZE_X, StageData.MAP_SIZE_Y];

			for (int y = 0; y < StageData.MAP_SIZE_Y; y++)
			{
				for (int x = 0; x < StageData.MAP_SIZE_X; x++)
				{
					if (x == 0 || x == StageData.MAP_SIZE_X - 1 || y == 0 || y == StageData.MAP_SIZE_Y-1)
					{
						map[x, y] = 0;
					}
					else
					{
						map[x, y] = 2;
					}
				}
			}


			return map;
		}
	}
}