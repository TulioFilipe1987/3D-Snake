using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// criei

public class GameplayController : MonoBehaviour{

    public static GameplayController instance;

    public GameObject fruit_PickUp, bomb_Pickup;

    private float min_X = -4.25f, max_X = 4.25f , min_y= -2.26f, max_y = 2.26f;
    private float z_Pos = 5.8f;


    private Text score_Text;
    private int  scoreCount;


    void Awake(){
        MakeInstace();
        
    }

    void Start(){
        score_Text = GameObject.Find("Score").GetComponent<Text>();

        Invoke("StartSpawning", 0.5f);

    }

    void MakeInstace(){
            if(instance == null){
                instance = this; 
                    
                    }
                
                }

    void StartSpawning(){ //  ativar a rotina 

        StartCoroutine(SpawnPickUps());
    
    }

    public void CancelSpawning(){ //  cncelar 
        CancelInvoke("StartSpawning");
    
    }
    
    
     IEnumerator SpawnPickUps(){
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));   

        if(Random.Range(0,10)>= 2){


            Instantiate(fruit_PickUp, new Vector3(Random.Range(min_X, max_X),
                                                   Random.Range(min_y, max_y), z_Pos),
                                    Quaternion.identity);   // randomico em maximo x e y / minimo x e y
        }
        else{

            Instantiate(bomb_Pickup, new Vector3(Random.Range(min_X, max_X),
                                                   Random.Range(min_y, max_y), z_Pos),
                                    Quaternion.identity);


        }

        Invoke("StartSpawning", 0f);
    }
    

    public void IncreaseScore(){
        scoreCount++;
        score_Text.text = "Score:" + scoreCount;

    }

}//class
