﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject forestPrefab;
    public GameObject sandPrefab;
    public Transform tileLocation1;
    public GameObject[] emptyTiles;
    public int columnCount = 18;
    public int rowCount = 10;
    public int[] values;

    // Start is called before the first frame update
    void Start()
    {
        emptyTiles = GameObject.FindGameObjectsWithTag("EmptyTile");
        values = Enumerable.Range(0, 181).ToArray();
        int currentIndex = Random.Range(0, 179);
        int prefabCount = 0;
        int bug = 0;
        string stuff = "";
        while (prefabCount < emptyTiles.Length / 3)
        {
            List<int> change = new List<int> { 1, -1, columnCount, -columnCount };
            int changeIndex = Random.Range(0, change.Count);
            int nextIndex = currentIndex + change[changeIndex];
            Debug.Log("cur: " + currentIndex + " nextIndex: " + nextIndex + " changeIndex: " + changeIndex);
            while (nextIndex < 0 || nextIndex > emptyTiles.Length - 1 || values[nextIndex] == -1 ||
                Mathf.Abs(change[changeIndex]) == 1 && nextIndex / columnCount != currentIndex / columnCount)
            {
                change.RemoveAt(changeIndex);
                if (change.Count < 1)
                {
                    currentIndex = Random.Range(0, 179);
                    while (values[currentIndex] != -1) currentIndex = Random.Range(0, 179);
                    change = new List<int> { 1, -1, columnCount, -columnCount };
                }
                changeIndex = Random.Range(0, change.Count);
                nextIndex = currentIndex + change[changeIndex];
                bug++;
                if (bug > 500)
                {
                    Debug.Log("BREAKKKKKK");
                    break;
                }
                stuff += nextIndex + ", ";
            }
            Debug.Log("stuff: " + stuff);
            currentIndex = nextIndex;
            prefabCount++;
            GameObject go = Instantiate(
                forestPrefab,
                emptyTiles[currentIndex].transform.position,
                emptyTiles[currentIndex].transform.rotation,
                forestPrefab.transform.parent);
            go.transform.localScale = emptyTiles[currentIndex].transform.localScale;
            go.name = "Forest " + currentIndex;
            Destroy(emptyTiles[currentIndex]);
            values[currentIndex] = -1;

            bug++;
            if (bug > 1000) { Debug.Log("bugggggg"); break; }
        }
        Debug.Log("pre count: " + prefabCount);
        // Generate sand tiles
        currentIndex = Random.Range(0, 179);

        prefabCount = 0;
        bug = 0;
        while (prefabCount < emptyTiles.Length / 3)
        {
            while (values[currentIndex] == -1) currentIndex = Random.Range(0, 179);
            GameObject go = Instantiate(
                sandPrefab,
                emptyTiles[currentIndex].transform.position,
                emptyTiles[currentIndex].transform.rotation,
                sandPrefab.transform.parent);
            go.transform.localScale = emptyTiles[currentIndex].transform.localScale;
            go.name = "Sand " + currentIndex;
            Destroy(emptyTiles[currentIndex]);
            values[currentIndex] = -1;
            prefabCount++;
            bug++;
            if (bug > 2000) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}