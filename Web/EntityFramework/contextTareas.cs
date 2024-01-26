using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web
{
    //La clase contextTareas hereda el contexto del entityframework para la aplicacion 
    public class contextTareas: DbContext
    {
        //El DbSet es un set de datos del modelo creado, por lo que representa una tabla en el modelo de Entity Framework
        public DbSet<modeloCategoria> Categorias{get; set;}        
        public DbSet<modeloTarea> Tareas{get;set;}
        //Es un constructor el cual recibe un objeto y lo pasa a la clase DbContext para que pueda realizar la conexion a la base de datos 
        public contextTareas(DbContextOptions<contextTareas> options) :base(options) { }




        //-----------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //EL FLUENT API NO SE USA EN LOS PROYECTOS AUN, ENFOCARSE EN ENTITY FRAMEWORK
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------


        //De esta forma se define el Fluent API dando las mismas categorias a las tablas pero con codigo y no en el modelo
        //Se crea el metodo override para que cuando se ejecute, pueda sobreescribir la funcion del metodo OnModelCreating y haciendo
        //uso del ModelBuilder para crear los modelos con código
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //Constructor para el modelo de Categoria
            modelBuilder.Entity<modeloCategoria>(categoria=>{
                categoria.ToTable("Categoria");
                categoria.HasKey(p=>p.CategoriaId);
                categoria.Property(p=>p.Nombre).IsRequired().HasMaxLength(150);
                categoria.Property(p=>p.Descripcion);
            });

            //Constructor para el modelo de Tarea
            modelBuilder.Entity<modeloTarea>(tarea=>{
                tarea.ToTable("Tarea");
                tarea.HasKey(p=>p.TareaId);
                //Se dice que la clave tiene un ID de categoria debido a la clave foranea, y que ese mismo ID puede tener varias tareas.
                //Se especifica al final además que es una clave Foranea
                tarea.HasOne(p=>p.Categoria).WithMany(p=>p.Tareas).HasForeignKey(p=>p.CategoriaId);
                tarea.Property(p=>p.Titulo).HasMaxLength(200);
                tarea.Property(p=>p.Descripcion);
                tarea.Property(p=>p.PrioridadTarea);
                tarea.Property(p=>p.FechaCreacion);
                //Si no se desea agregar una variable a la base de datos, se debe colocar como Ignore
                tarea.Ignore(p=>p.Resumen);
            });
        }
    }
}
/*
.HasConversion(
                    v=>v.ToString(),
                    v=> (Prioridad)(Enum.Parse(typeof(Prioridad),v)));*/