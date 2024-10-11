using System;

var p = new Pattern();

Console.WriteLine("Bienvenido");


do
{
  Console.WriteLine("Seleccione la figua de su agrado:");

  var option = Menu.ShowMenu();
  char character = '\0';
  int size = 0;
  bool error = false;

  if (option == 7)
  {
    Console.WriteLine("Adios");
    System.Environment.Exit(1);
  }


  if (option < 1 || option > 6)
  {
    Console.WriteLine("Opcion no válida");
    continue;
  }

  do
  {
    try
    {

      Console.WriteLine("Ingrese el tamaño de la figura");
      size = int.Parse(Console.ReadLine());
      Console.WriteLine("Ingrese el caracter de la figura");
      character = char.Parse(Console.ReadLine());

      error = false;
    }
    catch (System.Exception)
    {

      Console.WriteLine("Ingrese un valor valido");
      error = true;
    }

  } while (error);


  Console.WriteLine("Diseño seleccionado: " + option);

  switch (option)
  {
    case 1:
      p.LeftStraightTriangle(size, character);
      break;
    case 2:
      p.ReversedLeftStraightTriangle(size, character);
      break;
    case 3:
      p.RightStraightTriangle(size, character);
      break;
    case 4:
      p.InvertedRightStraigthTriangle(size, character);
      break;
    case 5:
      p.IsoscelesTriangle(size, character);
      break;
    case 6:
      p.ReversedIsocelesPyramid(size, character);
      break;
  }

  Console.WriteLine("Presione una tecla para continuar");
  Console.ReadKey();
  Console.Clear();
} while (true);


Console.WriteLine("Adios");

public class Pattern
{
  public void LeftStraightTriangle(int n, char c = '*')
  {
    for (int i = 0; i < n; i++)
    {
      for (int j = n; j > i; j--)
        Console.Write(c);
      Console.WriteLine();
    }
  }

  public void ReversedLeftStraightTriangle(int n, char c = '*')
  {
    for (int i = 0; i < n; i++)
    {
      for (int j = 0; j < i; j++)
        Console.Write(' ');
      for (int j = n - i; j >= 1; j--)
        Console.Write(c);


      Console.WriteLine();
    }
  }

  public void RightStraightTriangle(int n, char c = '*')
  {
    for (int i = 0; i < n; i++)
    {
      for (int j = 0; j <= i; j++)
        Console.Write(c);

      Console.WriteLine();
    }
  }

  public void InvertedRightStraigthTriangle(int n, char c = '*')
  {
    for (int i = 0; i < n; i++)
    {
      for (int j = n; j > i; j--)
        Console.Write(c);


      Console.WriteLine();
    }
  }

  public void IsoscelesTriangle(int n, char c = '*')
  {
    var row = 0;
    for (int i = n; i >= 1; i--)
    {
      for (int j = i - 1; j >= 1; j--)
        Console.Write(' ');
      for (int j = 2 * row + 1; j >= 1; j--)
        Console.Write(c);

      row++;

      Console.WriteLine();
    }
  }

  public void ReversedIsocelesPyramid(int n, char c = '*')
  {
    for (int i = 0; i < n; i++)
    {
      for (int j = 0; j < i; j++)
        Console.Write(' ');
      for (int j = 2 * (n - i) - 1; j >= 1; j--)
        Console.Write(c);


      Console.WriteLine();
    }
  }
}

class Menu
{
  static public byte ShowMenu()
  {
    Console.WriteLine(@"
*****   *****        *             *           *         *********
****     ****        **           **          ***         *******
***       ***        ***         ***         *****         *****
**         **        ****       ****        *******         ***
*           *        *****     *****       *********         *
    ");
    Console.WriteLine("Diseño 1  Diseño 2   Diseño 3   Diseño 4   Diseño 5      Diseño 6  Salir");

    Console.Write("Seleccione una opcion: ");

    return byte.Parse(Console.ReadLine());
  }
}
