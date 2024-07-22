using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Variable Declarations
	[Header("Reference")]
	[SerializeField] ScreenEdge m_ScreenEdge;
	[SerializeField] UISlider m_ObjCount;
	[SerializeField] UISlider m_Speed;
	[SerializeField] UISlider m_Area;

	public static bool Simulation = true;
	public static bool DrawInstanced = false;
	public static bool DrawIndirect = false;
	public static bool SimOnMath = false;
	public static bool SimOnCompute = false;

	#endregion

	#region Unity Callbacks
	private void Start()
	{
		m_ObjCount.Slider.onValueChanged.AddListener(OnChanged_Count);
		m_Speed.Slider.onValueChanged.AddListener(OnChanged_Speed);
		m_Area.Slider.onValueChanged.AddListener(OnChanged_Area);

		m_ObjCount.Slider.value = EntityManager.ObjectCount;
		m_Speed.Slider.value = EntityManager.ObjectSpeed;
		m_Area.Slider.value = ScreenEdge.Size;
	}
	#endregion

	#region UI Callbacks
	public void SetSimulation(bool a_Enable) 
	{
		Simulation = a_Enable;
		Time.timeScale = a_Enable ? 1 : 0;
	}
	public void UseInstancing(bool a_Instanced)
	{
		if (a_Instanced) DrawIndirect = false;
		DrawInstanced = a_Instanced;
	}
	public void UseIndirect(bool a_Indirect)
	{
		if (a_Indirect) DrawInstanced = false;
		DrawIndirect = a_Indirect;
	}
	public void UseMath(bool a_Math)
	{
		SimOnMath = a_Math;
	}
	public void UseCompute(bool a_UseCompute)
	{
		SimOnCompute = a_UseCompute;
	}

	public void OnChanged_Count(float a_Value)
	{
		EntityManager.ObjectCount = (int)a_Value;
	}
	public void OnChanged_Area(float a_Value)
	{
		ScreenEdge.Setup_ScreenEdge(a_Value, 0);
	}
	public void OnChanged_Speed(float a_Value)
	{
		EntityManager.ObjectSpeed = a_Value;
	}
	public void OnChanged_FixSpeed(bool a_FixSpeed)
	{
		EntityManager.FixedSpeed = a_FixSpeed;
	}
	#endregion
}
