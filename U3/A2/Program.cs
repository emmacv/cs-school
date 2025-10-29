
// Excepción personalizada para saldo insuficiente.

public class SaldoInsuficienteException : Exception
{
  public SaldoInsuficienteException() : base("El saldo es insuficiente para realizar esta operación.") { }
  public SaldoInsuficienteException(string mensaje) : base(mensaje) { }
}

// Excepción personalizada para límite de sobregiro excedido.
public class LimiteSobregiroExcedidoException : Exception
{
    public LimiteSobregiroExcedidoException() : base("El límite de sobregiro de -2000 pesos ha sido excedido.") { }
    public LimiteSobregiroExcedidoException(string mensaje) : base(mensaje) { }
}

// Clase base
public class CuentaBancaria
{
    public string Titular { get; set; }
    public decimal Saldo { get; protected set; }

    public CuentaBancaria(string titular, decimal saldoInicial)
    {
        Titular = titular;
        Saldo = saldoInicial;
    }


    // Método para depositar dinero en la cuenta.
    public virtual void Depositar(decimal cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a depositar debe ser mayor a cero.");

        Saldo += cantidad;
    }

    // Método para retirar dinero de la cuenta.
    public virtual void Retirar(decimal cantidad)
    {
      if (cantidad <= 0)
        throw new ArgumentException("La cantidad a retirar debe ser mayor a cero.");

      if (cantidad > Saldo)
        // Lanzamos una excepción personalizada si no hay saldo suficiente.
        throw new SaldoInsuficienteException();

      Saldo -= cantidad;
    }
}

// Nueva clase CuentaPremium
public class CuentaPremium(string titular, decimal saldoInicial) : CuentaBancaria(titular, saldoInicial)
{
    private const decimal LIMITE_SOBREGIRO = -2000m;

  public override void Retirar(decimal cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a retirar debe ser mayor a cero.");

        decimal saldoResultante = Saldo - cantidad;

        if (saldoResultante < LIMITE_SOBREGIRO)
            throw new LimiteSobregiroExcedidoException();

        Saldo = saldoResultante;
    }
}

class Program
{
    static void Main(string[] args)
    {
        CuentaBancaria cuenta = null;

        while (true)
        {
            Console.WriteLine("\nBienvenido al Banco BBVM");
            Console.WriteLine("1. Crear cuenta normal");
            Console.WriteLine("2. Crear cuenta premium");
            Console.WriteLine("3. Depositar dinero");
            Console.WriteLine("4. Retirar dinero");
            Console.WriteLine("5. Mostrar estado de la cuenta");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();

            try
            {
                switch (opcion)
                {
                    case "1":
                        Console.Write("Ingrese el nombre del titular: ");
                        string titularNormal = Console.ReadLine();
                        cuenta = new CuentaBancaria(titularNormal, 0m);
                        Console.WriteLine("Cuenta normal creada correctamente.");
                        break;

                    case "2":
                        Console.Write("Ingrese el nombre del titular: ");
                        string titularPremium = Console.ReadLine();
                        cuenta = new CuentaPremium(titularPremium, 0m);
                        Console.WriteLine("Cuenta premium creada correctamente.");
                        break;

                    case "3":
                        ValidarCuentaCreada(cuenta);
                        Console.Write("Ingrese la cantidad a depositar: ");
                        decimal deposito = decimal.Parse(Console.ReadLine());
                        cuenta.Depositar(deposito);
                        Console.WriteLine("Depósito exitoso.");
                        break;

                    case "4":
                        ValidarCuentaCreada(cuenta);
                        Console.Write("Ingrese la cantidad a retirar: ");
                        decimal retiro = decimal.Parse(Console.ReadLine());
                        cuenta.Retirar(retiro);
                        Console.WriteLine("Retiro exitoso.");
                        break;

                    case "5":
                        ValidarCuentaCreada(cuenta);
                        Console.WriteLine($"Titular: {cuenta.Titular}");
                        Console.WriteLine($"Saldo actual: {cuenta.Saldo} USD");
                        break;

                    case "6":
                        Console.WriteLine("Gracias por visitarnos. ¡Hasta pronto!");
                        return;

                    default:
                        Console.WriteLine("Opción inválida. Intente de nuevo, por favor.");
                        break;
                }
            }
            catch (SaldoInsuficienteException ex)
            {
                Console.WriteLine($"Error de saldo: {ex.Message}");
            }
            catch (LimiteSobregiroExcedidoException ex)
            {
                Console.WriteLine($"Error de sobregiro: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error de argumento: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Formato de número inválido.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Operación finalizada.");
            }
        }
    }

    static void ValidarCuentaCreada(CuentaBancaria cuenta)
    {
        if (cuenta == null)
            throw new InvalidOperationException("Debe crear una cuenta antes de realizar operaciones.");
    }
}
