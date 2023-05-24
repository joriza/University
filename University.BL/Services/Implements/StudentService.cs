using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;
using University.BL.Repositories;



namespace University.BL.Services.Implements
{
    public class StudentService : GenericService<Student>
    {
        private readonly IStudentRepository studentRepository;
        public StudentService(IStudentRepository studentRepository) :base(studentRepository)
        {
            this.studentRepository = studentRepository; // Se coloca this. para que sepa que es la propiedad courseRepository y no el parámetro que está recibiendo.
        }

        public async Task<bool> DeleteCheckRelatedEntity(int id)
        {
            return await studentRepository.DeleteCheckRelatedEntity(id);
        }
    }
}
