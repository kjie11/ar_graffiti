using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//只用于combination中的dragfillcolor的位置吸附
public class SetPosition : MonoBehaviour
{

     public Collider targetCollider;
   
   public Transform savedTransform;
   public GameObject grab;
   public GameObject rayGrab;
   public combination1 combination1;//用于获取总的currentcount
//    public GameObject combinationObj; 

    // Start is called before the first frame update
    void Start()
    {
        
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
        
        // spawnArea.checkCount();
        if (savedTransform != null)
        {
            combination1.currentCount+=1;
            grab.SetActive(false);
            rayGrab.SetActive(false);
            transform.position = savedTransform.position;
            transform.rotation = savedTransform.rotation;
        }
       
    }
}