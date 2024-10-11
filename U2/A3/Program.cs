using System;


// Programa principal


// Comprueba que se pasen los argumentos necesarios
if (args.Length != 4)
{
  Console.WriteLine("Uso: Program <capacidadBateria> <consumoPromedioDiario> <ciclosCargaCompletos> <eficienciaBateria>");
  return;
}

Bateria bateria = new Bateria(
  int.Parse(args[0]),
  int.Parse(args[1]),
  int.Parse(args[2]),
  float.Parse(args[3])
);

Console.WriteLine("Capacidad de la batería (mAh): " + bateria.ObtenerCapacidadBateria());
Console.WriteLine("Consumo promedio diario (mAh): " + bateria.ObtenerConsumoPromedioDiario());
Console.WriteLine("Ciclos de carga completos: " + bateria.ObtenerCiclosCargaCompletos());
Console.WriteLine("Eficiencia de la batería (%): " + bateria.ObtenerEficienciaBateria());

float vidaUtil = bateria.CalcularVidaUtil(
  bateria.ObtenerCapacidadBateria(),
  bateria.ObtenerConsumoPromedioDiario(),
  bateria.ObtenerCiclosCargaCompletos(),
  bateria.ObtenerEficienciaBateria()
);

Console.WriteLine("Vida útil estimada de la batería (días): " + vidaUtil);


public class Bateria
{
  // Guarda los datos de la batería
  private int capacidadBateria;
  // Guarda el consumo promedio diario
  private int consumoPromedioDiario;
  // Guarda los ciclos de carga completos
  private int ciclosCargaCompletos;
  // Guarda la eficiencia de la batería en %
  private float eficienciaBateria;

  // Factor de degradación de la batería
  private const float factorDeDegradacion = 0.98f;

  // Constructor de la clase Bateria
  public Bateria(int capacidadBateria, int consumoPromedioDiario, int ciclosCargaCompletos, float eficienciaBateria)
  {
    this.capacidadBateria = capacidadBateria;
    this.consumoPromedioDiario = consumoPromedioDiario;
    this.ciclosCargaCompletos = ciclosCargaCompletos;
    this.eficienciaBateria = eficienciaBateria;
  }

  public int ObtenerCapacidadBateria() => capacidadBateria;

  // Métodos que devuelve el consumo promedio diario.
  public int ObtenerConsumoPromedioDiario() => consumoPromedioDiario;

  // Métodos que obtiene los ciclos de carga completos.
  public int ObtenerCiclosCargaCompletos() => ciclosCargaCompletos;

  // Métodos que obtiene la eficiencia de la batería.
  public float ObtenerEficienciaBateria() => eficienciaBateria;

  // Método que calcula y devuelve la vida útil de la batería.
  public float CalcularVidaUtil(
    int capacidadBateria, int consumoPromedioDiario,
    int ciclosCargaCompletos, float eficienciaBateria
  ) =>
  (capacidadBateria / (float)consumoPromedioDiario) * (eficienciaBateria / 100) - (ciclosCargaCompletos / factorDeDegradacion);
}


