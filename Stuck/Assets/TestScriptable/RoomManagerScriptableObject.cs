using UnityEngine;

[CreateAssetMenu(fileName = "DataRoom", menuName = "ScriptableObjects/RoomManagerScriptableObject", order = 1)]
public class RoomManagerScriptableObject : ScriptableObject
{
    public GameObject prefabRoom;
    //public string prefabName;

    public string entre;
    public string entreAccepte;
    public int size;
}