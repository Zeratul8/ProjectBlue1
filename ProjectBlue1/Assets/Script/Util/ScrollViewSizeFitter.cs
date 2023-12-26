using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public static class ScrollViewSizeFitter
{
    public static void SetScrollViewSize(ref ScrollRect scrollRect, GridLayoutGroup layout, int contentsCount)
    {

        int contentRowCount = Mathf.RoundToInt(scrollRect.content.sizeDelta.x / layout.cellSize.x);
        scrollRect.content.sizeDelta += new Vector2(0, (contentsCount / contentRowCount) * layout.cellSize.y + (contentsCount / contentRowCount + 1) * layout.spacing.y);
    }
}
