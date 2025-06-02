using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    //Los animator curves son parecidos a los que hay en la herramienta de animaci�n
    //https://docs.unity3d.com/6000.1/Documentation/ScriptReference/AnimationCurve.html
    //Estos son las curvas que relaccionan el movimiento a trav�s del tiempo y espacio (osea se la velocidad)
    public AnimationCurve moveCurve;
    public AnimationCurve rotationCurve;

    private bool isAnimating = false; //Contolador para evitar que dos animaciones se hagan al mismo tiempo

    public void AnimateCard(Transform startPos, Transform endPos, float duration, Vector3 finalEulerRotation) //Vamos a recoger dos variables, una posici�n inicial y otra posici�n final, �stas ser�n el componenente transform de alg�n objeto
        //haciendo que la carta vaya desde la posici�n del primer objeto con transform (startPos) al segundo, este m�todo es p�blico
    {
        if (!isAnimating)//Comprobamos que no est� siendo animada, esta booleana es modificada a trav�s de la corrutina de AnimateMovement
        {
            StartCoroutine(AnimateMovement(startPos.position,endPos.position,duration,Quaternion.Euler(finalEulerRotation)));//Le damos a la corrutina el componente position de los transform de los objetos , adem�s vamos a acceder
            // a la rotaci�n del objeto que contenga el script y le vamos a decir que deber� animar esa rotaci�n en base al finalEulerRotation que hemos declarado arriba y que hemos llamado final rotation
        }
    }

    private IEnumerator AnimateMovement(Vector3 from, Vector3 to, float duration, Quaternion finalRotation) //Recogemos el componente position que nos est� pasando la funci�n AnimateCard de startPos y le llamamos from
        //hacemos lo mismo con endPos y lo llamamos to, y la rotaci�n como finalRotation, ahcemos lo mismo con la rotaci�n y la duraci�n, �ste m�todo es privado
    {
        isAnimating = true;//cambiamos booleana para evitar que se ejecute otra animaci�n
        float time = 0f; //a�adimos un tiempo que nos va a llevar la cuenta de la cantidad de tiempo que est� durando la animaci�n, lo usaremos para el while
        Quaternion startRotation = transform.rotation; //Recogemos cual es la rotaci�n incial del objeto y lo llamamos startRotation

        while (time < duration)//Aqu� empezamos a animar el objetomoviendolo y rotandolo, una vez se termine el tiempo estipulado el while parar� de actuar
        {
            float t = time / duration;// t es el tiempo normalizado, su valor estar� entre el 0 y el 1
            float easedT = moveCurve.Evaluate(t); //El evaluate lo que hace es que recoge el tiempo que le has idicado en la curva de movimiento, esto lo hacemos para que de forma visual en el inspector puedas ver 
            //la curva de movimiento y el script la use, https://docs.unity3d.com/ScriptReference/AnimationCurve.Evaluate.html
            float easedRot = rotationCurve.Evaluate(t);

            //Interpolaci�n de movimiento entre posici�n inicial 
            transform.position = Vector3.Lerp(from, to, easedT); //Usamos el tiempo que queremos que pase en relacci�n a el tiempo de la gr�fica de moveCurve a trav�s de easedT

            //Rotaci�n de movimiento entre rotaci�n incial y la final
            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, easedRot);

            time += Time.deltaTime; //A�adimos el tiempo que pasa a time
            yield return null; //esto hace que la corrutina espere al frame y se vuelva a ejecutar, esto evita que
            //la corrutina se haga de golpe
        }

        isAnimating = false;
    }
}
