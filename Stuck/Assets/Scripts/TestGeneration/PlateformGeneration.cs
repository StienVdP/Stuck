using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformGeneration : MonoBehaviour {

    public int nbRooms = 10; // Nb de rooms a génerer

    public GameObject Level1;
    public GameObject[] tabRooms; // contient toutes les rooms 10x10
    /*
    public GameObject[] blocNE_N0;
    public GameObject[] blocSE_SO;
    public GameObject[] blocS_N;
    public GameObject[] blocNE_S0;
    public GameObject[] blocSE_NO;
    */


    private int compteur = 0;
    private int idLatestRoomImplented;

    // Use this for initialization
    void Start ()
    {
        
        //Instantiate(Level1, new Vector2(0, 0), Quaternion.identity);
        /*
        compteur++;
        while (compteur < 5) 
        {
            if(compteur%2 == 0)
            {
                Instantiate(blocSE_SO[0], new Vector2(compteur * 25.6f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(blocNE_N0[0], new Vector2(compteur * 25.6f, -12.8f), Quaternion.identity);
            }
            compteur++;
        }*/

        for (int room = 0; room < nbRooms; room++)
        {
            if (idLatestRoomImplented != 666) // Si la derniere room n'est pas level1
            {
                string lastRoomSortie = tabRooms[idLatestRoomImplented].gameObject.GetComponent<RoomsInfo>().entreAccepter;
                bool roomAjouter = false;
                while (roomAjouter == false)
                {

                    Debug.Log("Dans le while");
                    int idRoomToImplement = Random.Range(0, 5);
                    Debug.Log("entreaccepter lastroom : "+ lastRoomSortie+" entre de la roomImplenté : "+ tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre);
                    if (lastRoomSortie == tabRooms[idRoomToImplement].gameObject.GetComponent<RoomsInfo>().entre)
                    {
                        Debug.Log("Dans le if du while");
                        Instantiate(tabRooms[idRoomToImplement], new Vector2(room * 25.6f, 0), Quaternion.identity);
                        roomAjouter = true;
                        idLatestRoomImplented = idRoomToImplement;
                    }
                    //roomAjouter = true;
                }
                Debug.Log("Dans le if du for");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
    }
}
