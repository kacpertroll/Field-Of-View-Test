using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    public float moveSpeed = 5f;
    private Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        RotateTowardsMouse();
        
        //poruszanie FOV razem z graczem
        fieldOfView.SetOrigin(transform.position);
        

    }

    void FixedUpdate()
    {

        transform.position += new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.fixedDeltaTime;
        
    }

    void RotateTowardsMouse()
    {
        // sprawdza gdzie jest myszka
        Vector3 mouseScreenPosition = Input.mousePosition;

        // pozycja myszki na pozycje w œwiecie
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // pozycja myszki - pozycja gracza
        Vector3 direction = mouseWorldPosition - transform.position;

        // kierunek z na 0 bo niepotrzebny
        direction.z = 0;

        // k¹ty oibliczamy w radianach
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // obrót postaci w strone myszki po obliczeniu (ale -90 stopni bo obrócony w prawo jest)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}
