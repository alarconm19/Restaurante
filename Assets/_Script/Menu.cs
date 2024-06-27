using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
 
    public void EscenaJuego() 
    {

        SceneManager.LoadScene("juego");
    }
    public void Salir() 
    {
    
    Application.Quit();
    }
   
         public void VolverMenu()
         {

            SceneManager.LoadScene("menu");
         }

}
