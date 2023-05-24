using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;
using University.BL.Repositories;
using University.BL.Repositories.Implements;
using University.BL.Services.Implements;

namespace University.API.Controllers
{
    public class StudentsController : ApiController
    {
        private IMapper mapper;
        private static UniversityContext contex_     = UniversityContext.Create();
        private static IStudentRepository repo_       = new StudentRepository(contex_);
        private readonly StudentService studentService = new StudentService(repo_);
        //private readonly StudentService studentService = new StudentService(new StudentRepository(UniversityContext.Create())); //Todo en una linea, arriba está por separado.

        public StudentsController()
        {
            this.mapper = WebApiApplication.MapperConfiguration.CreateMapper();
        }
 
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var students = await studentService.GetAll();
            var studentsDTO = students.Select(x => mapper.Map<StudentDTO>(x));
            
            return Ok(studentsDTO);
        }
/*
        [HttpGet]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var student = await studentService.GetById(id);

            if (student == null)
                return NotFound();

            var studentDTO = mapper.Map<StudentDTO>(student);
            
            return Ok(studentDTO);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(StudentDTO studentDTO) // Si el método comienza con el verbo http no es necesario colocar el decorador. No resuelve por el nombre del metodo sino por el tipo de método. Tampoco importa el nombre el método, no entiendo como matchea.
        {
            if (!ModelState.IsValid) // Valida modelo de datos.
                return BadRequest(ModelState);

            try
            {
                var student = mapper.Map<Student>(studentDTO); //Comno el servicio debe recibir un modelo, con Mapper se envía studentDTO para recibir el modelo Student
                student = await studentService.Insert(student); //aunque la tabla curso no tiene id auto incremental, si lo fuera, por eso se guarda en una variable lo que devuelve el metodo Insert.
                return Ok(student); // Retorna 200 ok + el objeto que se ha insertado.
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(StudentDTO studentDTO, int id) // Recibe un objeto en el cuerpo de la peticion + un parámetro por la url.
        {
            if (!ModelState.IsValid) // Verifica modelo de datos
                return BadRequest(ModelState);

            if(studentDTO.StudentID != id) // Verifica que la peticion tenga un parámetro numerico.
                return BadRequest();

            var studentFlag = await studentService.GetById(id); // Consulta que el registro solicitado exista.
            if(studentFlag == null)
                return NotFound();

            try
            {
                var student = mapper.Map<Student>(studentDTO);
                student = await studentService.Update(student);
                return Ok(student);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var studentFlag = await studentService.GetById(id); // Consulta que el registro solicitado exista.
            if (studentFlag == null)
                return NotFound();

            try
            {
                if (!await studentService.DeleteCheckRelatedEntity(id)) // Verifica si el id está en tablas relacionadas, si no hay, procede a eliminar el registro.
                    await studentService.Delete(id);
                else
                    throw new Exception("El id exist en tablas relacionadas, primero debe eliminarlo de ellas.");
                return Ok();
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }

*/
    }
}
