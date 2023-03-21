using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMock : MonoBehaviour, ILevelProvider
{
    [SerializeField] private int _level;
    public int Value => _level;
}
