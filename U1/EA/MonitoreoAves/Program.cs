using System;
using Geolocation;

namespace MonitoreoAves {
  // Clase para almacenar los datos de monitoreo de un día
  public class RegistroMonitoreo {
    // Propiedades para almacenar las coordenadas y tiempos de inicio y fin del recorrido
    public double LatitudInicio { get; set; }
    public double LongitudInicio { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public double LatitudFin { get; set; }
    public double LongitudFin { get; set; }
    public DateTime FechaHoraFin { get; set; }

    // Constructor para inicializar un registro con las coordenadas y tiempos dados
    public RegistroMonitoreo(double latInicio, double lonInicio, DateTime fechaInicio, double latFin, double lonFin, DateTime fechaFin) {
      LatitudInicio = latInicio;
      LongitudInicio = lonInicio;
      FechaHoraInicio = fechaInicio;
      LatitudFin = latFin;
      LongitudFin = lonFin;
      FechaHoraFin = fechaFin;
    }
  }

  // Clase principal para el programa de monitoreo
  public class ProgramaMonitoreo {
    // Atributos para los registros de los 3 días
    private RegistroMonitoreo dia1;
    private RegistroMonitoreo dia2;
    private RegistroMonitoreo dia3;

    // Función para agregar datos de monitoreo por día
    public void AgregarDatos(int dia, double latInicio, double lonInicio, DateTime fechaInicio, double latFin, double lonFin, DateTime fechaFin) {
      var newDia = new RegistroMonitoreo(latInicio, lonInicio, fechaInicio, latFin, lonFin, fechaFin);

      switch (dia) {
        case 1:
          dia1 = newDia;
          break;
        case 2:
          dia2 = newDia;
          break;
        case 3:
          dia3 = newDia;
          break;
      }
    }

    // Función para calcular la distancia y dirección cardinal entre dos coordenadas dadas
    private (double distancia, string direccion) CalcularDistanciaYDireccion(double latInicio, double lonInicio, double latFin, double lonFin) {
      var inicio = new Coordinate { Latitude = latInicio, Longitude = lonInicio };
      var fin = new Coordinate { Latitude = latFin, Longitude = lonFin };
      var distancia = GeoCalculator.GetDistance(inicio, fin, 1, DistanceUnit.Kilometers);
      var direccion = GeoCalculator.GetDirection(inicio, fin);

      return (distancia, direccion.ToString());
    }

    // Función para generar un informe con la distancia total, velocidad promedio y día de menor distancia recorrida
    public void GenerarInforme() {
      double distanciaTotal = 0;
      double tiempoTotalHoras = 0;
      double menorDistancia = double.MaxValue;
      int diaMenorDistancia = 0;

      // Día 1
      var (distancia1, direccion1) = CalcularDistanciaYDireccion(dia1.LatitudInicio, dia1.LongitudInicio, dia1.LatitudFin, dia1.LongitudFin);
      var tiempoHoras1 = (dia1.FechaHoraFin - dia1.FechaHoraInicio).TotalHours;
      distanciaTotal += distancia1;
      tiempoTotalHoras += tiempoHoras1;
      if (distancia1 < menorDistancia) {
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
      if (distancia2 < menorDistancia) {
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
      if (distancia3 < menorDistancia) {
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

      PredecirUbicacionCuartoDia();
    }

    // Función para calcular la pendiente de una línea recta
    private static double CalcularPendiente(RegistroMonitoreo punto1, RegistroMonitoreo punto2) =>
      (punto2.LatitudFin - punto1.LatitudFin) / (punto2.LongitudFin - punto1.LongitudFin);

  // Función para calcular la intersección en Y de una línea recta
    private static double CalcularInterseccionY(double pendiente, RegistroMonitoreo punto) =>
      punto.LatitudFin - (pendiente * punto.LongitudFin);

    // Función para predecir la ubicación de la ave en el cuarto día
    private void PredecirUbicacionCuartoDia() {
      double pendiente1 = CalcularPendiente(dia1, dia2);
      double interseccionY1 = CalcularInterseccionY(pendiente1, dia1);
      double pendiente2 = CalcularPendiente(dia2, dia3);
      double interseccionY2 = CalcularInterseccionY(pendiente2, dia2);
      // Calcular las pendientes y la intersección Y promedios
      double pendientePromedio =
      (pendiente1 + pendiente2) / 2;
      double interseccionYPromedio =
      (interseccionY1 + interseccionY2) / 2;
      // Utilizar los valores promedio para predecir la próxima ubicación
      double nuevaLongitud = dia3.LongitudFin + (dia3.LongitudFin - dia2.LongitudFin);
      double nuevaLatitud =
      (pendientePromedio * nuevaLongitud) + interseccionYPromedio;
      // Mostrar la predicción de la nueva ubicación
      Console.WriteLine("Predicción de la ubicación para el cuarto día:");
      Console.WriteLine("Latitud: " + nuevaLatitud);
      Console.WriteLine("Longitud: " + nuevaLongitud);
    }

    // Función para leer los datos del usuario desde la consola
    private void LeerDatosUsuario() {
      for (int i = 1; i <= 3; i++) {
        Console.WriteLine($"Ingrese los datos para el día {i}:");

        Console.Write("Latitud de inicio: ");
        double latInicio = Convert.ToDouble(Console.ReadLine());

        Console.Write("Longitud de inicio: ");
        double lonInicio = Convert.ToDouble(Console.ReadLine());

        Console.Write("Fecha y hora de inicio (yyyy-MM-dd HH:mm:ss): ");
        DateTime fechaHoraInicio = DateTime.Parse(Console.ReadLine());

        Console.Write("Latitud de fin: ");
        double latFin = Convert.ToDouble(Console.ReadLine());

        Console.Write("Longitud de fin: ");
        double lonFin = Convert.ToDouble(Console.ReadLine());

        Console.Write("Fecha y hora de fin (yyyy-MM-dd HH:mm:ss): ");
        DateTime fechaHoraFin = DateTime.Parse(Console.ReadLine());

        AgregarDatos(i, latInicio, lonInicio, fechaHoraInicio, latFin, lonFin, fechaHoraFin);

        Console.WriteLine();
      }
    }

    // Método principal del programa
    static void Main(string[] args) {
      var programa = new ProgramaMonitoreo();

      programa.LeerDatosUsuario();
      programa.GenerarInforme();
    }
  }
}
