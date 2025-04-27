using UnityEngine;

[CreateAssetMenu(fileName = "PlayerItem", menuName = "ScriptableObjects/PlayerItem", order = 1)]
public class PlayerItem : ScriptableObject
{
    public GameObject itemModel;
    public string itemName;
    public string Description;
}
