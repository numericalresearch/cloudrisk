// See https://aka.ms/new-console-template for more information
using QuantLibrary;

Console.WriteLine("Hello, World!");


for(int i = -400; i <= 400; i+= 10)
{
    double x = i / 100.0;
    double y = Maths.N(x);
    Console.WriteLine($"{x}, {y}");
}