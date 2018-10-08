using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScopeType
{
    IRONSIGHTS,
    REDDOT,
    OPTICALZOOM,
    NIGHTVISION,
    THERMAL,
    TRACING,
    SONAR,
    AIMASSIST,
    AUTOLOCK
}

public class WeaponScope : MonoBehaviour
{
    [SerializeField]
    ScopeType scopeType;
    [SerializeField, Range(10.0f, 500.0f)]
    float scopeRange;
    [SerializeField, Range(0.01f, 1.0f)]
    float accuracy;
    [SerializeField]
    Image crossHair;

    Weapon weapon;
    WeaponClip weaponClip;
    WeaponBarrel weaponBarrel;

    public float GetAccuracy() { return accuracy; }

    void Start()
    {
        weapon = GetComponent<Weapon>();
        weaponClip = GetComponent<WeaponClip>();
        weaponBarrel = GetComponent<WeaponBarrel>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        CrossHairPos();
    }

    void CrossHairState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }

            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    void CrossHairPos()
    {
        if (Vector2.Distance(Input.mousePosition, Camera.main.WorldToScreenPoint(weapon.transform.position)) > scopeRange)
        {
            crossHair.rectTransform.transform.position = (Input.mousePosition - Camera.main.WorldToScreenPoint(weapon.transform.position)).normalized * scopeRange + Camera.main.WorldToScreenPoint(weapon.transform.position);
        }

        else
        {
            crossHair.rectTransform.position = Input.mousePosition;
        }
    }
}