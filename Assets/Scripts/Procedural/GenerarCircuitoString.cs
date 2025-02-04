using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerarCircuitoString : MonoBehaviour
{   // Crear vías ya conectadas en lugar de crear vías separadas, para luego conectarlas

    // PREFABS A USAR
    public GameObject prefabRecto, prefabCurva;

    // POSICIÓN DE REFERENCIA (Padre en la jerarquía de las vías)
    public Transform  transformReferencia; 

    // SEMILLA PARA ALEATORIEDAD
    public bool usarSemilla;
    public int semilla;
    private System.Random rand;

    // VIAS A GENERAR
    public int viasTotales;
    public List<GameObject> vias;
    private int nVia = 0;

    // PARÁMETROS DE GENERACIÓN
    public int tipoGeneracion;
    public int incrementoRectas, incrementoDiagonales;
    public float inicioIncremento, finIncremento;
    public int minTramoRecta, maxTramoRecta;
    public int minTramoDiagonal, maxTramoDiagonal;

    // Start is called before the first frame update
    void Start() {
        vias = new List<GameObject>();
        rand = usarSemilla ? (new System.Random(semilla)) : (new System.Random());
        
        string[] pruebaCadenas = {"a", "b", "A", "B"};

        Vector3 x_z = Vector3.zero; // Trasl. x,  Trasl. z
        float rotacion = 0.0f;

        // TRUE: recta,  FALSE: curva. 
        // Guarda la eleccion de la vía a instanciar y la anterior ya instanciada
        bool eleccion=true, lastEleccion=eleccion, curva=false;

        generarViasString(ref x_z, ref rotacion, ref eleccion, ref lastEleccion, ref curva, pruebaCadenas);
        Debug.Log("PRIMERA TANDA");
        //generarViasString(ref x_z, ref rotacion, ref eleccion, ref lastEleccion, ref curva, pruebaCadenas);
        //generarViasString(ref x_z, ref rotacion, ref eleccion, ref lastEleccion, ref curva, pruebaCadenas);
        //generarViasString(ref x_z, ref rotacion, ref eleccion, ref lastEleccion, ref curva, pruebaCadenas);
    }

    void generarViasString(ref Vector3 x_z, ref float rotacion, ref bool eleccion, ref bool lastEleccion, ref bool curva, string[] cadenaVias) {

        int numeroVias = cadenaVias.Length;

        // Variable para optimizar métodos GetRecta y GetCurva
        InfoRecta iRecta=null, iLastRecta=null; 
        InfoCurva iCurva=null, iLastCurva=null;

        string tipo = cadenaVias[0];          // Tipo de la vía actual a instanciar

        // Generar una primera via para el correcto funcionamiento del algoritmo (vias[i-1])
        generaVia(x_z, 0.0f, tipo, ref iRecta, ref iCurva);
        iLastRecta = iRecta;

        for (int i = 1; i < numeroVias; ++i) {
            
            // Dado el tipo, determinar la eleccion y si hay curva
            tipo     = cadenaVias[i];
            eleccion = getEleccion(tipo);
            curva    = eleccion != lastEleccion;

            // Generar posición, rotacion. Después no se pueden alterar
            xyzNuevaVia(ref x_z, ref rotacion, tipo, curva, eleccion, vias[i-1]);

            // Generar la via
            generaVia(x_z, rotacion, tipo, ref iRecta, ref iCurva);

            // Si es una curva, coincidir paredes de la via actual y la anterior
            if (curva) {
                coincidirParedes(tipo, eleccion, lastEleccion, iRecta, iLastRecta, iCurva, iLastCurva);
            }

            // Eliminar Transforms y objetos que no van a servir más
            if (lastEleccion)   iLastRecta.Eliminar();
            else                iLastCurva.Eliminar();

            // Actualizar variables
            lastEleccion = eleccion;
            iLastRecta = iRecta;
            iLastCurva = iCurva;
        }

        // x_z = vias[numeroVias-1].transform.Find(tipo).position; // IMPORTANTE, usa el de la última iteracion
        if (eleccion)           iRecta.Eliminar();
        else                    iCurva.Eliminar();
    }

    void xyzNuevaVia(ref Vector3 x_z, ref float rotacion, string tipo, bool curva, bool eleccion, GameObject lastVia) {
        if (!curva) {
            x_z = lastVia.transform.Find(tipo).position;
        } else {
            string tipoCorrespondiente = eleccion ? tipo.ToUpper() : tipo.ToLower();
            Transform correspondiente  = lastVia.transform.Find(tipoCorrespondiente);
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

    void generaVia(Vector3 x_z, float y_, string tipo, ref InfoRecta iRecta, ref InfoCurva iCurva) {
        GameObject go;
        if (getEleccion(tipo)) {    // Recta
            go = Instantiate(prefabRecto, transformReferencia);
            iRecta = GetRecta(go);  // Asigno a variable de clase
            iRecta.SetTipo(tipo);
        } else {                    // Curva
            go = Instantiate(prefabCurva, transformReferencia);
            iCurva = GetCurva(go);  // Asigno a variable de clase
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
        bool aux = tiposRecta.Contains(tipo);
        Debug.Log(tipo + (aux ? " recta" : " diagonal"));
        return aux;
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

}
