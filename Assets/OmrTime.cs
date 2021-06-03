using System;

public class OmrTime
{
	private static DateTime _base = new DateTime(2015, 11, 1);
	
	public static int GetCurrent()
	{
		return GetAsOmrTime(DateTime.Now);
	}
	
	public static DateTime GetAsDate(int omrTime)
	{
		return _base.AddSeconds(omrTime);
	}

	public static int GetAsOmrTime(DateTime dateTime)
	{
		return (int)(dateTime - _base).TotalSeconds;
	}
}