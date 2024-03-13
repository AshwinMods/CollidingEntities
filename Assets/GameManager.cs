using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Variable Declarations
	[Header("Reference")]
	[SerializeField] ScreenEdge m_ScreenEdge;
	#endregion

	#region Unity Callbacks

	#endregion

	#region UI Callbacks
	public void OnChanged_Count(float a_Value)
	{
		ObjEntityPool.ObjectCount = (int)a_Value;
	}
	public void OnChanged_Area(float a_Value)
	{
		ScreenEdge.Setup_ScreenEdge(a_Value, 0);
	}
	public void OnChanged_Speed(float a_Value)
	{
		ObjEntityPool.ObjectSpeed = a_Value;
	}
	public void OnChanged_FixSpeed(bool a_FixSpeed)
	{
		ObjEntityPool.FixedSpeed = a_FixSpeed;
	}
	#endregion
}
