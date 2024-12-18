using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{

     public GameObject prefab; //要生成的图片
    public float maxSpawnDistance=1.0f;
    public GameObject previewPrefab; // 用于预览的半透明对象
private GameObject currentPreview; // 当前的预览实例
 private Vector3 finalPosition;
     private Quaternion finalRotation;

    public int totalCount=4;
   public int currentCount=0;

private bool hasSpawned=false;
public TextMeshProUGUI checkText;
    // Start is called before the first frame update
    void Start()
    {
        

        if (GameManager.Instance.pictureprefab!= null)
        {
            prefab = GameManager.Instance.pictureprefab;
            checkText.text="recevied";
        }
        else
        {
            checkText.text=""+GameManager.Instance.pictureprefab;
        }

        
       currentPreview = Instantiate(previewPrefab);
        currentPreview.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        Area();
        if(currentCount==totalCount){
               Spawn();
        }
    }
    public void Area(){
       
        Ray ray = new Ray(
        OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), 
        OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward
    );

    

    MRUKAnchor sceneAnchor = null;
    var positioningMethod = MRUK.PositioningMethod.DEFAULT;
    LabelFilter labelFilter = new LabelFilter();

    var bestPose = MRUK.Instance?.GetCurrentRoom()?.GetBestPoseFromRaycast(ray, Mathf.Infinity, labelFilter, out sceneAnchor, positioningMethod);

    
        
    if (bestPose.HasValue && sceneAnchor != null)
    {
       float distanceToWall = Vector3.Distance(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), bestPose.Value.position);

      
        if(hasSpawned){
            return;
        }
        if (distanceToWall <= maxSpawnDistance)
        {
           

           
                 currentPreview.transform.position = bestPose.Value.position;
            currentPreview.transform.rotation = bestPose.Value.rotation;
                    currentPreview.SetActive(true);
            
            
            

            if (OVRInput.GetDown(OVRInput.Button.One) && prefab != null)
            {
               
                finalPosition=bestPose.Value.position;
                finalRotation=bestPose.Value.rotation;
                
                hasSpawned=true;
            }
        }
    }
    }

    public void Spawn(){
           
            Instantiate(prefab, finalPosition,finalRotation);
            currentPreview.SetActive(false);
                
              
    }

    public void checkCount(){
        currentCount+=1;
    }

    // public void ReceiveGameObject(GameObject obj)
    // {
    //     prefab = obj;  // 将接收到的 GameObject 赋值给公共变量
    //     Debug.Log("Received object: " + obj.name);
    // }
}
