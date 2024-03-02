using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjEntityPool : MonoBehaviour
{
	[Header("Reference")]
	[SerializeField] GameObject m_EntityPrefab;

	[Header("Config")]
	[SerializeField] int m_ObjCount;

	private int m_ObjInUse = 0;
	private Transform m_Trans;
	private List<GameObject> m_ObjectPool;
	private void Awake()
	{
		m_Trans = transform;
		m_ObjectPool = new List<GameObject>(m_ObjCount);
	}

	private void Update()
	{
		EnsureObjCount();
	}

	public void EnsureObjCount()
	{
		if (m_ObjectPool.Capacity < m_ObjCount)
			m_ObjectPool.Capacity = m_ObjCount;

		var l_DeltaCount = (m_ObjCount - m_ObjectPool.Count);
		if (l_DeltaCount == 0) return;

		for (; l_DeltaCount > 0; --l_DeltaCount)
		{
			var l_Obj = Instantiate(m_EntityPrefab, m_Trans);
			m_ObjectPool.Add(l_Obj);
		}

		for (int i = (m_ObjCount-l_DeltaCount-1); i >= m_ObjCount; --i)
		{
			m_ObjectPool[i].SetActive(false);
		}
	}
}
