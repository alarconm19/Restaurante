using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void EscenaJuego()
    {
        SceneManager.LoadScene("Restaurante");
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void VolverMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}