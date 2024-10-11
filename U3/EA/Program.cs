using System;

class HotelReservationSystem
{
    static void Main()
    {
        bool continuar = true;

        do
        {
            Console.Clear();
            Console.WriteLine(@"Bienvenido al sistema de reservaciones del ""Hotel Oasis Tropical""");
            Console.WriteLine("1. Realizar una nueva reserva");
            Console.WriteLine("2. Salir");
            int opcion = IngresarOpcion();

            switch (opcion)
            {
                case 1:
                    RealizarReserva();
                    break;
                case 2:
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Opción no válida, por favor intente de nuevo.");
                    break;
            }
        }
        while (continuar);
    }

    static void RealizarReserva()
    {
        decimal costoHabitacion = SeleccionarTipoHabitacion();
        int noches = IngresarDuracionEstancia();
        decimal costoHospedaje = costoHabitacion * noches;
        decimal costoServicios = SeleccionarServiciosAdicionales(noches);
        decimal descuento = 0m;

        decimal costoTotalEstancia = costoHospedaje + costoServicios;

        // Aplicar descuentos por duración de la estancia
        if (noches > 4)
        {
            descuento = AplicarDescuento(noches, costoHabitacion);
            Console.WriteLine($"\nDescuento aplicado: ${descuento:F2}");
            costoTotalEstancia -= descuento;
        }

        // Mostrar resumen de la reserva
        MostrarResumen(costoHabitacion, noches, costoServicios, costoTotalEstancia, descuento);
    }

    static int IngresarDuracionEstancia()
    {
        Console.Write("Cuantas noches?: ");
        return int.Parse(Console.ReadLine());
    }

    static decimal SeleccionarTipoHabitacion()
    {
        Console.WriteLine("\nSeleccione el tipo de habitación:");
        Console.WriteLine("1. Estándar - $1000/noche");
        Console.WriteLine("2. Deluxe - $1500/noche");
        Console.WriteLine("3. Suite - $2000/noche");
        int opcion = IngresarOpcion();

        return opcion switch
        {
            1 => 1000m,
            2 => 1500m,
            3 => 2000m,
            _ => 1000m,
        };
    }

    static decimal SeleccionarServiciosAdicionales(int noches)
    {
        decimal costoServicios = 0m;
        bool continuarSeleccion = true;

        while (continuarSeleccion)
        {
            Console.WriteLine("\nSeleccione un servicio adicional (0 para finalizar):");
            Console.WriteLine("1. Desayuno - $100/noche");
            Console.WriteLine("2. SPA - $200/día");
            Console.WriteLine("3. Traslado al aeropuerto - $100");
            int opcion = IngresarOpcion();

            switch (opcion)
            {
                case 1:
                    costoServicios += 100m * noches;
                    break;
                case 2:
                    costoServicios += 200m * noches;
                    break;
                case 3:
                    costoServicios += 100m;
                    break;
                case 0:
                    continuarSeleccion = false;
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
        return costoServicios;
    }

    static int IngresarOpcion()
    {
        bool error = false;
        int opcion = -1;

        do
        {
            try
            {
                Console.Write("Opción: ");
                opcion = int.Parse(Console.ReadLine());

                error = false;

            }
            catch (System.Exception)
            {
                Console.WriteLine("Opción no válida, por favor intente de nuevo.");

                error = true;
            }
        } while (error);

        return opcion;
    }

    static decimal AplicarDescuento(int noches, decimal costoHabitacion)
    {
        decimal descuentoPorNoche = costoHabitacion * 0.15m;
        return descuentoPorNoche * noches;
    }

    static void MostrarResumen(decimal costoHabitacion, int noches, decimal costoServicios, decimal costoTotal, decimal descuento)
    {
        Console.WriteLine("\n--- Resumen de la Reservación ---");
        Console.WriteLine($"Tipo de habitación: ${costoHabitacion}/noche");
        Console.WriteLine($"Duración de la estancia: {noches} noches");
        Console.WriteLine($"Costo de la habitación: ${costoHabitacion * noches:F2}");
        Console.WriteLine($"Costo de servicios adicionales: ${costoServicios:F2}");
        Console.WriteLine($"Descuento: ${descuento:F2}");

        Console.WriteLine($"Costo total de la estancia: ${costoTotal:F2}");
        Console.WriteLine("----------------------------------\n");

        Console.Write("Presione cualquier tecla para volver al menú principal...");
        Console.ReadKey();
    }
}
