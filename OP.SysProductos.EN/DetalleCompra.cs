using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.SysProductos.EN
{
    public class DetalleCompra
    {
        [Key]

        public int Id { get; set; }

        public int IdCompra { get; set; }

        [Required(ErrorMessage ="El producto es obligatorio.")]
        [ForeignKey("Producto")]

        public int IdProducto { get; set; }

        [Required(ErrorMessage ="La cantidad es obligatoria,")]
        [Range(1, int.MaxValue, ErrorMessage ="La cantidad debe ser al menos 1.")]

        public int Cantidad { get; set; }

        [Required(ErrorMessage ="El precio unitario es obligatorio.")]
        [Range(0.01, 99999999.99, ErrorMessage ="El subtotal debe ser mayor a 0.")]
        [Column(TypeName ="decimal(10,2)")]

        public decimal SubTotal { get; set; }

        public virtual Compra? Compra { get; set; }

        public virtual Producto? Producto { get; set; }


    }
}
