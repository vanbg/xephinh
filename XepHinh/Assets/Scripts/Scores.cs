using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BestScoreData
{
    public int score = 0;
}

public class Scores : MonoBehaviour
{   
    public SquareTextuaData squareTextuaData;
    public Text socreText;


    private bool newBestScore_ = false;
    private BestScoreData bestscore_ = new BestScoreData();
    
    private int currentScores_;

    private string bestScoreKey_ = "bsdat";
    void Start()
    {
        currentScores_ = 0;
        newBestScore_ = false;
        squareTextuaData.SetStarcolor();
        UpdateScoreText();
    }

    private void Awake()
    {
        if(BinaryDataStream.Exits(bestScoreKey_))
        {
            StartCoroutine(ReadDatafile());
        }
    }

    private IEnumerator ReadDatafile()
    {
        bestscore_ = BinaryDataStream.Read<BestScoreData>(bestScoreKey_);
        yield return new WaitForEndOfFrame();
        Debug.Log("Read best Score =" + bestscore_.score);
    }

    private void OnEnable()
    {
        GameEvent.AddScores += AddScores;
        GameEvent.GameOver += SaveBestScores;
    }

    private void OnDisable()
    {
        GameEvent.AddScores -= AddScores;
        GameEvent.GameOver -= SaveBestScores;
    }

    public void SaveBestScores(bool newBestScores)
    {
        BinaryDataStream.Save<BestScoreData>(bestscore_,bestScoreKey_);
    }

    private void AddScores(int socres)
    {
        currentScores_ += socres;
        if(currentScores_ > bestscore_.score)
        {
            newBestScore_ = true;
            bestscore_.score = currentScores_;
        }
        UpdateSquareColor();
        GameEvent.UpdateBestScoreBar(currentScores_,bestscore_.score);
        UpdateScoreText();
    }
    private void UpdateSquareColor()
    {
        if(currentScores_ >= squareTextuaData.tresholdVal)
        {
            squareTextuaData.UpdateColors(currentScores_);
        }
    }

    private void UpdateScoreText()
    {
        socreText.text = currentScores_.ToString();
    }

}
