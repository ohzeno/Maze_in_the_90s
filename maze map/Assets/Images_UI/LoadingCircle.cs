using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour
{
    public float speed = 5f;

    private Image circle;
    private bool clockwise;

    private void Start()
    {
        circle = GetComponent<Image>();
    }

    //Using FixedUpdate so it is  not framerate dependant. This means it is effected by physics speed. I did this in case your game goes slo mo, then the circle wil accordingly slow down.
    //If you want it to always spin the same speed regardless of physics timestep then change to Update and multiply by deltaTime. You may have to increase speed as well
    private void FixedUpdate()
    {
        if (clockwise)
        {
            circle.fillAmount += 0.01f * speed;
            if (circle.fillAmount >= 1f)
            {
                circle.fillAmount = 1f;
                clockwise = false;
                circle.fillClockwise = false;
            }
        }
        else
        {
            circle.fillAmount -= 0.01f * speed;
            if (circle.fillAmount <= 0f)
            {
                circle.fillAmount = 0f;
                clockwise = true;
                circle.fillClockwise = true;
            }
        }
    }
}
