using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRender : MonoBehaviour {

    private Image img_Frame;
    private Image img_Graphic;
    private Image[] ranks;
    private Text thisName;

    public Text Name
    { get { return thisName; } }
    public Image Img_Frame
    { get { return img_Frame; } }
    public Image Img_Graphic
    { get { return img_Graphic; }}

    // Use this for initialization
    void Awake ()
    {
        thisName = transform.Find("name").GetComponent<Text>();
        img_Frame = transform.Find("Frame").GetComponent<Image>();
        img_Graphic = transform.Find("Graphic").GetComponent<Image>();
        ranks = transform.Find("Rank").GetComponentsInChildren<Image>();
    }

    public void SetRank(int rank)
    {
        for(int i=0; i<(5-rank);i++)
        {
            Debug.Log("CALLED");
            ranks[i].gameObject.SetActive(false);
        }
    }
}
