using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    public static DuckController instance { get; private set; }
    public float xSpeed, ySpeed = 0f;
    public float xSpeedRand = 0.25f;
    public bool isAlive = true;
    Sprite spriteRend;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    [DllImport("Boss-Mode")]
    private static extern void SetBossStats(float x, float y);
    void Start()
    {
        SetBossStats(this.transform.localScale.x, this.transform.localScale.y);
        spriteRend = GetComponent<Sprite>();
        xSpeed = xSpeed + Random.Range(-xSpeedRand, xSpeedRand);
        if(xSpeed >= 0)
        {
            SpriteFlipX(false);
        }
        else
        {
            SpriteFlipX(true);
        }
        StartCoroutine(DirectionChange(Random.Range(2.5f, 4f)));
        StartCoroutine(DisappearAfterTime(10f));
    }

    private void LateUpdate()
    {
        transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0f);
    }

    public void setDirection(float dir)
    {
        xSpeed *= dir;
    }

    void SpriteFlipX(bool flip)
    {
        if(flip)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, this.transform.localScale.y, this.transform.localScale.z);
        }  
        else
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * 1f, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    void SpriteFlipY(bool flip)
    {
        if (flip)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * -1f, this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * 1f, this.transform.localScale.z);
        }
    }

    public void Down()
    {
        isAlive = false;
        SpriteFlipY(true);
        Destroy(this.GetComponent<Collider2D>());   
        ySpeed = -300f;
        xSpeed = 0f;
        StartCoroutine(DisappearAfterTime(0.8f));
    }
    public IEnumerator DisappearAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public void Disappear()
    {
        Destroy(gameObject);
    }

    IEnumerator DirectionChange(float time)
    {
        yield return new WaitForSeconds(time);
        if(isAlive == true)
        {
            ySpeed = Mathf.Abs(xSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ySpeed = -ySpeed;
        xSpeed = -xSpeed + Random.Range(-xSpeedRand, xSpeedRand);
        SpriteFlipX(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
