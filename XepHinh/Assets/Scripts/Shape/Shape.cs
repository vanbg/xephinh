using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Shape : MonoBehaviour,IPointerClickHandler,IPointerUpHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler
{
    public GameObject anhHinhVuong;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0f, 700f);


    [HideInInspector]
    public ShapeData DLhinhdanghientai;

    public int TotalSquareNumber { get; set; }

    private List<GameObject> _hinhdanghientai = new List<GameObject>();
    private Vector3 _shapeStarScale;
    private RectTransform _transform;
    private bool _shapeDraggable = true;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;

    



    public void Awake()
    {
        _shapeStarScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _shapeDraggable = true;
        _startPosition = _transform.localPosition;
        _shapeActive = true;
    }

    private void OnEnable()
    {
        GameEvent.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvent.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvent.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvent.SetShapeInactive -= SetShapeInactive;
    }


    public bool IsOnStarPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public bool IsAnyOfShapeSqaureActive()
    {
        foreach(var square in _hinhdanghientai)
        {
            if (square.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    public void DeactivaShape()
    {
        if(_shapeActive)
        {
            foreach(var square in _hinhdanghientai)
            {
                square?.GetComponent<ShapeSquare>().DeactivaShape();
            }
        }
        _shapeActive = false;
    }

    private void SetShapeInactive()
    {
        if(IsOnStarPosition()==false && IsAnyOfShapeSqaureActive())
        {
            foreach(var square in _hinhdanghientai)
            {
                square.gameObject.SetActive(false);
            }
        }
        
    }

    public void ActivateShape()
    {
        if(!_shapeActive)
        {
            foreach(var square in _hinhdanghientai)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }
        _shapeActive = true;
    } 

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapedata)// ham tao hinh dang
    {
        DLhinhdanghientai = shapedata;
        TotalSquareNumber = GetNumberOfSquares(shapedata);

        while (_hinhdanghientai.Count<= TotalSquareNumber)
        {
            _hinhdanghientai.Add(Instantiate(anhHinhVuong, transform) as GameObject);
        }

        foreach(var square in _hinhdanghientai)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = anhHinhVuong.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInlist = 0;

        //thiet lap vi tri tao hinh dang
        for (var row = 0; row < shapedata.rows; row++)
        {
            for (var colum = 0; colum < shapedata.columns; colum++)
            {
                if(shapedata.broad[row].column[colum])
                {
                    _hinhdanghientai[currentIndexInlist].SetActive(true);
                    _hinhdanghientai[currentIndexInlist].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForShapeSquare(shapedata, colum,moveDistance),GetYPositionForShapeSquare(shapedata,row,moveDistance));
                    currentIndexInlist++;
                }
            }
        }


    }

    private float GetYPositionForShapeSquare(ShapeData shapeData,int row,Vector2 moveDistance)// nhan vi tri y cho hinh vuong
    {
        float shifOnY = 0f;
        if (shapeData.rows > 1) //tinh toan vi tri ngang
        {
            float startYPos;
            if(shapeData.rows %2 !=0)
            {
                startYPos = (shapeData.rows / 2) * moveDistance.y;
            }
            else
            {

                startYPos = ((shapeData.rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;

            }
            shifOnY = startYPos - row * moveDistance.y;
        }
        return shifOnY;



    }

    private float GetXPositionForShapeSquare(ShapeData shapeData,int column,Vector2 moveDistance ) //nhan vi tri x cho hinh vuong hinh dang
    {
        float shifOnx = 0f;

        if(shapeData.columns>1)
        {
            float startXPos;
            if(shapeData.columns%2 !=0)
            {
                startXPos = (shapeData.columns / 2) * moveDistance.x * -1;
            }
            else
            {
                startXPos = ((shapeData.columns / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;

            }
            shifOnx = startXPos + column * moveDistance.x;
        }
        return shifOnx;
    }
    private int GetNumberOfSquares(ShapeData shapeData) //Ham dem cac o vuong dang hoat dong
    {
        int number = 0;
        
        foreach (var rowdata in shapeData.broad)
        {
            foreach(var active in rowdata.column)
            {
                if (active)
                    number++;
            }
        }
        return number;
    }
    public void OnPointerClick(PointerEventData eventData)//khi nhap chuot
    {
        _transform.Rotate(0, 0, 90);
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)// khi bat dau keo
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
            eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }
    public void OnEndDrag(PointerEventData eventData)//ket thuc keo
    {
        this.GetComponent<RectTransform>().localScale = _shapeStarScale;
        GameEvent.CheckIfShapeCanBePlaced();// kiem tra xem co the dat hinh dang dc ko
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    
    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;//vi tri ban dau
    }


}
