using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private LayerMask mouseHitLayerMask;

    [SerializeField]
    private Transform weaponRoot;

    private float charging = 0f;

    private bool isSwinging;

    [SerializeField]
    private float SwingChargeSpeed = 3f;

    [SerializeField]
    private float SwingSpeed = 7f;

    private float startSwingPower;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, mouseHitLayerMask)) {
            transform.LookAt(hit.point);
        }

        HandleHitCheck();
        HandleMoveCheck();

    }

    private void HandleMoveCheck()
    {
        if (Input.GetKey(KeyCode.W))
            Move(Vector3.forward * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D))
            Move(Vector3.right * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.A))
            Move(Vector3.left * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.S))
            Move(Vector3.back * Time.deltaTime * speed);
    }
    
    private void Move(Vector3 dir)
    {
        transform.position += dir;
    }


    private void HandleHitCheck()
    {
        ShowWeaponPosition();
        
        if (isSwinging)
            HandleSwing();

        if (!Input.GetMouseButton(1) && !Input.GetMouseButton(0) && charging != 0)
        {
            startSwingPower = Mathf.Abs(charging);
            isSwinging = true;
            return;
        }

        if (Input.GetMouseButton(1))
        {
            if (charging < 1)
                charging += Time.deltaTime * SwingChargeSpeed;
        }

        if (Input.GetMouseButton(0))
        {
            if (charging > -1)
                charging -= Time.deltaTime * SwingChargeSpeed;
        }
    }

    private void ShowWeaponPosition()
    {
        weaponRoot.localRotation = Quaternion.Euler(new Vector3(0, charging * 120, 0));  
    }
    
    private void HandleSwing()
    {
        var chargeSign = (charging > 0 ? 1f : -1f);
        charging -= Time.deltaTime * SwingSpeed * chargeSign;
        var chargeSign2 = (charging > 0 ? 1f : -1f);

        if (chargeSign != chargeSign2)
            SwingFinished();
        
        ShowWeaponPosition();
    }


    private void SwingFinished()
    {
        charging = 0;
        isSwinging = false;
    }
    
    
}
