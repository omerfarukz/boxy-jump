using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class iTweenExt : MonoBehaviour
{
	public List<iTweenExtItem> Items {
		get;
		set;
	}

	public iTweenExt ()
	{
		Items = new List<iTweenExtItem>();

	}

	public iTweenExt AddAction(Action action, float time)
	{
		return AddAction(action, time, false);
	}

	public iTweenExt AddAction(Action action, float time, bool isCoroutine)
	{
		var txi = new iTweenExtItem();
		txi.TargetAction = action; //()=> { iTween.MoveTo(target, hash); };
		txi.Time = time;
		txi.IsCoroutine = isCoroutine;
		Items.Add(txi);
		
		return this;
	}

	public iTweenExt AddCoroutine(IEnumerator enumerator)
	{
		var txi = new iTweenExtItem();
		txi.Coroutine = enumerator; //()=> { iTween.MoveTo(target, hash); };
		txi.Time = 0f;
		txi.IsCoroutine = true;
		Items.Add(txi);
	
		return this;
	}

	public iTweenExt Delay(float time)
	{
		return AddAction(null, time);
	}

	public IEnumerator Run()
	{
		if(Items==null)
			yield return null;

		foreach (var item in Items) {

			if(item.IsCoroutine)
			{
				yield return StartCoroutine(item.Coroutine);
			}
			else if(item.TargetAction != null)
			{
				item.TargetAction();
			}

			yield return new WaitForSeconds(item.Time);
		}
	}

}

public class iTweenExtItem
{
	public Action TargetAction {
		get;
		set;
	}
	
	public float Time {
		get;
		set;
	}

	public bool IsCoroutine{
		get;
		set;
	}

	public IEnumerator Coroutine {
		get;
		set;
	}
}