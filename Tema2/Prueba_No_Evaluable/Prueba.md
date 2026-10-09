# Prueba no evaluable UT1 - NovaLibrary

**Duración:** 2 horas

**Tipo de proyecto:** aplicación de consola en .NET 10 / C# 14

**Prueba no evaluable:** me permite comprobar qué conceptos habéis adquirido y cualesa no

## Contexto

La biblioteca del barrio quiere dejar de apuntar los préstamos en una libreta. Te piden una primera versión, en consola, de **NovaLibrary**: una aplicación que gestione el catálogo, los socios y los préstamos, y que calcule las multas por devolver tarde.

Es la misma idea que NovaWarehouse, aplicada a otro dominio: si te atascas, piensa en cómo lo resolvimos allí.

---

## Normas generales

1. Crea un proyecto de consola llamado `NovaLibrary`.
2. **Todos los tipos que tengan datos deben ser `class`**. No se permite usar `struct` ni `record`. Los `enum` y las `interface` sí están permitidos.
3. Los identificadores del código (clases, métodos, propiedades, variables) van **en inglés**. Los textos que ve el usuario en consola van **en español**.
4. Respeta las convenciones de nomenclatura de C#: `PascalCase` para tipos, métodos y propiedades; `camelCase` para parámetros y variables locales; prefijo `I` en las interfaces; `_camelCase` para los campos privados.
5. *Nullable reference types* activado (viene así por defecto). El proyecto debe compilar **sin warnings**.
6. Documenta con XML doc comments (`///`) los métodos públicos de la clase `Library`.
7. La aplicación **nunca debe cerrarse por una excepción**: todos los errores se capturan y se muestran al usuario con un mensaje claro.

---

## Estructura del proyecto

```text
NovaLibrary/
├── Models/
├── Exceptions/
├── Repositories/
├── Extensions/
├── Library.cs
└── Program.cs
```

Usa `namespace` acordes a cada carpeta (`NovaLibrary.Models`, `NovaLibrary.Exceptions`, …).

---

## 1. Modelos (`Models/`)

### 1.1. Enums

| Enum | Valores |
|---|---|
| `MemberType` | `Student`, `Teacher` |
| `LoanStatus` | `Active`, `Returned` |

### 1.2. Interfaz `ILoanable`

Representa a los materiales que **se pueden llevar a casa**. Debe exponer:

- `LoanDays`: número de días que dura el préstamo.
- `CalculateFine(int daysLate)`: multa, en euros, por devolver con `daysLate` días de retraso.

### 1.3. Materiales: `LibraryItem` y sus subclases

`LibraryItem` es una **clase abstracta**: no existe un "material" genérico, siempre es un libro, una revista o un DVD.

Propiedades comunes de `LibraryItem`:

| Propiedad | Tipo | Reglas |
|---|---|---|
| `Id` | `string` | No cambia una vez creado |
| `Title` | `string` | No puede estar vacío ni ser solo espacios: `ArgumentException` |
| `Year` | `int` | Mayor que 0 y no posterior al año actual: `ArgumentOutOfRangeException` |

`LibraryItem` tiene un método **`GetDescription()`** que devuelve una línea de texto con `Id`, `Title` y `Year`. Cada subclase **lo sobrescribe** para añadir sus propios datos (reutilizando la descripción de la clase base, no duplicándola).

Subclases:

| Clase | Propiedades propias (y reglas) | ¿Implementa `ILoanable`? | Días de préstamo | Multa |
|---|---|---|---|---|
| `Book` | `Author` (`string`, no vacío), `Pages` (`int`, > 0) | Sí | 21 | 0,20 € por día de retraso |
| `Dvd` | `DurationMinutes` (`int`, > 0) | Sí | 7 | 1,00 € por día de retraso, **con un máximo de 10,00 €** |
| `Magazine` | `IssueNumber` (`int`, > 0) | **No** (solo consulta en sala) | - | - |

Ejemplos de descripción:

```text
[B-001] El Quijote (1605) - Miguel de Cervantes, 1376 págs.
[D-002] Interstellar (2014) - 169 min
[R-001] National Geographic (2026) - nº 310
```

### 1.4. `Member` (socio)

| Propiedad | Tipo | Reglas |
|---|---|---|
| `Id` | `string` | Formato `M-001`, `M-002`… |
| `Name` | `string` | No puede estar vacío ni ser solo espacios: `ArgumentException` |
| `Type` | `MemberType` | |
| `Email` | `string?` | **Opcional**. Si no tiene, al mostrarlo se escribe `sin email` |
| `MaxLoans` | `int` | **Calculada** a partir del tipo: `Student`: 3, `Teacher`: 6 |

### 1.5. `Loan` (préstamo)

| Propiedad | Tipo | Descripción |
|---|---|---|
| `Id` | `string` | Formato `L-0001`, `L-0002`… |
| `ItemId` | `string` | Material prestado |
| `MemberId` | `string` | Socio que se lo lleva |
| `LoanDate` | `DateTime` | Fecha del préstamo |
| `DueDate` | `DateTime` | Fecha límite de devolución: `LoanDate` + días de préstamo del material |
| `ReturnDate` | `DateTime?` | `null` mientras no se haya devuelto |
| `Status` | `LoanStatus` | `Active` al crearse, `Returned` al devolverse |
| `Fine` | `decimal` | Multa cobrada al devolver (0 si no hubo retraso) |

Además, `Loan` debe poder responder si **está vencido en una fecha dada**: está activo y esa fecha es posterior a `DueDate`.

---

## 2. Excepciones (`Exceptions/`)

Crea estas excepciones propias del dominio. Cada una debe guardar en **propiedades** los datos que explican el error y generar un mensaje descriptivo.

| Excepción | Cuándo se lanza | Datos que guarda |
|---|---|---|
| `ItemNotFoundException` | No existe un material con ese Id | `ItemId` |
| `MemberNotFoundException` | No existe un socio con ese Id | `MemberId` |
| `ItemNotAvailableException` | El material no se puede prestar: es de solo consulta o ya está prestado | `ItemId`, `Reason` |
| `LoanLimitExceededException` | El socio ya tiene tantos préstamos activos como su máximo | `MemberId`, `MaxLoans` |

Para el resto de situaciones usa las excepciones de .NET que correspondan (`ArgumentException`, `ArgumentOutOfRangeException`, `InvalidOperationException`).

---

## 3. Repositorios (`Repositories/`)

1. Una interfaz **genérica** `IRepository<T>` con las operaciones:
    - `Add(T item)`
    - `Update(T item)`
    - `GetById(string id)`: devuelve el elemento o `null` si no existe
    - `GetAll()`: lista de solo lectura
2. Tres implementaciones **en memoria**:
    - `InMemoryItemRepository` (`IRepository<LibraryItem>`)
    - `InMemoryMemberRepository` (`IRepository<Member>`)
    - `InMemoryLoanRepository` (`IRepository<Loan>`)

---

## 4. La clase `Library`

Es la clase que contiene la lógica de negocio. **Recibe los tres repositorios por el constructor** y solo trabaja con la interfaz `IRepository<T>`: no sabe qué implementación concreta está usando.

| Miembro | Responsabilidad |
|---|---|
| `Name` | Nombre de la biblioteca |
| `Items`, `Members`, `Loans` | Listas de solo lectura con todos los materiales, socios y préstamos |
| `AddItem(LibraryItem item)` | Añade un material al catálogo |
| `RegisterMember(string name, MemberType type, string? email)` | Da de alta un socio generando su Id (`M-004`, `M-005`…) y lo devuelve |
| `GetItem(string id)` | Devuelve el material. Si no existe: `ItemNotFoundException` |
| `GetMember(string id)` | Devuelve el socio. Si no existe: `MemberNotFoundException` |
| `FindActiveLoan(string itemId)` | Devuelve el préstamo activo de ese material, o `null` si no está prestado |
| `SearchByTitle(string text)` | Materiales cuyo título contiene `text`, **sin distinguir mayúsculas y minúsculas** |
| `LendItem(string memberId, string itemId, DateTime loanDate)` | Crea el préstamo, lo guarda y lo devuelve |
| `ReturnItem(string itemId, DateTime returnDate)` | Registra la devolución, calcula la multa, la guarda y devuelve el préstamo actualizado |

### Reglas de `LendItem`

Se comprueban **en este orden**; en cuanto una falla, se lanza la excepción y no se crea nada:

1. El socio existe: si no, `MemberNotFoundException`.
2. El material existe: si no, `ItemNotFoundException`.
3. El material se puede llevar a casa (`ILoanable`): si no, `ItemNotAvailableException` con motivo *"solo consulta en sala"*.
4. El material no está ya prestado: si no, `ItemNotAvailableException` con motivo *"ya está prestado"*.
5. El socio tiene menos préstamos activos que su `MaxLoans`: si no, `LoanLimitExceededException`.

Si todo es correcto, el préstamo se crea con Id correlativo (`L-0001`, `L-0002`…), estado `Active` y `DueDate` calculada según el tipo de material.

### Reglas de `ReturnItem`

1. Si el material no existe: `ItemNotFoundException`.
2. Si el material no tiene ningún préstamo activo: `InvalidOperationException`.
3. Si `returnDate` es anterior a `LoanDate`: `ArgumentException`.
4. Días de retraso = días entre `DueDate` y `returnDate` (0 si se devuelve a tiempo). La multa la calcula **el propio material**.
5. El préstamo queda con `ReturnDate`, `Status = Returned` y `Fine` rellenos, y se guarda en el repositorio.

---

## 5. Extension methods (`Extensions/`)

Crea una clase de extensión con, al menos, el método:

- `Overdue(DateTime today)` sobre una colección de `Loan`: devuelve los préstamos vencidos en esa fecha.

Úsalo en el listado de préstamos activos y en las estadísticas.

---

## 6. `Program.cs`

### 6.1. Datos iniciales

`Program` decide qué repositorios se usan (los de memoria), crea la `Library` y carga estos datos **usando los métodos de `Library`**:

**Materiales**

| Id | Tipo | Título | Año | Dato propio |
|---|---|---|---|---|
| `B-001` | Book | El Quijote | 1605 | Miguel de Cervantes, 1376 págs. |
| `B-002` | Book | Clean Code | 2008 | Robert C. Martin, 464 págs. |
| `B-003` | Book | C# in Depth | 2019 | Jon Skeet, 528 págs. |
| `B-004` | Book | Cien años de soledad | 1967 | Gabriel García Márquez, 471 págs. |
| `R-001` | Magazine | National Geographic | 2026 | nº 310 |
| `R-002` | Magazine | Muy Interesante | 2026 | nº 540 |
| `D-001` | Dvd | El señor de los anillos | 2001 | 178 min |
| `D-002` | Dvd | Interstellar | 2014 | 169 min |

**Socios**

| Id | Nombre | Tipo | Email |
|---|---|---|---|
| `M-001` | Ana García | Student | ana@correo.es |
| `M-002` | Luis Pérez | Teacher | *(sin email)* |
| `M-003` | Marta Ruiz | Student | marta@correo.es |

**Préstamos**

| Socio | Material | Fecha de préstamo |
|---|---|---|
| `M-001` | `B-002` | Hace 30 días (estará vencido) |
| `M-002` | `D-002` | Hace 2 días |

### 6.2. Menú

El menú se repite hasta que el usuario elige salir. Una opción inválida muestra un aviso y vuelve a mostrar el menú.

```text
=== NOVALIBRARY ===
1. Ver catálogo
2. Buscar por título
3. Alta de socio
4. Prestar material
5. Devolver material
6. Préstamos activos
7. Estadísticas
0. Salir
```

| Opción | Qué hace |
|---|---|
| **1. Ver catálogo** | Lista todos los materiales con su descripción y su estado: `Disponible`, `Prestado hasta dd/MM/yyyy` o `Solo consulta en sala` |
| **2. Buscar por título** | Pide un texto y muestra los materiales que coinciden. Si no hay ninguno, lo indica |
| **3. Alta de socio** | Pide nombre, tipo (`Student`/`Teacher`) y email (*Intro para ninguno*). Muestra el Id asignado. Un tipo inválido no debe lanzar excepción: se avisa y se vuelve al menú |
| **4. Prestar material** | Pide Id de socio e Id de material. El préstamo se hace con fecha de hoy. Muestra el Id del préstamo y la fecha límite de devolución |
| **5. Devolver material** | Pide Id de material y fecha de devolución en formato `dd/MM/yyyy` (*Intro para hoy*). Una fecha mal escrita no debe lanzar excepción. Muestra los días de retraso y la multa |
| **6. Préstamos activos** | Lista los préstamos activos con: Id, título del material, nombre del socio y fecha límite. Los vencidos se marcan con `⚠ VENCIDO` |
| **7. Estadísticas** | Calculadas con LINQ: nº de materiales por tipo; los 3 materiales más prestados (título y nº de préstamos, contando también los devueltos); total recaudado en multas; nº de préstamos vencidos hoy |

**Gestión de errores en el menú**: cada excepción propia se captura **por separado** y muestra un mensaje en español con sus datos (por ejemplo: *"Préstamo rechazado: Ana García ya tiene 3 préstamos activos (máximo 3)."*). Cualquier otro error se captura al final con un mensaje genérico.

### 6.3. Registro de operaciones

Cada vez que se intenta **prestar** o **devolver**, se añade una línea al fichero `library.log` con la fecha y hora, la operación y su resultado (el Id del préstamo si ha ido bien, o el mensaje del error si ha fallado). Ejemplo:

```text
2026-10-09T10:15:02 LEND   M-001 B-001 OK L-0003
2026-10-09T10:15:40 LEND   M-001 D-001 ERROR Member 'M-001' has reached the limit of 3 loans.
2026-10-09T10:16:05 RETURN B-002 OK L-0001 fine 1.80
```

El fichero no se sobrescribe entre ejecuciones y **debe cerrarse correctamente aunque la operación lance una excepción**.

---

## 7. Casos de prueba

Ejecuta estos pasos **en orden**, justo después de arrancar la aplicación. "Hoy" es la fecha del día en que hagas la prueba.

| # | Acción | Resultado esperado |
|---|---|---|
| 1 | Ver catálogo | `B-002` y `D-002` prestados; `R-001` y `R-002` solo consulta en sala; el resto disponibles |
| 2 | Prestar `R-001` a `M-003` | Rechazado: solo consulta en sala |
| 3 | Prestar `B-002` a `M-003` | Rechazado: ya está prestado |
| 4 | Prestar `B-001` a `M-999` | Rechazado: el socio no existe |
| 5 | Prestar `X-001` a `M-003` | Rechazado: el material no existe |
| 6 | Prestar `B-001` y luego `B-003` a `M-001` | Préstamos `L-0003` y `L-0004` creados |
| 7 | Prestar `D-001` a `M-001` | Rechazado: límite de 3 préstamos alcanzado |
| 8 | Préstamos activos | 4 préstamos; `L-0001` marcado como vencido |
| 9 | Devolver `B-001` con fecha de ayer | Error: la fecha es anterior al préstamo |
| 10 | Devolver `B-002` con fecha de hoy | 9 días de retraso, multa **1,80 €** |
| 11 | Prestar `D-001` a `M-001` | Ahora sí: préstamo `L-0005` |
| 12 | Devolver `D-002` con fecha de dentro de 20 días | 15 días de retraso, multa **10,00 €** (tope del DVD) |
| 13 | Devolver `B-004` | Error: no está prestado |
| 14 | Prestar `B-002` a `M-003` | Préstamo `L-0006` |
| 15 | Alta de socio con nombre vacío | Error de validación; no se crea el socio |
| 16 | Alta de socio `Carlos Díaz`, `Teacher`, sin email | Socio `M-004` creado |
| 17 | Buscar por título `the` | Ningún resultado. Buscar `QUIJOTE`: `B-001` |
| 18 | Estadísticas | Book: 4, Magazine: 2, Dvd: 2 · Más prestado: Clean Code (2) · Multas: **11,80 €** · Vencidos hoy: 0 |
| 19 | Revisar `library.log` | Aparecen todos los intentos de préstamo y devolución, también los fallidos |

---

## 8. Orden de trabajo sugerido

| Tiempo | Tarea |
|---|---|
| 0:00 – 0:25 | Enums, `ILoanable`, `LibraryItem` y subclases, `Member`, `Loan` |
| 0:25 – 0:35 | Excepciones |
| 0:35 – 0:45 | `IRepository<T>` y repositorios en memoria |
| 0:45 – 1:15 | `Library` |
| 1:15 – 1:45 | `Program`: datos iniciales, menú y `library.log` |
| 1:45 – 2:00 | Extension method, estadísticas y casos de prueba |

Compila a menudo. Es mejor entregar algo que compila con alguna opción sin terminar que un proyecto completo que no compila.

---

## 9. Ampliaciones opcionales

Solo si has terminado y pasan todos los casos de prueba:

1. **Persistencia de préstamos**: crea `InFileLoanRepository` que guarde los préstamos en `loans.csv`. Cambiar de repositorio debe consistir en cambiar **una sola línea** de `Program`. Cuidado con el formato de las fechas y de los decimales.
2. **Multas pendientes**: el socio acumula las multas en una propiedad que nunca puede ser negativa. Un socio con multas pendientes no puede llevarse nada prestado (nueva excepción). Añade al menú la opción *Pagar multas*.
3. **Repositorio genérico**: sustituye los tres repositorios en memoria por un único `InMemoryRepository<T>`. Pista: ¿qué necesita saber de `T` para implementar `GetById`?

---

## 10. Lista de comprobación

Antes de entregar, revisa que tu proyecto:

- [ ] No contiene ningún `struct` ni `record`.
- [ ] Compila sin errores ni warnings.
- [ ] Valida los datos en las propiedades/constructores de los modelos (encapsulación).
- [ ] Usa una clase abstracta, herencia y sobrescritura (`override`) en los materiales.
- [ ] Usa la interfaz `ILoanable` para distinguir qué materiales se pueden prestar.
- [ ] Usa `enum` para los tipos de socio y los estados del préstamo.
- [ ] Usa `switch` expressions o pattern matching donde tiene sentido.
- [ ] Usa una interfaz genérica para los repositorios y `Library` depende solo de ella.
- [ ] Usa LINQ para búsquedas y estadísticas.
- [ ] Lanza excepciones propias con datos y las captura de la más específica a la más genérica.
- [ ] Usa `TryParse` para leer números, enums y fechas introducidos por el usuario.
- [ ] Trata correctamente los valores que pueden ser `null` (`Email`, `ReturnDate`, `GetById`…).
- [ ] Cierra el fichero de log aunque se produzca una excepción.
- [ ] Incluye al menos un extension method.
- [ ] Documenta con `///` los métodos públicos de `Library`.
- [ ] Respeta las convenciones de nomenclatura de C#.
- [ ] Supera los 19 casos de prueba.