using System;

Console.WriteLine("Hello, World!");

string nombre = "Asier";

Console.WriteLine($"Hola mi nombre es{nombre}");

var quantity = 120;

Persona persona = new Persona();

var persona2 = new Persona(); 

// Casa 

bool isExpress = false;

Console.Write("Package weight (kg): ");
string? input = Console.ReadLine();
double weightKg = double.Parse(input!);

const double baseCost = 3.50;
const double costPerKg = 0.80;
double totalCost = baseCost + (weightKg * costPerKg);

Console.WriteLine("¿Pedido prioritario por 5 € s/n?");
input = Console.ReadLine();

if(input == "s")
{
    isExpress = true;
}

if(isExpress)
{
    totalCost+=5;
}

Console.WriteLine($"Shipping cost: {totalCost:C2}");

class Persona{}