using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] PointerType m_type;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Pointer.TryChangeIcon(m_type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Pointer.TryChangeIcon(m_type);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Pointer.TryChangeIcon(m_type);
    }
}
