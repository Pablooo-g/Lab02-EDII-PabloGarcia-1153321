using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Lab02;

public class Program
{
    public static void Main()
    {
        try
        {
            ArithmeticCoding operando = new ArithmeticCoding();
            AVLTree<Persona> arbolPersonas = new AVLTree<Persona>();            
            string route = @"C:\EDII\input.csv";

            if (File.Exists(route))
            {
                string[] FileData = File.ReadAllLines(route);
                foreach (var item in FileData)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] fila = item.Split(";");
                        Persona? persona = JsonSerializer.Deserialize<Persona>(fila[1]);
                        List<Dictionary<char, Letra>> lista = new List<Dictionary<char, Letra>>();
                        Array.Sort(persona!.companies, (x, y) => String.Compare(x, y));
                        List<double> codigos = new List<double>();
                        for (int i = 0; i < persona.companies.Length; i++)
                        {
                            Dictionary<char, Letra> dictionario = new Dictionary<char, Letra>();
                            string codigo = persona.dpi + i.ToString(); 
                            foreach (var c in codigo) 
                            {
                                Letra nueva = new Letra();
                                if (!dictionario.TryAdd(c, nueva))
                                {

                                }
                                dictionario[c].frecuencia++;
                            }

                            double inf = 0, sup = 0, p = 0;
                            foreach (var c in dictionario)
                            {
                                p = (double)c.Value.frecuencia / codigo.Length;
                                sup = inf + p;
                                dictionario[c.Key].probabilidad = p;
                                dictionario[c.Key].inferior = inf;
                                dictionario[c.Key].superior = sup;
                                inf = sup;
                            }
                            
                            double newCode = operando.Coding(codigo, dictionario);
                            codigos.Add(newCode);
                            lista.Add(dictionario);
                        }
                        persona.dictionaries = lista;
                        persona.codes = codigos;
                        if (fila[0] == "INSERT")
                        {
                            arbolPersonas.Add(persona!, Delegates.DPIComparison);
                        }
                        else if (fila[0] == "DELETE")
                        {
                            arbolPersonas.Delete(persona!, Delegates.DPIComparison);
                        }
                        else if (fila[0] == "PATCH")
                        {
                            arbolPersonas.Patch(persona!, Delegates.DPIComparison);
                        }
                    }
                }
            }

            Console.WriteLine("Ingrese el dpi de la persona que quiere buscar: ");
            string? dpi = Console.ReadLine();
            Persona persona1 = new Persona();
            persona1.dpi = dpi!;
            Persona resultado = arbolPersonas.Search(persona1, Delegates.DPIComparison);
            if (resultado != null)
            {
                string datos = "name: " + resultado.name
                    + "\n" + "dpi: " + resultado.dpi
                    + "\n" + "datebirth: " + resultado.datebirth
                    + "\n" + "address: " + resultado.address
                    + "\n" + "companies: {" ;

                for (int i = 0; i < resultado.companies.Length; i++)
                {
                    datos += "\n" +"  " + resultado.companies[i] + ": " + resultado.codes[i].ToString();
                }
                datos += "\n}";
                Console.WriteLine(datos);
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Escoja la operación que quiere realizar"
                    + "\n" + "1. Exportar datos de la persona"
                    + "\n" + "2. Decodificar DPI");
                int opcion = Convert.ToInt32(Console.ReadLine());
                switch (opcion)
                {
                    case 1:                        
                        string output = @"C:\EDII\input.csv" + resultado.name + ".txt";
                        File.WriteAllText(output, datos);
                        break;
                    case 2:
                        Console.WriteLine("Ingrese el nombre de la empresa");
                        string empresa = Console.ReadLine()!;
                        if (empresa != null)
                        {
                            int index = Array.BinarySearch(resultado.companies, empresa);
                            if (index < 0)
                            {
                                throw new Exception("La empresa ingresada no existe");
                            }
                            double code = resultado.codes[index];
                            Dictionary<char, Letra> dic1 = resultado.dictionaries[index];                            
                            string resul = operando.Decode(code, dic1);
                            Console.WriteLine("El DPI de la persona es: " + resultado.dpi);
                            Console.WriteLine("El DPI decodificado es:" + resul);                            
                        }
                        break;
                    default:
                        Console.WriteLine("La opción ingresada no es válida");
                        break;
                }

            }


        }
        catch (Exception e)
        {
            Console.WriteLine("Ha ocurrido un error inesperado");
        }
    }
}