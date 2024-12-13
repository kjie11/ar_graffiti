using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Meta.XR.MRUtilityKit;
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
     private bool successed=false;
     private Renderer ren;
     [SerializeField] private Material customMaterial; 
     private Vector3 finalPosition;
     private Quaternion finalRotation;

      private LineRenderer lineRenderer;
    private List<Vector3> linePoints = new List<Vector3>();
public Color lineColor = Color.blue;

public GameObject previewPrefab; // 用于预览的半透明对象
private GameObject currentPreview; // 当前的预览实例

    
    // Start is called before the first frame update
    void Start()
    {
        // prefab.SetActive(false);
         WhiteBoardRenderer = whiteBoard.GetComponent<Renderer>();

         lineRenderer = gameObject.GetComponent<LineRenderer>();
         prefab.SetActive(false);

         if (previewPrefab != null)
    {
        currentPreview = Instantiate(previewPrefab);
        currentPreview.SetActive(false);
    }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckPathWithRaycast();
        // if(hasSpawned==false){

        // }
        SpawnArea();
        // if(successed==true){
        //     if(ren){
              
        //         {
        //             ren.material= customMaterial;
        //         }
        //     }
        //     else{

        //         ren.material.color = Color.blue;
        //     }
        // }
        // Instantiate(NewPrefab, finalPosition, finalRotation);
        

    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("in the collider");
    //     // if (other.gameObject.name == "whiteBoard" )
    //     if (other.gameObject.tag.CompareTo("path")==0)
    //     {
            
    //         WhiteBoardRenderer.material.color = Color.green;
    //     }
    //     if (paths.Contains(other.gameObject))
    // {
    //     WhiteBoardRenderer.material.color = Color.blue;
    // }
    // }

    //  private void OnTriggerEnter(Collider other)
    // {
    //     if (currentPathIndex < 11 && other.gameObject == paths[currentPathIndex])
    //     {
            
    //         other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    //         Debug.Log(currentPathIndex);
    //         Debug.Log(paths[currentPathIndex]);
            
            
    //         currentPathIndex++;

            
    //         if (currentPathIndex >= paths.Count)
    //         {
                
    //             WhiteBoardRenderer.material.color = Color.green;
    //             Debug.Log("Successfully followed all paths!");
    //             successed=true;
    //             // ChangeMat();
    //         }
    //     }
    // }
    
    // private void ChangeMat(){
    //     GameObject instantiatedPrefab = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
    //     Renderer r = instantiatedPrefab.GetComponent<Renderer>();
    // if (r != null)
    // {
    //     r.material.color = Color.red; // Change to the desired color
    // }
    
   
    
    // }
     private void CheckPathWithRaycast()
    {
        if(OVRInput.Get(OVRInput.RawButton.RIndexTrigger)){
        Ray ray = new Ray(RayStartPoint.position, RayStartPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            drawWithLine(ray, hit);
            if (currentPathIndex < paths.Count && hit.collider.gameObject == paths[currentPathIndex])
            {
                // 将路径点颜色变蓝
                
                // hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                currentPathIndex++;

                // 如果按顺序射线触碰所有路径点，触发条件
                if (currentPathIndex >= paths.Count)
                {
                    // WhiteBoardRenderer.material.color = Color.yellow;
                   
                    Debug.Log("Successfully followed all paths with raycast!");
                    successed=true;
                    Instantiate(NewPrefab, finalPosition, finalRotation);

                }
            }
        }
        }

         else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            // 松开按钮后，可以清空当前绘制的线条（可选）
            linePoints.Clear();
            lineRenderer.positionCount = 0;
        }
    }

    private void drawWithLine(Ray ray,RaycastHit hit){

        // if(OVRInput.Get(OVRInput.RawButton.RIndexTrigger)){
                Vector3 hitPoint = hit.point;
                
                    // 将碰撞点添加到 linePoints 列表中
                    linePoints.Add(hitPoint);

                    if(linePoints.Count>=4){
                        DrawBezierCurve();
                    }
                    // lineRenderer.positionCount = linePoints.Count;
                    // lineRenderer.SetPositions(linePoints.ToArray());
        // }
        //  else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        // {
        //     // 松开按钮后，可以清空当前绘制的线条（可选）
        //     linePoints.Clear();
        //     // lineRenderer.positionCount = 0;
        // }
    }

    private void DrawBezierCurve()
    {
        int segmentCount = 20; 
        List<Vector3> bezierPoints = new List<Vector3>();

        // 每四个点生成一个贝塞尔曲线片段
        for (int i = 0; i <= linePoints.Count - 4; i += 3)
        {
            Vector3 p0 = linePoints[i];
            Vector3 p1 = linePoints[i + 1];
            Vector3 p2 = linePoints[i + 2];
            Vector3 p3 = linePoints[i + 3];

            // 在每两个控制点之间生成分段
            for (int j = 0; j <= segmentCount; j++)
            {
                float t = j / (float)segmentCount;
                Vector3 bezierPoint = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
                bezierPoints.Add(bezierPoint);
            }
        }

        lineRenderer.positionCount = bezierPoints.Count;
        lineRenderer.SetPositions(bezierPoints.ToArray());
    }


     private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0;        // (1 - t)^3 * P0
        point += 3 * uu * t * p1;        // 3 * (1 - t)^2 * t * P1
        point += 3 * u * tt * p2;        // 3 * (1 - t) * t^2 * P2
        point += ttt * p3;               // t^3 * P3

        return point;
    }
    private void SpawnArea(){
        // if(hasSpawned==true){
        //     return;
        // }
        
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

        // prefab.transform.position = bestPose.Value.position;
        //     prefab.transform.rotation = bestPose.Value.rotation;
        if (distanceToWall <= maxSpawnDistance)
        {
           

            if(!hasSpawned){
                 currentPreview.transform.position = bestPose.Value.position;
            currentPreview.transform.rotation = bestPose.Value.rotation;
                    currentPreview.SetActive(true);
            }
            
            

            if (OVRInput.GetDown(OVRInput.Button.One) && prefab != null)
            {
                // prefab.SetActive(true);
                Instantiate(prefab, bestPose.Value.position, bestPose.Value.rotation);
                ren=prefab.GetComponent<Renderer>();
                prefab.transform.position=bestPose.Value.position;
                prefab.transform.rotation=bestPose.Value.rotation;
                finalPosition=bestPose.Value.position;
                finalRotation=bestPose.Value.rotation;
                whiteBoard.SetActive(true);
                // currentPreview.SetActive(false);
                hasSpawned=true;
            }
        }
    }
    // else
    // {
    //      if (Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         float distanceToHit = Vector3.Distance(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), hit.point);
    //         prefab.transform.position = hit.point;
    //             prefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(-90, 0, 0);

    //         if (distanceToHit <= maxSpawnDistance) 
    //         {
                

    //             if (OVRInput.GetDown(OVRInput.Button.One) && prefab != null)
    //             {
    //                 Instantiate(prefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(-90, 0, 0));
    //             }
    //         }
    //     }
    
    // }
    
    
     
    }
    
    // public void setTransparent(){
    //     foreach (GameObject obj in paths)
    //     {
    //         Renderer r = obj.GetComponent<Renderer>();
    //         if (r != null)
    //         {
    //             Color color = r.material.color;
    //             color.a = 0.0f; // 设置为半透明
    //             r.material.color = color;
    //         }
    //         else{
    //                 r.material.color = Color.green;
                
    //         }
    //     }
    // }

   

    }

