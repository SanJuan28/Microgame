using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float thrustForce = 200f;
    public float rotationSpeed = 200f;
    private Rigidbody _rigid; // _ por delante indicando que está oculta

    public static int SCORE = 0;

    public GameObject gun, bulletPrefab;
    public float xBorderLimit, yBorderLimit;

    // Start is called before the first frame update
    void Start()
    {
        // se usa para aplicar fuerzas al objeto del jugador y controlar su comportamiento físico
        _rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // comprueba si el juego no está pausado
        if (!PauseControl.gameIsPaused)
        {

            var newPos = transform.position;
            if (newPos.x > xBorderLimit)
                newPos.x = -xBorderLimit + 1;
            else if (newPos.x < -xBorderLimit)
                newPos.x = xBorderLimit - 1;
            else if (newPos.y > yBorderLimit)
                newPos.y = -yBorderLimit + 1;
            else if (newPos.y < -yBorderLimit)
                newPos.y = yBorderLimit - 1;

            transform.position = newPos;

            // Horizontal
            float rotation = Input.GetAxis("Rotate") * Time.deltaTime;
            // Vertical
            float thrust = Input.GetAxis("Thrust") * Time.deltaTime; // Time.deltaTime, se multiplica por el tiempo por cada frame (cuanto peor el pc, mejor)

            Vector3 thrustDirection = transform.right; // vector derecha al que apunta nuestro objeto

            _rigid.AddForce(thrustForce * thrustDirection * thrust);

            transform.Rotate(Vector3.forward, rotationSpeed * -rotation);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Instantiate crea una copia del objeto bulletPrefab que es una ref. al GameObject que va a crear
                // GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity); // sin rotación
                GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();

                if (bullet != null)
                {
                    bullet.transform.position = gun.transform.position;
                    bullet.transform.rotation = gun.transform.rotation;
                    bullet.SetActive(true);

                    //Bullet balaScript = bullet.GetComponent<Bullet>();
                    //balaScript.targetVector = transform.right;
                }
            }
        }
    }

    // Para saber cual de las dos funciones corre, habrá que ver el 
    // (Capsule) Collider y ver si esta activado o desactivado la casilla Is Trigger
    private void OnCollisionEnter(Collision collision)
    { // para trabajar colisiones, para atravesamiento OnTriggerEnter/Exit/Stay

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("MiniEnemy"))
        {
            SCORE = 0;
            // SceneManager.GetActiveScene().name devuelve el nombre de la escena actual o poner "SampleScene", es lo mismo
            SceneManager.LoadScene((SceneManager.GetActiveScene().name));
        }
        else
        {
            Debug.Log("He colisionado con otra cosa...");
        }


    }

    /* private void OnTriggerEnter(Collider other) {

    } */
}
