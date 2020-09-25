using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Vector3 tempPos = new Vector3();
    private Vector3 posOffset = new Vector3();
    private Vector3 basePosOffset = new Vector3();
    private float fadeDuration = 0.75f, amplitude = 0.2f, speed = 1f, fadeSpeed = 5f;
    private float pickupDuration = 5f;
    private bool collected;

    [Header("Bonuses")]
    public float bonusMoveSpeed;
    public float bonusHealth;
    public float bonusAttackSpeed;
    public float bonusRegen;
    public int pickupID;

    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
        Vector2 temp = new Vector2(0, 1.5f);
        basePosOffset = (Vector2)transform.position + temp;

        StartCoroutine(PickupDurationDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
            transform.position = Vector3.MoveTowards(transform.position, basePosOffset, fadeSpeed * Time.deltaTime);
        else
        {
            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * speed) * amplitude;

            transform.position = tempPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collected = true;
            Invoke("DestroyPickup", fadeDuration);
            collision.GetComponent<Player>().PickupBonus(bonusMoveSpeed, bonusAttackSpeed, bonusHealth, bonusRegen, pickupID);
            // call player script to add bonus
        }
    }

    void DestroyPickup()
    {
        Destroy(gameObject);
    }

    IEnumerator PickupDurationDestroy()
    {
        yield return new WaitForSeconds(pickupDuration);
        if (!collected)
            DestroyPickup();
    }
}
