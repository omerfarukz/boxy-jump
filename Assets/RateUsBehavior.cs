using UnityEngine;
using System.Collections;

public class RateUsBehavior : MonoBehaviour {

	public int GamePlayModulationCount = 10;

	void Start () {
		if(Game.Instance.PlayCount>0 && Game.Instance.PlayCount % GamePlayModulationCount == 0)
		{
			// TODO: would you like to rate us?
		}
	}
}
