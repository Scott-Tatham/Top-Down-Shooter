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
    [SerializeField, Range(1.0f, 20.0f)]
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
    }

    void Update()
    {
        CrossHairPos();
    }

    void CrossHairPos()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(Camera.main.WorldToScreenPoint(weapon.transform.position));
        }

        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(weapon.transform.position);
        crossHair.rectTransform.position = Vector2.ClampMagnitude(Input.mousePosition, scopeRange * 10.0f) + new Vector2(playerScreenPos.x, playerScreenPos.y);
    }
}