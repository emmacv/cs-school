using System;
using System.Collections.Generic;

namespace SistemaEscolar
{

  // Clase que representa el registro de un estudiante
  public class RegistroEstudiante
  {
    // Propiedades para el estudiante y su promedio
    public Estudiante estudiante { get; }
    public float promedio { get; private set; }

    // Constructor que inicializa el estudiante y su promedio
    public RegistroEstudiante(Estudiante estudiante)
    {
      this.estudiante = estudiante;
      this.promedio = 0.0f;
    }

    // Método para calcular el promedio del estudiante
    public void CalcularPromedio()
    {
      float suma = 0;
      foreach (var nota in estudiante.notas.Values)
      {
        suma += nota;
      }

      this.promedio = (float)suma / estudiante.notas.Count;
    }

    // Método para mostrar el resultado (aprobado/reprobado) basado en el promedio
    public void MostrarResultado()
    {
      CalcularPromedio();
      var estatus = this.promedio >= 6.0f ? "aprobó" : "no aprobó";

      Console.WriteLine($"{estudiante.nombre}, con el tutor {estudiante.Tutor} {estatus}.");
    }
  }

  // Clase que representa a un estudiante
  public class Estudiante
  {
    // Propiedades para los detalles del estudiante y sus notas
    public string nombre { get; set; }
    public int numeroEstudiante { get; set; }
    public int grado { get; set; }
    public int edad { get; set; }
    public string matricula { get; set; }
    public string Tutor { get; set; }
    public Dictionary<string, float> notas { get; set; }

    // Constructor que inicializa los detalles del estudiante
    public Estudiante(string nombre, int numeroEstudiante, int grado, int edad, string matricula)
    {
      this.nombre = nombre;
      this.numeroEstudiante = numeroEstudiante;
      this.grado = grado;
      this.edad = edad;
      this.matricula = matricula;
      notas = new Dictionary<string, float>();
    }

    public Estudiante()
    {
      notas = new Dictionary<string, float>();
    }

    // Método para asignar un tutor al estudiante de una lista de tutores
    public void AsignarTutor(List<string> tutores)
    {
      Random rand = new Random();
      Tutor = tutores[rand.Next(tutores.Count)];
    }

    // Método para registrar las notas del estudiante en diferentes materias
    public void RegistrarNotas(float matematicas, float ciencias, float lenguas, float historia, float educacionFisica)
    {
      Console.WriteLine(this);
      notas["Matematicas"] = matematicas;
      notas["Ciencias"] = ciencias;
      notas["Lenguas"] = lenguas;
      notas["Historia"] = historia;
      notas["Educacion Fisica"] = educacionFisica;
    }
  }

  // Clase principal que contiene el punto de entrada del programa
  class Program
  {
    public static void CreaEstudiante(List<Estudiante> estudiantes)
    {
      foreach (var i in new int[] { 1, 2 })
      {
        Estudiante estudiante = new Estudiante();

        Console.Write($"Estudiante {i}\nNombre: ");
        estudiante.nombre = Console.ReadLine();
        Console.Write("Número de estudiante: ");
        estudiante.numeroEstudiante = int.Parse(Console.ReadLine());
        Console.Write("Grado: ");
        estudiante.grado = int.Parse(Console.ReadLine());
        Console.Write("Edad: ");
        estudiante.edad = int.Parse(Console.ReadLine());
        Console.Write("Matrícula: ");
        estudiante.matricula = Console.ReadLine();

        estudiantes.Add(estudiante);
      }

    }

    static void Main(string[] args)
    {
      // Lista de tutores disponibles
      List<string> tutores = new List<string> { "Juan Pérez", "María Gómez", "Carlos Ruiz", "Laura Sánchez" };

      Console.WriteLine("Sistema de registro de estudiantes\n");
      Console.WriteLine("Ingrese los datos de los estudiantes:");

      List<Estudiante> estudiantes = new List<Estudiante>();

      // Creación de estudiantes
      CreaEstudiante(estudiantes);

      // Bucle para asignar tutor y registrar notas a cada estudiante
      foreach (var estudiante in estudiantes)
      {
        estudiante.AsignarTutor(tutores);

        Console.WriteLine($"Ingrese las notas para {estudiante.nombre}:");
        Console.Write("Matemáticas: ");
        float matematicas = float.Parse(Console.ReadLine());
        Console.Write("Ciencias: ");
        float ciencias = float.Parse(Console.ReadLine());
        Console.Write("Lenguas: ");
        float lenguas = float.Parse(Console.ReadLine());
        Console.Write("Historia: ");
        float historia = float.Parse(Console.ReadLine());
        Console.Write("Educación Física: ");
        float educacionFisica = float.Parse(Console.ReadLine());

        estudiante.RegistrarNotas(matematicas, ciencias, lenguas, historia, educacionFisica);
        new RegistroEstudiante(estudiante).MostrarResultado();
      }
    }
  }
}
