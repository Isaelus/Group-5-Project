using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class currentTime : MonoBehaviour
{
    public GameObject entity;
    static public int score;
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        score=0;
        player = entity.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if (player.health>0){
            score++;
        }
        else{
            SceneManager.LoadScene("gameover");
        }
        TextMeshProUGUI gt = this.GetComponent<TextMeshProUGUI>();
        gt.text = "Score: "+score;
        if (score>highscore.score){
            highscore.score = score;
        }
    }
}
