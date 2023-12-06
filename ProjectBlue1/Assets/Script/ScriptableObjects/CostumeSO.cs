using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Costume", menuName = "Costume")]
public class CostumeSO : ScriptableObject
{
    public CostumeType type;
    public List<Sprite> sprites;

    public Dictionary<string, Sprite> spritesDictionary = new Dictionary<string, Sprite>();

    public void SetDictionary()
    {
        spritesDictionary.Clear();
        foreach(Sprite sprite in sprites)
        {
            spritesDictionary.Add(sprite.name, sprite);
        }
    }

}

public enum CostumeType
{
    None = -1,
    Weapon,
    Max
}
