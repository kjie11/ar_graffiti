using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine;


public class fillColor : MonoBehaviour
{

    [SerializeField] private List<GameObject> ObjToDraw;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>(); // 用于存储原始颜色


    [SerializeField] private GameObject paintingParent;
 
    private Stack<GameObject> colorChangeStack = new Stack<GameObject>(); 
    public bool allOpaque = false;
    public GameObject pen;
    private Renderer penRender;
     private Color penColor;

    public GameObject prefab; //要生成的图片
    public float maxSpawnDistance=1.0f;
    public GameObject previewPrefab; // 用于预览的半透明对象
private GameObject currentPreview; // 当前的预览实例
 private Vector3 finalPosition;
     private Quaternion finalRotation;
  
   

private bool hasSpawned=false;

    // Start is called before the first frame update
    void Start()
    {
        currentPreview = Instantiate(previewPrefab);
        currentPreview.SetActive(false);
        penRender=pen.GetComponent<Renderer>();

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
        penColor=penRender.material.color;
        if(OVRInput.Get(OVRInput.Button.Two)){
            UndoLastColorChange();
        }
        
        if(allOpaque==true){
            Spawn();
        }
        SpawnArea();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (ObjToDraw.Contains(other.gameObject))
        {
            
            SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                colorChangeStack.Push(other.gameObject);
               
              Color color = spriteRenderer.color; //画上的color
              color=penColor;
                // color.a = 1.0f; 
                spriteRenderer.color = color;
                
                CheckAllOpaque();
            }
        }
    }

    private void UndoLastColorChange()
    {
        if (colorChangeStack.Count > 0)
        {
            
            GameObject lastChangedObject = colorChangeStack.Pop();

            
            SpriteRenderer spriteRenderer = lastChangedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
               
                Color color = spriteRenderer.color;
                color.a = 0.5f; 
                spriteRenderer.color = color;
                
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
        

    

    private void Spawn(){
        
            paintingParent.SetActive(false);
            prefab.SetActive(true);
            Instantiate(prefab, finalPosition,finalRotation);
            prefab.SetActive(false);
                // ren=prefab.GetComponent<Renderer>();
                // prefab.transform.position=bestPose.Value.position;
                // prefab.transform.rotation=bestPose.Value.rotation;
              
    }
}
