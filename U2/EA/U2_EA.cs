using System.Globalization;

namespace U2_EA;

// ---------- Clase base ----------
public class GestorConsumo {
  public string IdVivienda { get; set; }
  public string PerfilOriginal { get; set; } // cadena de dígitos '1'..'9'
  public string PerfilOptimizado { get; set; }
  public DateTime FechaRegistro { get; set; }

  public GestorConsumo(string id, string perfil) {
    IdVivienda = id;
    PerfilOriginal = perfil;
    PerfilOptimizado = perfil;
    FechaRegistro = DateTime.Now;
  }

  // Convertir perfil string a int[]
  protected int[] PerfilToArray(string perfil) => [..perfil.Select(c => {
    // TODO: validar caracteres antes de convertir
      if (!char.IsDigit(c)) throw new FormatException("Perfil contiene caracteres no numéricos.");
      return (int)char.GetNumericValue(c);
    })];

  protected string ArrayToPerfil(int[] arr) => string.Concat(arr.Select(i => $"{Math.Clamp(i, 1, 9)}"));

  public virtual void OptimizarConsumo() {
    // Implementación base: no hace cambios
    PerfilOptimizado = PerfilOriginal;
  }

  public virtual void RestaurarPerfil() {
    PerfilOptimizado = PerfilOriginal;
  }

  public virtual double CalcularCostoTotal(double tarifaDefault = 0.15) => PerfilToArray(PerfilOptimizado).Sum() * tarifaDefault;

  public double SumaPerfilOriginal() => PerfilToArray(PerfilOriginal).Sum();

  public double SumaPerfilOptimizado() => PerfilToArray(PerfilOptimizado).Sum();
}

// ---------- GestorTarifaHoraria ----------
public class GestorTarifaHoraria : GestorConsumo {
  private int _porcentajeReduccion;

  public List<int> HorasPico { get; set; } = [];
  // 10..50
  public int PorcentajeReduccion {
    get => _porcentajeReduccion;
    set => _porcentajeReduccion = value < 10 ? 0 : Math.Clamp(value, 10, 50);
  }
  public double TarifaPico { get; set; } = 0.30;
  public double TarifaValle { get; set; } = 0.10;

  public GestorTarifaHoraria(string id, string perfil) : base(id, perfil) { }

  public override void OptimizarConsumo() {
    var arr = PerfilToArray(PerfilOriginal);
    var optim = (int[])arr.Clone();

    foreach (int hora in HorasPico) {
      if (hora < 0 || hora >= optim.Length) continue;
      double consumo = arr[hora];
      double nuevo = consumo * (100 - PorcentajeReduccion) / 100.0;

      optim[hora] = (int)Math.Round(nuevo < 1 ? 1 : nuevo, MidpointRounding.AwayFromZero);
    }

    PerfilOptimizado = ArrayToPerfil(optim);
  }

  public override double CalcularCostoTotal(double tarifaDefault = 0.15) {
    var optim = PerfilToArray(PerfilOptimizado);
    // suma total

    double costo = optim.Aggregate(0.0, (acc, val) => {
      if (HorasPico.Contains(val)) acc += val * TarifaPico;
      else acc += val * TarifaValle;

      return acc;
    });

    return costo;
  }
}

// ---------- GestorSolar ----------
public class GestorSolar : GestorConsumo {
  public List<int> HorasSolares { get; set; } = [];
  public int GeneracionPorHora { get; set; } = 2; // 1..5
  public double CapacidadInstalada { get; set; } = 1.0;
  public int EficienciaActual { get; set; } = 90; // 80..100

  public GestorSolar(string id, string perfil) : base(id, perfil) { }

  public override void OptimizarConsumo() {
    var arr = PerfilToArray(PerfilOriginal);
    var optim = (int[])arr.Clone();

    foreach (int hora in HorasSolares) {
      if (hora < 0 || hora >= optim.Length) continue;
      double consumo = arr[hora];
      double generacion = GeneracionPorHora * (EficienciaActual / 100.0);
      double nuevo = consumo - generacion;
      nuevo = Math.Max(1, nuevo);

      optim[hora] = (int)Math.Round(nuevo, MidpointRounding.AwayFromZero);
    }

    PerfilOptimizado = ArrayToPerfil(optim);
  }

  public (double ahorroKWh, double ahorroDinero) CalcularAhorroSolar(double tarifa = 0.15) {
    double ahorroKWh = SumaPerfilOriginal() - SumaPerfilOptimizado();
    double ahorroDinero = ahorroKWh * tarifa;

    return (ahorroKWh, ahorroDinero);
  }
}

public class GestorInteligenteDemanda : GestorSolar {
  public string AlgoritmoML { get; set; }
  public int NivelBalanceo { get; set; } = 1; // 1..3
  public string SenalRed { get; set; } = "Normal"; // "Normal","Alerta","Crítico"

  public GestorInteligenteDemanda(string id, string perfil) : base(id, perfil) { }

  public override void OptimizarConsumo() {
    // 1. base solar
    base.OptimizarConsumo();

    // 2. Balanceo de curva
    var optim = PerfilToArray(PerfilOptimizado);
    foreach (int _ in Enumerable.Range(0, NivelBalanceo)) {
      int (pico, valle) = (
        Array.IndexOf(optim, optim.Max()), 
        Array.IndexOf(optim, optim.Min())
      );

      while ((pico - valle) > 2) {
        optim[pico] = Math.Max(1, optim[pico] - 1);
        optim[valle] = Math.Min(9, optim[valle] + 1);
      }
    }

    if (SenalRed == "Crítico") {
      for (int i = 0; i < optim.Length; i++) {
        if (optim[i] > 6) optim[i] = 6;
      }
    }

    PerfilOptimizado = ArrayToPerfil(optim);
  }
}

public class GestorMultiDispositivo : GestorTarifaHoraria {
  public string TipoGestion { get; set; } = "Casa-Completa"; // "Casa-Completa" o "Dispositivos-Individuales"
  public int LimiteCapacidad { get; set; } = 9; // max por hora para string
  public int[] PrioridadesDispositivos { get; set; } = [];
  public double ConsumoMinimo { get; set; } = 0.5;

  public GestorMultiDispositivo(string id, string perfil) : base(id, perfil) { }

  // Overload: optimizar cuando tiene perfil string (aplica reducciones de horas pico y límites)
  public void OptimizarConsumo(string perfil) {
    PerfilOriginal = perfil;

    // aplicar reducción heredada en HorasPico
    OptimizarConsumo(); // llama la versión sin parámetros (esta clase hereda la versión que usa HorasPico)
    var perfil = PerfilToArray(PerfilOptimizado);
    for (int hora = 0; hora < perfil.Length; hora++) {
      if (perfil[hora] > LimiteCapacidad)
        perfil[hora] = LimiteCapacidad;
    }

    PerfilOptimizado = ArrayToPerfil(perfil);
  }

  // Overload: optimizar cuando se recibe array de consumos por dispositivo (double[])
  public void OptimizarConsumo(double[] dispositivos) {
    // TODO: validar entrada
    // if (dispositivos == null || dispositivos.Length == 0) throw new ArgumentException("Array de dispositivos vacío.");

    // copia para no modificar entrada

    double[] nuevoDispositivos = (from num in dispositivos
                    orderby num ascending
                    select num).ToArray();
    double consumoTotal = nuevoDispositivos.Sum();
    double[] dispo = (double[])dispositivos.Clone();

    // ordenar indices por prioridad: se espera PrioridadesDispositivos del mismo tamaño
    int cantidadDispositivos = dispo.Length;
    int[] prioridades = PrioridadesDispositivos.Length == cantidadDispositivos ? PrioridadesDispositivos : Enumerable.Repeat(1, cantidadDispositivos).ToArray();

    // objetivo: reducir consumo total a 70% (30% de reducción)
    double consumoTotal = dispo.Sum();
    double objetivo = consumoTotal * 0.7;
    double diferencia = consumoTotal - objetivo;

    // crear lista de indices ordenados por prioridad descendente (prioridad alta = mayor número)
    var indices = Enumerable.Range(0, cantidadDispositivos).OrderByDescending(i => prioridades[i]).ToList();

    foreach (int idx in indices) {
      if (diferencia <= 0) break;
      double prioridad = prioridades[idx];
      if (prioridad >= 3) {
        double reducible = dispo[idx] - ConsumoMinimo;
        if (reducible <= 0) continue;
        if (reducible >= diferencia) {
          dispo[idx] -= diferencia;
          diferencia = 0;
          break;
        }
        else {
          dispo[idx] = ConsumoMinimo;
          diferencia -= reducible;
        }
      }
    }

    // Resultado: generar PERFIL agregando consumos por hora: para simplicidad, aquí generamos una cadena donde
    // cada posición representa consumo promedio por hora redondeado a entero entre 1 y 9 (suponiendo un día corto).
    // En una implementación real, se mapearía la reducción por horas específicas.
    int len = Math.Max(1, dispo.Length);
    int[] perfil = new int[len];
    for (int i = 0; i < len; i++) {
      perfil[i] = (int)Math.Round(Math.Clamp(dispo[i], 1.0, 9.0));
    }
    PerfilOriginal = ArrayToPerfil(perfil);
    PerfilOptimizado = PerfilOriginal; // ya reducido
  }
}

// ---------- Menu Interactivo ----------
public class Menu {
  private GestorConsumo gestor = null;

  public void Mostrar() {
    string opcion;
    do {
      Console.WriteLine("\n=== OPTIMIZADOR ENERGÉTICO DOMÓTICA ===");
      Console.WriteLine("1. Crear GestorTarifaHoraria");
      Console.WriteLine("2. Crear GestorSolar");
      Console.WriteLine("3. Crear GestorInteligenteDemanda");
      Console.WriteLine("4. Crear GestorMultiDispositivo");
      Console.WriteLine("5. Cargar perfil (cadena de dígitos 1-9)");
      Console.WriteLine("6. Configurar parámetros (horas pico/solar, tarifas, etc.)");
      Console.WriteLine("7. Ejecutar optimización");
      Console.WriteLine("8. Mostrar perfil original y optimizado");
      Console.WriteLine("9. Calcular costo/ahorro");
      Console.WriteLine("0. Salir");
      Console.Write("Opción: ");
      opcion = Console.ReadLine()?.Trim();

      try {
        switch (opcion) {
          case "1": CrearGestorTarifa(); break;
          case "2": CrearGestorSolar(); break;
          case "3": CrearGestorInteligente(); break;
          case "4": CrearGestorMulti(); break;
          case "5": CargarPerfil(); break;
          case "6": ConfigurarParametros(); break;
          case "7": EjecutarOptimizacion(); break;
          case "8": MostrarPerfiles(); break;
          case "9": CalcularCostos(); break;
          case "0": Console.WriteLine("Saliendo..."); break;
          default: Console.WriteLine("Opción inválida."); break;
        }
      }
      catch (FormatException) { Console.WriteLine("Error: formato numérico inválido."); }
      catch (ArgumentException ex) { Console.WriteLine($"Error: {ex.Message}"); }
      catch (Exception ex) { Console.WriteLine($"Error inesperado: {ex.Message}"); }

    } while (opcion != "0");
  }

  private void CrearGestorTarifa() {
    Console.Write("Id vivienda: "); var id = Console.ReadLine();
    Console.Write("Perfil inicial (ej. 4567823): "); var perfil = Console.ReadLine();
    gestor = new GestorTarifaHoraria(id, perfil);
    Console.WriteLine("GestorTarifaHoraria creado.");
  }

  private void CrearGestorSolar() {
    Console.Write("Id vivienda: "); var id = Console.ReadLine();
    Console.Write("Perfil inicial (ej. 4567823): "); var perfil = Console.ReadLine();
    gestor = new GestorSolar(id, perfil);
    Console.WriteLine("GestorSolar creado.");
  }

  private void CrearGestorInteligente() {
    Console.Write("Id vivienda: "); var id = Console.ReadLine();
    Console.Write("Perfil inicial (ej. 4567823): "); var perfil = Console.ReadLine();
    gestor = new GestorInteligenteDemanda(id, perfil);
    Console.WriteLine("GestorInteligenteDemanda creado.");
  }

  private void CrearGestorMulti() {
    Console.Write("Id vivienda: "); var id = Console.ReadLine();
    Console.Write("Perfil inicial (ej. 4567823): "); var perfil = Console.ReadLine();
    gestor = new GestorMultiDispositivo(id, perfil);
    Console.WriteLine("GestorMultiDispositivo creado.");
  }

  private void CargarPerfil() {
    EnsureGestor();
    Console.Write("Nuevo perfil (cadena de dígitos 1-9): ");
    string perfil = Console.ReadLine();
    // validar
    if (string.IsNullOrWhiteSpace(perfil) || !perfil.All(char.IsDigit)) {
      Console.WriteLine("Perfil inválido.");
      return;
    }
    gestor.PerfilOriginal = perfil;
    gestor.PerfilOptimizado = perfil;
    gestor.FechaRegistro = DateTime.Now;
    Console.WriteLine("Perfil cargado.");
  }

  private void ConfigurarParametros() {
    EnsureGestor();
    switch (gestor) {
      case GestorTarifaHoraria gth:
        Console.Write("Horas pico (índices separados por coma, ej. 0,1,2): ");
        var s = Console.ReadLine();
        gth.HorasPico = ParseIndices(s);
        Console.Write("Porcentaje reducción (10-50): ");
        gth.PorcentajeReduccion = int.Parse(Console.ReadLine());
        Console.Write("Tarifa pico: ");
        gth.TarifaPico = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        Console.Write("Tarifa valle: ");
        gth.TarifaValle = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        Console.WriteLine("Parámetros de tarifa configurados.");
        break;
      case GestorSolar gs:
        Console.Write("Horas solares (índices, ej. 6,7,8): ");
        gs.HorasSolares = ParseIndices(Console.ReadLine());
        Console.Write("Generación por hora (1-5): ");
        gs.GeneracionPorHora = int.Parse(Console.ReadLine());
        Console.Write("Eficiencia actual (80-100): ");
        gs.EficienciaActual = int.Parse(Console.ReadLine());
        Console.WriteLine("Parámetros solares configurados.");
        break;
      case GestorInteligenteDemanda gid:
        Console.Write("Horas solares (índices, ej. 6,7,8): ");
        gid.HorasSolares = ParseIndices(Console.ReadLine());
        Console.Write("Generación por hora (1-5): ");
        gid.GeneracionPorHora = int.Parse(Console.ReadLine());
        Console.Write("Eficiencia actual (80-100): ");
        gid.EficienciaActual = int.Parse(Console.ReadLine());
        Console.Write("Nivel de balanceo (1-3): ");
        gid.NivelBalanceo = int.Parse(Console.ReadLine());
        Console.Write("Señal red (Normal/Alerta/Crítico): ");
        gid.SenalRed = Console.ReadLine();
        Console.WriteLine("Parámetros inteligencia configurados.");
        break;
      case GestorMultiDispositivo gmd:
        Console.Write("Horas pico (índices separados por coma): ");
        gmd.HorasPico = ParseIndices(Console.ReadLine());
        Console.Write("Porcentaje reducción (10-50): ");
        gmd.PorcentajeReduccion = int.Parse(Console.ReadLine());
        Console.Write("Limite capacidad por hora (1-9): ");
        gmd.LimiteCapacidad = int.Parse(Console.ReadLine());
        Console.WriteLine("Parámetros multi-dispositivo configurados.");
        break;
      default:
        Console.WriteLine("Tipo de gestor no reconocido para configuración.");
        break;
    }
  }

  private void EjecutarOptimizacion() {
    EnsureGestor();
    // Caso especial: GestorMultiDispositivo con entrada por dispositivos
    if (gestor is GestorMultiDispositivo dispositivo) {
      Console.Write("¿Optimizar por dispositivos individuales? (s/n): ");
      var r = Console.ReadLine();
      if (r?.ToLower() == "s") {
        Console.Write("Ingrese consumos por dispositivo separados por coma (double): ");
        var s = Console.ReadLine();
        var parts = s.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => double.Parse(p, CultureInfo.InvariantCulture)).ToArray();
        var gmd = dispositivo;
        Console.Write("Ingrese prioridades separadas por coma (enteros, mayor=prioridad): ");
        var prio = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToArray();
        gmd.PrioridadesDispositivos = prio;
        gmd.OptimizarConsumo(parts);
        Console.WriteLine("Optimización multi-dispositivo aplicada.");
        return;
      }
    }

    // En general, llamar al método virtual OptimizarConsumo()
    gestor.OptimizarConsumo();
    Console.WriteLine("Optimización ejecutada.");
  }

  private void MostrarPerfiles() {
    EnsureGestor();
    Console.WriteLine($"ID: {gestor.IdVivienda}  Fecha: {gestor.FechaRegistro}");
    Console.WriteLine($"Perfil original : {gestor.PerfilOriginal}");
    Console.WriteLine($"Perfil optimizado: {gestor.PerfilOptimizado}");
  }

  private void CalcularCostos() {
    EnsureGestor();
    Console.Write("Ingrese tarifa por kWh (si deja vacío usa 0.15): ");
    var s = Console.ReadLine();
    double tarifa = 0.15;
    if (!string.IsNullOrWhiteSpace(s)) tarifa = double.Parse(s, CultureInfo.InvariantCulture);

    switch (gestor) {
      case GestorTarifaHoraria gth:
        Console.WriteLine($"Costo total optimizado (tarifas pico/valle): {gth.CalcularCostoTotal():C2}");
        break;
      case GestorSolar gs:
        gs.OptimizarConsumo();
        var (ahorroKwh, ahorroDin) = gs.CalcularAhorroSolar(tarifa);
        Console.WriteLine($"Ahorro kWh: {ahorroKwh:F2}, ahorro dinero: {ahorroDin:C2}");
        Console.WriteLine($"Costo total optimizado (a tarifa {tarifa}): {gs.CalcularCostoTotal(tarifa):C2}");
        break;
      default:
        double costo = gestor.CalcularCostoTotal(tarifa);
        Console.WriteLine($"Costo total: {costo:C2}");
        break;
    }
  }

  private void EnsureGestor() {
    if (gestor == null) throw new InvalidOperationException("Debe crear un gestor primero.");
  }

  private List<int> ParseIndices(string s) {
    if (string.IsNullOrWhiteSpace(s)) return new List<int>();
    return s.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .ToList();
  }
}

// ---------- Programa principal ----------
class Program {
  static void Main() {
    new Menu().Mostrar();
  }
}
  
