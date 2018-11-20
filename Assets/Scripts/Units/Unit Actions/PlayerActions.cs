using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LazyTitan.Serialization.Attributes;
// Needs to be updated to support 4 players on controllers.
public class PlayerActions : MonoBehaviour
{
    [SerializeField, FieldProperties(RuntimeBehaviour.BOTH, true)]
    List<Vector3> listedInt;

    PlayerStats playerStats;
    Weapon weapon;

    void Start()
    {
        listedInt = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            listedInt.Add(new Vector3(i - 1, i, i + 1));
        }
        playerStats = GetComponent<PlayerStats>();
        weapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        if (!DevConsole.GetDevConsole().GetIsActive())
        {
            Move();
            Rotation();
            Activate();
            Deactivate();
            Fire();
        }
    }

    void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(xMove, 0, zMove), playerStats.GetCurrentMoveSpeed() * Time.deltaTime);
    }

    void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    void Activate()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    void Deactivate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            weapon.DeactivatePrimaryFire();
        }
    }

    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            weapon.PrimaryFire();
        }
    }
}