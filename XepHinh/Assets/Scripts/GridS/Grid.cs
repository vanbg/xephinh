using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStronge shapeStronge;
    public int columns = 0;
    public int rows = 0;
    public float squaresGrap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySqualeOffSet = 0.0f;

    private LineIndicator _lineIndicator;
    private Vector2 _offSet = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();



    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;

    }

    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreatGrid(); 
    }

    private void CreatGrid()
    {
        GripSquares_sinhra();
        GripSquares_vitri();
    }
    //tao luoi vuong
    private void GripSquares_sinhra()
    {
        // 1 2 3 4 5
        // 6 7 8 9 10
        int square_index = 0;

        for (var row = 0; row < rows; ++row) 
        {
            for (var column = 0; column < columns; ++column) 
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);

                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                //_gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().setImage(square_index % 2 == 0);//xap xep sole 2 hinh anh

                square_index++;
                

            }
        }
    }
    //vi tri cua o vuong luoi
    private void GripSquares_vitri()
    {
        int colum_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_move = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offSet.x = square_rect.rect.width * square_rect.transform.localScale.x + everySqualeOffSet;
        _offSet.y = square_rect.rect.height * square_rect.transform.localScale.y + everySqualeOffSet;

        foreach(GameObject square in _gridSquares)
        {
            if (colum_number + 1 > columns) 
            {
                square_gap_number.x = 0;
                // go to next colums
                colum_number = 0;
                row_number++;
                row_move = false;

            }
            var pos_x_offset = _offSet.x * colum_number + (square_gap_number.x * squaresGrap);
            var pos_y_offset = _offSet.y * row_number + (square_gap_number.y * squaresGrap);

            if (colum_number > 0 && colum_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGrap;
            }
            if (row_number > 0 && row_number % 3 == 0 && row_move ==false) 
            {
                row_move = true;
                square_gap_number.y++;
                pos_y_offset += squaresGrap;

            }
            var rect = square.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x:startPosition.x + pos_x_offset, y:startPosition.y - pos_y_offset);
            rect.localPosition = new Vector3(x:startPosition.x + pos_x_offset, y:startPosition.y - pos_y_offset, 0);
            colum_number++;
        }



    }
    private void CheckIfShapeCanBePlaced() // kich hoat o vuong tren luoi
    {
        var squareIndexs = new List<int>();

        foreach (var square in _gridSquares)
        {
            var girdSquare = square.GetComponent<GridSquare>();
            if (girdSquare.Selected && !girdSquare.SquareOccupied)
            {
                //girdSquare.ActivateSquare();
                squareIndexs.Add(girdSquare.SquareIndex);
                girdSquare.Selected = false;


            }
        }

        var currentSelectShape = shapeStronge.GetCurrentSelectedShape();
        if (currentSelectShape == null)
        {
            return;// ko co hinh dang lua chon
        }

        if(currentSelectShape.TotalSquareNumber == squareIndexs.Count)//neu so hinh vuong duoc chon ban so hinh vuong tren luoi
        {
               foreach(var squareIndex in squareIndexs)
               {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnboard(); //dat hinh vuong tren bang
               }

            var shapeLeft = 0;

            foreach(var shape in shapeStronge.shapeList)
            {
                if(shape.IsOnStarPosition() && shape.IsAnyOfShapeSqaureActive())
                {
                    shapeLeft++;
                }
            }

            

            if (shapeLeft==0)
            {
                GameEvent.RequestNewShapes();
            }
            else
            {
                GameEvent.SetShapeInactive();
            }

            CheckIfAnyLineIsCompleted();
            
        }
        else
        {
            GameEvent.MoveShapeToStartPosition(); // chuyen ve vi tri ban dau
        }
        
    }

    void CheckIfAnyLineIsCompleted() //kiem tra da hoan thanh hang va cot chua
    {
        List<int[]> lines = new List<int[]>();

        //colums
        foreach(var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        //rows
        for (var row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for(var index =0; index <9; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }

            lines.Add(data.ToArray());
        }

        var completedLines = CheckIfSquaresAreCompleted(lines);

        if(completedLines>2)// neu hoan thanh hon 2 dong
        {
            // todo: play bonus animation
        }

        //tod : add scores
        var totalScores = 10 * completedLines;
        GameEvent.AddScores(totalScores);
        CheckPlayerLost();
        

    }


    private int CheckIfSquaresAreCompleted(List<int[]> data)// kiem tra xem o vuong da hoan thanh chua
    {
        List<int[]> completedLines = new List<int[]>();

        var linesCompleted = 0;

        foreach(var line in data)
        {
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if(comp.SquareOccupied ==false)
                {
                    lineCompleted = false;
                }
            }

            if(lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach ( var line in completedLines)
        {
            var completed = false;

            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
            }

            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if(completed)
            {
                linesCompleted++;
            }
        }

        return linesCompleted;
    }

    private void CheckPlayerLost()
    {
        var valiShapes = 0;
        for (var index = 0; index < shapeStronge.shapeList.Count; index++)
        {
            var isShapeActive = shapeStronge.shapeList[index].IsAnyOfShapeSqaureActive();
            if (CheckIfShapeCanBePlacedOnGrid(shapeStronge.shapeList[index]) && isShapeActive)
            {
                shapeStronge.shapeList[index]?.ActivateShape();
                valiShapes++;
            }
        }

        if (valiShapes == 0)
        {
            //Game Over
            GameEvent.GameOver(false);
            //Debug.Log("Game Over");
        }
    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.DLhinhdanghientai;
        var shapeColums = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        //Akk indexes of filled up squares
        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for (var rowindex = 0; rowindex < shapeRows; rowindex++)
        {
            for (var columnIndex = 0; columnIndex < shapeColums; columnIndex++)
            {
                if (currentShapeData.broad[rowindex].column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }
        }
        if (currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
            Debug.LogError("number of filled up squares are not the same as the original shape have .");

        var squarelist = GetAllSquaresCombination(shapeColums, shapeRows);

        bool canBePlaced = false;
        foreach (var number in squarelist)
        {
            bool shapeCanBePlancedOnTheBoard = true;
            foreach (var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    shapeCanBePlancedOnTheBoard = false;
                }
            }
            if (shapeCanBePlancedOnTheBoard)
            {
                canBePlaced = true;
            }
        }
        return canBePlaced;
    }

    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        var squarelist = new List<int[]>();
        var lastcolumnIdex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while (lastRowIndex + (rows - 1) < 9)
        {
            var rowData = new List<int>();
            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var column = lastcolumnIdex; column < lastcolumnIdex + columns; column++)
                {
                    rowData.Add(_lineIndicator.line_data[row, column]);
                }

            }
            squarelist.Add(rowData.ToArray());
            lastcolumnIdex++;
            if (lastcolumnIdex + (columns - 1) >= 9)
            {
                lastRowIndex++;
                lastcolumnIdex = 0;
            }
            safeIndex++;
            if (safeIndex > 100)
            {
                break;
            }
        }
        return squarelist;


    }


}
