using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScriot : MonoBehaviour
{

    public float speed; 
    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
   
    public AudioClip winMusic;
    public AudioClip normMusic;
    public AudioSource musicSource;

    private Rigidbody2D rd2d;
    private int count;
    private int lives;
    private int level;

    private bool isOnGround;
    private bool facingRight = true; 

    private bool winState;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent <Rigidbody2D> ();
        count = 0;
        lives = 3;
        level = 1;

        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
       
        winState = false;
        musicSource.clip = normMusic;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        
        if (hozMovement == 0 && isOnGround)
        {
            anim.SetInteger ("State" , 0);
        }
        
        if(hozMovement > 0  && isOnGround)
        {
            anim.SetInteger ("State" , 1);
        }

        if (isOnGround == false)
        {
            anim.SetInteger ("State" , 2);
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

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void SetCountText()
    {
        countText.text = "Count:" + count.ToString();
        livesText.text = "Lives:" + lives.ToString();
        if (count >= 8)
        {
            winTextObject.SetActive(true);
            PlayWin();

        }
        else if (lives <= 0)
        {
            loseTextObject.SetActive(true);
        }
    }

    void PlayWin()
    {
        if(winState == false)
        {
            winState = true;
            musicSource.clip = winMusic;
            musicSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            count = count + 1;
            SetCountText();
        }
              
        else if (collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            SetCountText();
        }
 // teleport to nect scene by changing the position of character 
        if (count == 4 && level == 1)
        {
            Teleport();
        }
    }

    void Teleport()
    {
        transform.position = new Vector2(227.9f, 8.2f);
        level = 2;
        lives = 3; 
        SetCountText();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isOnGround = true; 
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0,3),ForceMode2D.Impulse);
                isOnGround = false;
            }
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
