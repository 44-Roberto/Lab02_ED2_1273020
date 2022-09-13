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
        }

        internal class Program
        {

            static void Main(string[] args)
            {
                //Comienza el procedimiento de lectura del archivo
                //************* SE DEBE DE INSERTAR LA DIRECCIÓN DEL ARCHIVO***************
                var reader = new StreamReader(File.OpenRead(@"C:\Users\Roberto Moya\Desktop\Lab2-E2\Lab02_ED2_1273020\Pruebita.txt"));
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
                        listaJSon.Add_Lista(personaN.name, personaN.dpi, personaN.datebirth, personaN.address,personaN.companies, personaN);//Llamada a añadir a la lista
                    }
                    else if ("PATCH" == values[0])//Si es "Patch" actualizará en la lista
                    {
                        //Sumo en 1 el contador cuando se actualice un elemento a la lista
                        c++;
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

                //Menú
                while (true)
                {
                    Console.WriteLine("\n\n****  Menú  ***");
                    Console.WriteLine("1) Buscar registros por persona.");
                    Console.WriteLine("2) Imprimir toda la lista.");
                    Console.WriteLine("3) Salir.");
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
                                Console.WriteLine(s);
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
                            Console.WriteLine(s);
                        }
                    }

                    }
                    else if (llave == 3)
                    {
                        Environment.Exit(0);//Sale de la consola
                    }

                }








                //Console.ReadKey();
            }
        }
    }

