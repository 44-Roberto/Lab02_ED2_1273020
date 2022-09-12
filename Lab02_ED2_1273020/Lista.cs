using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02_ED2_1273020
{
    class Lista<T> //where T : IComparable<T>
    {
        //Creo mi nodo  
        public class nodo : Persona//Heredo de mi clase persona
        {



            public T valor;
            public nodo siguiente;



        }

        //declaro mi primer Nodo "Head"

        nodo Header = new nodo();


        public Lista()//Instancio mi Head como null
        {
            Header = null;

        }

        //Metodo para añadir a la lista
        public void Add_Lista(string Name, string Dpi, string BD, string Address, T value)
        {
            //Instancio variables auxiliares
            nodo nuevoNodo = new nodo();
            nodo Temp = Header;

            //Guardo los valores en mi nodo
            nuevoNodo.valor = value;
            nuevoNodo.name = Name;
            nuevoNodo.dpi = Dpi;
            nuevoNodo.address = Address;
            nuevoNodo.datebirth = BD;

            //Añado el nodo a la lista
            if (Header == null)
            {
                Header = nuevoNodo;

                return;
            }
            else
            {
                while (Temp.siguiente != null)
                {
                    Temp = Temp.siguiente;
                }
                Temp.siguiente = nuevoNodo;
                return;
            }

        }

        //Metodo para actualizar los datos
        public void EditItem(string Name, string Dpi, string BD, string Address, T value)
        {
            nodo Temp = Header;

            while (Temp.siguiente != null)//Se recorre el nodo
            {
                if ((Temp.name == Name) && (Temp.dpi == Dpi))//Si el nombre y el dpi coinciden
                {
                    Temp.valor = value;
                    Temp.address = Address;
                    Temp.datebirth = BD;

                    return;
                }
                else
                {
                    Temp = Temp.siguiente;

                }
            }


        }

        public T Get(int pos)//Metodo que me permite obtener el T en una posición especifica de la lista
        {
            nodo Temp = Header;
            int auxPos = 0;
            while (Temp.siguiente != null)
            {
                if (pos == auxPos)
                {
                    return Temp.valor;
                }
                else
                {
                    Temp = Temp.siguiente;
                    auxPos++;
                }
            }

            return Temp.valor;
        }


        //No se utiliza
        public T GetItem(string Name)//Metodo que me devuelve el T por medio del nombre
        {
            nodo Temp = Header;

            while (Temp.siguiente != null)
            {
                if (Temp.name == Name)
                {
                    return Temp.valor;

                }
                else
                {
                    Temp = Temp.siguiente;

                }
            }

            return Temp.valor;
        }



        //Metodo de eliminacion por medio de la busqueda por nombre y dpi
        public void delete(string Name, string Dpi)
        {

            nodo Temp = Header;
            nodo aux = new nodo();
            Persona personaaux = new Persona();

            while (Temp != null)
            {
                if ((Temp.name == Name) && (Temp.dpi == Dpi))//Si el nombre y dpi coinciden
                {
                    aux.siguiente = Temp.siguiente;//Se deja fuera el nodo
                    return;
                }
                else
                {
                    aux = Temp;
                    Temp = Temp.siguiente;
                }
            }



        }


    }
}
