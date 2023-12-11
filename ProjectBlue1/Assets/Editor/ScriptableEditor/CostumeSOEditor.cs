using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CostumeSO))]
public class CostumeSOEditor : Editor
{
    CostumeSO costumeSO;
    List<Sprite> sprites = new List<Sprite>();
    readonly string weaponPath = "Costumes/Weapon/MeleeWeapon1H";

    private void OnEnable()
    {
        costumeSO = (CostumeSO)target;
        sprites = costumeSO.sprites;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("GetSprites"))
        {
            GetSprites();
        }
        GUILayout.EndHorizontal();
    }

    void GetSprites()
    {
        //리소스 로딩해와서 List에 넣기
        sprites = Resources.LoadAll<Sprite>(weaponPath).ToList();

        costumeSO.sprites = sprites;

        costumeSO.SetDictionary();kfemt


        EditorUtility.SetDirty(costumeSO);
        
    }

}
