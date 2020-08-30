using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudManager : MonoBehaviour
{

    [NonSerialized]
    public Vector2 worldWind = Vector2.right;
    private double cloudPopup = 0.01;

    private GameObject cloud1 = default;
    private GameObject cloud2 = default;
    private GameObject cloud3 = default;
    private GameObject cloud4 = default;
    private GameObject cloud5 = default;

    [NonSerialized]
    public List<Cloud> clouds = new List<Cloud>();

    private void Awake()
    {
        cloud1 = (GameObject)Resources.Load("cloud1");
        cloud2 = (GameObject)Resources.Load("cloud2");
        cloud3 = (GameObject)Resources.Load("cloud3");
        cloud4 = (GameObject)Resources.Load("cloud4");
        cloud5 = (GameObject)Resources.Load("cloud5");

        var x = Random.Range(Constants.world_x_min, Constants.world_x_max);
        if(Random.Range(0,2) == 0)
        {
            x *= -1;
        }
        worldWind = new Vector2(x, 0);
        cloudPopup = Random.Range(Constants.pop_min , Constants.pop_max);
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 1.0f) < cloudPopup)
        {
            GenerateCloud();
        }
        DestroyCloud();
    }

    private void GenerateCloud()
    {
        Cloud cloud = default; 
        switch (Random.Range(0, 5)) {
            case 0:
                cloud = Instantiate(cloud1, GeneratePopPoint(), Quaternion.identity).GetComponent<Cloud>();
                break;
            case 1:
                cloud = Instantiate(cloud2, GeneratePopPoint(), Quaternion.identity).GetComponent<Cloud>();
                break;
            case 2:
                cloud = Instantiate(cloud3, GeneratePopPoint(), Quaternion.identity).GetComponent<Cloud>();
                break;
            case 3:
                cloud = Instantiate(cloud4, GeneratePopPoint(), Quaternion.identity).GetComponent<Cloud>();
                break;
            case 4:
                cloud = Instantiate(cloud5, GeneratePopPoint(), Quaternion.identity).GetComponent<Cloud>();
                break;
        
        }
        cloud.manager = this;
        cloud.Init();
        clouds.Add(cloud);
    }

    private Vector2 GeneratePopPoint()
    {
        return new Vector2(worldWind.x > 0 ? Constants.camera_view.x - 3 : Constants.camera_view.x + Constants.camera_view.width + 3, Random.Range(Constants.camera_view.y, Constants.camera_view.y + Constants.camera_view.height));
    } 

    private void DestroyCloud()
    {
        List<Cloud> idxs = default;
        if(worldWind.x > 0)
        {
            idxs = clouds.Where(c => c.gameObject.transform.position.x > Constants.camera_view.x + Constants.camera_view.width + 3).ToList();
        }
        else
        {
            idxs = clouds.Where(c => c.gameObject.transform.position.x < Constants.camera_view.x - 3).ToList();
        }

        for (int i = 0; i < idxs.Count; i++)
        {
            clouds.Remove(idxs[i]);
            Destroy(idxs[i].gameObject);
        }

    }
}
