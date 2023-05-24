using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;
using University.BL.Repositories;

// Esta es la clase a la que invoca el controlador.

namespace University.BL.Services.Implements
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly ICourseRepository courseRepository;
        public CourseService(ICourseRepository courseRepository) : base(courseRepository)
        {
            this.courseRepository = courseRepository; // Se coloca this. para que sepa que es la propiedad courseRepository y no el parámetro que está recibiendo.
        }

        public async Task<bool> DeleteCheckRelatedEntity(int id)
        {
            return await courseRepository.DeleteCheckRelatedEntity(id);
        }
    }
}
