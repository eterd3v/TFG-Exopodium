using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GenerarCircuitoHex : MonoBehaviour {   
    // Crear vías ya conectadas en lugar de crear vías separadas, para luego conectarlas

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
        InfoRecta iRecta=null, iLastRecta=null;

        Vector3 x_z = Vector3.zero; // Trasl. x, Rot. Y, Trasl. z
        float rotacion = 0.0f;
        
        // TRUE: recta,  FALSE: curva. 
        // Guarda la eleccion de la vía a instanciar y la anterior ya instanciada
        bool eleccion=true, lastEleccion=eleccion, curva=false;

        string tipo = "a";          // Tipo de la vía actual a instanciar

        // Generar una primera via para el correcto funcionamiento del algoritmo (vias[i-1])
        generaVia(x_z, 0.0f, tipo, ref iRecta);
        iLastRecta = iRecta;

        for (int i = 1; i < viasGenerar - 1; ++i) {
            // Determinar la eleccion y si hay curva
            pickVia(i, ref eleccion, ref curva, lastEleccion);  
            // Generar posición, rotacion y tipo. Después no se pueden alterar
            xyzNuevaVia(ref x_z, ref rotacion, ref tipo, curva, eleccion, lastEleccion, vias[i-1]);
            // Generar la via
            generaVia(x_z, rotacion, tipo, ref iRecta);              
            // Si es una curva, coincidir paredes de la via actual y la anterior
            if (curva)
                coincidirParedes(tipo, eleccion, lastEleccion, iRecta, iLastRecta);
            // Eliminar Transforms y objetos que no van a servir más
            iLastRecta.Eliminar();
            
            // Actualizar variables
            lastEleccion = eleccion;
            iLastRecta = iRecta;
        }

        x_z = vias[viasGenerar-2].transform.Find(tipo).position; // IMPORTANTE, usa el de la última iteracion
        generaFinal(x_z, rotacion); // Situa el último prefab en vias[viasGenerar-1]

        iRecta.Eliminar();
    }

    void xyzNuevaVia(ref Vector3 x_z, ref float rotacion, ref string tipo, bool curva, bool eleccion, bool lastEleccion, GameObject lastVia ){
        if (!curva) {           // Es una recta
            tipo = "a";
            x_z = lastVia.transform.Find(tipo).position;
        }else{
            // PARA DETERMINAR SI UNA VÍA ESTÁ A PUNTO DE SER MAL COLOCADA
            bool limitado = rotacion >= 59.999f || rotacion <= -59.999f;

            // ELEGIR TIPO DE VIA
            if (limitado) {             // Si está a 60 o -60 grados, no puede tomar ciertos tipos => Se incluye en la búsqueda 
                tipo = rotacion > 0.0f ? "c" : "b";
            } else {
                tipo = rand.Next()%2 == 0 ? "b" : "c";
            }

            // GameObject de la posición correspondiente con la vía i
            Transform correspondiente = lastVia.transform.Find(tipo);

            // POSICIONAR
            x_z = correspondiente.position;

            // ROTACIÓN EN BASE A LA PIEZA CORRESPONDIENTE DE LA ANTERIOR VÍA
            rotacion += correspondiente.localEulerAngles.y; // Rotación local, no global
            if (limitado)   
                rotacion = 0.0f;  // Para que el circuito crezca o se mantenga en el eje X
        }
    }

    void coincidirParedes(string tipo, bool eleccion, bool lastEleccion, InfoRecta iRecta, InfoRecta iLastRecta){
        if (lastEleccion) {     // Anterior: Recta, Actual: Curva. Aquí b y a son sinónimos
            string modAnt = tipo == "b" ? "c" : "b";
            iLastRecta.SetTipo(modAnt);
        } else {                // Anterior: Curva, Actual: Recta
            string modAnt = tipo == "b" ? "b" : "c";
            iLastRecta.SetTipo(modAnt); 
        }
                                
        iRecta.SetTipo("a");    // MODIFICAR VIA ACTUAL PARA DEJARLA COMO UNA VIA NORMAL (después de i-1)
    }

    InfoRecta GetRecta(GameObject go) { // Internamente en Unity es una operación algo lenta
        return go.GetComponent<InfoRecta>();
    }

    void generaVia(Vector3 x_z, float y_, string tipo, ref InfoRecta iRecta) {
        GameObject go = Instantiate(prefHex, transformReferencia);  // <- Siempre va a ser una recta
        iRecta = GetRecta(go);      // Asigno a variable de clase
        iRecta.SetTipo(tipo);
        go.name = "V" + nVia++;
        go.transform.SetParent(this.transform);
        go.transform.Translate(x_z);                      // Primero: traslación
        go.transform.Rotate(Vector3.up, y_, Space.Self);  // Segundo: rotación sobre sí mismo en el eje Y
        go.isStatic = true;
        vias.Add(go);
    }

    void generaFinal(Vector3 x_z, float y_){
        GameObject nuevaVia = Instantiate(prefHex, transformReferencia);
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
                tramoRecta = restablecerTramo(i, 1, maxTramoRecta);
                eleccion = rand.Next() % 2 == 0;
            }
        } else {            // diagonal
            if (tramoDiagonal-- <= 0) {
                tramoDiagonal = restablecerTramo(i, 1, maxTramoDiagonal);
                eleccion = rand.Next() % 2 == 0;
            }
        }

        curva = eleccion != lastEleccion; // Se detecta la vía i empieza un cambio (curva)

    }

    int restablecerTramo(int i, int min, int max ){
        return rand.Next(min, max);
    }

    float randFloat(){
        return (float)rand.Next(999999) / 1000000.0f;
    }

}
