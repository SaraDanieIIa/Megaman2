using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
   
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float FuerzaDis;

    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    public GameObject disparoI;
    public Transform Disparador;

    float FireRate = 0;
    float FireTime = 0;
    float tiempoL = 2;

    public float gravedad;
    public bool Dash;
    public float dash_T;
    public float dashSpeed;

    private bool doubleJump = true;


    // Start is called before the first frame update
    void Start()
    {

        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        dash();
        Movimiento();
        Saltar();
        Caer();
        Disparar();
        disparo();
    }

    void Mover()
    {

        float movH = Input.GetAxis("Horizontal");

        Vector2 movimiento = new Vector2(movH * Time.deltaTime * speed, 0);

        transform.Translate(movimiento);
        
        if(movH != 0)
        {
            myAnimator.SetBool("isRunning", true);
            if(movH < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void Movimiento()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * speed;
        myAnimator.SetBool("isRunning", true);
        if (!Mathf.Approximately(0, movement) && !Dash)
        {
            transform.rotation = movement > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        if (movement != 0)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void Caer()
    {
        if(myBody.velocity.y < 0 && !myAnimator.GetBool("Takeof") && !myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myAnimator.SetBool("isFalling", true);
            myAnimator.SetBool("Takeof", false);
        }
        else
        {
            myAnimator.SetBool("isFalling", false);
            if (Dash)
            {
                gravedad = 4;
            }
            else
            {
                gravedad = -5;
            }
        }

    }

    void Saltar()
    {
        
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myAnimator.GetBool("Takeof"))
        {
            doubleJump = true;
            myAnimator.SetBool("isFalling", false);
            myAnimator.SetBool("Takeof", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                myAnimator.SetTrigger("salto");
                myAnimator.SetBool("Takeof", true);
            }

        }
        else if (doubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                myBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                myAnimator.SetTrigger("salto");
                myAnimator.SetBool("Takeof", true);
                doubleJump = false;
            }
        }
    }
    void terminarDeSaltar()
    {
        myAnimator.SetBool("isFalling", true);
        myAnimator.SetBool("Takeof", false);
    }

    void Disparar()
    {
        if(Input.GetKey(KeyCode.X))
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            tiempoL--;
            if (tiempoL <= 1)
            {
                myAnimator.SetLayerWeight(1, 0);
            }
        }
    }

    void disparo()
    {
        if (Input.GetKeyDown(KeyCode.X) && Time.time >= FireTime)
        {
            Instantiate(disparoI, Disparador.transform.position, transform.rotation);
            FireTime = Time.time + FireRate;
        }   
    }

    void dash()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            dash_T += 1 * Time.deltaTime;
            if (dash_T < 0.35f)
            {
                Dash = true;
                myAnimator.SetBool("Dash", true);
                transform.Translate(Vector3.left * dashSpeed * Time.fixedDeltaTime);
            }
            else
            {
                Dash = false;
                myAnimator.SetBool("Dash", false);
            }
        }
        else
        {
            Dash = false;
            myAnimator.SetBool("Dash", false);
            dash_T = 0;
        }
    }

}
