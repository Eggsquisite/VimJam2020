using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public Player player;
    public LayerMask layers;
    private GameObject clickedObject;
    private RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, layers);

            if (hit.collider != null)
            {
                //Debug.Log("hit: " + hit.collider.name);
                if (hit.collider.tag == "Waypoint")
                {
                    if (clickedObject != null)
                        clickedObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                        //clickedObject.GetComponent<SpriteRenderer>().enabled = true;

                    clickedObject = hit.collider.gameObject;
                    clickedObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    //clickedObject.GetComponent<SpriteRenderer>().enabled = false;
                    player.UpdateWaypoint(clickedObject);
                }
                else { 
                    clickedObject = null;
                }
            }
        }
    }
}
