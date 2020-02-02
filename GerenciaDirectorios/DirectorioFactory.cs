using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciaDirectorios
{
    public class DirectorioFactory
    {
        private List<string> _lista;
        private List<Directorio> _directorios = new List<Directorio>();
        public DirectorioFactory(List<string> listaEntrada)
        {
            _lista = listaEntrada;
        }

        //LISTA
        public void ImprimeLista()
        {
            foreach (string linea in _lista)
            {
                Console.WriteLine(linea.ToString());
            }
        }

        //DIRECTORIOS
        public void CreaDirectorios()
        {
            Directorio raiz = new Directorio();
            raiz.Padre = null;
            raiz.Nombre = "/";
            raiz.Tamano = 0;
            _directorios.Add(raiz);

            foreach (string linea in _lista)
            {
                TratarLinea(linea);
            }

        }

        private void TratarLinea(string Item)
        {
            string[] delimitadores = { "/", " (", "kb)" };
            string[] elemento = Item.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < elemento.Length; i++)
            {
                Directorio objeto = new Directorio();

                string _parent = "/";
                string _nombre = "";
                int _tamano = 0;

                if (i > 0) _parent = elemento[i - 1];

                if (i < elemento.Length - 1)
                {
                    _nombre = elemento[i];
                    if (i == elemento.Length - 2)
                    {
                        _tamano = Convert.ToInt16(elemento[i+1]);
                    }

                    objeto.Padre = _parent;
                    objeto.Nombre = _nombre;
                    objeto.Tamano = _tamano;

                    if(!_directorios.Exists(x => x.Padre==objeto.Padre && x.Nombre == objeto.Nombre)){
                        _directorios.Add(objeto);
                        if (!_directorios.Exists(x => x.Padre == objeto.Nombre))
                        {
                            ActualizarTamano(objeto, _directorios, objeto.Tamano);
                        }
                    }
                }
            }
        }


        public void ImprimirArbol()
        {
            Directorio raiz = _directorios.Find(r => r.Padre ==null);
            Console.WriteLine(raiz.Nombre + " (" + raiz.Tamano+"kb)");
            ConstruirArbol(raiz, _directorios,0);
        }

        static void ConstruirArbol(Directorio Rama, List<Directorio> _arbol,int _nivel)
        {
            _nivel++;
            List<Directorio> Arbol = _arbol.FindAll(r => r.Padre == Rama.Nombre);
            foreach (Directorio rama in Arbol)
            {
                Console.WriteLine(String.Concat(Enumerable.Repeat("---", _nivel))+" "+ rama.Nombre+"  ("+rama.Tamano + "kb)");
                ConstruirArbol(rama, _arbol, _nivel);
            }
        }
    
        static void ActualizarTamano(Directorio Rama, List<Directorio> _arbol, int _tamano)
        {
            Directorio Papa = _arbol.Find(r => r.Nombre== Rama.Padre);
            if (Papa!=null)
            {
                Papa.Tamano += _tamano;
                ActualizarTamano(Papa, _arbol,_tamano);
            }

        }

    }
}
