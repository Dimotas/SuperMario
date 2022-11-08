using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class marioControler : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Speed;
    private bool ViradoEsquerda;
    private Animator anim;
    public bool grounded = false;
    public Transform posicao;
    public float raio = 0.2f;
    public LayerMask WhatIsGround;
    public bool doubleJump;
    private int estrela;
    private BoxCollider2D bc;
    public GameObject fechada;
    public GameObject aberta;
    public GameObject parede;
    public GameObject Key;
    int scene;
    public GameObject gameover;
    private bool playing = true;
    // private Scene Cena;


    void Start()
    {
        Speed = 7.0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        ViradoEsquerda = false;
        grounded = false;
        doubleJump = true;
        estrela = 6;
        scene = SceneManager.GetActiveScene().buildIndex;
        parede.SetActive(true);
        fechada.SetActive(true);
        aberta.SetActive(false);
        Key.SetActive(false);
        gameover.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || doubleJump))
        {
            rb.AddForce(new Vector2(0.0f, 400.0f));
           if (!grounded) doubleJump = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            anim.SetBool("baixo", true);
            bc.offset = new Vector2(0.035f, -0.09f);
            bc.size = new Vector2(0.23f, 0.15f);
        }
       if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetBool("baixo", false);
            bc.offset = new Vector2(0.035f, 0.02953595f);
            bc.size = new Vector2(0.9533334f, 1.169847f);
        }


    }
    void FixedUpdate()
    {
        float Movimento = Input.GetAxis("Horizontal");
        anim.SetFloat("Hspeed", Mathf.Abs(Movimento));
        grounded = Physics2D.OverlapCircle(posicao.position, raio, WhatIsGround);
         if (grounded) doubleJump = true;
            if (playing)
            {
          if ((Movimento > 0 && !ViradoEsquerda) || (Movimento < 0 && ViradoEsquerda)) Virar();
           rb.velocity = new Vector2(Movimento * Speed, rb.velocity.y);
             }
    }

    void Virar()
    {
        ViradoEsquerda = !ViradoEsquerda;
        float escala = transform.localScale.x;
        escala *= -1;
        transform.localScale = new Vector3(escala, transform.localScale.y, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("estrela"))
        {
            Destroy(collision.gameObject);
            estrela--;
            if (estrela == 0)
            {
                parede.SetActive(false);
                Key.SetActive(true);
            }
           }
   
         else if (collision.gameObject.CompareTag("Key"))
            {
                Debug.Log("Apanhaste a Chave! Resgata a Princesa");
                Destroy(collision.gameObject);


                    fechada.SetActive(false);
                    aberta.SetActive(true);
                    Key.SetActive(false);
             }

           else if (collision.gameObject.CompareTag("espigoes"))
        {
            Debug.Log("MORRESTE!");
            playing = false;
            SceneManager.LoadScene(scene);

        }

          else if (collision.gameObject.CompareTag("aberta"))
          {
         Debug.Log("Parabens Resgatas-te a Princesa!");
           playing = false;
         
         SceneManager.LoadScene("Nivel2");
        }

         else if (collision.gameObject.CompareTag("princesa"))
          {
         Debug.Log("Parabens Resgatas-te a Princesa!");
           playing = false;
         
         SceneManager.LoadScene("SampleScene");
        }
       
    }
}

