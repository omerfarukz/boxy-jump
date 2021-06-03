public class CubeJumpHistoryItem
{
	public float Time {
		get;
		set;
	}

	public float Speed {
		get;
		set;
	}

	public bool IsOneJump {
		get;
		set;
	}

	public CubeJumpHistoryItem (float time, float speed, bool isOneJump)
	{
		this.Time = time;
		this.Speed = speed;
		this.IsOneJump = isOneJump;
	}
}