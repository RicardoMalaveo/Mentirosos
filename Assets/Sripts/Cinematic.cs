using UnityEngine;
using UnityEngine.Playables;

public class Cinematic : MonoBehaviour
{
    public PlayableDirector cinematica;

    public void ActivarCinematica()
    {
        if (cinematica != null)
        {
            cinematica.Play();
            Debug.Log("Cinemática activada");
        }
    }
}
