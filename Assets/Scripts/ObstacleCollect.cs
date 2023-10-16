using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;

public class ObstacleCollect : MonoBehaviour
{
    void Awake(){
        BoxCollider[] boxColliders = GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < boxColliders.Length; i++)
        {
            float minX = boxColliders[i].transform.position.x -
                         boxColliders[i].size.x*boxColliders[i].transform.lossyScale.x*0.5f;
            float minZ = boxColliders[i].transform.position.z -
                         boxColliders[i].size.z*boxColliders[i].transform.lossyScale.z*0.5f;
            float maxX = boxColliders[i].transform.position.x +
                         boxColliders[i].size.x*boxColliders[i].transform.lossyScale.x*0.5f;
            float maxZ = boxColliders[i].transform.position.z +
                         boxColliders[i].size.z*boxColliders[i].transform.lossyScale.z*0.5f;

            IList<RVO.Vector2> obstacle = new List<RVO.Vector2>();
            obstacle.Add(new RVO.Vector2(maxX, maxZ));
            obstacle.Add(new RVO.Vector2(minX, maxZ));
            obstacle.Add(new RVO.Vector2(minX, minZ));
            obstacle.Add(new RVO.Vector2(maxX, minZ));
            Simulator.Instance.addObstacle(obstacle);
        }
        
        Simulator.Instance.processObstacles();
    }

}
