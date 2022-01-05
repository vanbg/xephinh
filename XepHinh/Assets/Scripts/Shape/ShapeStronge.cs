using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStronge : MonoBehaviour//lu tru hinh dang
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;


    private void Start()
    {
        foreach(var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }

    private void OnEnable()
    {
        GameEvent.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvent.RequestNewShapes -= RequestNewShapes;
    }



    public Shape GetCurrentSelectedShape()
    {
        foreach(var shape in shapeList)
        {
            if (shape.IsOnStarPosition() == false && shape.IsAnyOfShapeSqaureActive())
                return shape;
        }
        Debug.LogError("Ko co hinh danh nao dc chon!");
        return null;
    }

    private void RequestNewShapes()// rest lai hinh da cho
    {
       foreach(var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }
}
