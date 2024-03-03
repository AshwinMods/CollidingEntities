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
		if (m_ObjCount <= m_ObjectPool.Count && 
			m_ObjInUse == m_ObjCount) return;

		if (m_ObjectPool.Capacity < m_ObjCount)
			m_ObjectPool.Capacity = m_ObjCount;

		var l_NewCount = (m_ObjCount - m_ObjectPool.Count);
		for (; l_NewCount > 0; --l_NewCount)
			m_ObjectPool.Add(Instantiate(m_EntityPrefab, m_Trans));

		var l_DisIdx = m_ObjInUse - 1;
		for (; l_DisIdx >= m_ObjCount; --l_DisIdx)
			m_ObjectPool[l_DisIdx].SetActive(false);

		for (; m_ObjInUse < m_ObjCount; ++m_ObjInUse)
			m_ObjectPool[m_ObjInUse].SetActive(true);

		m_ObjInUse = m_ObjCount;
	}
}
