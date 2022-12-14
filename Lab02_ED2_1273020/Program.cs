using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;

namespace Lab02_ED2_1273020
{
    
        public class Persona
        {
            //Atributos de mi clase persona
            public string name { get; set; }    //Nombre    
            public string dpi { get; set; }     //DPI
            public string datebirth { get; set; }//Fecha de nacimiento
            public string address { get; set; } //Dirección(localidad)
            public string[] companies { get; set; }

            public List<string> listaCOmp = new List<string>();
    }

   
    
        internal class Program
        {

        

        static void Main(string[] args)
            {

           
            //Metodos publicos de compresion LZ78
            //Variables 
           

            string LZ_Comprimir(List<string> L_Comp, string CompDPI)
            {
                
                string texto = "";
                string CompCaracter = "";
                int index = 0;
                int regresar = 0;
                texto = "0 " + CompDPI[0]+"\n";
                L_Comp.Add("");//Primer elemento nulo
                L_Comp.Add(CompDPI[0] + "");

                for (int indiceTexto = 1; indiceTexto < CompDPI.Length; indiceTexto++)
                {
                    CompCaracter += CompDPI[indiceTexto];

                    if (L_Comp.IndexOf(CompCaracter) != -1)
                    {
                        index = L_Comp.IndexOf(CompCaracter);
                        regresar = 1;
                        if (indiceTexto + 1 == CompDPI.Length)
                        {
                            texto += index + " null\n";
                        }
                    }
                    else
                    {
                        if (regresar == 1)
                        {
                            texto += index + " " + CompCaracter[CompCaracter.Length - 1] + "\n";
                        }
                        else
                        {
                            texto += "0 " + CompCaracter + "\n";
                        }
                        L_Comp.Add(CompCaracter);
                        CompCaracter = "";
                        regresar = 0;
                    }
                }


                return texto;
            }

            int puntero = 0;
            string SigCaracter = "";

            string LZ_Descomprimir(List<string> L_Comp, string EncodeID)
            {
                string texto = "";
                string[] CompararResultado = EncodeID.Split();
                
                for(int i =0; i<EncodeID.Length; i+=2)
                {
                    if(CompararResultado[i].Length ==0)
                    {
                        break;
                    }
                    puntero = int.Parse(CompararResultado[i]);
                    SigCaracter = CompararResultado[i + 1];

                    if(SigCaracter!="null")
                    {
                        texto += L_Comp[puntero] + SigCaracter;
                    }
                    else
                    {
                        texto+= L_Comp[puntero];
                    }
                    puntero = 0;
                    SigCaracter = "";
                }

                puntero = 0;
                SigCaracter = "";
                
                return texto;
            }


            //Comienza el procedimiento de lectura del archivo
            //************* SE DEBE DE INSERTAR LA DIRECCIÓN DEL ARCHIVO***************
            var reader = new StreamReader(File.OpenRead(@"C:\Users\Roberto Moya\Desktop\Lab2-E2\Lab02_ED2_1273020\Pruebita2.txt"));
                // List<string> list = new List<string>(); Listas que utilicé para pruebas pero no se utilizan
                // List<string> list2 = new List<string>();
                Lista<Persona> listaJSon = new Lista<Persona>(); //Instancio mi Lista para guardar los archivos del JSon

                //Variables de contadores
                int a = 0;
                int b = 0;
                int c = 0;
                while (!reader.EndOfStream)//Recorre todo el archivo de inicio a fin
                {
                    var lines = reader.ReadLine();//Guardo la linea 
                    var values = lines.Split(';');//Realizo el split para guardar la acción y el json por separado
                                                  //Ejemplo: values[0] tiene la acción (INSERT, PATCH, DELETE).
                                                  //values[1] contiene la serialización json.

                    string jsonString = values[1];//Paso a una cadena string el contenido de values[1], que es donde está la cadena json
                    Persona personaN = JsonSerializer.Deserialize<Persona>(jsonString);//deserializo el string con el json y lo guardo en mi clase persona
                   
                    //Comienza la validación de la acción
                    if ("INSERT" == values[0]) //Si es "INSERT" insertará en la List para el json
                    {
                        a++;//Sumo en 1 el contador cuando se inserte un elemento a la lista

                        for(int i=0; i < personaN.companies.Length; i++)
                     {
                        personaN.companies[i] = personaN.companies[i].Replace(" ", "_");
                        personaN.companies[i]=LZ_Comprimir(personaN.listaCOmp, personaN.dpi+personaN.companies[i]);

                    }


                    listaJSon.Add_Lista(personaN.name, personaN.dpi, personaN.datebirth, personaN.address,personaN.companies, personaN);//Llamada a añadir a la lista
                    }
                    else if ("PATCH" == values[0])//Si es "Patch" actualizará en la lista
                    {
                        //Sumo en 1 el contador cuando se actualice un elemento a la lista
                        c++;

                    for (int i = 0; i < personaN.companies.Length; i++)
                    {
                       personaN.companies[i] = personaN.companies[i].Replace(" ","_");
                        personaN.companies[i] = LZ_Comprimir(personaN.listaCOmp, personaN.dpi + personaN.companies[i]);
                    }
                    listaJSon.EditItem(personaN.name, personaN.dpi, personaN.datebirth, personaN.address,personaN.companies, personaN);//Lamada a editar
                    }
                    else if ("DELETE" == values[0])//Si es "DELETE" eliminará el elemento deseado en la lista
                    {
                        b++;//Sumo en 1 el contador cuando se elimine un elemento en la lista
                        listaJSon.delete(personaN.name, personaN.dpi);//Llamada a eliminar
                    }
                    else
                    {   //Corregí el mensaje*
                        Console.WriteLine("No se realizó ninguna acción.");//Si no se encontró una accion, imprime el mensaje 
                    }


                }

                //Imprimo los resultados de los contadores
                Console.WriteLine("****Se realizó la lectura del archivo correctamente***");
                Console.WriteLine("Se realizaron: " + a + " inserciones.");
                Console.WriteLine("Se realizaron: " + b + " eliminaciones.");
                Console.WriteLine("Se realizaron: " + c + " actualizaciones.");
                //Dependiendo de los contadores, será el tamaño de mi lista
                int nodosFinales = a - b;//Guardo la diferencia entre las inserciones y las eliminaciones.

                //Variable que guarda la ubicación del archivo de salida
                //************* SE DEBE DE INSERTAR LA DIRECCIÓN DEL ARCHIVO***************
                string LugarArchivoSalida = @"C:\Users\Roberto Moya\Desktop\Lab1-E2\Lab_01_1273020\Lab_01_1273020\bin\Debug\DocSalida.csv";


                //Inicializo variables
                int llave = 0;
                string nombreBus = "";//Variable de Nombre para realizar la busqueda
                 string dpiBus = "";//Variable de dpi para realizar la busqueda
                //Menú
                while (true)
                {
                    Console.WriteLine("\n\n****  Menú  ***");
                    Console.WriteLine("1) Buscar registros por persona.");
                    Console.WriteLine("2) Mostrar la lista completa.");
                    Console.WriteLine("3) Busqueda por DPI. (Datos no sensibles)");
                    Console.WriteLine("4) Busqueda por DPI. (Datos sensibles)");
                    Console.WriteLine("5) Salir.");
                    llave = Convert.ToInt16(Console.ReadLine());
                    //Leo la llave y me dirijo a la acción que quiera realizar
                    if (llave == 1)
                    {
                        int varaux = 0;
                        //Busqueda
                        Console.WriteLine("Ingrese el nombre:");
                        nombreBus = Console.ReadLine();

                        for (int i = 0; i < nodosFinales; i++)//Recorro mi lista
                        {

                            if (listaJSon.Get(i).name == nombreBus)
                            {
                                varaux++;//Incremento mi auxiliar si encontró la persona
                                         //Escribo en consola la paersona buscada
                                Console.WriteLine(i + "\t name: " + listaJSon.Get(i).name + "\t dpi: " + listaJSon.Get(i).dpi + "\t dateBirth: " + listaJSon.Get(i).datebirth + "\t address: " + listaJSon.Get(i).address);
                                foreach (var s in listaJSon.Get(i).companies)
                                {
                                    Console.WriteLine(LZ_Descomprimir(listaJSon.Get(i).listaCOmp,s));
                                }
                                string jsonSalida = JsonSerializer.Serialize(listaJSon.Get(i));//Vuelvo a serializarlo en un jSon
                                File.AppendAllText(LugarArchivoSalida, "\n" + jsonSalida);//Se realiza la escritura de salida en otro archivo
                            }

                        }
                        if (varaux == 0)//si no se encontró, mi auxiliar es 0 e imprimo el siguiente mensaje
                        {
                            Console.WriteLine("No se encontró.");
                        }


                    }
                    else if (llave == 2)
                    {
                        for (int i = 0; i < nodosFinales; i++)//Imprimo todos las personas ingresadas en la lista
                        {
                            Console.WriteLine(i + "\t name: " + listaJSon.Get(i).name + "\t dpi: " + listaJSon.Get(i).dpi + "\t dateBirth: " + listaJSon.Get(i).datebirth + "\t address: " + listaJSon.Get(i).address);
                            Console.WriteLine("\tcompanies: ");
                        foreach (var s in listaJSon.Get(i).companies)
                        {
                          Console.WriteLine(LZ_Descomprimir(listaJSon.Get(i).listaCOmp, s));
                           // Console.WriteLine(s);
                        }
                    }

                    }
                    else if (llave == 4)
                    {
                        Console.WriteLine("Ingrese el DPI a buscar.");
                        dpiBus=Console.ReadLine();
                        int varaux = 0;
                        for (int i = 0; i < nodosFinales; i++)//Recorro mi lista
                        {

                            if (listaJSon.Get(i).dpi == dpiBus)
                            {
                                varaux++;//Incremento mi auxiliar si encontró la persona
                                         //Escribo en consola la paersona buscada
                                Console.WriteLine(i + "\t name: " + listaJSon.Get(i).name + "\t dpi: "+ listaJSon.Get(i).dpi);

                                foreach (var s in listaJSon.Get(i).companies)
                                {
                                    
                                    Console.WriteLine(s);
                                }


                        }

                        }
                        if (varaux == 0)//si no se encontró, mi auxiliar es 0 e imprimo el siguiente mensaje
                        {
                            Console.WriteLine("No se encontró.");
                        }
                        int llave2=0;
                    //*********Codificar o decodificar
                    while (true)
                    {
                        Console.WriteLine("\n\n1) Mostrar los datos codificados");
                        Console.WriteLine("2) Mostrar los datos decodificados");
                        Console.WriteLine("3) Salir al menú.");
                        llave2 = Convert.ToInt32(Console.ReadLine());


                        if (llave2 == 1)
                        {
                            for (int i = 0; i < nodosFinales; i++)//Recorro mi lista
                            {

                                if (listaJSon.Get(i).dpi == dpiBus)
                                {
                                    varaux++;//Incremento mi auxiliar si encontró la persona
                                             //Escribo en consola la paersona buscada
                                    Console.WriteLine(i + "\t name: " + listaJSon.Get(i).name );

                                    foreach (var s in listaJSon.Get(i).companies)
                                    {

                                        Console.WriteLine(s);
                                    }


                                }
                            }

                        }
                        else if (llave2 == 2)
                        {
                            for (int i = 0; i < nodosFinales; i++)//Recorro mi lista
                            {

                                if (listaJSon.Get(i).dpi == dpiBus)
                                {
                                    varaux++;//Incremento mi auxiliar si encontró la persona
                                             //Escribo en consola la paersona buscada
                                    Console.WriteLine(i + "\t name: " + listaJSon.Get(i).name );

                                    foreach (var s in listaJSon.Get(i).companies)
                                    {
                                        string txtAux = "";
                                        txtAux =LZ_Descomprimir(listaJSon.Get(i).listaCOmp, s);
                                        string txtDpi = "";
                                        string txtComp = "";
                                        for(int j = 0; j < txtAux.Length; j++)
                                        {
                                            if (j < 13)
                                            {
                                                txtDpi+= txtAux[j];
                                            }
                                            else
                                            {
                                                txtComp+=txtAux[j];
                                            }
                                        }

                                        Console.WriteLine("dpi: "+txtDpi+"\tcompañía: "+txtComp);
                                    }


                                }
                            }
                        }
                        else if(llave2 == 3)
                        {
                            break;
                        }

                    }
                }
                else if (llave == 3)
                {
                    Console.WriteLine("Ingrese el DPI a buscar.");
                    dpiBus = Console.ReadLine();
                    int varaux = 0;
                    for (int i = 0; i < nodosFinales; i++)//Recorro mi lista
                    {

                        if (listaJSon.Get(i).dpi == dpiBus)
                        {
                            varaux++;//Incremento mi auxiliar si encontró la persona
                                     //Escribo en consola la paersona buscada
                            Console.WriteLine(i + "\t name: " + listaJSon.Get(i).name + "\t dateBirth: " + listaJSon.Get(i).datebirth + "\t address: " + listaJSon.Get(i).address);

                        }

                    }
                    if (varaux == 0)//si no se encontró, mi auxiliar es 0 e imprimo el siguiente mensaje
                    {
                        Console.WriteLine("No se encontró.");
                    }

                }
                else if (llave == 5)
                    {
                        Environment.Exit(0);//Sale de la consola
                    }

            }








                //Console.ReadKey();
            }
        }
    }

