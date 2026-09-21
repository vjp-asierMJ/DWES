namespace EjercicioFactory.Models;

record Order(string Id, CustomerType Customer, decimal Total, decimal DiscountRate);