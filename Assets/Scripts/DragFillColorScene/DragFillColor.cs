using System.Collections;
using System.Collections.Generic;
using Meta.XR.Acoustics;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class DragFillColor : MonoBehaviour
{

    public Collider targetCollider;
   
   public Transform savedTransform;
   public GameObject grab;
   public GameObject rayGrab;
   private SpawnArea spawnArea;
   public GameObject scriptObj;
   
   

   
    // Start is called before the first frame update
    void Start()
    {
        spawnArea=scriptObj.GetComponent<SpawnArea>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   private void OnTriggerEnter(Collider other)
    {
        // 检查是否与指定的Collider碰撞
        if (other == targetCollider)
        {
            
            SetTransform();
            
        }
        
    }

    public void SetTransform(){
      
        spawnArea.checkCount();
        if (savedTransform != null)
        {
            
            grab.SetActive(false);
            rayGrab.SetActive(false);
            transform.position = savedTransform.position;
            transform.rotation = savedTransform.rotation;
        }
       
    }


    
}
