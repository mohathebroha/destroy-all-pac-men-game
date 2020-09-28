using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGraph : MonoBehaviour
{
    private static int width = 28;
    private static int height = 15;

    public GameObject[,] graph = new GameObject[width, height];

    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<Node>() != null)
            {
                Vector2 position = obj.transform.position;
                graph[(int)position.x, (int)position.y] = obj;
                //Debug.Log(position);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
