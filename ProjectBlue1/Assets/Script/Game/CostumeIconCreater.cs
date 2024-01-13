using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeIconCreater : MonoBehaviour
{
    [SerializeField]
    GameObject costumeSelector;
    [SerializeField]
    CostumeSO costumeData;
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GridLayoutGroup layout;

    List<CostumeController> costumeControllers = new List<CostumeController>();

    public List<CostumeController> CreateIcons()
    {
        if(scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();
        if(layout == null)
            layout = GetComponentInChildren<GridLayoutGroup>();
        int count = costumeData.sprites.Count;
        Debug.Log("ÄÜÅÙÃ÷ °¹¼ö : " + count);
        for (int i = 0; i < count; i++)
        {
            GameObject icon = Instantiate(costumeSelector, scrollRect.content);
            Image[] images = icon.GetComponentsInChildren<Image>();
            Image iconImg = null;
            foreach (Image image in images)
            {
                Debug.Log("ÀÌ¹ÌÁö ÀÌ¸§ : " + image.gameObject.name);
                if (image.name.Equals("IMG_Costume"))
                {
                    iconImg = image;
                    break;
                }
            }
            if (iconImg != null)
            {
                iconImg.sprite = costumeData.sprites[i];
                costumeControllers.Add(icon.GetComponent<CostumeController>());
            }
            icon.SetActive(false);
        }

        //ScrollViewSizeFitter.SetScrollViewSize(ref scrollRect, layout, count);

        /*
        int contentRowCount = Mathf.RoundToInt(scrollRect.content.sizeDelta.x / layout.cellSize.x);
        Debug.Log("»çÀÌÁî : " + scrollRect.content.sizeDelta.x);
        Debug.Log("Ä«¿îÆ® °¹¼ö : " + contentRowCount);
        scrollRect.content.sizeDelta += new Vector2(0, (count / contentRowCount) * 150 + (count / contentRowCount + 1) * 50);
        */

        if(costumeControllers.Count > 0)
            return costumeControllers;
        else return null;
    }
}
