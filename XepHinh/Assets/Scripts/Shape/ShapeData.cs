using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]//tao liet ke trong meun
[System.Serializable]//De thuoc tinh xuat hien ben ngoai Inspector
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class row
    {
        public bool[] column;
        private int _size = 0;

        public row() { }
        public row(int size)
        {
            CreatRow(size);
        }
        public void CreatRow(int size)
        {
            _size = size;
            column = new bool[_size];
            ClearRow();
        }
        public void ClearRow()
        {
            for (int i = 0; i < _size; i++)
            {
                column[i] = false;
            }
        }


    }
    public int columns = 0;
    public int rows = 0;
    public row[] broad;

    public void Clear()
    {
        for (var i = 0; i < rows; i++)
        {
            broad[i].ClearRow();
        }
    }

    public void CreateNewBroar()
    {
        broad = new row[rows];
        for (var i = 0; i < rows; i++)
        {
            broad[i] = new row(columns);
        }
    }
}
