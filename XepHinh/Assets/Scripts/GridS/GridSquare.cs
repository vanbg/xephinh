using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    //public Image normalImage;
    //public List<Sprite> normalImages;
    //// chon hinh anh
    //public void setImage(bool setFirstImage)
    //{
    //    normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    //}

    public Image hooveImage;
    public Image activeImage;

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }


    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }
    // temp function.Remove it
    public bool CanWeUseThisSquare()
    {
        return hooveImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnboard()
    {
        ActivateSquare();
    }

    public void ActivateSquare()// kich hoat hinh 
    {
        hooveImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    public void Deactivate()
    {
        activeImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        Selected = false;
        SquareOccupied = false;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)// kich hoat khi bat dau va tram
    {
        if (SquareOccupied == false)// ko kich hoat SquareOccupied ms dc tao hinh
        {
            Selected = true;
            hooveImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)// neu doi tuong co 1 thanh phan cua hinh vuong
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)// kich van dang va tram
    {
        Selected = true;

        if (SquareOccupied == false)
        {
            hooveImage.gameObject.SetActive(true);
        }

        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)// khi dung va tram
    {
        if (SquareOccupied == false)
        {
            Selected = false;
            hooveImage.gameObject.SetActive(false);

        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UpSetOccupied();
        }
    }

    
   
}
