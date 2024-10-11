using System;

class Programa
{
    // Datos globales
    private static int frecuenciaCardiaca;
    private static int nivelOxigeno;
    private static double porcentajeGrasaCorporal;
    private static double porcentajeMasaMuscular;
    private static double porcentajeOtrosComponentes;

    static void Main()
    {
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("Menú:");
            Console.WriteLine("1. Recopilar datos");
            Console.WriteLine("2. Analizar datos");
            Console.WriteLine("3. Salir");
            Console.Write("Seleccione una opción (1-3): ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
            bool falla = false;

                    do
                    {
                        try
                        {
                            RecopilarDatosVitales();
                            RecopilarDatosComposicionCorporal();
                        }
                        catch (System.Exception)
                        {
                            falla = true;
                            Console.WriteLine("Error: Los datos ingresados no son válidos.");
                        }
                    } while (falla);

                    break;

                case "2":
                    double indiceRendimiento = CalcularIndiceRendimiento();
                    bool condicionesOptimas = DeterminarCondicionesOptimas();
                    Console.WriteLine($"Índice de Rendimiento: {indiceRendimiento}");
                    Console.WriteLine($"Condiciones óptimas para competir: {(condicionesOptimas ? "Sí" : "No")}");
                    break;

                case "3":
                    salir = true;
                    break;

                default:
                    Console.WriteLine("Opción no válida. Inténtelo de nuevo.");
                    break;
            }
        }
    }

    // Método para recopilar datos vitales
    static void RecopilarDatosVitales()
    {

        Console.Write("Ingrese la frecuencia cardíaca (bpm): ");
        frecuenciaCardiaca = int.Parse(Console.ReadLine());

        Console.Write("Ingrese el nivel de oxígeno en la sangre (%): ");
        nivelOxigeno = int.Parse(Console.ReadLine());

    }

    // Método para recopilar datos de composición corporal
    static void RecopilarDatosComposicionCorporal()
    {
        Console.Write("Ingrese el porcentaje de grasa corporal (%): ");
        porcentajeGrasaCorporal = double.Parse(Console.ReadLine());

        Console.Write("Ingrese el porcentaje de masa muscular (%): ");
        porcentajeMasaMuscular = double.Parse(Console.ReadLine());

        Console.Write("Ingrese el porcentaje de otros componentes (%): ");
        porcentajeOtrosComponentes = double.Parse(Console.ReadLine());
    }

    // Método para calcular el índice de rendimiento
    static double CalcularIndiceRendimiento()
    {
        if (frecuenciaCardiaca <= 0 || nivelOxigeno < 0 || porcentajeGrasaCorporal < 0 || porcentajeMasaMuscular < 0 || porcentajeOtrosComponentes < 0)
        {
            throw new InvalidOperationException("Datos insuficientes para el cálculo.");
        }

        // Normalizar la composición corporal
        double sumaPorcentajes = porcentajeGrasaCorporal + porcentajeMasaMuscular + porcentajeOtrosComponentes;
        if (Math.Abs(sumaPorcentajes - 100) > 0.01)
        {
            throw new InvalidOperationException("La suma de los porcentajes de composición corporal debe ser igual a 100.");
        }

        // Calcular el factor de corrección
        double factorCorrecion = 1 + (porcentajeGrasaCorporal * 0.1) + (porcentajeMasaMuscular * 0.05) + (porcentajeOtrosComponentes * 0.07);

        // Calcular el índice de rendimiento
        double indiceRendimiento = (nivelOxigeno * 100) / (frecuenciaCardiaca * factorCorrecion);

        return indiceRendimiento;
    }

    // Método para determinar si el atleta está en condiciones óptimas
    static bool DeterminarCondicionesOptimas()
    {
        if (frecuenciaCardiaca <= 0 || nivelOxigeno < 0)
        {
            throw new InvalidOperationException("Datos insuficientes para la identificación.");
        }

        // Definir umbral de condiciones óptimas
        double umbralCondicionesOptimas = 90;

        // Calcular el factor de condición
        double factorCondicion = (nivelOxigeno * 0.6) - (frecuenciaCardiaca * 0.4);

        return factorCondicion > umbralCondicionesOptimas;
    }
}
