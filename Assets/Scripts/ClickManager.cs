using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public Player player;
    public LayerMask layers;
    private GameObject clickedObject;
    private RaycastHit2D hit;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            player.UpdateWaypoint(mousePos2D);

            /*
            hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, layers);

            if (hit.collider != null)
            {
                //Debug.Log("hit: " + hit.collider.name);
                if (hit.collider.tag == "Waypoint" || hit.collider.tag == "Enemy")
                {
                    //if (clickedObject != null)
                        //clickedObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                    clickedObject = hit.collider.gameObject;
                    //clickedObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    player.UpdateWaypoint(clickedObject);
                }
                else { 
                    clickedObject = null;
                }
                */

            }
        }
    }
