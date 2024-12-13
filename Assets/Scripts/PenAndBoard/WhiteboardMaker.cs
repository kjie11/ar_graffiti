using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

// using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WhiteboardMaker : MonoBehaviour
{

    [SerializeField] private Transform tip;
    [SerializeField] private int penSize=20;
    public GameObject whiteBoard;
    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;
    private RaycastHit _touch;
    private WhiteBoard _whiteboard;
    private Vector2 _touchPos,_lastTouchPos;
    private bool _touchLastFrame;
    private Quaternion _lastTouchRot;
     float heightOffset = 0.01f;
      private bool isErasing = false;
      
    private Color eraseColor;
    public Slider penSizeSlider;
    // Start is called before the first frame update
    void Start()
    {
        _renderer=tip.GetComponent<Renderer>();
        _colors=Enumerable.Repeat(_renderer.material.color,penSize*penSize).ToArray();
        _tipHeight=tip.localScale.y+heightOffset;
       Renderer whiteboardRenderer = whiteBoard.GetComponent<Renderer>();
        eraseColor=whiteboardRenderer.material.color;
    // Transparent erase color (fully transparent)
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }
    public void UpdatePenSize()
    {
        // Update the penSize based on the slider value
        penSize = (int)penSizeSlider.value;  // Convert float to int if necessary
       

        // Update the color array with the new pen size
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
    }
    public void ChangeColor(){
        _colors=Enumerable.Repeat(_renderer.material.color,penSize*penSize).ToArray();
    }

    public void ToggleEraseMode()
    {
        isErasing = !isErasing;

        if (isErasing)
        {
            // Set the color array to fully transparent (erase)
            _colors = Enumerable.Repeat(eraseColor, penSize * penSize).ToArray();
        }
        else
        {
            // Set back to the drawing color
            _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        }
    }

    private void Draw(){
        if(Physics.Raycast(tip.position,transform.up,out _touch, _tipHeight)){
            if(_touch.transform.CompareTag("WhiteBoard")){
                if(_whiteboard==null){
                    _whiteboard=_touch.transform.GetComponent<WhiteBoard>();
                }
                _touchPos=new Vector2(_touch.textureCoord.x,_touch.textureCoord.y);
                var x=(int)(_touchPos.x*_whiteboard.textureSize.x-(penSize/2));
                var y=(int)(_touchPos.y*_whiteboard.textureSize.y-(penSize/2));
                if(y<0||y>_whiteboard.textureSize.y||x<0||x>_whiteboard.textureSize.x){return;}

                if(_touchLastFrame){
                    _whiteboard.texture.SetPixels(x,y,penSize,penSize,_colors);
                    for(float f=0.01f;f<1.00f;f+=0.01f){
                        var lerpX=(int)Mathf.Lerp(_lastTouchPos.x,x,f);
                        var lerpY=(int)Mathf.Lerp(_lastTouchPos.y,y,f);
                        _whiteboard.texture.SetPixels(lerpX,lerpY,penSize,penSize,_colors);
                        
                    }

                    transform.rotation=_lastTouchRot;
                    _whiteboard.texture.Apply();

                }

                _lastTouchPos=new Vector2(x,y);
                _lastTouchRot=transform.rotation;
                _touchLastFrame=true;
                return;
          }
        }
        _whiteboard=null;
        _touchLastFrame=false;
    }
}
