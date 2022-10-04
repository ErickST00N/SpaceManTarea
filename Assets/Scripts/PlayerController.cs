 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Variables del movimiento del personaje
    public float jumpForce = 6f;
    public Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;


    //Son las variables que creamos dentro del animator de Unity del personaje principal
    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";

    //Es donde pisará el personaje (Suelo)
    public LayerMask groundMask;

    //Variables de Joystick
    float horizontalMove = 0;
    float verticalMove = 0;

    public float runSpeedHorizontal = 3;
    public float runSpeedVertical = 3;
    public float runSpeedJoystick = 1;
    public float runSpeed = 3;


    public Joystick joystickPlayer;






    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        
    }

    // Start is called before the first frame update
    void Start()
    {

        //Capturamos la posición en la que inicia nuestro jugador
        startPosition = this.transform.position;
    }

    //Inicia Juego en player
    public void StartGame()
    {
        //Cuando inicia el juego, indicamos que el personaje
        ////está vivo y que aún no toca el piso
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);

        Invoke("RestartPosition", 0.2f);
    }

    void RestartPosition()
    {
        //Colocamos al jugador en la posición inicial
        //y la velocidad la colocamos en 0
        this.transform.position = this.startPosition;
        this.rigidBody.velocity = Vector2.zero;

    }


    // Update is called once per frame
    void Update()
    {
        //Ejecutará Jump() cuando se presione la tecla de Espacio, Click Derecho y/o Flecha arriba
        

        if (Input.GetButtonDown("Jump"))
        {

            Jump();
        }
        


        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        //Creamos una linea para que nos ayude a ver la altura del personaje
        Debug.DrawRay(this.transform.position, Vector2.down*1.4f, Color.red);
    }


    private void FixedUpdate()
    {
        
        Move();
    }


    //Configuramos para que se mueva
    void Move()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {//Si estamos en modo partida, el juego iniciará


            //TECLADO
            
            //Avanza solo
            /*if (rigidBody.velocity.x < runSpeed)
            {
                rigidBody.velocity = new Vector2(runSpeed, rigidBody.velocity.y);
            }*/

            //Flechas controller
            if (Input.GetKey(KeyCode.RightArrow))//Avanza con la flecha derecha
            {
                rigidBody.velocity = new Vector2(runSpeed, rigidBody.velocity.y);
            }
            if (Input.GetKey(KeyCode.LeftArrow))//Retrocede con la flecha izquierda
            {
                rigidBody.velocity = new Vector2(-runSpeed, rigidBody.velocity.y);
            }

            //Letras de teclado
            if (Input.GetKey(KeyCode.D))//Avanza con la letra A
            {
                rigidBody.velocity = new Vector2(runSpeed, rigidBody.velocity.y);
            }
            if (Input.GetKey(KeyCode.A))//Retrocede con la letra D
            {
                rigidBody.velocity = new Vector2(-runSpeed, rigidBody.velocity.y);
            }


            //JOYSTICKS
            verticalMove = joystickPlayer.Vertical * runSpeedVertical;
            horizontalMove = joystickPlayer.Horizontal * runSpeedHorizontal;

            transform.position += new Vector3(horizontalMove, verticalMove,
                0) * Time.deltaTime * runSpeedJoystick;
        }
        else
        {//Si no estamos dentro de la partida, el juego se detendrá
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    void Jump()
    {
        if (IsTouchingTheGround() && GameManager.sharedInstance.
            currentGameState == GameState.inGame)
        {
            //Si estamos en modo partida, el juego iniciará
            //Solo salta si toca el suelo

            animator.enabled = true;
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else
        {//Si no estamos dentro de la partida, el juego se detendrá
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            animator.enabled = false;
        }


    }

    //Nos indica si el personaje toca el suelo
    bool IsTouchingTheGround() 
    { 
        if(Physics2D.Raycast(this.transform.position,
            Vector2.down,
            1.4f,
            groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }
    public void Die()
    {
        //Activamos la animación de muerte e
        //Indicamos que murió el jugador
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }
}
