using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.BL.DTOs
{
    public class CourseDTO
    {
        [Required(ErrorMessage = "El campo CouseID es obligatorio")]
        public int CourseID { get; set; }
        [Required(ErrorMessage = "El campo Titulo es obligatorio")]
        [StringLength(50)]
        public string Title { get; set; }
        [Required(ErrorMessage = "El campo Creditos es obligatorio")]
        public int Credits { get; set; }
    }

    public class CourseDTOid
    {
        [Required(ErrorMessage = "El campo CouseID es obligatorio")]
        public int CourseID { get; set; }
    }

}
