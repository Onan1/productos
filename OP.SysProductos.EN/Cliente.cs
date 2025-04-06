using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OP.SysProductos.EN
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(150)]
        public string Correo { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; }

        // Relación con Ventas
        public virtual ICollection<Venta>? Ventas { get; set; }

    }
}
