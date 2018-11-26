using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour {
    /**
     * Tuto de generation procedurale 
     * Marche mais ne correspond pas a notre projet Stuck
     */
    public GameObject groundTop, bridge, spikes, groundMid;

    public int minimumPlateformeSize = 1;
    public int maxPlateformeSize = 10;
    public int maxHazardSize = 3; // Espace au milieu (trou)
    public int maxHight = 3;
    public int maxDrop = -3;

    public int plateforms = 100; //nb of plateforms
    [Range (0.0f, 1)]
    public float hazardChance = 0.5f;
    [Range(0.0f, 1)]
    public float bridgeChance = 0.1f;
    [Range(0.0f, 1)]
    public float spikeChance = 0.5f;

    private int blocNumber = 1;
    private int blocHight;
    private bool isHazard;

	// Use this for initialization
	void Start () {
        // Begening Tile
        Instantiate(groundTop, new Vector2(0, 0), Quaternion.identity);

        for (int plat = 1; plat < plateforms; plat++){
            if(isHazard == true)
            {
                isHazard = false;
            }
            else
            {
                if(Random.value < hazardChance)
                {
                    isHazard = true;
                }
                else
                {
                    isHazard = false;
                }
            }


            if (isHazard == true)
            {
                int hazardSize = Mathf.RoundToInt(Random.Range(1, maxHazardSize));

                if (Random.value < spikeChance)
                {
                    // generate spike trap
                    for (int tiles = 0; tiles < hazardSize; tiles++)
                    {
                        Instantiate(spikes, new Vector2(blocNumber, blocHight-2), Quaternion.identity);

                        for (int grdMid = 1; grdMid < 5; grdMid++)
                        {
                            Instantiate(groundMid, new Vector2(blocNumber, (blocHight - grdMid)-2), Quaternion.identity);
                        }

                        blocNumber++;
                    }
                }
                else
                {
                    // hole in the ground
                    blocNumber += hazardSize;
                }
            }
            else
            {
                if (Random.value < bridgeChance)
                {
                    // Bridge generation
                    int platformeSize = Mathf.RoundToInt(Random.Range(minimumPlateformeSize, maxPlateformeSize));
                    blocHight = blocHight + Random.Range(maxDrop, maxHight);

                    for (int tiles = 0; tiles < platformeSize; tiles++)
                    {
                        if(tiles == 0 || tiles == (platformeSize - 1))
                        {
                            Instantiate(groundTop, new Vector2(blocNumber, blocHight), Quaternion.identity);

                            for (int grdMid = 1; grdMid < 5; grdMid++)
                            {
                                Instantiate(groundMid, new Vector2(blocNumber, blocHight - grdMid), Quaternion.identity);
                            }
                            
                            blocNumber++;
                        }
                        else
                        {
                            Instantiate(bridge, new Vector2(blocNumber, blocHight), Quaternion.identity);
                            blocNumber++;
                        }
                    }
                }
                else
                {
                    // Plateforme generation
                    int platformeSize = Mathf.RoundToInt(Random.Range(minimumPlateformeSize, maxPlateformeSize));
                    blocHight = blocHight + Random.Range(maxDrop, maxHight);

                    for (int tiles = 0; tiles < platformeSize; tiles++)
                    {
                        Instantiate(groundTop, new Vector2(blocNumber, blocHight), Quaternion.identity);

                        for (int grdMid = 1; grdMid < 5; grdMid++)
                        {
                            Instantiate(groundMid, new Vector2(blocNumber, blocHight-grdMid), Quaternion.identity); 
                        }

                        blocNumber++;
                    }
                }

            }

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
