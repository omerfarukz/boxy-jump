using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateAndRepeatBlocksBehavior : MonoBehaviour {

	private int _countOfSideBySideCubes = 0;
	private int _totalBlocksCount = 0;
	private Vector3 _lastInstantietedBlockPosition;
	private Vector3 _startupPositon = new Vector3(-200,0, 0);
	
	private Queue<GameObject> GoodBlocks;
	private Queue<GameObject> BadBlocks;
	
	public GameObject Cube;
	
	public GameObject BlockGoodInstance;
	public GameObject BlockBadInstance;
	
	public float DefaultPositionY = -0.62f;
	public float Margin = 1f;
	public int MaxSideBySideBlocksCount = 4;
	public float GenerationDistance;
	public int GenerateCount = 10;
	public int FirstGenerationCount = 20;
	public bool AlwaysGoodFortune = false;
	public int FirstPlayFirstTimeGoodBlocksCount = 6;
	public List<bool> PredefinedFortune;

	void Start () {
		GoodBlocks = new Queue<GameObject>();
		BadBlocks = new Queue<GameObject>();
		
		GenerateGoodAndBads();

		GenerateNewFortune(FirstGenerationCount);
	}
	
	void Update () {
		if(Game.Instance.IsDead || Game.Instance.IsPaused)
			return;

		var max_x = Mathf.Max(Cube.transform.position.x, Camera.main.transform.position.x);
		if((_lastInstantietedBlockPosition.x - max_x) < GenerationDistance)
		{
			GenerateNewFortune(GenerateCount);
		}
	}
	
	void GenerateGoodAndBads ()
	{
		for (int i = 0; i < FirstGenerationCount; i++) {
			var newGoodBlock = Instantiate(BlockGoodInstance);
			var newBadBlock = Instantiate(BlockBadInstance);
			
			newBadBlock.transform.position = _startupPositon;
			newGoodBlock.transform.position = _startupPositon;
			
			newGoodBlock.transform.SetParent(this.transform);
			newBadBlock.transform.SetParent(this.transform);
			
			GoodBlocks.Enqueue(newGoodBlock);
			BadBlocks.Enqueue(newBadBlock);
		}
	}
	
	void GenerateNewFortune (int count)
	{
		int newTotalBlocksCount = _totalBlocksCount + count;
		
		for (int i = _totalBlocksCount; i < newTotalBlocksCount; i++) {

			bool isGoodFortune = false;
			
			if(AlwaysGoodFortune)
			{
				isGoodFortune = true;
			}
			else if(PredefinedFortune != null && PredefinedFortune.Count > 0)
			{
				isGoodFortune = PredefinedFortune[0];
				PredefinedFortune.RemoveAt(0);
			}
			else if(Game.Instance.IsFirstPlayMode && i < FirstPlayFirstTimeGoodBlocksCount)
			{
				isGoodFortune = true;
			}
			else if(i == _totalBlocksCount)
			{
				isGoodFortune = true;
			}
			else if(_countOfSideBySideCubes == 1)
			{
				isGoodFortune = true;
			}
			else if(_countOfSideBySideCubes == MaxSideBySideBlocksCount)
			{
				//fake bad fortune
				isGoodFortune = false;
			}
			else
			{
				isGoodFortune = Random.Range(0,10) % 3 == 0;
			}
			
			if(AlwaysGoodFortune)
				isGoodFortune = true;
			
			GameObject newBlock;
			
			var blockPosition = new Vector3(i * Margin, DefaultPositionY, 0f);
			_lastInstantietedBlockPosition = blockPosition;
			
			if(isGoodFortune)
			{
				newBlock = GoodBlocks.Dequeue();
				newBlock.transform.position = blockPosition;
				GoodBlocks.Enqueue(newBlock);
				
				++_countOfSideBySideCubes;
			}
			else
			{
				newBlock = BadBlocks.Dequeue();
				newBlock.transform.position = blockPosition;
				BadBlocks.Enqueue(newBlock);
				
				_countOfSideBySideCubes = 1;
			}
			
		} // for
		
		_totalBlocksCount = newTotalBlocksCount;
	}
}
