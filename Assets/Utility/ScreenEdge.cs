using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdge : MonoBehaviour
{
	[SerializeField] Camera m_Camera;
	[SerializeField] Transform[] m_Edges;
	[SerializeField] float m_Offset;

	private void Start()
	{
		Setup_ScreenEdge();
	}
	private void OnValidate()
	{
		Setup_ScreenEdge();
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
}
