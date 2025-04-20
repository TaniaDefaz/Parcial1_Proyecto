using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Moneda : MonoBehaviour
{
    public void Start()
    {
        
    }
    public void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga el tag "Player"
        {
            GameManagenment.instance.IncrementarMonedas(); // Incrementa el contador de monedas
            Destroy(gameObject); // Destruye la moneda
  
        }
    }
}
