using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web;
using Web.Models;   

var builder = WebApplication.CreateBuilder(args);

//Se agrega la opcion de crear una base de datos en memoria
//Solo sirve para la memoria virtual
//builder.Services.AddDbContext<contextTareas>(p=> p.UseInMemoryDatabase("TareasDb"));

//Opcion para la base de datos SQL
//Al final se termina eliminando para agregar la conexion al archivo json
//builder.Services.AddSqlServer<contextTareas>("Data Source=(local); Initial Catalog=TareasDb;TrustServerCertificate=True;Integrated Security=True");

//Se crea el servicio para que acceda a la base de datos usando la configuracion dada en el Json
builder.Services.AddSqlServer<contextTareas>(builder.Configuration.GetConnectionString("conectionTareas"));

var app = builder.Build();


app.MapGet("/", () => "Hola Buenas Tardes");

//Se crea la base de datos en memoria para realizar las pruebas
app.MapGet("/dbconexion", ([FromServices] contextTareas DbContext)=>{
    DbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria virtual: " + DbContext.Database.IsInMemory());
});

//Mostrar datos de la tabla de tareas
app.MapGet("api/tareas", ([FromServices] contextTareas DbContext)=>{
    return Results.Ok(DbContext.Tareas.Include(p=>p.Categoria));
});

//Mostrar datos de la tabla de categoria
app.MapGet("api/categoria", ([FromServices] contextTareas DbContext)=>{
    return Results.Ok(DbContext.Categorias);
});

//Filtrar tareas con nivel de dificultdad alto
app.MapGet("api/filtrarAlta",([FromServices] contextTareas DbContext)=>{
    //Con el parametro Include intenta traer todos los otros parametros de la tabla Categoria
    return Results.Ok(DbContext.Tareas.Include(p=>p.Categoria).Where(p=>p.PrioridadTarea==Prioridad.Alta));
});

//Filtrar tareas con nivel de dificultad medio
app.MapGet("api/filtrarMedio",([FromServices] contextTareas DbContext)=>{
    return Results.Ok(DbContext.Tareas.Include(p=>p.Categoria).Where(p=>p.PrioridadTarea==Prioridad.Media));
});

//Filtrar tareas con nivel de dificultad bajo
app.MapGet("api/filtrarBaja",([FromServices] contextTareas DbContext)=>{
    return Results.Ok(DbContext.Tareas.Include(p=>p.Categoria).Where(p=>p.PrioridadTarea==Prioridad.Baja));
});

//Agregar los datos desde el fluent API
//En este caso si es necesario usar el async dentro del MapPost debido a que se usa un await dentro de la operación
//El await y async son usados ya que son necesarios para que la variable espere hasta recibir los datos y no los envie al instante para
//continuar con la ejecucion del codigo
app.MapPost("api/agregarTarea", async ([FromServices] contextTareas DbContext, [FromBody] modeloTarea tarea)=>{
    tarea.TareaId=Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await DbContext.AddAsync(tarea);

    //Hay que agregar el metodo SaveChanges para poder agregar los datos a la base de datos
    await DbContext.SaveChangesAsync();
    return Results.Ok("Registro Agregado");
});

//Se aplican los metodos para la actualización de los datos de Tarea
//El id del registro que se desea actualizar se debe de poner en la direccion de la aplicacion
app.MapPut("api/actualizarTarea/{id}", async ([FromServices] contextTareas DbContext, [FromBody] modeloTarea tarea,[FromRoute] Guid id)=>{
    //Definicion de la variable encargada de encontrar el id de la tarea y poder editarla
    var tareaActual = DbContext.Tareas.Find(id);

    if(tareaActual != null){
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.Descripcion = tarea.Descripcion;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        await DbContext.SaveChangesAsync();
        return Results.Ok("Registro Editado");
    }
    return Results.NotFound("El registro no se encontró en la base de datos");
});

//Implementación del meetodo para poder eliminar un registro
app.MapDelete("api/eliminarTarea/{id}", async ([FromServices] contextTareas DbContext, [FromRoute] Guid id)=>{
    //definicion dela variable encargada de encontrar el id de la tarea para poder eliminarla
    var tareaActual = DbContext.Tareas.Find(id);

    if(tareaActual != null){
        DbContext.Remove(tareaActual);
        await DbContext.SaveChangesAsync();
        return Results.Ok("Registros Eliminados");
    }
    return Results.NotFound("El registro no se encontró en la base de datos");
});

app.Run(); 