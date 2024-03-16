using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
	#region Variable Declaration
	private static EntityManager s_Inst;

	[Header("Reference")]
	[SerializeField] Entity m_EntityPrefab;
	[Space]
	[Header("Instanced")]
	[SerializeField] Mesh m_Mesh;
	[SerializeField] Material m_Material;
	[SerializeField] ComputeShader m_SimCompute;

	[Header("Config")]
	[SerializeField] int m_MaxItemLimit = 10000;
	[SerializeField] int m_NewItemLimit = 1;
	[SerializeField] int m_ObjCount = 10;
	[SerializeField] float m_ObjSpeed = 10;
	[SerializeField] bool m_EnsureSpeed;

	private int m_ObjInUse = 0;
	private Transform m_Trans;
	private List<Entity> m_ObjectPool;
	private Matrix4x4[] m_WMatrices;

	private bool m_DrawingInstanced = false;
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

	public EntityManager() => s_Inst = this;

	#region Unity Callbacks
	private void Awake()
	{
		m_Trans = transform;
		m_ObjectPool = new List<Entity>(m_MaxItemLimit);
		m_WMatrices = new Matrix4x4[m_MaxItemLimit];

		Physics.invokeCollisionCallbacks = false;
		Physics.autoSyncTransforms = true;
	}
	private void Update()
	{
		EnsureObjCount(m_NewItemLimit);
		if (m_EnsureSpeed) EnsureObjSpeed();
		if (m_DrawingInstanced ^ GameManager.DrawInstanced) RefreshEntities();
		if (m_DrawingInstanced) RenderViaInstance();
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
			var l_RandPos = Random.insideUnitCircle;
			var l_Entity = Instantiate(m_EntityPrefab, 
				m_EntityPrefab.RB.position + l_RandPos, 
				Quaternion.identity, m_Trans);

			l_Entity.RB.velocity = l_RandPos.normalized * m_ObjSpeed;
			m_ObjectPool.Add(l_Entity);
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
		for (var i = 0; i < m_ObjInUse; ++i)
		{
			var l_Vel = m_ObjectPool[i].RB.velocity;
			var l_Mag = l_Vel.magnitude;

			if (l_Mag == m_ObjSpeed) continue;
			else if (l_Mag < 0.1f) l_Vel = Random.insideUnitCircle.normalized * m_ObjSpeed;
			else l_Vel = (l_Vel / l_Mag) * m_ObjSpeed;

			m_ObjectPool[i].RB.velocity = l_Vel;
		}
	}

	private void RefreshEntities()
	{
		var l_UseRB = !GameManager.SimOnMath;
		var l_UseRend = !GameManager.DrawInstanced;
		for (int i = 0; i < m_ObjectPool.Count; i++)
		{
			m_ObjectPool[i].RB.simulated = l_UseRB;
			m_ObjectPool[i].Rend.enabled = l_UseRend;
		}
		m_DrawingInstanced = GameManager.DrawInstanced;
	}
	public void RenderViaInstance()
	{
		if (GameManager.Simulation)
		{
			for (int i = 0; i < m_ObjInUse; i++)
				m_WMatrices[i] = m_ObjectPool[i].Trans.localToWorldMatrix;
		}
		Graphics.DrawMeshInstanced(m_Mesh, 0, m_Material, m_WMatrices, m_ObjInUse);
	}
	#endregion
}
