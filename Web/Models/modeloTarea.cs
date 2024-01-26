using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{

    //Se puede especificar el nombre de la tabla en la base de datos con la notación Table
    //[Table("Tarea")]
    public class modeloTarea
    {
        //[Key]
        public Guid TareaId {get; set;}
        //Relacion con el modelo de Categoria mediante claves foraneas
        //[ForeignKey("CategoriaId")]
        public Guid CategoriaId {get; set;}
        //[Required]
        //[MaxLength(200)]
        public string Titulo {get; set;}
        public string Descripcion {get; set;}
        //Se hace referencia a la clase especial "Prioridad"  que se crea mas abajo
        public Prioridad PrioridadTarea{get; set;}

        public DateTime FechaCreacion {get; set;}

        //Se hace referencia a la otra clase para poder acceder a su metodo "Categoria"
        public virtual modeloCategoria Categoria {get; set;}

        //El atributo notMapped funciona para que no se tome en cuenta al momento de ingresar la información a la base de datos
        //[NotMapped]
        public string Resumen{get; set;}
    }
    
    //Si se desea llamar a la funcion publica en otro contexto será necesario que esté dentro del mismo espacio
    public enum Prioridad {
    Baja, 
    Media,
    Alta
    }
}

