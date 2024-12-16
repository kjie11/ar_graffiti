using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    [SerializeField] private Transform RayStartPoint;
    public GameObject whiteBoard;
    private Renderer WhiteBoardRenderer;
    // public GameObject path;
    [SerializeField] private List<GameObject> paths;
     private int currentPathIndex = 0; 
     public GameObject prefab;
     public GameObject NewPrefab;
     public float maxSpawnDistance=1.0f;
     private bool hasSpawned=false;
    //  private bool successed=false;
    //  private Renderer ren;
    //  [SerializeField] private Material customMaterial; 
     private Vector3 finalPosition;
     private Quaternion finalRotation;

    //   private LineRenderer lineRenderer;
    private List<Vector3> linePoints = new List<Vector3>();
    private Texture2D whiteBoardTexture;
// public Color lineColor = Color.blue;

public GameObject previewPrefab; // 用于预览的半透明对象
private GameObject currentPreview; // 当前的预览实例

private Vector2 lastTouchPos;
    public int penSize = 80; // Pen size for drawing
    public Color penColor = Color.blue; // Pen color
    private Vector2 textureSize;
     private Color[] originalPixels;
      private Color[] penColorArray;
    public TextMeshProUGUI checkText;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

   
    }

    
    void Update()
    {
        
        CheckPathWithRaycast();
       
        SpawnArea();
        
        

    }

    

     private void CheckPathWithRaycast()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            Ray ray = new Ray(RayStartPoint.position, RayStartPoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                
                DrawOnTexture(ray, hit);
                if (currentPathIndex < paths.Count && hit.collider.gameObject == paths[currentPathIndex])
                {   
                    checkText.text =hit.collider.gameObject+":"+paths[currentPathIndex];
                    currentPathIndex++;
                    
                    if (currentPathIndex >= paths.Count)
                    {
                       
                        Instantiate(NewPrefab, finalPosition, finalRotation);
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

   
    int x = (int)(touchPos.x * whiteBoardTexture.width - (penSize / 2));
    int y = (int)(touchPos.y * whiteBoardTexture.height - (penSize / 2));

    
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
           

            if(!hasSpawned){
                 currentPreview.transform.position = bestPose.Value.position;
            currentPreview.transform.rotation = bestPose.Value.rotation;
                    currentPreview.SetActive(true);
            }
            
            

            if (OVRInput.GetDown(OVRInput.Button.One) && prefab != null)
            {
                
                // Instantiate(prefab, bestPose.Value.position, bestPose.Value.rotation);
                // ren=prefab.GetComponent<Renderer>();
                prefab.transform.position=bestPose.Value.position;
                prefab.transform.rotation=bestPose.Value.rotation;
                finalPosition=bestPose.Value.position;
                finalRotation=bestPose.Value.rotation;
                whiteBoard.SetActive(true);
                
                hasSpawned=true;
            }
        }
    }
    
    
     
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

