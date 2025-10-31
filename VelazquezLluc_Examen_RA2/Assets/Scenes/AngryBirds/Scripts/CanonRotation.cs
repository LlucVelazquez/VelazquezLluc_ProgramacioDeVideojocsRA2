using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanonRotation : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    private float _offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float _initialScaleX;
    private Vector2 _distanceBetweenMouseAndPlayer;
    private bool isRaising = false;
    private InputSystem_Actions inputActions;
    [SerializeField] private float _multiplier = 10f;
    private Vector3 mousePos;
    private float xvalue;
    private void Awake()
    {

        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
        _initialScaleX = PotencyBar.transform.localScale.x;
        inputActions.Enable();

    }
    void Update()
    {
        //mousePos = Input.mousePosition; //obtenir el valor del click del cursor (Fer amb new input system)
        _distanceBetweenMouseAndPlayer = mousePos - gameObject.transform.position; //obtenir el vector distància entre el canó i el cursor
        
        var ang = (Mathf.Atan2(_distanceBetweenMouseAndPlayer.y, _distanceBetweenMouseAndPlayer.x) * 180f / Mathf.PI + _offset);
        //Debug.Log(ang);
        transform.rotation = Quaternion.Euler(0, 0,ang); //en quin dels tres eixos va l'angle?

        if (isRaising)
        {
            ProjectileSpeed = Time.deltaTime * _multiplier + ProjectileSpeed; //acotar entre dos valors (mirar variables)
            CalculateBarScale();
        }
        
        CalculateBarScale();

    }
    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, _initialScaleX, ProjectileSpeed / MaxSpeed),
            transform.localScale.y,
            transform.localScale.z);
    }
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRaising = true;
        }
        if (context.canceled)
        {
            var projectile = Instantiate(Bullet,ShootPoint.transform.position, Quaternion.identity); //canviar la posició on s'instancia
            projectile.GetComponent<Rigidbody2D>().linearVelocity = _distanceBetweenMouseAndPlayer;
                ProjectileSpeed = 0f;
                isRaising = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 positionMouse = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        mousePos = positionMouse;
    }
}
