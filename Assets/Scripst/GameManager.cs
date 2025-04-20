using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Necesario para usar TextMeshPro.
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagenment : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuPrincipal;
    public GameObject menuGameOver;
    public GameObject col;
    public Renderer fondo;
    public List<GameObject> cols;
    public float velocidad = 2;
    public GameObject ob1;
    public GameObject ob2;
    public GameObject ob3;
    public GameObject ob4;
    public GameObject ob5;

    // Ave 
    public GameObject ob6;
    public GameObject ob7;


    public List<GameObject> obstaculos;

    public bool gameOver=false;
    public bool star = false;


    public GameObject metaPrefab; // Prefab de la meta
    private bool metaGenerada = false; // Controla si la meta ya fue generada
    public float distanciaParaMeta = 50f; // Distancia recorrida antes de generar la meta
    private float distanciaRecorrida = 0f; // Lleva el control de la distancia recorrida
    public GameObject menuVictoria;

    private GameObject metaActiva; // Referencia a la meta activa

    // Monedas
    public GameObject monedaPrefab; // Asignar prefab de moneda
    public List<GameObject> monedas; // Lista para gestionar las monedas
    public List<GameObject> aves;
    public GameObject avesPrefab;
    // Lista para gestionar las monedas
    public static GameManagenment instance; // Singleton para acceso global.
    public TextMeshProUGUI textoMonedas; // Campo para mostrar el contador en la UI.
    private int contadorMonedas = 0; // Contador inicial de monedas.

    // Control de aparición de monedas
    private float spawnInterval = 2f; // Intervalo en segundos para generar nuevas monedas
    private float spawnTimer = 0f; // Temporizador para controlar el intervalo

    public int nivelActual = 1; // Controla el nivel actual
    public GameObject menuFinalNivel2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para incrementar monedas
    public void IncrementarMonedas()
    {
        contadorMonedas++;
        textoMonedas.text = "Monedas: " + contadorMonedas;
    }





    void Start()


    {

        if (SceneManager.GetActiveScene().name == "Nivel2")
        {
            nivelActual = 2;
        }

        for (int i=0;i<21;i++)
        {
            cols.Add(Instantiate(col, new Vector2(-10 + i, -3), Quaternion.identity)); 

        }

        obstaculos.Add(Instantiate(ob1, new Vector2(6, -2), Quaternion.identity));//
        obstaculos.Add(Instantiate(ob2, new Vector2(11, -2), Quaternion.identity));
        obstaculos.Add(Instantiate(ob3, new Vector2(14, -2), Quaternion.identity));//
        obstaculos.Add(Instantiate(ob4, new Vector2(19, -2), Quaternion.identity));
        obstaculos.Add(Instantiate(ob5, new Vector2(21, -2), Quaternion.identity));
        float yAereo = Random.Range(4f, 6f); // Rango para que aparezca entre 2 y 4 en Y
        obstaculos.Add(Instantiate(ob6, new Vector2(11, yAereo), Quaternion.identity));




    }

    // Update is called once per frame
    void Update()
    {
        if (star==false)
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                star = true;
            }
        }
        if (star == true && gameOver==true)
        {
            menuGameOver.SetActive(true);

            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }


        if (star==true && gameOver==false)
        {
            menuPrincipal.SetActive(false);
            
            fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.015f, 0) * Time.deltaTime;
            for (int i = 0; i < cols.Count; i++)
            {
                if (cols[i].transform.position.x <= -10)
                {
                    cols[i].transform.position = new Vector3(10, -3, 0);
                }
                cols[i].transform.position = cols[i].transform.position + new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;

            }

               for (int i = 0; i < obstaculos.Count; i++)
                 {
                     if (obstaculos[i].transform.position.x <= -10)
                     {
                         float randomObs = Random.Range(9, 18);
                         obstaculos[i].transform.position = new Vector3(randomObs, -2, 0);
                     }
                     obstaculos[i].transform.position = obstaculos[i].transform.position + new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;


                 }



            MoverMonedas();
            MoverAves();


        }
        //
        if (star && !gameOver)
        {
            // Actualiza la distancia recorrida
            distanciaRecorrida += Time.deltaTime * velocidad;

            // Genera la meta si se alcanza la distancia requerida
            if (distanciaRecorrida >= distanciaParaMeta && !metaGenerada)
            {
                GenerarMeta();
                metaGenerada = true;
            }

            // Mueve la meta si está activa
            if (metaActiva != null)
            {
                if (metaActiva.transform.position.x <= -10)
                {
                    Destroy(metaActiva); // Destruye la meta si sale de los límites
                    metaActiva = null;
                }
                else
                {
                    metaActiva.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
                }
            }
        }

        // Mantén las demás actualizaciones (movimiento de monedas, obstáculos, etc.)
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            GenerarMoneda();
            GenerarAve();
            spawnTimer = 0f; // Reiniciar temporizador
        }







    }
    void MoverMonedas()
    {
        for (int i = 0; i < monedas.Count; i++)
        {
            if (monedas[i] != null)
            {
                if (monedas[i].transform.position.x <= -10)
                {
                    Destroy(monedas[i]); // Destruir monedas fuera del límite
                    monedas.RemoveAt(i);
                    i--;
                }
                else
                {
                    monedas[i].transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * velocidad;
                }
            }
        }
    }



    void MoverAves()
    {
        for (int i = 0; i < aves.Count; i++)
        {
            if (aves[i] != null)
            {
                if (aves[i].transform.position.x <= -12)
                {
                    Destroy(aves[i]); // Destruir monedas fuera del límite
                    aves.RemoveAt(i);
                    i--;
                }
                else
                {
                    aves[i].transform.position += new Vector3(-5, 0, 0) * Time.deltaTime * velocidad;
                }
            }
        }
    }




    void GenerarMoneda()
    {
        float randomX = Random.Range(9, 18); // Posición X aleatoria
        float randomY = Random.Range(-2, 2); // Posición Y aleatoria
        GameObject nuevaMoneda = Instantiate(monedaPrefab, new Vector2(randomX, randomY), Quaternion.identity);
        monedas.Add(nuevaMoneda); // Añade la nueva moneda a la lista
    }


    void GenerarAve()
    {
        float randomX = Random.Range(9, 18); // Posición X aleatoria
        float randomY = Random.Range(-1, 3); // Posición Y aleatoria
        GameObject ave = Instantiate(avesPrefab, new Vector2(randomX, randomY), Quaternion.identity);
        aves.Add(ave); // Añade la nueva moneda a la lista

    }

    void GenerarMeta()
    {
        float posX = 7f; // Ajusta según el diseño del escenario
        float posY = -2f; // Ajusta según el diseño del escenario
        metaActiva = Instantiate(metaPrefab, new Vector2(posX, posY), Quaternion.identity);
    }

    public void GanarJuego()
    {
        gameOver = true;
        star = false;


        if (nivelActual == 1)
        {
            menuVictoria.SetActive(true); // Activa la pantalla de victoria
            // Cambia a la escena del Nivel 2 después de un pequeño retraso
            StartCoroutine(CargarSiguienteNivel());
        }
        else if (nivelActual == 2)
        {
            // Muestra el menú final del nivel 2
            MostrarMenuFinal();
        }
    }


    private IEnumerator CargarSiguienteNivel()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        SceneManager.LoadScene("Nivel2"); // Cambia a la escena "Nivel2"
    }
    public void MostrarMenuFinal()
    {
        menuFinalNivel2.SetActive(true); // Activa el menú final
    }
    public void VolverNivel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReintentarNivel2()
    {
        SceneManager.LoadScene("Nivel2");
    }

    public void SalirDelJuego()
    {
        Application.Quit(); // Solo funciona al compilar el juego
    }


}
