using System;

Program p = new();

(double sueldoBase, double ventas) = p.ObtenerSueldoBaseYVentas();
// Calcular el bono basado en la cantidad de ventas
double bono = p.CalcularBono(sueldoBase, ventas);

// Calcular el sueldo más comisión
double sueldoMasComision = p.CalcularSueldoMasComision(sueldoBase, bono);

// Calcular el sueldo neto
double sueldoNeto = p.CalcularSueldoNeto(sueldoMasComision, out double retencion);

// Imprimir los resultados
Console.WriteLine($"Bono: ${bono:F2}");
Console.WriteLine($"Retención: ${retencion:F2}");
Console.WriteLine($"Sueldo neto a pagar: ${sueldoNeto:F2}");

class Program
{
  public double CalcularBono(double sueldoBase, double ventas)
  {
    bool esValido = sueldoBase > 0 && ventas >= 0;

    double bono;
    if (ventas < 1000)
    {
      bono = sueldoBase * 0.01;
    }
    else if (ventas <= 10000)
    {
      bono = sueldoBase * 0.05;
    }
    else if (ventas <= 50000)
    {
      bono = sueldoBase * 0.10;
    }
    else
    {
      bono = sueldoBase * 0.15;
    }
    return bono;
  }

  public (double, double) ObtenerSueldoBaseYVentas()
  {
    // Solicitar al usuario el sueldo base y la cantidad de ventas
    Console.Write("Ingrese el sueldo base del empleado: ");
    double sueldoBase = Convert.ToDouble(Console.ReadLine());

    Console.Write("Ingrese la cantidad de ventas realizadas durante el mes: ");
    double ventas = Convert.ToDouble(Console.ReadLine());

    return (sueldoBase, ventas);
  }

  public double CalcularSueldoNeto(double sueldoMasComision, out double retencion)
  {
    if (sueldoMasComision < 1000)
    {
      retencion = sueldoMasComision * 0.05;
    }
    else if (sueldoMasComision <= 10000)
    {
      retencion = sueldoMasComision * 0.10;
    }
    else if (sueldoMasComision <= 100000)
    {
      retencion = sueldoMasComision * 0.20;
    }
    else
    {
      retencion = sueldoMasComision * 0.30;
    }

    // Calcular el sueldo neto
    return sueldoMasComision - retencion;
  }

  public double CalcularSueldoMasComision(double sueldoBase, double bono) => sueldoBase + bono;
}
