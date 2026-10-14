namespace NovaWarehouse.Repositories;

/// <summary>
/// Stores and retrieves entities of type <typeparamref name="T"/>, hiding where they are kept
/// (memory, a file, a database...).
/// </summary>
/// <remarks>
/// <c>where T : class</c> restricts <typeparamref name="T"/> to reference types,
/// so <c>T?</c> means "may be null" (for a value type such as <c>int</c> it would just be <c>int</c>).
/// </remarks>
internal interface IRepository<T> where T : class
{
    void Add(T item);

    // Con fichero, modificar el objeto devuelto por GetById no cambia
    // lo guardado, porque es una copia leída del fichero. Hay que pedir al repositorio que lo guarde
    /// <summary>
    /// Saves the changes made to <paramref name="item"/>, replacing the stored entity with the same identifier.
    /// </summary>
    void Update(T item);

    /// <returns>The entity with that identifier, or <see langword="null"/> if it does not exist.</returns>
    T? GetById(string id);

    IReadOnlyList<T> GetAll();
}
