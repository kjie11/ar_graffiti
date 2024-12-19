using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class combinationHandler : MonoBehaviour
{
    public GameObject checkCube;
    public String contentClass; //生成的内容与任务组合绑定
    public float maxSpawnDistance=1.0f;
    public GameObject flowerPrefab;
    public GameObject treePrefab;
    public GameObject catPrefab;
    private DrawPath drawPath;
    // private fillColor fillColorscript;

    public GameObject scriptObj;


    public GameObject previewPrefab;
    private GameObject currentPreview; // 当前的预览实例

    // //dawn path的参数
    // [SerializeField] private Transform RayStartPoint;
    //  private int currentPathIndex = 0; 
    //  private bool drawPathCheck=false;
    // [SerializeField] private List<GameObject> paths;
    // private Texture2D whiteBoardTexture;
    // public GameObject whiteBoard;
    // private Renderer WhiteBoardRenderer;
    // private Color[] originalPixels;
    // private Color[] penColorArray;
    // private int penSize = 80; // Pen size for drawing
    // private Color penColor = Color.blue; // Pen color
    // private Vector2 lastTouchPos;

    // //fillcolor的参数
    // public Collider targetCollider;
    // public Transform savedTransform;
    // public GameObject grab;
    // public GameObject rayGrab;
    // public GameObject fillColorBoard;
    
    void Start()
    {
        currentPreview = Instantiate(previewPrefab);
        currentPreview.SetActive(false);
        GameManager.Instance.contentClass=contentClass;
        GameManager.Instance.isCombination=true;
        // contentClass=GameManager.Instance.contentClass;

        drawPath=scriptObj.GetComponent<DrawPath>();


        

        


        // //drawpath
        //  WhiteBoardRenderer = whiteBoard.GetComponent<Renderer>();
        // whiteBoardTexture = (Texture2D)WhiteBoardRenderer.material.mainTexture;
        // originalPixels = whiteBoardTexture.GetPixels();
        
        //  penColorArray = new Color[penSize * penSize];
        // for (int i = 0; i < penColorArray.Length; i++)
        // {
        //     penColorArray[i] = penColor;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.complete){
             SpawnArea();
        }
       

        if(contentClass=="Flower"&&GameManager.Instance.hasSpawned){ //spawned preview时
            // prefab=flowerPrefab;
             GameManager.Instance.pictureprefab=flowerPrefab;
            //  whiteBoard.SetActive(true);
            // if(GameManager.Instance.drawPathCheck==false){
                SceneManager.LoadScene("PathSpawn");
            // }
            // else if(GameManager.Instance.dragFillColorCheck==false){
                // SceneManager.LoadScene("DragFillColor");
            // }
            // else{
            //     Spawn();
            // }
           
            
            
            
            
        }
        else if(contentClass=="Tree"){
            GameManager.Instance.pictureprefab=treePrefab;
            combination2();
        }
        else{
            GameManager.Instance.pictureprefab=catPrefab;
            combination3();
        }


        // if(contentClass=="Flower"&&GameManager.Instance.hasSpawned==false){
                
        //         if(drawPathCheck==true){
        //              whiteBoard.SetActive(false);
        //              fillColorBoard.SetActive(true);
        //         }
        //         else{
        //             CheckPathWithRaycast();
                   
        //         }
                
        // }
        

    }
    //flower: drawPath_fillcolor
    public void combination1(){
       
       
    }

    public void combination2(){

    }
    public void combination3(){

    }


private void SpawnArea(){
   
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

        
        if (distanceToWall <= maxSpawnDistance)
        {
           

            if(!GameManager.Instance.hasSpawned){
                 currentPreview.transform.position = bestPose.Value.position;
            currentPreview.transform.rotation = bestPose.Value.rotation;
                    currentPreview.SetActive(true);
            }
            
            

            if (OVRInput.GetDown(OVRInput.Button.One) && GameManager.Instance.pictureprefab != null)
            {
                GameManager.Instance.finalPosition=bestPose.Value.position;
                GameManager.Instance.finalRotation=bestPose.Value.rotation;
               
                GameManager.Instance.hasSpawned=true; //spawned preview
            }
        }
    }
    
}
 public void Spawn(){
           
            Instantiate(GameManager.Instance.pictureprefab, GameManager.Instance.finalPosition,GameManager.Instance.finalRotation);
            currentPreview.SetActive(false);
                
              
    }

//      private void CheckPathWithRaycast()
//     {
//         if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
//         {
//             Ray ray = new Ray(RayStartPoint.position, RayStartPoint.forward);
//             int layerMask = ~LayerMask.GetMask("DrawPathTrigger"); // 替换为你要排除的 Layer 名称
//             // if (Physics.Raycast(ray, out RaycastHit hit))
//             if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
//             {
                
//                 DrawOnTexture(ray, hit);
//                 if (currentPathIndex < paths.Count && hit.collider.gameObject == paths[currentPathIndex])
//                 {   
//                     Renderer r=paths[currentPathIndex].GetComponent<Renderer>();
//                     r.material.color=Color.blue;
//                     currentPathIndex++;
                    
//                     if (currentPathIndex >= paths.Count)
//                     {
//                         drawPathCheck=true;
//                         currentPreview.SetActive(false);
//                     }
//                 }
//             }
//         }
//         else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
//         {
//             // linePoints.Clear();
//             if (whiteBoardTexture != null && originalPixels != null)
//         {
//             whiteBoardTexture.SetPixels(originalPixels);
            
//             whiteBoardTexture.Apply();
//         }
//         }
//     }

//     private void DrawOnTexture(Ray ray, RaycastHit hit)
// {
//     Vector2 touchPos = new Vector2(hit.textureCoord.x, hit.textureCoord.y);  
//     int x = (int)(touchPos.x * whiteBoardTexture.width- (penSize / 2) );
//     int y = (int)(touchPos.y * whiteBoardTexture.height- (penSize / 2));
    
//     x = Mathf.Clamp(x, 0, whiteBoardTexture.width - penSize);
//     y = Mathf.Clamp(y, 0, whiteBoardTexture.height - penSize);

//     // Only draw if the touch position is different from the last one
//     if (lastTouchPos != touchPos)
//     {
//         whiteBoardTexture.SetPixels(x, y, penSize, penSize, penColorArray);
//         whiteBoardTexture.Apply();  // Apply the changes
//     }
//     // Update the last touch position
//     lastTouchPos = touchPos;
// }

// //fillColor
// private void OnTriggerEnter(Collider other)
//     {
//         // 检查是否与指定的Collider碰撞
//         if (other == targetCollider)
//         {
            
//             SetTransform();
            
//         }
        
//     }

//     public void SetTransform(){
      
//         spawnArea.checkCount();
//         if (savedTransform != null)
//         {
            
//             grab.SetActive(false);
//             rayGrab.SetActive(false);
//             transform.position = savedTransform.position;
//             transform.rotation = savedTransform.rotation;
//         }
       
//     }


    
}
