using System;
using System.Collections.Generic;

namespace SistemaEnergiasRenovables;

interface IActivacion
{
    void Activar();
    void Desactivar();
}

interface IEstado
{
    void MostrarEstado();
}




// Clase base
public abstract class InstalacionRenovable : IActivacion, IEstado
{
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public bool Estado { get; set; }
    public string Parametros { get; set; }

    public InstalacionRenovable(string nombre, string tipo)
    {
        Nombre = nombre;
        Tipo = tipo;
        Estado = false;
        Parametros = string.Empty;
    }

    public virtual void Activar()
    {
        Estado = true;
        Console.WriteLine($"{Nombre} ha sido activada.");
    }

    public virtual void Desactivar()
    {
        Estado = false;
        Console.WriteLine($"{Nombre} está en mantenimiento.");
    }

    public void AgregarParametro(string parametro)
    {
        Parametros += $"{parametro}\n";
        Console.WriteLine($"Parámetro agregado: {parametro}");
    }

    public virtual void MostrarEstado()
    {
        Console.WriteLine($"\nInstalación: {Nombre} ({Tipo})");
        Console.WriteLine($"Estado: {(Estado ? "Operativa" : "En Mantenimiento")}");
        this.MostrarParametros();
    }

    public void MostrarParametros()
    {
        Console.WriteLine("Parámetros:");
        Console.WriteLine(Parametros);
    }
}

    public class InstalacionSolar : InstalacionRenovable
    {
        public int CapacidadPaneles { get; set; }
        public string Ubicacion { get; set; }

        public InstalacionSolar(string nombre, int capacidad, string ubicacion)
            : base(nombre, "Solar")
        {
            CapacidadPaneles = capacidad;
            Ubicacion = ubicacion;
        }

        public override void Activar()
        {
            base.Activar();
            Console.WriteLine($"Paneles solares activados con {CapacidadPaneles} kWp en {Ubicacion}.");
        }

        public override void MostrarEstado()
        {
            base.MostrarEstado();
            Console.WriteLine($"Capacidad: {CapacidadPaneles} kWp\nUbicación: {Ubicacion}");
            this.MostrarParametros();
        }
    }

    public class InstalacionEolica : InstalacionRenovable
    {
        public int NumeroTurbinas { get; set; }
        public double VelocidadVientoOptima { get; set; }

        public InstalacionEolica(string nombre, int turbinas, double velocidadOptima)
            : base(nombre, "Eólica")
        {
            NumeroTurbinas = turbinas;
            VelocidadVientoOptima = velocidadOptima;
        }

        public override void Activar()
        {
            base.Activar();
            Console.WriteLine($"Instalación eólica con {NumeroTurbinas} turbinas activada. Velocidad óptima: {VelocidadVientoOptima} m/s.");
        }

        public override void MostrarEstado()
        {
            base.MostrarEstado();
            Console.WriteLine($"Turbinas: {NumeroTurbinas}\nVelocidad Óptima: {VelocidadVientoOptima} m/s");
        }
    }

    public class InstalacionHibrida : IActivacion, IEstado
    {
        public InstalacionSolar Solar { get; set; }
        public InstalacionEolica Eolica { get; set; }
        public string SistemasAlmacenamiento { get; set; }

        public InstalacionHibrida(InstalacionSolar solar, InstalacionEolica eolica, string almacenamiento)
        {
            Solar = solar;
            Eolica = eolica;
            SistemasAlmacenamiento = almacenamiento;
        }

        public virtual void Activar()
        {
            Solar.Activar();
            Eolica.Activar();
            Console.WriteLine("Instalación híbrida activada.");
            Console.WriteLine("Sistemas de almacenamiento: " + SistemasAlmacenamiento);
        }

        public virtual void Desactivar()
        {
            Solar.Desactivar();
            Eolica.Desactivar();
            Console.WriteLine("Instalación híbrida desactivada.");
        }

        public virtual void MostrarEstado()
    {
        Solar.MostrarEstado();
        Eolica.MostrarEstado();
        Console.WriteLine("Sistemas de almacenamiento: " + SistemasAlmacenamiento);
    }
    }

    public class InstalacionInteligente : InstalacionHibrida
    {
        public int NivelAutomatizacion { get; set; }
        public string RegistroProduccion { get; set; }

        public InstalacionInteligente(InstalacionSolar solar, InstalacionEolica eolica, string almacenamiento, int nivel)
            : base(solar, eolica, almacenamiento)
        {
            NivelAutomatizacion = nivel;
        }

        public override void Activar()
        {
            base.Activar();
            Console.WriteLine($"Sistema inteligente activado. Nivel de automatización: {NivelAutomatizacion}/10");
        }

        public override void Desactivar()
        {
            base.Desactivar();
            Console.WriteLine("Sistema inteligente desactivado.");
        }

        public override void MostrarEstado()
    {
        base.MostrarEstado();
        Console.WriteLine("Nivel de Automatización: " + NivelAutomatizacion);
        Console.WriteLine("Registro de producción: " + RegistroProduccion);
    }

        public void OptimizarRendimiento()
        {
            Console.WriteLine("Optimizando rendimiento basado en patrones de producción y consumo...");
        }
    }

    public class MenuInteractivo
    {

        private InstalacionSolar? instalacionSolar;
        private InstalacionEolica? instalacionEolica;
        private InstalacionHibrida? instalacionHibrida;
        private InstalacionInteligente? instalacionInteligente;

        List<object> instalaciones = new List<object>();

        public void MostrarMenu()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n--- MENÚ DEL SISTEMA DE ENERGÍAS RENOVABLES ---");
            Console.WriteLine("1. Crear Instalación Solar");
            Console.WriteLine("2. Crear Instalación Eólica");
            Console.WriteLine("3. Crear Instalación híbrida");
            Console.WriteLine("4. Crear Instalación Inteligente");
            Console.WriteLine("5. Activar Instalación");
            Console.WriteLine("6. Desactivar Instalación");
            Console.WriteLine("7. Agregar Parámetro");
            Console.WriteLine("8. Mostrar Estado de Instalaciones");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion)) opcion = -1;

            switch (opcion)
            {
                case 1:
                    CrearSolar();
                    break;
                case 2:
                    CrearEolica();
                    break;
                case 3:
                    CrearHibrida();
                    break;
                case 4:
                    CrearInteligente();
                    break;
                case 5:
                    Activar();
                    break;
                case 6:
                    Desactivar();
                    break;
                case 7:
                    AgregarParametro();
                    break;
                case 8:
                    MostrarEstados();
                    break;
                case 0:
                    Console.WriteLine("Saliendo del sistema...");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }
        } while (opcion != 0);
    }

        private void CrearSolar()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Capacidad (kWp): ");
            int capacidad = int.Parse(Console.ReadLine());
            Console.Write("Ubicación: ");
            string ubicacion = Console.ReadLine();
            Console.Write("Parámetros: ");
            string parametros = Console.ReadLine();
            instalacionSolar = new InstalacionSolar(nombre, capacidad, ubicacion)
            {
                Parametros = parametros
            };
            instalaciones.Add(instalacionSolar);
            Console.WriteLine($"Instalación solar '{nombre}' creada.");
        }

    private void CrearEolica()
    {
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Nº de turbinas: ");
        int turbinas = int.Parse(Console.ReadLine());
        Console.Write("Velocidad óptima (m/s): ");
        double velocidad = double.Parse(Console.ReadLine());
        Console.Write("Parámetros: ");
        string parametros = Console.ReadLine();
        instalacionEolica = new InstalacionEolica(nombre, turbinas, velocidad)
        {
            Parametros = parametros
        };
        instalaciones.Add(instalacionEolica);
        Console.WriteLine($"Instalación eólica '{nombre}' creada.");
        }

        private void CrearHibrida()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();
            Console.Write("Capacidad (kWp): ");
            int capacidad = int.Parse(Console.ReadLine());
            Console.Write("Ubicación: ");
            string ubicacion = Console.ReadLine();
            Console.Write("Nº de turbinas: ");
            int turbinas = int.Parse(Console.ReadLine());
            Console.Write("Velocidad óptima (m/s): ");
            double velocidad = double.Parse(Console.ReadLine());
            Console.Write("Sistemas de almacenamiento: ");
            string almacenamiento = Console.ReadLine();

            Console.Write("Parámetros instalacion solar: ");
            string parametrosSolar = Console.ReadLine();
            Console.Write("Parámetros instalacion eolica: ");
            string parametrosEolica = Console.ReadLine();

            var instalacionSolar = new InstalacionSolar(nombre, capacidad, ubicacion) {
                Parametros = parametrosSolar
            };
            var instalacionEolica = new InstalacionEolica(nombre, turbinas, velocidad) {
                Parametros = parametrosEolica
            };
            instalacionHibrida = new InstalacionHibrida(instalacionSolar, instalacionEolica, almacenamiento);
            instalaciones.Add(instalacionHibrida);
            Console.WriteLine($"Instalación híbrida '{nombre}' creada.");
        }

    private void CrearInteligente()
    {
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Capacidad (kWp): ");
        int capacidad = int.Parse(Console.ReadLine());
        Console.Write("Ubicación: ");
        string ubicacion = Console.ReadLine();
        Console.Write("Nº de turbinas: ");
        int turbinas = int.Parse(Console.ReadLine());
        Console.Write("Velocidad óptima (m/s): ");
        double velocidad = double.Parse(Console.ReadLine());
        Console.Write("Sistemas de almacenamiento: ");
        string almacenamiento = Console.ReadLine();

        var instalacionSolar = new InstalacionSolar(nombre, capacidad, ubicacion);
        var instalacionEolica = new InstalacionEolica(nombre, turbinas, velocidad);
        instalacionInteligente = new InstalacionInteligente(instalacionSolar, instalacionEolica, almacenamiento, capacidad);
        instalaciones.Add(instalacionInteligente);
        Console.WriteLine($"Instalación inteligente '{nombre}' creada.");
    }

    private void Activar()
    {
        MostrarIndices();
        Console.Write("Seleccione instalación: ");
        int i = int.Parse(Console.ReadLine());

        (instalaciones[i] as IActivacion).Activar();
    }

    private void Desactivar()
    {
        MostrarIndices();
        Console.Write("Seleccione instalación: ");
        int i = int.Parse(Console.ReadLine());
        (instalaciones[i] as IActivacion).Desactivar();
    }

    private void AgregarParametro()
    {
        // var instalacionesRenovables = instalaciones.FindAll(inst => inst is InstalacionRenovable);
        var instalacionesRenovables = from inst in instalaciones
                                       where inst is InstalacionRenovable
                                       select inst;
        instalacionesRenovables = instalacionesRenovables.ToList();

        int index = 1;
        foreach (var item in instalacionesRenovables)
        {
            Console.WriteLine($"{index}. {((InstalacionRenovable)item).Nombre}");
            index++;
        }

        Console.Write("Seleccione instalación: ");
        int i = int.Parse(Console.ReadLine());
        Console.Write("Parámetro: ");
        string p = Console.ReadLine();
        ((InstalacionRenovable)instalaciones[i - 1]).AgregarParametro(p);
    }

        private void MostrarEstados()
        {
            Console.WriteLine("\n--- ESTADO DE LAS INSTALACIONES ---");

            foreach (var inst in instalaciones)
                (inst as IEstado)?.MostrarEstado();
        }

        private void MostrarIndices()
        {
            Console.WriteLine("\n--- INSTALACIONES ---");
            int index = 0;
            foreach (var inst in instalaciones)
            {
            if (inst is InstalacionRenovable) {
                Console.WriteLine($"{index}. {((InstalacionRenovable)inst).Nombre}");
            } else if (inst is InstalacionHibrida) {
                Console.WriteLine($"{index}. {((InstalacionHibrida)inst).Solar.Nombre} + {((InstalacionHibrida)inst).Eolica.Nombre}");
            }
            index++;
        }
    }
}

    class Program
    {
        static void Main(string[] args)
        {
            MenuInteractivo menu = new MenuInteractivo();
            menu.MostrarMenu();
        }
    }

