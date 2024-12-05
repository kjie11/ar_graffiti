using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SprayOnWhiteboard : MonoBehaviour
{
    [SerializeField] private Transform rayStartPoint; // 控制器发射射线的起点
    public GameObject whiteBoard; // 白板对象
    private Renderer whiteBoardRenderer; // 获取白板的 Renderer
    private Texture2D whiteBoardTexture; // 白板的纹理
    private Color[] penColorArray; // 存储笔刷颜色的数组
    private Vector2 lastTouchPos; // 上一次涂鸦的位置
    public int penSize = 20; // 笔刷大小
    public Color penColor = Color.blue; // 笔刷颜色
    public GameObject checkObj;
    // public TextMeshProUGUI text;
    private Vector2 textureSize;

    private Color[] originalPixels;  // 存储原始纹理的像素数据

    private float colorThreshold = 0.1f; 
    private int coveredPixels=0;
    private  int totalPixels;
    Color[] targetAreaPixels;

    private void Start()
    {
        // 获取白板的 Renderer 和纹理
        whiteBoardRenderer = whiteBoard.GetComponent<Renderer>();
        whiteBoardTexture = (Texture2D)whiteBoardRenderer.material.mainTexture;

        if (whiteBoardTexture == null)
        {
            Debug.LogError("No Texture2D found on the whiteboard material.");
        }
        else
        {
            // 将纹理的宽度和高度存储在 textureSize 中
            textureSize = new Vector2(whiteBoardTexture.width, whiteBoardTexture.height);

            // 保存原始纹理的像素数据
            originalPixels = whiteBoardTexture.GetPixels();
        }

        // 初始化涂鸦颜色数组
        penColorArray = new Color[penSize * penSize];
        for (int i = 0; i < penColorArray.Length; i++)
        {
            penColorArray[i] = penColor;
        }

        // 获取目标区域的原始像素（即涂鸦前的主体像素）
    targetAreaPixels = originalPixels;  // 使用存储的原始像素数据
    // bool isCovered = false;
    coveredPixels = 0;  // 记录已被涂鸦覆盖的像素数量
    totalPixels = targetAreaPixels.Length;  // 获取总的像素数量
    }

    private void Update()
    {
        // 按下 trigger 时开始涂鸦
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            
            // 发射射线进行检测
            Ray ray = new Ray(rayStartPoint.position, rayStartPoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 判断是否碰到白板
                if (hit.transform.CompareTag("WhiteBoard"))
                {
                    // Renderer r = checkObj.GetComponent<Renderer>();
                    // r.material.color = Color.red;

                    Draw(hit);
                }

                


            }
        }
        else if(OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger)){
                 Renderer r = checkObj.GetComponent<Renderer>();
                    r.material.color = Color.yellow;
                 
            CheckCoverage();
        }
        
        
    }
    

    private void Draw(RaycastHit hit)
    {
        // 获取触摸位置的纹理坐标
        Vector2 touchPos = new Vector2(hit.textureCoord.x, hit.textureCoord.y);

        // 计算涂鸦的纹理坐标
        int x = (int)(touchPos.x * whiteBoardTexture.width - (penSize / 2));
        int y = (int)(touchPos.y * whiteBoardTexture.height - (penSize / 2));
       

      
        // 确保涂鸦坐标不会超出纹理范围
        x = Mathf.Clamp(x, 0, whiteBoardTexture.width - penSize);
        y = Mathf.Clamp(y, 0, whiteBoardTexture.height - penSize);

        // 如果当前触摸位置与上次触摸位置不同，则进行涂鸦
        if (lastTouchPos != touchPos)
        {
            whiteBoardTexture.SetPixels(x, y, penSize, penSize, penColorArray);

            // 可选：用线条平滑连接涂鸦路径（Lerp）
            DrawLine(lastTouchPos, touchPos);

            // 应用涂鸦修改
            whiteBoardTexture.Apply();
            //  CheckCoverage(x, y);

            
        }

        // 更新上次触摸的位置
        lastTouchPos = touchPos;
    }

    private void DrawLine(Vector2 start, Vector2 end)
    {
        // 计算线条的像素间隔（避免高频繁更新导致的性能问题）
        int steps = 10; // 可以调整步长来控制线条的平滑度
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector2 lerpedPos = Vector2.Lerp(start, end, t);
            int lerpedX = (int)(lerpedPos.x * whiteBoardTexture.width);
            int lerpedY = (int)(lerpedPos.y * whiteBoardTexture.height);

            // 绘制线条
            whiteBoardTexture.SetPixels(lerpedX, lerpedY, penSize, penSize, penColorArray);
        }
    }

  private void CheckCoverage()
{
    // 获取白板的所有像素
    Color[] whiteBoardPixels = whiteBoardTexture.GetPixels();
    List<int> nonTransparentPixels = new List<int>();  // 用于存储非透明像素的索引
    bool isCovered = false;  // 初始假设目标区域没有被涂鸦覆盖

    // 遍历所有像素，找到所有非透明区域（alpha > 0.1f）
    for (int i = 0; i < whiteBoardPixels.Length; i++)
    {
        if (whiteBoardPixels[i].a > 0.1f)  // 判断是否为目标区域（非透明区域）
        {
            // 将非透明区域的像素索引添加到 nonTransparentPixels 数组
            nonTransparentPixels.Add(i);
        }
    }

    int coveredCount = 0;  // 统计已涂鸦覆盖的目标区域像素数量
    int totalTargetPixels = nonTransparentPixels.Count;  // 目标区域的总像素数量

    // 遍历 nonTransparentPixels 数组，检查这些区域是否被涂鸦覆盖
    foreach (int index in nonTransparentPixels)
    {
        // 获取当前目标区域像素
        Color pixelColor = whiteBoardPixels[index];

        // 检查该像素是否被涂鸦颜色覆盖
        if (ColorDistance(pixelColor, penColor) <= colorThreshold)
        {
            coveredCount++;  // 统计已被涂鸦覆盖的目标区域像素
        }
    }

    // 计算涂鸦覆盖的目标区域像素的比例
    float coveragePercentage = (float)coveredCount / totalTargetPixels;
    // text.text="coverage:"+coveragePercentage;
    // 如果涂鸦覆盖比例大于或等于80%，认为目标区域已被涂鸦覆盖
    if (coveragePercentage >= 0.98f)
    {
        isCovered = true;
    }

    // 根据 isCovered 的值，决定是否改变材质颜色
    Renderer r = checkObj.GetComponent<Renderer>();
    if (isCovered)
    {
        r.material.color = Color.blue;  // 如果目标区域已完全被涂鸦覆盖，设置为蓝色
    }
    else
    {
        r.material.color = Color.green;  // 如果目标区域没有完全被涂鸦覆盖，设置为绿色
    }
}
// 计算两个颜色之间的距离（颜色相似度）
private float ColorDistance(Color a, Color b)
{
    // 计算 RGB 每个分量的差异，并返回总差异
    return Mathf.Sqrt(Mathf.Pow(a.r - b.r, 2) + Mathf.Pow(a.g - b.g, 2) + Mathf.Pow(a.b - b.b, 2));
}


    // 在游戏退出时恢复原始纹理
    private void OnApplicationQuit()
    {
        if (whiteBoardTexture != null && originalPixels != null)
        {
            whiteBoardTexture.SetPixels(originalPixels);
             
            whiteBoardTexture.Apply();
        }
    }
}
