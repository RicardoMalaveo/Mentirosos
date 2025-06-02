using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    //Los animator curves son parecidos a los que hay en la herramienta de animación
    //https://docs.unity3d.com/6000.1/Documentation/ScriptReference/AnimationCurve.html
    //Estos son las curvas que relaccionan el movimiento a través del tiempo y espacio (osea se la velocidad)
    public AnimationCurve moveCurve;
    public AnimationCurve rotationCurve;

    private bool isAnimating = false; //Contolador para evitar que dos animaciones se hagan al mismo tiempo

    public void AnimateCard(Transform startPos, Transform endPos, float duration, Vector3 finalEulerRotation) //Vamos a recoger dos variables, una posición inicial y otra posición final, éstas serán el componenente transform de algún objeto
        //haciendo que la carta vaya desde la posición del primer objeto con transform (startPos) al segundo, este método es público
    {
        if (!isAnimating)//Comprobamos que no esté siendo animada, esta booleana es modificada a través de la corrutina de AnimateMovement
        {
            StartCoroutine(AnimateMovement(startPos.position,endPos.position,duration,Quaternion.Euler(finalEulerRotation)));//Le damos a la corrutina el componente position de los transform de los objetos , además vamos a acceder
            // a la rotación del objeto que contenga el script y le vamos a decir que deberá animar esa rotación en base al finalEulerRotation que hemos declarado arriba y que hemos llamado final rotation
        }
    }

    private IEnumerator AnimateMovement(Vector3 from, Vector3 to, float duration, Quaternion finalRotation) //Recogemos el componente position que nos está pasando la función AnimateCard de startPos y le llamamos from
        //hacemos lo mismo con endPos y lo llamamos to, y la rotación como finalRotation, ahcemos lo mismo con la rotación y la duración, éste método es privado
    {
        isAnimating = true;//cambiamos booleana para evitar que se ejecute otra animación
        float time = 0f; //añadimos un tiempo que nos va a llevar la cuenta de la cantidad de tiempo que está durando la animación, lo usaremos para el while
        Quaternion startRotation = transform.rotation; //Recogemos cual es la rotación incial del objeto y lo llamamos startRotation

        while (time < duration)//Aquí empezamos a animar el objetomoviendolo y rotandolo, una vez se termine el tiempo estipulado el while parará de actuar
        {
            float t = time / duration;// t es el tiempo normalizado, su valor estará entre el 0 y el 1
            float easedT = moveCurve.Evaluate(t); //El evaluate lo que hace es que recoge el tiempo que le has idicado en la curva de movimiento, esto lo hacemos para que de forma visual en el inspector puedas ver 
            //la curva de movimiento y el script la use, https://docs.unity3d.com/ScriptReference/AnimationCurve.Evaluate.html
            float easedRot = rotationCurve.Evaluate(t);

            //Interpolación de movimiento entre posición inicial 
            transform.position = Vector3.Lerp(from, to, easedT); //Usamos el tiempo que queremos que pase en relacción a el tiempo de la gráfica de moveCurve a través de easedT

            //Rotación de movimiento entre rotación incial y la final
            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, easedRot);

            time += Time.deltaTime; //Añadimos el tiempo que pasa a time
            yield return null; //esto hace que la corrutina espere al frame y se vuelva a ejecutar, esto evita que
            //la corrutina se haga de golpe
        }

        isAnimating = false;
    }
}
