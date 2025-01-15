using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//combination1
public class combinationTrueOne : MonoBehaviour
{
    public GameObject checkCube;
    public float maxSpawnDistance=1.0f;
    public GameObject previewPrefab;
    private GameObject currentPreview; // 当前的预览实例

    //dawn path的参数
    [SerializeField] private Transform RayStartPoint;
     private int currentPathIndex = 0; 
     private bool drawPathCheck=false;
    [SerializeField] private List<GameObject> paths;
    private Texture2D whiteBoardTexture;
    public GameObject whiteBoard;
    private Renderer WhiteBoardRenderer;
    private Color[] originalPixels;
    private Color[] penColorArray;
    private int penSize = 80; // Pen size for drawing
    private Color penColor = Color.blue; // Pen color
    private Vector2 lastTouchPos;
    private Vector2 textureSize;



    public GameObject NewPrefab; //最终生成的prefab
    private bool spawnAreaSpawned=false; //是否生成的preview的位置
    private Vector3 finalPosition;
     private Quaternion finalRotation;

    //fillcolor的参数
    [SerializeField] private List<GameObject> ObjToDraw;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>(); // 用于存储原始颜色


    [SerializeField] private GameObject paintingParent;
 
    private Stack<GameObject> colorChangeStack = new Stack<GameObject>(); 
    public bool allOpaque = false;
    public GameObject pen;
    private Renderer fillColorPenRender; //fillcolor独有penRender
     private Color fillColorPenColor;

  
    //fillcolor 任务,默认关闭
    public GameObject fillcolorObj;
    
    void Start()
    {
        currentPreview = Instantiate(previewPrefab);
        currentPreview.SetActive(false);
        // GameManager.Instance.contentClass=contentClass;
        // GameManager.Instance.isCombination=true;
       
         if (GameManager.Instance.pictureprefab!= null)
        {
            NewPrefab = GameManager.Instance.pictureprefab;
            // checkText.text="recevied";
        }
       
         WhiteBoardRenderer = whiteBoard.GetComponent<Renderer>();
        whiteBoardTexture = (Texture2D)WhiteBoardRenderer.material.mainTexture;
        textureSize = new Vector2(whiteBoardTexture.width, whiteBoardTexture.height);
        originalPixels = whiteBoardTexture.GetPixels();
        
         penColorArray = new Color[penSize * penSize];
        for (int i = 0; i < penColorArray.Length; i++)
        {
            penColorArray[i] = penColor;
        }

        // Initialize the preview object
        if (previewPrefab != null)
    {
        currentPreview = Instantiate(previewPrefab);
        currentPreview.SetActive(false);
    }
    else
    {
        Debug.LogError("previewPrefab is not assigned in the inspector.");
    }
        //fillcolor
        fillColorPenRender=pen.GetComponent<Renderer>();
         foreach (GameObject obj in ObjToDraw)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColors[obj] = spriteRenderer.color; // 存储原始颜色
            }
        }
        

    }

    
    



    // Update is called once per frame
    void Update()
    {
        
        SpawnArea();
        if(!drawPathCheck){
            
            CheckPathWithRaycast();
       
        
        }
        else{
            Renderer r=checkCube.GetComponent<Renderer>();
                r.material.color=Color.blue;
                //画完了路径再fillcolor
                fillcolorObj.SetActive(true);

                fillColorPenColor=fillColorPenRender.material.color;
                
                
                if(allOpaque==true){
                    Spawn();
                }
                   
               
        
        }
        
        
        

    }

    //fillColor
     private void OnTriggerEnter(Collider other)
    {
        
        if (ObjToDraw.Contains(other.gameObject))
        {
            
            SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                colorChangeStack.Push(other.gameObject);
               
              Color color = spriteRenderer.color; //画上的color
              color=fillColorPenColor;
                // color.a = 1.0f; 
                spriteRenderer.color = color;
                
                CheckAllOpaque();
            }
        }
    }

    private void CheckAllOpaque()
    {
        foreach (GameObject obj in ObjToDraw)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.color.a < 1.0f)
            {
                

                return;
            }
            
        }
        
        allOpaque = true;
        
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

        if(spawnAreaSpawned){
            return;
        }
        
        if (distanceToWall <= maxSpawnDistance)
        {
           
            //  if(!spawnAreaSpawned){
            // if(!GameManager.Instance.hasSpawned){
                 currentPreview.transform.position = bestPose.Value.position;
            currentPreview.transform.rotation = bestPose.Value.rotation;
                    currentPreview.SetActive(true);
            // }
            
            

            if (OVRInput.GetDown(OVRInput.Button.One) )
            {
                Renderer r=checkCube.GetComponent<Renderer>();
                r.material.color=Color.red;
                // GameManager.Instance.finalPosition=bestPose.Value.position;
                // GameManager.Instance.finalRotation=bestPose.Value.rotation;
                finalPosition=bestPose.Value.position;
                finalRotation=bestPose.Value.rotation;
               
                // GameManager.Instance.hasSpawned=true; //spawned preview
                spawnAreaSpawned=true; //spawned preview
            }
        }
    }
    
}
 public void Spawn(){
           
            Instantiate(GameManager.Instance.pictureprefab, finalPosition,finalRotation);
            currentPreview.SetActive(false);
                
              
    }

     private void CheckPathWithRaycast()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            Ray ray = new Ray(RayStartPoint.position, RayStartPoint.forward);
            int layerMask = ~LayerMask.GetMask("DrawPathTrigger"); // 替换为你要排除的 Layer 名称
            // if (Physics.Raycast(ray, out RaycastHit hit))
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                
                DrawOnTexture(ray, hit);
                if (currentPathIndex < paths.Count && hit.collider.gameObject == paths[currentPathIndex])
                {   
                    Renderer r=paths[currentPathIndex].GetComponent<Renderer>();
                    r.material.color=Color.blue;
                    currentPathIndex++;
                    
                    if (currentPathIndex >= paths.Count)
                    {
                        drawPathCheck=true;
                        currentPreview.SetActive(false);
                    }
                }
            }
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            // linePoints.Clear();
            if (whiteBoardTexture != null && originalPixels != null)
        {
            whiteBoardTexture.SetPixels(originalPixels);
            
            whiteBoardTexture.Apply();
        }
        }
    }

    private void DrawOnTexture(Ray ray, RaycastHit hit)
{
    Vector2 touchPos = new Vector2(hit.textureCoord.x, hit.textureCoord.y);  
    int x = (int)(touchPos.x * whiteBoardTexture.width- (penSize / 2) );
    int y = (int)(touchPos.y * whiteBoardTexture.height- (penSize / 2));
    
    x = Mathf.Clamp(x, 0, whiteBoardTexture.width - penSize);
    y = Mathf.Clamp(y, 0, whiteBoardTexture.height - penSize);

    // Only draw if the touch position is different from the last one
    if (lastTouchPos != touchPos)
    {
        whiteBoardTexture.SetPixels(x, y, penSize, penSize, penColorArray);
        whiteBoardTexture.Apply();  // Apply the changes
    }
    // Update the last touch position
    lastTouchPos = touchPos;
}



private void OnApplicationQuit()
    {
        if (whiteBoardTexture != null && originalPixels != null)
        {
            whiteBoardTexture.SetPixels(originalPixels);
            whiteBoardTexture.Apply();
        }
    }

    
}
