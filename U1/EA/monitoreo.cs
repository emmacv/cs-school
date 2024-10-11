using System;
using Geolocation;

namespace MonitoreoAves
{
    // Clase para almacenar los datos de monitoreo de un día
    public class RegistroMonitoreo
    {
        public double LatitudInicio { get; set; }
        public double LongitudInicio { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public double LatitudFin { get; set; }
        public double LongitudFin { get; set; }
        public DateTime FechaHoraFin { get; set; }

        // Constructor para inicializar un registro
        public RegistroMonitoreo(double latInicio, double lonInicio, DateTime fechaInicio, double latFin, double lonFin, DateTime fechaFin)
        {
            LatitudInicio = latInicio;
            LongitudInicio = lonInicio;
            FechaHoraInicio = fechaInicio;
            LatitudFin = latFin;
            LongitudFin = lonFin;
            FechaHoraFin = fechaFin;
        }
    }

    // Clase para el programa de monitoreo
    public class ProgramaMonitoreo
    {
        // Atributos para los registros de los 3 días
        private RegistroMonitoreo dia1;
        private RegistroMonitoreo dia2;
        private RegistroMonitoreo dia3;

        // Función para agregar datos de monitoreo
        public void AgregarDatos(int dia, double latInicio, double lonInicio, DateTime fechaInicio, double latFin, double lonFin, DateTime fechaFin)
        {
            if (dia == 1)
            {
                dia1 = new RegistroMonitoreo(latInicio, lonInicio, fechaInicio, latFin, lonFin, fechaFin);
            }
            else if (dia == 2)
            {
                dia2 = new RegistroMonitoreo(latInicio, lonInicio, fechaInicio, latFin, lonFin, fechaFin);
            }
            else if (dia == 3)
            {
                dia3 = new RegistroMonitoreo(latInicio, lonInicio, fechaInicio, latFin, lonFin, fechaFin);
            }
        }

        // Función para calcular la distancia y dirección entre dos coordenadas
        private (double distancia, string direccion) CalcularDistanciaYDireccion(double latInicio, double lonInicio, double latFin, double lonFin)
        {
            var inicio = new Coordinate(latInicio, lonInicio);
            var fin = new Coordinate(latFin, lonFin);
            var distancia = GeoCalculator.GetDistance(inicio, fin, DistanceUnit.Kilometers);
            var direccion = GeoCalculator.GetDirection(inicio, fin);

            return (distancia, direccion.ToString());
        }

        // Función para calcular la distancia total y la velocidad promedio
        public void GenerarInforme()
        {
            double distanciaTotal = 0;
            double tiempoTotalHoras = 0;
            double menorDistancia = double.MaxValue;
            int diaMenorDistancia = 0;

            // Día 1
            var (distancia1, direccion1) = CalcularDistanciaYDireccion(dia1.LatitudInicio, dia1.LongitudInicio, dia1.LatitudFin, dia1.LongitudFin);
            var tiempoHoras1 = (dia1.FechaHoraFin - dia1.FechaHoraInicio).TotalHours;
            distanciaTotal += distancia1;
            tiempoTotalHoras += tiempoHoras1;
            if (distancia1 < menorDistancia)
            {
                menorDistancia = distancia1;
                diaMenorDistancia = 1;
            }

            Console.WriteLine($"Día 1:");
            Console.WriteLine($"  Distancia: {distancia1} km");
            Console.WriteLine($"  Dirección: {direccion1}");
            Console.WriteLine();

            // Día 2
            var (distancia2, direccion2) = CalcularDistanciaYDireccion(dia2.LatitudInicio, dia2.LongitudInicio, dia2.LatitudFin, dia2.LongitudFin);
            var tiempoHoras2 = (dia2.FechaHoraFin - dia2.FechaHoraInicio).TotalHours;
            distanciaTotal += distancia2;
            tiempoTotalHoras += tiempoHoras2;
            if (distancia2 < menorDistancia)
            {
                menorDistancia = distancia2;
                diaMenorDistancia = 2;
            }

            Console.WriteLine($"Día 2:");
            Console.WriteLine($"  Distancia: {distancia2} km");
            Console.WriteLine($"  Dirección: {direccion2}");
            Console.WriteLine();

            // Día 3
            var (distancia3, direccion3) = CalcularDistanciaYDireccion(dia3.LatitudInicio, dia3.LongitudInicio, dia3.LatitudFin, dia3.LongitudFin);
            var tiempoHoras3 = (dia3.FechaHoraFin - dia3.FechaHoraInicio).TotalHours;
            distanciaTotal += distancia3;
            tiempoTotalHoras += tiempoHoras3;
            if (distancia3 < menorDistancia)
            {
                menorDistancia = distancia3;
                diaMenorDistancia = 3;
            }

            Console.WriteLine($"Día 3:");
            Console.WriteLine($"  Distancia: {distancia3} km");
            Console.WriteLine($"  Dirección: {direccion3}");
            Console.WriteLine();

            double velocidadPromedio = distanciaTotal / tiempoTotalHoras;

            Console.WriteLine("Informe de Monitoreo:");
            Console.WriteLine($"  Distancia total recorrida: {distanciaTotal} km");
            Console.WriteLine($"  Velocidad promedio de vuelo: {velocidadPromedio} km/h");
            Console.WriteLine($"  Día de menor distancia recorrida: Día {diaMenorDistancia}");
        }

        static void Main(string[] args)
        {
            var programa = new ProgramaMonitoreo();

            // Ejemplo de ingreso de datos
            programa.AgregarDatos(1, 10.0, -84.0, new DateTime(2024, 7, 21, 6, 0, 0), 10.5, -84.5, new DateTime(2024, 7, 21, 18, 0, 0));
            programa.AgregarDatos(2, 10.5, -84.5, new DateTime(2024, 7, 22, 6, 0, 0), 11.0, -85.0, new DateTime(2024, 7, 22, 18, 0, 0));
            programa.AgregarDatos(3, 11.0, -85.0, new DateTime(2024, 7, 23, 6, 0, 0), 11.5, -85.5, new DateTime(2024, 7, 23, 18, 0, 0));

            // Generar el informe
            programa.GenerarInforme();
        }
    }
}
