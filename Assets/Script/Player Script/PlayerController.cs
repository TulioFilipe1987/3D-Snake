using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    [HideInInspector]
    public PlayerDirection direction;  // é playerdirection

    [HideInInspector]
    public float step_length = 0.2f; // 0,2f

    [HideInInspector]
    public float movement_Frequency = 0.1f; // 0.1f

    private float counter;
    private bool move;

    [SerializeField]
    private GameObject tailPrefab;

    private List<Vector3> delta_Postion;
    private List<Rigidbody> nodes;

    private Rigidbody main_Body; // BODY
    private  Rigidbody head_Body;// HEAD
    private Transform tr;

    private bool create_Node_At_Tail;

    void Awake(){

        tr = transform;
        main_Body = GetComponent<Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        delta_Postion = new List<Vector3>(){

            new Vector3(-step_length, 0f),//-x .. LEFT
            new Vector3(0f,step_length),// y ..  Up
            new Vector3(step_length,0f),// x .. RIGHT
            new Vector3(0f,-step_length)// -y.. DOWN

        };
        
    }

     
    void Update()
    {

        CheckMovementFrequency();
    }

    void FixedUpdate()
    {
        if (move){ 

        move = false;

        Move();

        }
    }

    void InitSnakeNodes(){

        nodes = new List<Rigidbody>();

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

       // nodes.Add(tr.GetChild(3).GetComponent<Rigidbody>()); // coloquei agora 



        head_Body = nodes[0];

    }

    void SetDirectionRandom(){
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;


    }



    void InitPlayer(){

        switch (direction){

            case PlayerDirection.RIGHT:

                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f); // X Y Z
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE* 2f, 0f,0f);


                break;

            case PlayerDirection.LEFT:

                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);

                break;

            case PlayerDirection.UP:

                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f,Metrics.NODE * 2f, 0f);


                break;

            case PlayerDirection.DOWN:

                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;

        }

    }

    void Move(){

        Vector3 dPosition = delta_Postion[(int)direction];

        Vector3 parentsPos = head_Body.position;
        Vector3 prevPosition;

        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;

        for(int i= 1; i < nodes.Count; i++){

            prevPosition = nodes[i].position;

            nodes[i].position = parentsPos;
            parentsPos = prevPosition;

        }

        // check if we need to create a nww node
        // because we ate a fruit

        if (create_Node_At_Tail){  // instaciar o olho

            create_Node_At_Tail = false;

          GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
           

            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());


        }

         
    }

    void CheckMovementFrequency(){

        counter += Time.deltaTime;

       if(counter >= movement_Frequency){

            counter = 0f;
            move = true;

        }

    
    }

    public void SetInputDirection(PlayerDirection dir){

          if(dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
             dir == PlayerDirection.DOWN && direction == PlayerDirection.UP||
             dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT||
             dir ==  PlayerDirection.LEFT && direction == PlayerDirection.RIGHT){

            return;

       }

        direction = dir;

        ForceMove();

    
    }

    void ForceMove(){

        counter = 0;
        move = false;
        Move();
    
    }

    void OnTriggerEnter(Collider  target){

        if(target.tag == Tags.FRUIT){

            target.gameObject.SetActive(false);

            create_Node_At_Tail = true;

            GameplayController.instance.IncreaseScore();  // aqui é ativado a pontuação
          
            AudioManager.instance.Play_PickUpSound();// o somda fruta 
        
        }



        if(target.tag == Tags.WALL  || target.tag == Tags.BOMB || target.tag == Tags.TAIL){
            print("Snake touched tail ");

            Time.timeScale = 0f;

            AudioManager.instance.Play_DeadSound(); // o som da morte

        }
        
    }



}//class
