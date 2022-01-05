using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    public Image occupImage;

    void Start()
    {
        occupImage.gameObject.SetActive(false);
    }

    public void DeactivaShape()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void ActivateShape()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void SetOccupied()
    {
        occupImage.gameObject.SetActive(true);
    }

    public void UpSetOccupied()
    {
        occupImage.gameObject.SetActive(false);
    }
}
