using UnityEngine;

[CreateAssetMenu(menuName ="dreamCase/Item")]
public class Item : ScriptableObject
{
    public int value; //-1 for completed
    public Sprite sprite;
}
