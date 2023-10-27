using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
	private static PointerType m_CurrentType;
	private static PointerType? m_OverrideType;
	private static Pointer m_Instance;
	private static PointerSO[] m_Datas;

	public static void TryChangeIcon(PointerType toType)
	{
		if (m_CurrentType == toType) { return; } //already in the new type for some reason
		m_CurrentType = toType;
		if (m_Instance == null || m_OverrideType != null) { return; }
		m_Instance.ChangeType(toType);
	}

	public static void TryBackupSetIcon(PointerType toType)
	{
		if (m_CurrentType == toType || m_CurrentType != PointerType.DEFAULT) { return; }
		m_CurrentType = toType;
		if (m_Instance == null || m_OverrideType != null) { return; }
		m_Instance.ChangeType(toType);
	}

	public static void TryResetIcon(PointerType fromType)
	{
		if (m_CurrentType == PointerType.DEFAULT || m_CurrentType != fromType) { return; } //either already default type or something has changed us since this request so don't override
		m_CurrentType = PointerType.DEFAULT;
		if (m_Instance == null || m_OverrideType != null) { return; }
		m_Instance.ChangeType(PointerType.DEFAULT);
	}

	public static void TrySetOverrideIcon(PointerType overrideType)
	{
		if (m_OverrideType == overrideType) { return; }
		m_OverrideType = overrideType;
		if (m_Instance == null) { return; }
		m_Instance.ChangeType(overrideType);
	}

	public static void TryResetOverrideIcon(PointerType fromOverrideType)
	{
		if (m_OverrideType == null || m_OverrideType != fromOverrideType) { return; }
		m_OverrideType = null;
		if (m_Instance == null) { return; }
		m_Instance.ChangeType(m_CurrentType);
	}

	private void ChangeType(PointerType type)
	{
		//find the pointer data object for the pointer type
		int dataIndex = -1;
		for (int i = 0; i < m_Datas.Length; i++)
		{
			if (m_Datas[i].Type == type)
			{
				dataIndex = i;
				break;
			}
		}
		if (dataIndex == -1) { Debug.LogWarning($"Tried to change to {type.ToString()} but no data for that type has been loaded!"); return; }

		Cursor.SetCursor(m_Datas[dataIndex].Texture, m_Datas[dataIndex].Hotspot, CursorMode.ForceSoftware);
	}
	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		if (m_Instance == this) { return; } //already iinitialised this as the instance
		if (m_Instance != null) { Destroy(this.gameObject); return; } //there is another instance in the world so destry this one, there can be only one!
		m_Instance = this;
		m_Datas = Resources.LoadAll<PointerSO>("PointerData");
		m_CurrentType = PointerType.DEFAULT;
		ChangeType(PointerType.DEFAULT);
	}

}

public enum PointerType
{
	DEFAULT,
	RESIZE_FORWARDSLASH,
	RESIZE_BACKSLASH,
	RESIZE_HORIZONTAL,
	RESIZE_VETICAL,
	MOVE,
	CLICK,
	NO_CLICK
}
