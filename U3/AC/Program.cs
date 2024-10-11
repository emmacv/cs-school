using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Nucleotido
{
    // Diccionario que contiene la información de los nucleótidos
    private static Dictionary<string, string[]> _nucleotidos = new Dictionary<string, string[]>()
    {
        {"A", new [] {"Base purina", "Timina (T) en ADN, Uracilo (U) en ARN", "Forma dos enlaces de hidrógeno con Timina o Uracilo. Esencial para la formación de ATP (adenosina trifosfato)."}},
        {"T", new [] {"Base pirimidina", "Adenina (A)", "Exclusiva del ADN. Forma dos enlaces de hidrógeno con Adenina. Importante para la estabilidad estructural del ADN."}},
        {"C", new [] {"Base pirimidina", "Guanina (G)", "Forma tres enlaces de hidrógeno con Guanina. Participa en la regulación de genes y en la estabilidad del ADN."}},
        {"G", new [] {"Base purina", "Citosina (C)", "Forma tres enlaces de hidrógeno con Citosina. Importante en la síntesis de proteínas y en la formación de estructuras secundarias del ARN."}},
        {"U", new [] {"Base pirimidina", "Adenina (A)", "Exclusiva del ARN. Forma dos enlaces de hidrógeno con Adenina. Permite la formación de estructuras secundarias y terciarias del ARN."}},
        {"I", new [] {"Base purina", "Varía", "Se encuentra en el ARN de transferencia (tRNA) y puede emparejarse con adenina, citosina o uracilo."}},
        {"5mC", new [] {"Base pirimidina", "Guanina (G)", "Variante de citosina con un grupo metilo añadido. Participa en la regulación epigenética y en la inactivación del cromosoma X."}},
        {"H", new [] {"Base purina", "Varía", "Derivada de la adenina por desaminación. Se encuentra en el tRNA y puede emparejarse con varios nucleótidos."}},
        {"m7G", new [] {"Base purina", "Varía", "Encontrada en la caperuza del ARN mensajero. Protege al mRNA de la degradación y ayuda en la iniciación de la traducción."}}
    };

    // Propiedades de la clase
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string ParBase { get; set; }
    public string Propiedades { get; set; }

    // Constructor que asigna las propiedades con base en el diccionario
    public Nucleotido(string nombre)
    {
        if (!_nucleotidos.ContainsKey(nombre))
        {
            throw new ArgumentException("El nucleótido no existe: " + nombre);
        }

        Nombre = nombre;
        Tipo = _nucleotidos[nombre][0];
        ParBase = _nucleotidos[nombre][1];
        Propiedades = _nucleotidos[nombre][2];
    }

    // Método para obtener la descripción completa del nucleótido
    public static string ObtenerDescripcion(string nombre)
    {
        if (!_nucleotidos.ContainsKey(nombre))
        {
            throw new ArgumentException("El nucleótido no existe: " + nombre);
        }

        var info = _nucleotidos[nombre];
        return $"Tipo: {info[0]}, Par de bases: {info[1]}, Propiedades: {info[2]}";
    }
}

class ProgramaADN
{

    static string secuencia = "";
    static Dictionary<string, int> conteo = null;

    static int pirinas = 0;

    static int pirimidinas = 0;


    // Método para contar los nucleótidos de la secuencia
    static Dictionary<string, int> ContarNucleotidos(string secuencia)
    {
        var pattern = "(A|T|C|G|U|I|H|m7G)";
        Regex regex = new(pattern);
        conteo = new Dictionary<string, int> { { "A", 0 }, { "T", 0 }, { "C", 0 }, { "G", 0 }, { "U", 0 }, { "I", 0 }, { "H", 0 }, { "m7G", 0 } };

        var matches = Regex.Matches(secuencia, pattern, RegexOptions.IgnoreCase);

        foreach (var match in matches)
            conteo[match.ToString()]++;

        return conteo;
    }

    // Método para analizar la secuencia de ADN
    static void AnalizarSecuencia()
    {
        if (string.IsNullOrEmpty(secuencia))
        {
            Console.WriteLine("No hay resultados. Primero debe ingresar y analizar una secuencia.");

            return;
        }

        conteo = ContarNucleotidos(secuencia);

        // Contar bases
        pirinas = conteo["A"] + conteo["G"];
        pirimidinas = conteo["T"] + conteo["C"];

        Console.WriteLine($"Secuencia analizada.");
    }

    // Método para mostrar los resultados del análisis
    static void VerResultados()
    {
        int totalNucleotidos = secuencia.Length;

        Console.WriteLine("Resultados del Análisis:");
        Console.WriteLine($"Secuencia: {secuencia}");
        Console.WriteLine($"Total de Nucleótidos: {totalNucleotidos}");

        foreach (var par in conteo)
        {
            double proporcion = (double)par.Value / totalNucleotidos * 100;
            Console.WriteLine($"{par.Key}: {par.Value}/{totalNucleotidos} ({proporcion:F2}%)");
        }

        // Buscar patrones
        if (secuencia.Contains("TATA"))
        {
            Console.WriteLine("Región Promotora (TATA) encontrada.");
        }
        if (secuencia.Contains("CACGTG"))
        {
            Console.WriteLine("Sitio de Unión a Proteínas (CACGTG) encontrado.");
        }
    }

    // Método principal
    static void Main()
    {
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("Menú:");
            Console.WriteLine("1. Agregar secuencia de ADN");
            Console.WriteLine("2. Analizar secuencia");
            Console.WriteLine("3. Ver resultados");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Ingrese la secuencia de ADN: ");
                    secuencia = Console.ReadLine();
                    break;

                case "2":


                    AnalizarSecuencia();
                    break;

                case "3":

                    VerResultados();
                    break;

                case "4":
                    salir = true;
                    break;

                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }
}
