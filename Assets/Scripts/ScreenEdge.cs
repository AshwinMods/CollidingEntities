using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ScreenEdge : MonoBehaviour
{
	#region Variable Declaration
	private static ScreenEdge s_Inst;

	[Header("Reference")]
	[SerializeField] Camera m_Camera;
	[SerializeField] Transform[] m_Edges;

	[Header("Config")]
	[SerializeField] float m_Size;
	[SerializeField] float m_Offset;

	public static float Size 
	{
		get => s_Inst.m_Size;
		set => Setup_ScreenEdge(value, s_Inst.m_Offset);
	}
	#endregion

	public ScreenEdge()
	{
		s_Inst = this;
	}

	#region Unity Callbacks
	private void Start()
	{
		//Setup_ScreenEdge(m_Size, m_Offset);
	}
	private void OnValidate()
	{
		//Setup_ScreenEdge();
	}
	#endregion

	#region Public Functions
	public static void Setup_ScreenEdge(float a_Size, float a_Offset)
	{
		s_Inst.m_Size = a_Size;
		s_Inst.m_Camera.orthographicSize = a_Size;
		s_Inst.m_Offset = a_Offset;
		s_Inst.Setup_ScreenEdge();
	}
	private void Setup_ScreenEdge()
	{
		var l_Size = m_Camera.orthographicSize;
		var l_Sides = l_Size * m_Camera.aspect;

		Vector3 l_WidthSize = new Vector3(l_Sides * 2, 1, 1);
		Vector3 l_HeightSize = new Vector3(1, l_Size * 2, 1);
		m_Edges[0].localScale = l_WidthSize;
		m_Edges[1].localScale = l_WidthSize;
		m_Edges[2].localScale = l_HeightSize;
		m_Edges[3].localScale = l_HeightSize;

		l_Size += m_Offset;
		l_Sides += m_Offset;
		m_Edges[0].position = Vector3.up * l_Size;
		m_Edges[1].position = Vector3.down * l_Size;
		m_Edges[2].position = Vector3.left * l_Sides;
		m_Edges[3].position = Vector3.right * l_Sides;
	}
	#endregion
}
