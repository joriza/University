using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Data;
using University.BL.Models;




namespace University.BL.Repositories.Implements
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly UniversityContext universityContext;
        public StudentRepository(UniversityContext universityContext) : base(universityContext)
        {
            this.universityContext = universityContext;
        }

        public async Task<bool> DeleteCheckRelatedEntity(int id) // Valida que el curso a eliminar no tenga dependencias. La consulta no la hace el genérico sino aqui. Es una validacion propia del CourseRepository.
        {
            var flag = await universityContext.Enrollments.AnyAsync(x => x.CourseID == id); // Consulta si existe ese id en la tabla Enrollments. Si hubiera mas posibles tablas relacionadas se debe verificar en todas ellas.
            return flag;
        }

    }
}
