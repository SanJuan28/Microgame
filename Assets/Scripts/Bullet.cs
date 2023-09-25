using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float speed = 10f, splitSpeed = 5f;
    public float maxLifeTime = 2f;
    public Vector3 targetVector;
    public GameObject miniMeteorPrefab;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // translate se usa para mover el objeto en el espacio tridimensional
        transform.Translate(speed * targetVector * Time.deltaTime);

        if(IsTheBulletOutOfGameArea()) { // si la bala está fuera de los límites del juego
            gameObject.SetActive(false); // desactiva la bala 
            transform.position = new Vector3(20f,20f,0f);
        }
    }

    private bool IsTheBulletOutOfGameArea()
    {
        float xLimit = 10f;
        float yLimit = 5f;
        var bulletPos = transform.position;

        if(bulletPos.x > xLimit || bulletPos.x < -xLimit ||
            bulletPos.y > yLimit || bulletPos.y < -yLimit) {
                return true; // la bala está fuera del área del juego
        }

        return false; // la bala está dentro del área deljuego
    }

    private void OnCollisionEnter(Collision collision) {
    
        if(collision.gameObject.CompareTag("Enemy")) {
            IncreaseScore();
            GameObject meteor = collision.gameObject;
            // collision.contact devuelve la matriz de estructuras, cada ContactPoint representa un punto de contacto en la colisión entre los dos objetos
            // collision.contact[0] accede al primer punto de contacto en esa matriz
            // colision.contact[0].point representa las coordenadas en el espacio tridim. donde ocurrió el impacto entre los dos objetos
            Vector3 impactPoint = collision.contacts[0].point;
            Vector3 bulletDirection = transform.right; // dirección de la bala

            // calcula una dirección perpendicular a la dirección de la bala
            Vector3 perpendicularDirection = new Vector3(-bulletDirection.y, bulletDirection.x, 0f);

            // divide al asteroide en dos partes
            GameObject miniMeteor1 = Instantiate(miniMeteorPrefab,impactPoint,Quaternion.identity);
            GameObject miniMeteor2 = Instantiate(miniMeteorPrefab,impactPoint,Quaternion.identity);

            // aplica fuerzas para que las partes se muevan en direcciones opuestas
            miniMeteor1.GetComponent<Rigidbody>().velocity = perpendicularDirection * splitSpeed;
            miniMeteor2.GetComponent<Rigidbody>().velocity = -perpendicularDirection * splitSpeed;

            Destroy(meteor); // destruye el meteorito grande
            Destroy(miniMeteor1, maxLifeTime);
            Destroy(miniMeteor2, maxLifeTime);
            gameObject.SetActive(false); // desactiva la bala
            transform.position = new Vector3(20f,20f,0f); // configura la posición fuera del área del juego

        } else if(collision.gameObject.CompareTag("MiniEnemy")) {
            IncreaseScore();
            Destroy(collision.gameObject); // desactiva el meteorito pequeño
            gameObject.SetActive(false); // desactiva la bala
            transform.position = new Vector3(20f,20f,0f); // configura la posición fuera del área del juego
        }

    }

    private void IncreaseScore() {
        Player.SCORE++;
        Debug.Log(Player.SCORE);
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        GameObject go = GameObject.FindGameObjectWithTag("UI");
        go.GetComponent<Text>().text = "Puntos : " + Player.SCORE;
    }
}
