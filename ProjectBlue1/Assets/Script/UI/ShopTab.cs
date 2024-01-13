using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour
{
    [SerializeField]
    ScrollRect scrollRect;

    [SerializeField]
    Button rewardAdBtn;

    GridLayoutGroup layoutGroup;


    void Start()
    {
        layoutGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
        int contentsCount = scrollRect.content.childCount;
        //ScrollViewSizeFitter.SetScrollViewSize(ref scrollRect, layoutGroup, contentsCount);

        rewardAdBtn.onClick.AddListener(()=>AdManager.Instance.LoadCristalRewardedAd(rewardAdBtn));
    }

}
