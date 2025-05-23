using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    Animator anim;
    int inputX;
    private void Awake()
    {
        anim = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (isTouchRight && h > 0) h = 0;
        if (isTouchLeft && h < 0) h = 0;
        if (isTouchTop && v > 0) v = 0;
        if (isTouchBottom && v < 0) v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0)*speed*Time.deltaTime;

        transform.position = curPos + nextPos;
        int newInputX = (int)h;

        if (newInputX != inputX)
        {
            inputX = newInputX;
            anim.SetInteger("Input", inputX);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
