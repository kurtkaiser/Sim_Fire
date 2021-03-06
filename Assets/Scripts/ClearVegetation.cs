﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearVegetation : MonoBehaviour
{
    private GameManager gameManager;
    private int totalEnergyUsed;
    private int energyLevel;
    private int tileIndex;
    private GameObject clear;
    public GameObject clearNorth;
    public GameObject clearSouth;
    public GameObject clearEast;
    public GameObject clearWest;

    public int WorkRate;        // % of unit's energy bar per second
    public int FinishAmount;    // % of unit's energy bar needed to finish clearing vegetation

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Clear(GameObject targetTile, string direction)
    {
        totalEnergyUsed = 0;

        switch(direction)
        {
            case "North": clear = clearNorth; break;
            case "South": clear = clearSouth; break;
            case "East": clear = clearEast; break;
            case "West": clear = clearWest; break;
        }

        // attempt to clear vegetation, and succeed if totalEnergyUsed >= FinishAmount
        while ((totalEnergyUsed < FinishAmount) && (targetTile != null))
        {
            // fetch the energy level from the calling unit
            energyLevel = gameObject.GetComponent<FireCrew>().energyLevel;
            
            // clear vegetation from the target
            if (targetTile.GetComponent<TileScript>().GetObstacles() > 0 && energyLevel >= WorkRate)
            {
                energyLevel -= WorkRate;
                totalEnergyUsed += WorkRate;
                clear.SetActive(true);
            }
            else
            {
                targetTile = null;
                gameObject.GetComponent<FireCrew>().targetTile = null;
            }

            // required energy has been expended, so call DestroyObstacles()
            if (totalEnergyUsed >= FinishAmount)
            {
                // Grab tile index
                for(int i = 0; i < gameManager.AllTiles.Length; i++ ) 
                {
                    if(GameObject.ReferenceEquals(targetTile, gameManager.AllTiles[i])) tileIndex = i;
                }
                Debug.Log("Cleared vegetation on tile: " + (tileIndex + 1).ToString());
                targetTile.GetComponent<TileScript>().DestroyObstacle();
                targetTile = null;
                gameObject.GetComponent<FireCrew>().targetTile = null;
                clear.SetActive(false);
            }

            // update the energy level of the calling unit
            gameObject.GetComponent<FireCrew>().energyLevel = energyLevel;

            yield return new WaitForSeconds(1.0f);

        }
    }
}
