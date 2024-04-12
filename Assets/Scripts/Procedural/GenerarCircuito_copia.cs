using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Runtime.Serialization.Formatters.Binary; // Para los guardar estado del random

public class GenerarCircuitoBacktrack : MonoBehaviour
{   // Crear vías ya conectadas en lugar de crear vías separadas, para luego conectarlas
/*
    // PREFABS A USAR
    public GameObject prefabFinal, prefabRecto, prefabCurva;

    // POSICIÓN DE REFERENCIA (Padre en la jerarquía de las vías)
    public Transform  transformReferencia; 

    // SEMILLA PARA ALEATORIEDAD
    public bool usarSemilla;
    public int semilla;
    private Random rand;

    // VIAS A GENERAR
    public int viasGenerar;
    public List<GameObject> vias;
    private int nVia = 0;

    // PARÁMETROS DE GENERACIÓN
    public int tipoGeneracion;
    public int incrementoRectas, incrementoDiagonales;
    public float inicioIncremento, finIncremento;
    public int maxTramoRecta;
    public int maxTramoDiagonal;

    // VARIABLES PRIVADAS PARA LA GENERACIÓN
    private float pMutacionEleccion;
    private int contador;
    private int tramoRecta;
    private int tramoDiagonal;
    
    // Start is called before the first frame update
    void Start() {

        if (!usarSemilla){
            rand = new Random();
            semilla = rand.Next();
        }
        rand = new Random(semilla);

        vias = new List<GameObject>();
        if (maxTramoRecta <= 0)      maxTramoRecta = rand.Next(1,5);
        if (maxTramoDiagonal <= 0)   maxTramoDiagonal = rand.Next(1,5);
        tramoRecta =    rand.Next(1, maxTramoRecta);
        tramoDiagonal = rand.Next(1, maxTramoDiagonal);
        pMutacionEleccion = randFloat();
        generarVias();
    }

    Random guardarRandom(Random rand_){
        // https://stackoverflow.com/questions/8188844/is-there-a-way-to-grab-the-actual-state-of-system-random/8188878#8188878
        BinaryFormatter formateador = new BinaryFormatter(); 
        MemoryStream stream = new MemoryStream();
        formateador.Serialize(stream, rand_);
        return (Random) formateador.Deserialize(stream);
    }

    bool backtrack(int i){
        if (i >= viasGenerar)
            return true; // && rotacionFinal && posicionFinal; // De momento en true a secas
        else{
            // Guardar estado del Random actual
            Random randEstadoPrevio = guardarRandom(rand);

            pickVia(i, ref eleccion, ref curva, lastEleccion);  
            generarVia(i);
            if (backtrack(i+1)){
                
                return true;
            }else{
                vias.RemoveAt(i);   // Elimina de la lista
                rand = randEstadoPrevio;  // Restaurar estado del Random
            }
        }
        return false;
    }

    void generarVia(int i){
        InfoRecta iRecta=null, iLastRecta=null; 
        InfoCurva iCurva=null, iLastCurva=null;
        Vector3 x_z = Vector3.zero; // Trasl. x, Rot. Y, Trasl. z
        float rotacion = 0.0f;
        bool eleccion=true, lastEleccion=eleccion, curva=false;
        string tipo = "a";          // Tipo de la vía actual a instanciar
        iLastRecta = iRecta;

        xyzNuevaVia(ref x_z, ref rotacion, ref tipo, curva, eleccion, lastEleccion, vias[i-1]);
        generaVia(x_z, rotacion, tipo, ref iRecta, ref iCurva);              
        if (curva)
            coincidirParedes(tipo, eleccion, lastEleccion, iRecta, iLastRecta, iCurva, iLastCurva);
        lastEleccion = eleccion;
        iLastRecta = iRecta;
        iLastCurva = iCurva;
    }

    void xyzNuevaVia(ref Vector3 x_z, ref float rotacion, ref string tipo, bool curva, bool eleccion, bool lastEleccion, GameObject lastVia ){
        if (!curva){
            tipo = eleccion ? "a" : "A";
            x_z = lastVia.transform.Find(tipo).position;
        }else{
            bool limitado = rotacion >= 90.0f || rotacion <= -90.0f;
            /*if (limitado) { // Si está a 90 o -90 grados, no puede tomar ciertos tipos => Se incluye en la búsqueda 
                bool anguloPos =        rotacion > 0.0f;
                if (anguloPos)          tipo = eleccion ? "c" : "C";
                else                    tipo = eleccion ? "b" : "B";
            }else{
                
            }*//*
            if (lastEleccion)       tipo = rand.Next()%2 == 0 ? "B" : "C";
            else                    tipo = rand.Next()%2 == 0 ? "b" : "c";

            string tipoCorrespondiente = eleccion ? tipo.ToUpper() : tipo.ToLower();
            Transform correspondiente = lastVia.transform.Find(tipoCorrespondiente);
            x_z = correspondiente.position;
            rotacion += correspondiente.localEulerAngles.y; // Rotación local, no global
        }
    }

    void coincidirParedes(string tipo, bool eleccion, bool lastEleccion, InfoRecta iRecta, InfoRecta iLastRecta, InfoCurva iCurva, InfoCurva iLastCurva){
        if (lastEleccion) {     // Anterior: Recta, Actual: Curva. Aquí b y a son sinónimos
            string modAnt = tipo == "B" ? "c" : "b";
            iLastRecta.SetTipo(modAnt);
        } else {                // Anterior: Curva, Actual: Recta
            string modAnt = tipo == "b" ? "B" : "C";
            iLastCurva.SetTipo(modAnt); 
        }   
                                // MODIFICAR VIA ACTUAL PARA DEJARLA COMO UNA VIA NORMAL (después de i-1)
        if (eleccion)           iRecta.SetTipo("a");
        else                    iCurva.SetTipo("A"); 
    }

    bool getCurva(string tipo1, string tipo2) {
        bool condicion1 = getEleccion(tipo1) && !getEleccion(tipo2);
        bool condicion2 = getEleccion(tipo2) && !getEleccion(tipo1);
        return condicion1 || condicion2;
    }

    InfoRecta GetRecta(GameObject go) { // Internamente en Unity es una operación algo lenta
        return go.GetComponent<InfoRecta>();
    }

    InfoCurva GetCurva(GameObject go) { // Internamente en Unity es una operación algo lenta 
        return go.GetComponent<InfoCurva>();
    }

    void generaVia(Vector3 x_z, float y_, string tipo, ref InfoRecta iRecta, ref InfoCurva iCurva) {
        GameObject go;
        if (getEleccion(tipo)) {        // Recta
            go = Instantiate(prefabRecto, transformReferencia);
            iRecta = GetRecta(go);      // Asigno a variable de clase
            iRecta.SetTipo(tipo);
        } else {                        // Curva
            go = Instantiate(prefabCurva, transformReferencia);
            iCurva = GetCurva(go);      // Asigno a variable de clase
            iCurva.SetTipo(tipo);
        }
        go.name = "V" + nVia++;
        go.transform.SetParent(this.transform);
        go.transform.Translate(x_z);                      // Primero: traslación
        go.transform.Rotate(Vector3.up, y_, Space.Self);  // Segundo: rotación sobre sí mismo en el eje Y
        go.isStatic = true;
        vias.Add(go);
    }

    bool getEleccion(string tipo) { // "a", "b", "c" es true, y false en otro caso
        string tiposRecta = "abc";
        return tiposRecta.Contains(tipo);
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

    void pickVia(int i, ref bool eleccion, ref bool curva, bool lastEleccion) {

        if (eleccion) {     // recta
            if (tramoRecta-- <= 0) {
                tramoRecta = rand.Next(1, maxTramoRecta);
                eleccion = !eleccion;
            }
        } else {            // diagonal
            if (tramoDiagonal-- <= 0) {
                tramoDiagonal = rand.Next(1, maxTramoDiagonal);
                eleccion = !eleccion;
            }
        }

        curva = eleccion != lastEleccion; // Se detecta la vía i empieza un cambio (curva)

    }
*/
}
