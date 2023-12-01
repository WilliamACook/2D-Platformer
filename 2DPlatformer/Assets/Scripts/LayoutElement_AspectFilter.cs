//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class LayoutElement_AspectFilter : LayoutElement
//{
//    [SerializeField] float m_aspect;
//    [SerializeField] AspectPriority m_aspectPriority;

//    protected IEnumerator Start()
//    {
//        yield return null;
//        OnValidate();
//    }

//    protected override void OnValidate()
//    {
//        base.OnValidate();

//        if(m_aspectPriority == AspectPriority.WIDTH_FROM_HEIGHT)
//        {
//            preferredWidth = (transform as RectTransform).rect.height * m_aspect;
//        }
//        else
//        {
//            preferredHeight = (transform as RectTransform).rect.width / m_aspect;
//        }
//    }

//    public override void CalculateLayoutInputVertical()
//    {
//        OnValidate();
//    }
//}

//public enum AspectPriority
//{
//    WIDTH_FROM_HEIGHT,
//    HEIGHT_FROM_WIDTH,
//}
