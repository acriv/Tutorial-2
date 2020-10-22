using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text winText;
    public Text lives;
    public Text loseText;
    private int scoreValue = 0;
    private int livesValue = 3;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    Animator anim;
    private bool facingRight = true;
    private bool isGrounded;
    
    

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        winText.text = "";
        loseText.text = "";
        lives.text = "Lives: " + livesValue.ToString();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement));

        Collider2D currentCollider = GetComponent<Collider2D>();

        if (Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1), new Vector2(0.1f * currentCollider.bounds.size.x, 0.05f * currentCollider.bounds.size.y), 0f, 1 << 8) != null)
        {
            if (isGrounded == false)
            {
                anim.SetInteger("State", 0);
            }

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            anim.SetInteger("State", 3);
        }

        if(isGrounded)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                rd2d.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
                isGrounded = false;
            }
            
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetInteger("State", 2);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collider.gameObject);
            if (scoreValue == 4)
        {
            transform.position = new Vector3(50.0f, -0.81f, 0.0f);
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
        }
        if (scoreValue >= 8)
        {
            winText.text = "You Win!\nGame Created by:\nAlfredo Rivera";
            musicSource.clip = musicClipOne;
            musicSource.Play();
            musicSource.loop = false;
            Destroy(gameObject);
        }
        }
        if(collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collider.gameObject);
        }
        if (livesValue == 0)
        {
            loseText.text = "You Lose!\nTry Again!";
            Destroy(this);
        }
    }

    
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
