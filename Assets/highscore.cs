using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class highscore : MonoBehaviour
{
    static public int score=1000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI gt = this.GetComponent<TextMeshProUGUI>();
        gt.text = "High Score: "+score;
    }
}
