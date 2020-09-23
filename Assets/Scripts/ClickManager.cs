using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public Player player;
    private GameObject clickedObject;
    private RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Waypoint")
                {
                    if (clickedObject != null)
                        clickedObject.GetComponent<SpriteRenderer>().enabled = true;

                    clickedObject = hit.collider.gameObject;
                    clickedObject.GetComponent<SpriteRenderer>().enabled = false;
                    player.UpdateWaypoint(clickedObject);
                    //Debug.Log(clickedObject.name);
                }
                else
                    clickedObject = null;
            }
        }
    }
}
