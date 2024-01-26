using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web.Models
{ 

    //[Table("Categoria")]
    public class modeloCategoria
    {
        //El Guid significa Identificador Unico Global
        //[Key] //Con el data annotations Key se puede espeificar de mejor manera cual será la clave principal
        public Guid CategoriaId {get;set;}

        //[Required]
        //[MaxLength(150)]
        public string Nombre {get;set;}

        public string Descripcion {get;set;}


        //El JsonIgnore funciona para poder hacer que ignore las posibles colecciones relacionadas, evitando asi los ciclos infinitoss
        [JsonIgnore]
        public virtual ICollection<modeloTarea> Tareas {get;set;}
    }
}