using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GenerarCircuitoHex : MonoBehaviour {   
    // Crear vías ya conectadas en lugar de crear vías separadas, para luego conectarlas

    public NaveMovimiento nave;

    // PREFABS A USAR
    public GameObject prefHex;

    // POSICIÓN DE REFERENCIA (Padre en la jerarquía de las vías)
    public Transform  transformReferencia; 

    // SEMILLA PARA ALEATORIEDAD
    public bool usarSemilla;
    public int semilla;
    private Random rand;

    // VIAS A GENERAR
    public int viasGenerar;
    public List<GameObject> vias;
    public List<TriggerPista> tVias;
    private int nVia = 0;

    // PARÁMETROS DE GENERACIÓN
    public int tipoGeneracion;
    public int incrementoRectas, incrementoDiagonales;
    public float inicioIncremento, finIncremento;
    public int maxTramoRecta;
    public int maxTramoDiagonal;

    // VARIABLES PRIVADAS PARA LA GENERACIÓN
    private int contador;
    private int tramoRecta;
    private int tramoDiagonal;
    
    // Start is called before the first frame update
    void Start() {
        if (!usarSemilla) {
            rand = new Random();
            semilla = rand.Next();
        }
        rand = new Random(semilla);
        vias = new List<GameObject>();
        if (maxTramoRecta <= 0)      maxTramoRecta = rand.Next(1,5);
        if (maxTramoDiagonal <= 0)   maxTramoDiagonal = rand.Next(1,5);
        tramoRecta =    rand.Next(1, maxTramoRecta);
        tramoDiagonal = rand.Next(1, maxTramoDiagonal);
        generarVias();
    }

    void generarVias() {

        // Variable para optimizar método GetRecta
        InfoHex iHex=null, iLastHex =null;

        Vector3 x_z = Vector3.zero; // Trasl. x, Rot. Y, Trasl. z
        float rotacion = 0.0f;
        
        // TRUE: recta,  FALSE: curva. 
        // Guarda la eleccion de la vía a instanciar y la anterior ya instanciada
        bool eleccion=true, lastEleccion=eleccion, curva=false;

        string tipo = "a";          // Tipo de la vía actual a instanciar
        string lastTipoCurva = "NA";       // Último tipo escogido en una curva

        // Generar una primera via para el correcto funcionamiento del algoritmo (vias[i-1])
        generaVia(x_z, 0.0f, tipo, ref iHex);
        iLastHex  = iHex;

        for (int i = 1; i < viasGenerar - 1; ++i) {
            // Determinar la eleccion y si hay curva
            pickVia(i, ref eleccion, ref curva, lastEleccion);  
            // Generar posición, rotacion y tipo. Después no se pueden alterar
            xyzNuevaVia(ref x_z, ref rotacion, ref tipo, ref lastTipoCurva, curva, vias[i-1].transform);
            // Generar la via
            generaVia(x_z, rotacion, tipo, ref iHex);    
            // Si es una curva, coincidir paredes de la via actual y la anterior
            if (curva)
                coincidirParedes(tipo,iHex, iLastHex );
            // Eliminar Transforms y objetos que no van a servir más
            iLastHex.Eliminar();
            // Actualizar variables
            lastEleccion = eleccion;
            iLastHex = iHex;
        }

        x_z = vias[viasGenerar-2].transform.Find(tipo).position; // IMPORTANTE, usa el tipo de la última iteracion del bucle
        generaFinal(x_z, rotacion); // Situa el último prefab en vias[viasGenerar-1]

        iHex.Eliminar();
    }

    void xyzNuevaVia(ref Vector3 x_z, ref float rotacion, ref string tipo, ref string lastTipoCurva, bool curva, Transform lastVia ){
        if (!curva) {           // Es una recta. La rotación no cambia, solo la posición.
            tipo = "a";
            x_z = lastVia.Find(tipo).position;
        } else {
            if (rotacion == 0)                  tipo = rand.Next()%2 ==  0  ? "b" : "c";
            else                                tipo = lastTipoCurva == "b" ? "c" : "b";

            Transform correspondiente = lastVia.Find(tipo);
            x_z = correspondiente.position;
            rotacion += correspondiente.localEulerAngles.y;

            if (rotacion > 60.0f)               rotacion -= 360.0f;         // Corrección del ángulo a un rango cerrado [-60,60]
            else if (rotacion < -60.0f)         rotacion += 360.0f;
            else                                rotacion = 0;
    
            if (rotacion != 0)
                lastTipoCurva = tipo;
        }
    }

    void coincidirParedes(string tipo,InfoHex iHex, InfoHex iLastHex ){
        iLastHex.SetTipo(tipo == "b" ? "c" : "b"); // La anterior será una via distinta a la actual. b o c 
        iHex.SetTipo("a");    // MODIFICAR VIA ACTUAL PARA DEJARLA COMO UNA VIA NORMAL (después de i-1)
    }

    InfoHex GetHex(GameObject go) { // Internamente en Unity es una operación algo lenta
        return go.GetComponent<InfoHex>();
    }

    void generaVia(Vector3 x_z, float y_, string tipo, ref InfoHex iHex) {
        GameObject go = Instantiate(prefHex, transformReferencia);  // <- Siempre va a ser una recta

        iHex = GetHex(go);
        iHex.SetTipo(tipo);
        iHex.tPista.setRotacion(new Vector3(0,y_,0));
        iHex.tPista.nm = nave;

        go.name = "V" + nVia++;
        go.transform.SetParent(this.transform);
        go.transform.Translate(x_z);                      // Primero: traslación
        go.transform.Rotate(Vector3.up, y_, Space.Self);  // Segundo: rotación sobre sí mismo en el eje Y
        go.isStatic = true;
        vias.Add(go);
        tVias.Add(iHex.tPista);
    }

    void generaFinal(Vector3 x_z, float y_) {
        GameObject nuevaVia = Instantiate(prefHex, transformReferencia);

        InfoHex iHex = GetHex(nuevaVia);
        iHex.tPista.setRotacion(new Vector3(0,y_,0));

        nuevaVia.name = "VFinal";
        nuevaVia.transform.SetParent(this.transform);
        nuevaVia.transform.Translate(x_z.x, 0 ,x_z.z); // El +10 es por la última via recta del bucle
        nuevaVia.transform.Rotate(0,y_,0);
        nuevaVia.isStatic = true;
        vias.Add(nuevaVia);
        tVias.Add(iHex.tPista);
    }

    void pickVia(int i, ref bool eleccion, ref bool curva, bool lastEleccion) {

        if (eleccion) {     // recta
            if (tramoRecta-- <= 0) {
                tramoRecta = rand.Next(1, maxTramoRecta);
                eleccion = rand.Next() % 2 == 0;
            }
        } else {            // diagonal
            if (tramoDiagonal-- <= 0) {
                tramoDiagonal = rand.Next(1, maxTramoDiagonal);
                eleccion = rand.Next() % 2 == 0;
            }
        }

        curva = eleccion != lastEleccion; // Se detecta la vía i empieza un cambio (curva)

    }

}
