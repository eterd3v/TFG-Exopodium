using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarVias : MonoBehaviour
{   // Crear vías ya conectadas en lugar de crear vías separadas, para luego conectarlas

    // PREFABS A USAR
    public GameObject prefabFinal, prefabRecto, prefabCurva;

    // POSICIÓN DE REFERENCIA (Padre en la jerarquía de las vías)
    public Transform  transformReferencia; 

    // SEMILLA PARA ALEATORIEDAD
    public bool usarSemilla;
    public int semilla;
    private System.Random rand;

    // VIAS A GENERAR
    public int viasGenerar;
    public List<GameObject> vias;

    // PARÁMETROS DE GENERACIÓN
    public int tipoGeneracion;
    public int incrementoRectas, incrementoDiagonales;
    public float inicioIncremento, finIncremento;
    public int minTramoRecta, maxTramoRecta;
    public int minTramoDiagonal, maxTramoDiagonal;

    // VARIABLES PRIVADAS PARA LA GENERACIÓN
    private bool eleccion, lastEleccion, curva;
    private float pMutacionEleccion;
    private int contador;
    private int tramoRecta;
    private int tramoDiagonal;
    private float rotacion;   // (-1.0f, 0.0f, 1.0f) * 90.0f
    
    // ÓPTIMIZACIÓN PARA LOS MÉTODOS GetRecta y GetCurva
    private InfoRecta iRecta, iLastRecta; 
    private InfoCurva iCurva, iLastCurva;
    
    // Start is called before the first frame update
    void Start() {
        vias = new List<GameObject>();
        rand = usarSemilla ? (new System.Random(semilla)) : (new System.Random());
        tramoRecta =    rand.Next(minTramoRecta,    maxTramoRecta);
        tramoDiagonal = rand.Next(minTramoDiagonal, maxTramoDiagonal);
        pMutacionEleccion = randFloat();
        generarVias();
    }

    void generarVias() {

        Vector3 x_z = Vector3.zero; // Trasl. x, Rot. Y, Trasl. z
        rotacion = 0.0f;

        eleccion = true;            // true: recta,  false: curva
        lastEleccion = eleccion;    // Para guardar la vía actual (a instanciar) y la anterior (ya instanciada)

        string tipo = "a";          // Tipo de la vía actual a instanciar

        // Generar una primera via para el correcto funcionamiento del algoritmo (vias[i-1])
        generaVia(0, x_z, 0.0f, tipo);
        iLastRecta = iRecta;

        for (int i = 1; i < viasGenerar - 1; ++i) {
            
            pickVia(i);  // DETERMINA SI HAY UNA CURVA O NO

            if (!curva){
                tipo = eleccion ? "a" : "A";
                x_z = vias[i-1].transform.Find(tipo).position;
                // rotacion No se modifica aquí ya que sigue la misma rotación de la anterior via 
            }else{

                // PARA DETERMINAR SI UNA VÍA ESTÁ A PUNTO DE SER MAL COLOCADA
                bool limitado = rotacion >= 90.0f || rotacion <= -90.0f;

                // ELEGIR TIPO DE VIA
                if (limitado) { // Si está a 90 o -90 grados, no puede tomar ciertos tipos => Se incluye en la búsqueda 
                    bool anguloPos =        rotacion > 0.0f;
                    if (anguloPos)          tipo = eleccion ? "c" : "C";
                    else                    tipo = eleccion ? "b" : "B";
                }else{
                    if (lastEleccion)       tipo = rand.Next()%2 == 0 ? "B" : "C";
                    else                    tipo = rand.Next()%2 == 0 ? "b" : "c";
                }

                // Da el tipo correspondiente a-A, b-B, c-C, según si la via es recta o diagonal
                string tipoCorrespondiente = eleccion ? tipo.ToUpper() : tipo.ToLower();

                // GameObject de la posición correspondiente con la vía i
                Transform correspondiente = vias[i-1].transform.Find(tipoCorrespondiente);

                // POSICIONAR
                x_z = correspondiente.position;

                // ROTACIÓN EN BASE A LA PIEZA CORRESPONDIENTE DE LA ANTERIOR VÍA
                rotacion += correspondiente.localEulerAngles.y; // Rotación local, no global
                if (limitado)   
                    rotacion = 0.0f;  // Para que el circuito crezca o se mantenga en el eje X

            }

            // COLOCA LA VÍA
            generaVia(i, x_z, rotacion, tipo);              // Generar dicha vía. La posición ya no se puede alterar

            if (curva) {                
                                        // MODIFICAR VIA ANTERIOR PARA QUE LAS PAREDES COINCIDAN
                string modAnt;
                if (lastEleccion) {     // Anterior: Recta, Actual: Curva. Aquí b y a son sinónimos
                    modAnt = tipo == "B" ? "c" : "b";
                    iLastRecta.SetTipo(modAnt);
                } else {                // Anterior: Curva, Actual: Recta
                    modAnt = tipo == "b" ? "B" : "C";
                    iLastCurva.SetTipo(modAnt); 
                }   
                                        // MODIFICAR VIA ACTUAL PARA DEJARLA COMO UNA VIA NORMAL (después de i-1)
                if (eleccion)           iRecta.SetTipo("a");
                else                    iCurva.SetTipo("A"); 
            }
                                        // BORRAR COSAS INNECESARIAS DE LA VÍA ANTERIOR
            if (lastEleccion)           iLastRecta.Eliminar();
            else                        iLastCurva.Eliminar();


            // ACTUALIZAR VARIABLES ============================================================
            lastEleccion = eleccion;
            iLastRecta = iRecta;
            iLastCurva = iCurva;

        }

        x_z = vias[viasGenerar-2].transform.Find(tipo).position; // IMPORTANTE, usa el de la última iteracion
        generaFinal(x_z, rotacion); // Situa el último prefab en vias[viasGenerar-1]   

        //if (eleccion)           iRecta.Eliminar();
        //else                    iCurva.Eliminar();
        iCurva = iLastCurva = null;
        iRecta = iLastRecta = null;
    }

//    Debug.DrawRay(vias[i-1].transform.position, vias[i].transform.position - vias[i-1].transform.position, Color.red, 600); // Ver el trazo de la curva en el editor. Expira en 60 segs.

    void pickVia(int i) {

        if (eleccion) {     // recta
            if (tramoRecta-- <= 0) {
                tramoRecta = restablecerTramo(i, minTramoRecta, maxTramoRecta, incrementoRectas);
                eleccion = !eleccion;
                //mutacionEleccion();
            }
        } else {            // diagonal
            if (tramoDiagonal-- <= 0) {
                tramoDiagonal = restablecerTramo(i, minTramoDiagonal, maxTramoDiagonal,  incrementoDiagonales);
                eleccion = !eleccion;
                //mutacionEleccion();
            }
        }

        curva = eleccion != lastEleccion; // Se detecta la vía i empieza un cambio (curva)

    }

    int restablecerTramo(int i, int min, int max, int incremento){
        float porcentaje = (float) i / (float) viasGenerar;
        if (inicioIncremento <= porcentaje && porcentaje <= finIncremento) {
            min += incremento;
            max += incremento;
        }
        return rand.Next(min, max);
    }

    void mutacionEleccion(){ // Mutación de la elección bajo una probabilidad
        if (pMutacionEleccion < randFloat())  {
            eleccion = !eleccion;

        }
    }

    float randFloat(){
        return (float)rand.Next(999999) / 1000000.0f;
    }

    InfoRecta GetRecta(int i) { // Internamente es una operación algo lenta
        return vias[i].GetComponent<InfoRecta>();
    }

    InfoCurva GetCurva(int i) { // Internamente es una operación algo lenta
        return vias[i].GetComponent<InfoCurva>();
    }

    void generaVia(int i, Vector3 x_z, float y_, string tipo) {
        if (eleccion) {         // Recta
            vias.Add(Instantiate(prefabRecto, transformReferencia));
            iRecta = GetRecta(i); // Asigno a variable de clase
            iRecta.SetTipo(tipo);
        } else {                // Curva
            vias.Add(Instantiate(prefabCurva, transformReferencia));
            iCurva = GetCurva(i); // Asigno a variable de clase
            iCurva.SetTipo(tipo);
        }
        vias[i].name = "V" + i;
        vias[i].transform.SetParent(this.transform);
        vias[i].transform.Translate(x_z);              // Primero: traslación
        vias[i].transform.Rotate(Vector3.up, y_, Space.Self);  // Segundo: rotación sobre sí mismo en el eje Y
        vias[i].isStatic = true;
    }

    void generaFinal(Vector3 x_z, float y_){
        GameObject nuevaVia = Instantiate(prefabFinal, transformReferencia);
        nuevaVia.name = "VFinal";
        nuevaVia.transform.SetParent(this.transform);
        nuevaVia.transform.Translate(x_z.x, 0 ,x_z.z); // El +10 es por la última via recta del bucle
        nuevaVia.transform.Rotate(0,y_,0);
        nuevaVia.isStatic = true;
        vias.Add(nuevaVia);
    }

}
