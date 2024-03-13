using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjEntityPool : MonoBehaviour
{
	#region Variable Declaration
	private static ObjEntityPool s_Inst;

	[Header("Reference")]
	[SerializeField] Rigidbody2D m_EntityPrefab;

	[Header("Config")]
	[SerializeField] int m_NewItemLimit;
	[SerializeField] int m_ObjCount;
	[SerializeField] bool m_EnsureSpeed;
	[SerializeField] float m_ObjSpeed;

	private int m_ObjInUse = 0;
	private Transform m_Trans;
	private List<Rigidbody2D> m_ObjectPool;

	#endregion

	#region Properties
	public static int ObjectCount 
	{ 
		get => s_Inst.m_ObjCount; 
		set
		{
			s_Inst.m_ObjCount = value;
		}
	}
	public static bool FixedSpeed 
	{
		get => s_Inst.m_EnsureSpeed; 
		set => s_Inst.m_EnsureSpeed = value; 
	}
	public static float ObjectSpeed
	{
		get => s_Inst.m_ObjSpeed;
		set
		{
			s_Inst.m_ObjSpeed = value;
			s_Inst.EnsureObjSpeed();
		}
	}
	#endregion

	public ObjEntityPool() => s_Inst = this;

	#region Unity Callbacks
	private void Awake()
	{
		m_Trans = transform;
		m_ObjectPool = new List<Rigidbody2D>(m_ObjCount);
	}
	private void Update()
	{
		EnsureObjCount(m_NewItemLimit);
		if (m_EnsureSpeed) EnsureObjSpeed();
	}
	private void OnValidate()
	{
		if (m_ObjSpeed <= 0) m_ObjSpeed = 1;
	}
	#endregion

	#region Private Functions
	private void EnsureObjCount(int a_MaxNewCount)
	{
		if (m_ObjCount <= m_ObjectPool.Count && 
			m_ObjInUse == m_ObjCount) return;

		// Ensure List Capacity Ahead
		if (m_ObjectPool.Capacity < m_ObjCount)
			m_ObjectPool.Capacity = m_ObjCount;

		// Add New Instance to Ensure ObjectCount
		var l_NewCount = (m_ObjCount - m_ObjectPool.Count);
		l_NewCount = Mathf.Min(a_MaxNewCount, l_NewCount);
		var l_CurObjCount = m_ObjectPool.Count + l_NewCount;

		for (; l_NewCount > 0; --l_NewCount) 
		{
			var l_InstRB = Instantiate(m_EntityPrefab, m_Trans);
			l_InstRB.velocity = Random.insideUnitCircle.normalized * m_ObjSpeed;
			m_ObjectPool.Add(l_InstRB);
			m_ObjInUse++;
		}

		// Ensure New Unused Objects are Disabled
		var l_DisIdx = m_ObjInUse - 1;
		for (; l_DisIdx >= l_CurObjCount; --l_DisIdx)
			m_ObjectPool[l_DisIdx].gameObject.SetActive(false);
		
		// Ensure New InUse objects are Enabled
		for (; m_ObjInUse < l_CurObjCount; ++m_ObjInUse)
			m_ObjectPool[m_ObjInUse].gameObject.SetActive(true);

		// Update InUse count, (yeahh! Incr in Enable Loop is also an issue)
		m_ObjInUse = l_CurObjCount;
	}

	private void EnsureObjSpeed()
	{
		for (var i = 0; i < m_ObjCount; ++i)
		{
			var l_Vel = m_ObjectPool[i].velocity;
			var l_Mag = l_Vel.magnitude;

			if (l_Mag == m_ObjSpeed) continue;
			else if (l_Mag < 0.1f) l_Vel = Random.insideUnitCircle.normalized * m_ObjSpeed;
			else l_Vel = (l_Vel / l_Mag) * m_ObjSpeed;

			m_ObjectPool[i].velocity = l_Vel;
		}
	}
	#endregion
}
