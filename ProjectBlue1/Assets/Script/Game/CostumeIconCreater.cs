using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeIconCreater : MonoBehaviour
{
    [SerializeField]
    GameObject costumeSelector;
    [SerializeField]
    ScrollRect scrollRect;

    [SerializeField]
    CostumeSO weaponData;

    List<CostumeController> costumeControllers = new List<CostumeController>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public List<CostumeController> CreateIcons()
    {
        int count = weaponData.sprites.Count;
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
                iconImg.sprite = weaponData.sprites[i];
                costumeControllers.Add(icon.GetComponent<CostumeController>());
            }
            icon.SetActive(false);
        }

        if(costumeControllers.Count > 0)
            return costumeControllers;
        else return null;
    }
}
