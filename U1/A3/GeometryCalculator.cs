using System;

namespace Geometria
{
  using System;
  /// <summary>
  /// Clase utilizada para realizar  para cálculos geométricos.
  /// </summary>
  public class GeometryCalculator
  {
    /// <summary>
    /// Método para calcular el área de un rectángulo.
    /// </summary>
    /// <param name="_base">Base del rectángulo.</param>
    /// <param name="altura">Altura del rectángulo.</param>
    /// <returns>El área del rectángulo.</returns>
    public static double CalcularAreaRectangulo(double _base, double altura) => _base * altura;
    /// <summary>
    /// Método para calcular el volumen de una esfera.
    /// </summary>
    /// <param name="radio">Radio de la esfera.</param>
    /// <returns>El volumen de la esfera.</returns>
    public static double CalcularVolumenEsfera(double radio) => (4.0 / 3.0) * Math.PI * Math.Pow(radio, 3);
    /// <summary>
    /// Método para calcula la diagonal de un rectángulo.
    /// </summary>
    /// <param name="_base">Base del rectángulo.</param>
    /// <param name="altura">Altura del rectángulo.</param>
    /// <returns>La longitud de la diagonal.</returns>
    public static double CalcularDiagonalRectangulo(double _base, double altura) => Math.Sqrt(Math.Pow(_base, 2) + Math.Pow(altura, 2));

    // Agrega más métodos de cálculo según las necesidades del proyecto.
  }

// Clase principal de la aplicación
  class MainClass {

    // Método principal de la aplicación
    static void Main(string[] args) {
      Console.WriteLine("Ingrese la base del rectángulo en centímetros:");
      double baseRectangulo = double.Parse(Console.ReadLine());
      Console.WriteLine("Ingrese la altura del rectángulo en centímetros:");
      double alturaRectangulo = double.Parse(Console.ReadLine());
      Console.WriteLine("Ingrese el radio de la esfera en centímetros:");
      double radioEsfera = double.Parse(Console.ReadLine());
      Console.WriteLine("Ingrese la base del rectángulo en centímetros:");
      double baseRectanguloDiagonal = double.Parse(Console.ReadLine());
      Console.WriteLine("Ingrese la altura del rectángulo en centímetros:");
      double alturaRectanguloDiagonal = double.Parse(Console.ReadLine());
      Console.WriteLine("Ingrese su año de nacimiento:");
      int anioNacimiento = int.Parse(Console.ReadLine());
      double areaRectangulo = GeometryCalculator.CalcularAreaRectangulo(baseRectangulo, alturaRectangulo);
      double volumenEsfera = GeometryCalculator.CalcularVolumenEsfera(radioEsfera);
      double diagonalRectangulo = GeometryCalculator.CalcularDiagonalRectangulo(baseRectanguloDiagonal, alturaRectanguloDiagonal);

      int anioActual = DateTime.Now.Year;
      int edad = anioActual - anioNacimiento;

      Console.WriteLine($"El área del rectángulo es: {areaRectangulo} cm²");
      Console.WriteLine($"El volumen de la esfera es: {volumenEsfera} cm³");
      Console.WriteLine($"La diagonal del rectángulo es: {diagonalRectangulo} cm");
      Console.WriteLine($"Su edad es: {edad} años");
    }
  }
}
