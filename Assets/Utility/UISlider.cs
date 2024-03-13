using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
	#region Variable Declarations
	[SerializeField] Slider m_Slider;
	[Space]
	[SerializeField] Text m_Value;
	[SerializeField] string m_ValueFormat;
	[SerializeField] float m_InitValue;
	#endregion

	#region Unity Callbacks
	private void Start()
	{
		m_Slider.onValueChanged.AddListener(OnValueChange);
		m_Slider.value = m_InitValue;
	}
	#endregion

	#region UI Callbacks
	private void OnValueChange(float a_Value)
	{
		m_Value.text = a_Value.ToString(m_ValueFormat);
	}
	#endregion
}
