using System.Globalization;

namespace U1_EA_EMCV {
    class Especie
    {
    public string NombreCientifico { get; set; }
    public string Clasificacion { get; set; }
    public string FuncionEcologica { get; set; }
    public string CodigoGenetico { get; set; }
    public float Abundancia { get; set; } // 0.0 - 1.0

    private Especie(string nombre, string clasificacion, string funcion, string codigoGenetico, float abundancia) {
      NombreCientifico = nombre;
      Clasificacion = clasificacion;
      FuncionEcologica = funcion;
      CodigoGenetico = codigoGenetico;
      Abundancia = abundancia;
    }

    // Comparar secuencia genética (porcentaje de coincidencias sobre la longitud mínima)
    public string CompararEspecies(Especie otraEspecie) {
      if (string.IsNullOrWhiteSpace(CodigoGenetico) || string.IsNullOrWhiteSpace(otraEspecie.CodigoGenetico))
        return "Comparación imposible: una de las especies no tiene código genético.";
      if (EsLaMismaEspecie(otraEspecie))
        return "Las especies son idénticas.";

      int minLen = Math.Min(CodigoGenetico.Length, otraEspecie.CodigoGenetico.Length);
      int coincidencias = 0;
      for (int i = 0; i < minLen; i++) {
        if (CodigoGenetico[i] == otraEspecie.CodigoGenetico[i]) coincidencias++;
      }
      double similitud = (double)coincidencias / minLen;
      double diferenciaAbundancia = Math.Abs(Abundancia - otraEspecie.Abundancia);

      return $"Similitud genética: {similitud:P2}. Diferencia de abundancia: {diferenciaAbundancia:P2}.";
    }

    // create a method to check if this especie is equal to another especie based on its properties
    public bool EsLaMismaEspecie(Especie e) => $"{this}" == $"{e}";
  
    public void ModificarAbundancia(float nuevaAbundancia) {
      if (nuevaAbundancia < 0f || nuevaAbundancia > 1f)
        throw new ArgumentOutOfRangeException(nameof(nuevaAbundancia), "Abundancia debe estar entre 0.0 y 1.0.");
      Abundancia = nuevaAbundancia;
      Console.WriteLine($"Abundancia actualizada: {Abundancia:P2}");
    }

    public void MostrarInformacionDetallada() {
      Console.WriteLine($"Nombre científico : {NombreCientifico}");
      Console.WriteLine($"Clasificación     : {Clasificacion}");
      Console.WriteLine($"Función ecológica : {FuncionEcologica}");
      Console.WriteLine($"Código genético   : {(string.IsNullOrWhiteSpace(CodigoGenetico) ? "(vacío)" : CodigoGenetico)}");
      Console.WriteLine($"Abundancia        : {Abundancia:P2}");
    }

    private bool TieneFuncionEcologica(Especie e, string termino) => e.FuncionEcologica.Contains(termino, StringComparison.OrdinalIgnoreCase);

    private bool EsArbol(Especie e) => TieneFuncionEcologica(e, "arbol");

    private bool EsPlanta(Especie e) => e.Clasificacion.Contains("plantae", StringComparison.OrdinalIgnoreCase)
      && !EsArbol(e);

    // private bool EsCarnivoro(Especie e) => MyRegex().Match(e.Clasificacion).Success;

    private bool EsFijador(Especie e) => EsPlanta(e) && TieneFuncionEcologica(e, "fijador");

    private bool EsFrutifero(Especie e) => (EsPlanta(e) || EsArbol(e)) && TieneFuncionEcologica(e, "frugivoro");

    private bool EsPolinizador(Especie e) => TieneFuncionEcologica(e, "polinizador");

    private bool EsRelacionPlantaArbolMutualista(Especie otra) {
      return (EsArbol(this) && EsFijador(otra))
        || (EsArbol(otra) && EsFijador(this));
    }

    private bool EsRelacionPlantaFructiferaDepredadorMutualista(Especie otra) {
      return (EsFrutifero(this) && EsDepredador(otra) && !EsDepredadorTope(otra))
        || (EsFrutifero(otra) && EsDepredador(this) && !EsDepredadorTope(this));
    }

    private bool EsRelacionPlantaPolinizadorMutualista(Especie otra) {
      return (EsPlanta(this) || EsFrutifero(this)) && EsPolinizador(otra)
        || (EsPlanta(otra) || EsFrutifero(otra)) && EsPolinizador(this);
    }

    private bool EsProductor(Especie e) => TieneFuncionEcologica(e, "productor");

    private bool EsDepredador(Especie e) => TieneFuncionEcologica(e, "depredador");

    private bool EsDepredadorTope(Especie e) => EsDepredador(e) && TieneFuncionEcologica(e, "tope");

    private bool EsRelacionMutua(Especie otra) => EsRelacionPlantaArbolMutualista(otra)
      || EsRelacionPlantaFructiferaDepredadorMutualista(otra)
      || EsRelacionPlantaPolinizadorMutualista(otra);

    private bool EsRelacionCompetitiva(Especie otra) =>
      EsProductor(this) && EsProductor(otra)
      || EsDepredador(this) && EsDepredador(otra)
      || EsPlanta(this) && EsPlanta(otra);

    public string SimularInteraccionEcologica(Especie otra) {
      string result = "";
      float diferenciaAbundancia = Math.Abs(Abundancia - otra.Abundancia);

      // Regla efecto de abundancia (prioritaria al final si aplica)
      if (diferenciaAbundancia > 0.5f) {
        string dominante = Abundancia > otra.Abundancia ? NombreCientifico : otra.NombreCientifico;
        result += $"Efecto de abundancia: {dominante} domina los recursos (diferencia {diferenciaAbundancia:P2}).\n";
      }

      if (EsRelacionMutua(otra)) {
        return result += "Mutualismo (beneficio mutuo) probable.";
      }
      if (EsRelacionCompetitiva(otra)) {
        return result += "Competencia por recursos probable.";
      }

      return result += "Neutralismo (sin relación significativa) probable.";
    }

    public Especie ClonarEspecie() => new(NombreCientifico, Clasificacion, FuncionEcologica, CodigoGenetico, Abundancia);

    public static Especie CrearEspecie() {
      Console.Write("Nombre científico: ");
      var nombre = Console.ReadLine();
      Console.Write("Clasificación (ej. Plantae, Magnoliophyta, Fagales): ");
      var clasificacion = Console.ReadLine();
      Console.Write("Función ecológica: ");
      var funcion = Console.ReadLine();
      Console.Write("Código genético (ej. ATCG...): ");
      string codigo = Console.ReadLine();

      Console.Write("Abundancia (0.0 - 1.0): ");
      var abundStr = Console.ReadLine();
      if (!float.TryParse(abundStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float abundancia))
        throw new FormatException();

      if (abundancia < 0f || abundancia > 1f)
        throw new ArgumentOutOfRangeException(nameof(abundancia), "Abundancia debe estar entre 0.0 y 1.0.");

      Especie especie = new(nombre, clasificacion, funcion, codigo, abundancia);
      Console.WriteLine("Especie creada con éxito.");

      return especie;
    }

    public override string ToString() =>
      $"{NombreCientifico} | {Clasificacion} | {FuncionEcologica} | {(string.IsNullOrWhiteSpace(CodigoGenetico) ? "(vacío)" : CodigoGenetico)} | {Abundancia:P2}";
    // [GeneratedRegex("(C|c)arnivora", RegexOptions.IgnoreCase, "en-MX")]
    // private static partial Regex MyRegex();
  }

  // Clase para menú interactivo
  public class Menu {
    private readonly List<Especie> especies = [];

    public void Mostrar() {
      int opcion;

      do {
        try {
          Console.WriteLine("""
                      --- Sistema de Gestión de Ecosistemas Forestales ---
                          1. Comparar dos especies
                          2. Modificar abundancia de la especie
                          3. Mostrar información de la especie
                          4. Simular interacción ecológica entre especies
                          5. Clonar especies
                          0. Salir
                      ----------------------------------------------------
                  """);
          Console.Write("Seleccione una opción: ");
          opcion = int.Parse(Console.ReadLine()?.Trim());
        }
        catch (FormatException) {
          Console.WriteLine("Error: formato numérico inválido.");
          opcion = -1;
        }

        try {
          switch (opcion) {
            case 1:
              CompararDosEspecies();
              break;
            case 2:
              ModificarAbundancia();
              break;
            case 3:
              MostrarInfoDetallada();
              break;
            case 4:
              SimularInteraccion();
              break;
            case 5:
              ClonarEspecie();
              break;
            case 0:
              Console.WriteLine("Hasta luego.");
              break;
            default:
              Console.WriteLine("Opción inválida.");
              break;
          }
        }
        catch (FormatException) {
          Console.WriteLine("Error: formato numérico inválido.");
        }
        catch (ArgumentOutOfRangeException ex) {
          Console.WriteLine($"Error de validación: {ex.Message}");
        }
        catch (InvalidOperationException ex) {
          Console.WriteLine($"Operación inválida: {ex.Message}");
        }
        catch (Exception ex) {
          Console.WriteLine($"Error inesperado: {ex.Message}");
        }
      } while (opcion != 0);
    }

    private void MostrarEspecies() {
      if (especies.Count == 0) {
        Console.WriteLine("No hay especies registradas.");
        return;
      }
      
      Console.WriteLine("Especies registradas:");
      foreach (var especie in especies) {
        especie.MostrarInformacionDetallada();
      }
    }

    private int SeleccionarIndice() {
      foreach (var e in especies) {
        Console.WriteLine($"- {e.NombreCientifico}");
      }
      Console.Write("Seleccione índice: ");
      var s = Console.ReadLine();
      if (!int.TryParse(s, out int idx) || idx < 0 || idx >= especies.Count) throw new InvalidOperationException("Índice inválido.");
      return idx;
    }

    private void MostrarInfoDetallada() {
      int idx = SeleccionarIndice();
      especies[idx].MostrarInformacionDetallada();
    }

    private void ModificarAbundancia() {
      int idx = SeleccionarIndice();
      Console.Write("Nueva abundancia (0.0 - 1.0): ");
      var val = Console.ReadLine();
      if (!float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float nueva)) throw new FormatException();
      especies[idx].ModificarAbundancia(nueva);
    }

    private void CompararDosEspecies() {
      Console.WriteLine("Ingrese la primera especie:");
      Especie e1 = Especie.CrearEspecie();
      especies.Add(e1);
      Console.WriteLine("Ingrese la segunda especie:");
      Especie e2 = Especie.CrearEspecie();
      especies.Add(e2);

      Console.WriteLine(e1.CompararEspecies(e2));
    }

    private void SimularInteraccion() {
      Console.WriteLine("Ingrese la primera especie:");
      Especie e1 = Especie.CrearEspecie();
      especies.Add(e1);
      Console.WriteLine("Ingrese la segunda especie:");
      Especie e2 = Especie.CrearEspecie();
      especies.Add(e2);

      Console.WriteLine(e1.SimularInteraccionEcologica(e2));
    }

    private void ClonarEspecie() {
      int idx = SeleccionarIndice();
      var clon = especies[idx].ClonarEspecie();
      // opcional: modificar nombre del clon para identificarlo
      clon.NombreCientifico += " (clon)";
      especies.Add(clon);
      Console.WriteLine("Especie clonada correctamente.");
    }
  }

  class MainClass {
    static void Main() {
      new Menu().Mostrar();
    }
  }
}
