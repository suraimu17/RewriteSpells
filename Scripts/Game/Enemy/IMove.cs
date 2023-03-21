using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.players;

public interface IMove
{
	void SetTarget(GameObject newtarget,PlayerHP hp);
}