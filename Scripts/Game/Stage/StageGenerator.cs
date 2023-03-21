using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage
{
	public class StageGenerator : MonoBehaviour
	{
		private int maxRoom;

		private List<Range> roomList = new List<Range>();
		private List<Range> rangeList = new List<Range>();
		private List<Range> passList = new List<Range>();
		private List<Range> roomPassList = new List<Range>();

		int[,] checkedMap = new int[StageData.MAP_SIZE_X, StageData.MAP_SIZE_Y];

		public bool CanDownFloor { private set; get; } = false;
		public int[,] GenerateMap(int maxRoom)
		{

			int[,] map = new int[StageData.MAP_SIZE_X, StageData.MAP_SIZE_Y];

			CreateRange(maxRoom);
			CreateRoom();

			// �����܂ł̌��ʂ���x�z��ɔ��f����
			foreach (Range pass in passList)
			{
				for (int x = pass.Start.X; x <= pass.End.X; x++)
				{
					for (int y = pass.Start.Y; y <= pass.End.Y; y++)
					{
						map[x, y] = 1;
					}
				}
				//Debug.Log(pass);
			}
			foreach (Range roomPass in roomPassList)
			{
				for (int x = roomPass.Start.X; x <= roomPass.End.X; x++)
				{
					for (int y = roomPass.Start.Y; y <= roomPass.End.Y; y++)
					{
						map[x, y] = 1;
					}
				}
			}
			foreach (Range room in roomList)
			{
				for (int x = room.Start.X; x <= room.End.X; x++)
				{
					for (int y = room.Start.Y; y <= room.End.Y; y++)
					{
						map[x, y] = 2;
					}
				}
			}

			TrimPassList(ref map);

			return map;
		}

		public void CreateRange(int maxRoom)
		{
			// ���̃��X�g�̏����l�Ƃ��ă}�b�v�S�̂�����
			rangeList.Add(new Range(0, 0, StageData.MAP_SIZE_X - 1, StageData.MAP_SIZE_Y - 1));

			bool isDevided;
			do
			{
				// �c �� �� �̏��Ԃŕ�������؂��Ă����B�����؂�Ȃ�������I��
				isDevided = DevideRange(false);
				isDevided = DevideRange(true) || isDevided;

				// �������͍ő��搔�𒴂�����I��
				if (rangeList.Count >= maxRoom)
				{
					break;
				}
			} while (isDevided);

		}

		public bool DevideRange(bool isVertical)
		{
			bool isDevided = false;

			// ��悲�Ƃɐ؂邩�ǂ������肷��
			List<Range> newRangeList = new List<Range>();
			foreach (Range range in rangeList)
			{
				// ����ȏ㕪���ł��Ȃ��ꍇ�̓X�L�b�v
				if (isVertical && range.GetWidthY() < StageData.MINIMUM_RANGE_WIDTH * 2 + 1)
				{
					continue;
				}
				else if (!isVertical && range.GetWidthX() < StageData.MINIMUM_RANGE_WIDTH * 2 + 1)
				{
					continue;
				}

				System.Threading.Thread.Sleep(1);

				// 40���̊m���ŕ������Ȃ�
				// �������A���̐���1�̎��͕K����������
				if (rangeList.Count > 1 && RogueUtils.RandomJadge(0.4f))
				{
					continue;
				}

				// ��������ŏ��̋��T�C�Y2���������A�c�肩�烉���_���ŕ����ʒu�����߂�
				int length = isVertical ? range.GetWidthY() : range.GetWidthX();
				int margin = length - StageData.MINIMUM_RANGE_WIDTH * 2;
				int baseIndex = isVertical ? range.Start.Y : range.Start.X;
				int devideIndex = baseIndex + StageData.MINIMUM_RANGE_WIDTH + RogueUtils.GetRandomInt(1, margin) - 1;

				// �������ꂽ���̑傫����ύX���A�V��������ǉ����X�g�ɒǉ�����
				// �����ɁA�����������E��ʘH�Ƃ��ĕۑ����Ă���
				Range newRange = new Range();
				if (isVertical)
				{
					passList.Add(new Range(range.Start.X, devideIndex, range.End.X, devideIndex));
					newRange = new Range(range.Start.X, devideIndex + 1, range.End.X, range.End.Y);
					range.End.Y = devideIndex - 1;
				}
				else
				{
					passList.Add(new Range(devideIndex, range.Start.Y, devideIndex, range.End.Y));
					newRange = new Range(devideIndex + 1, range.Start.Y, range.End.X, range.End.Y);
					range.End.X = devideIndex - 1;
				}
				newRangeList.Add(newRange);
				// �ǉ����X�g�ɐV��������ޔ�����B
				isDevided = true;
			}

			// �ǉ����X�g�ɑޔ����Ă������V��������ǉ�����B
			rangeList.AddRange(newRangeList);


			return isDevided;
		}

		private void CreateRoom()
		{
			// �����̂Ȃ���悪�΂�Ȃ��悤�Ƀ��X�g���V���b�t������
			rangeList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

			// 1��悠����1����������Ă����B���Ȃ���������B
			foreach (Range range in rangeList)
			{
				System.Threading.Thread.Sleep(1);
				// 20���̊m���ŕ��������Ȃ�
				// �������A�ő啔�����̔����ɖ����Ȃ��ꍇ�͍��
				if (roomList.Count > maxRoom / 2 && RogueUtils.RandomJadge(0.2f))
				{
					continue;
				}

				// �P�\���v�Z
				int marginX = range.GetWidthX() - StageData.MINIMUM_RANGE_WIDTH + 1;
				int marginY = range.GetWidthY() - StageData.MINIMUM_RANGE_WIDTH + 1;

				// �J�n�ʒu������
				int randomX = RogueUtils.GetRandomInt(1, marginX);
				int randomY = RogueUtils.GetRandomInt(1, marginY);

				// ���W���Z�o
				int startX = range.Start.X + randomX;
				int endX = range.End.X - RogueUtils.GetRandomInt(0, (marginX - randomX)) - 1;
				int startY = range.Start.Y + randomY;
				int endY = range.End.Y - RogueUtils.GetRandomInt(0, (marginY - randomY)) - 1;

				// �������X�g�֒ǉ�
				Range room = new Range(startX, startY, endX, endY);
				roomList.Add(room);

				// �ʘH�����
				CreatePass(range, room);
			}
		}

		private void CreatePass(Range range, Range room)
		{
			List<int> directionList = new List<int>();
			if (range.Start.X != 0)
			{
				// X�}�C�i�X����
				directionList.Add(0);
			}
			if (range.End.X != StageData.MAP_SIZE_X - 1)
			{
				// X�v���X����
				directionList.Add(1);
			}
			if (range.Start.Y != 0)
			{
				// Y�}�C�i�X����
				directionList.Add(2);
			}
			if (range.End.Y != StageData.MAP_SIZE_Y - 1)
			{
				// Y�v���X����
				directionList.Add(3);
			}

			// �ʘH�̗L�����΂�Ȃ��悤�A���X�g���V���b�t������
			directionList.Sort((a, b) => RogueUtils.GetRandomInt(0, 1) - 1);

			bool isFirst = true;
			foreach (int direction in directionList)
			{
				System.Threading.Thread.Sleep(1);
				// 70%�̊m���ŒʘH�����Ȃ�
				// �������A�܂��ʘH���Ȃ��ꍇ�͕K�����
				if (!isFirst && RogueUtils.RandomJadge(0.6f))
				{
					continue;
				}
				else
				{
					isFirst = false;
				}

				// �����̔���
				int random;
				switch (direction)
				{
					case 0: // X�}�C�i�X����
						random = room.Start.Y + RogueUtils.GetRandomInt(1, room.GetWidthY()) - 1;
						roomPassList.Add(new Range(range.Start.X, random, room.Start.X - 1, random));
						break;

					case 1: // X�v���X����
						random = room.Start.Y + RogueUtils.GetRandomInt(1, room.GetWidthY()) - 1;
						roomPassList.Add(new Range(room.End.X + 1, random, range.End.X, random));
						break;

					case 2: // Y�}�C�i�X����
						random = room.Start.X + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
						roomPassList.Add(new Range(random, range.Start.Y, random, room.Start.Y - 1));
						break;

					case 3: // Y�v���X����
						random = room.Start.X + RogueUtils.GetRandomInt(1, room.GetWidthX()) - 1;
						roomPassList.Add(new Range(random, room.End.Y + 1, random, range.End.Y));
						break;
				}
			}

		}

		private void TrimPassList(ref int[,] map)
		{
			// �ǂ̕����ʘH������ڑ�����Ȃ������ʘH���폜����
			for (int i = passList.Count - 1; i >= 0; i--)
			{
				Range pass = passList[i];

				bool isVertical = pass.GetWidthY() > 1;

				// �ʘH�������ʘH����ڑ�����Ă��邩�`�F�b�N
				bool isTrimTarget = true;
				if (isVertical)
				{
					int x = pass.Start.X;
					for (int y = pass.Start.Y; y <= pass.End.Y; y++)
					{
						if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
						{
							isTrimTarget = false;
							break;
						}
					}
				}
				else
				{
					int y = pass.Start.Y;
					for (int x = pass.Start.X; x <= pass.End.X; x++)
					{
						if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
						{
							isTrimTarget = false;
							break;
						}
					}
				}

				// �폜�ΏۂƂȂ����ʘH���폜����
				if (isTrimTarget)
				{
					passList.Remove(pass);

					// �}�b�v�z�񂩂���폜
					if (isVertical)
					{
						int x = pass.Start.X;
						for (int y = pass.Start.Y; y <= pass.End.Y; y++)
						{
							map[x, y] = 0;
						}
					}
					else
					{
						int y = pass.Start.Y;
						for (int x = pass.Start.X; x <= pass.End.X; x++)
						{
							map[x, y] = 0;
						}
					}
				}
			}

			// �O���ɐڂ��Ă���ʘH��ʂ̒ʘH�Ƃ̐ڑ��_�܂ō폜����
			// �㉺�
			for (int x = 0; x < StageData.MAP_SIZE_X - 1; x++)
			{
				if (map[x, 0] == 1)
				{
					for (int y = 0; y < StageData.MAP_SIZE_Y; y++)
					{
						if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
						{
							break;
						}
						map[x, y] = 0;
					}
				}
				if (map[x, StageData.MAP_SIZE_Y - 1] == 1)
				{
					for (int y = StageData.MAP_SIZE_Y - 1; y >= 0; y--)
					{
						if (map[x - 1, y] == 1 || map[x + 1, y] == 1)
						{
							break;
						}
						map[x, y] = 0;
					}
				}
			}
			// ���E�
			for (int y = 0; y < StageData.MAP_SIZE_Y - 1; y++)
			{
				if (map[0, y] == 1)
				{
					for (int x = 0; x < StageData.MAP_SIZE_Y; x++)
					{
						if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
						{
							break;
						}
						map[x, y] = 0;
					}
				}
				if (map[StageData.MAP_SIZE_X - 1, y] == 1)
				{
					for (int x = StageData.MAP_SIZE_X - 1; x >= 0; x--)
					{
						if (map[x, y - 1] == 1 || map[x, y + 1] == 1)
						{
							break;
						}
						map[x, y] = 0;
					}
				}
			}
		}
		//�v���C���[���ړ����ĊK�i�܂ł��ǂ蒅���邩���f
		public void CanFloorDown(Position stairPos,int currentCheckPosX,int currentCheckPosY,int [,]map) 
		{

			if (currentCheckPosX == 0 || currentCheckPosY == 0) return;
			if (currentCheckPosX == StageData.MAP_SIZE_X || currentCheckPosY == StageData.MAP_SIZE_Y) return;
			if (map[currentCheckPosX, currentCheckPosY] == 0) return;
			if (checkedMap[currentCheckPosX, currentCheckPosY] == 1) return;

			checkedMap[currentCheckPosX, currentCheckPosY] = 1;

			if (stairPos.X == currentCheckPosX && stairPos.X == currentCheckPosY)
			{

				Debug.Log("��������");
				CanDownFloor = true;
				return;
			}

			CanFloorDown(stairPos, currentCheckPosX, currentCheckPosY + 1, map);
			CanFloorDown(stairPos, currentCheckPosX + 1, currentCheckPosY, map);
			CanFloorDown(stairPos, currentCheckPosX, currentCheckPosY - 1, map);
			CanFloorDown(stairPos, currentCheckPosX - 1, currentCheckPosY, map);

		}

		//���s�����Ƃ��̏���������
		public void ResetCheckData()
		{
			CanDownFloor = false;
			roomList.Clear();
			rangeList.Clear();
			passList.Clear();
			roomPassList.Clear();
			for (int x = 0; x < StageData.MAP_SIZE_X; x++) {
				for (int y = 0; y < StageData.MAP_SIZE_Y; y++) {
					
					checkedMap[x,y] = 0;
				}
			
			}
		}

	}
}