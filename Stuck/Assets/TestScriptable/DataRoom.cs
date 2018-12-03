using UnityEngine;

public class DataRoom : MonoBehaviour
{
    // The GameObject to instantiate.
    private GameObject entityToSpawn;

    // An instance of the ScriptableObject defined above.
    public RoomManagerScriptableObject[] tabRoomManagerValues; // TODO En faire pour room normal, endroom et debutroom

    // This will be appended to the name of the created entities and increment when each is created.
    int instanceNumber = 1;
    

    void Start()
    {
        entityToSpawn = this.gameObject;
        getEntre();
        getEntreeAccepte();

    }
    /*
    public GameObject getPrefab(RoomManagerScriptableObject data)
    {
        return data.prefabRoom;
    }

    public String getEntre(RoomManagerScriptableObject data)
    {
        return data.entre;
    }

    public String getEntre(RoomManagerScriptableObject data)
    {
        return data.entre;
    }*/


    void getEntre()
    {
        Debug.Log("Entre : " + tabRoomManagerValues[1].entre);
    }

    void getEntreeAccepte()
    {
        Debug.Log("Entre Acceptées: " + tabRoomManagerValues[1].entreAccepte);
    }
}