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
    public class CoursesController : ApiController
    {
        private IMapper mapper;
        private static UniversityContext contex_     = UniversityContext.Create();
        private static ICourseRepository repo_       = new CourseRepository(contex_);
        private readonly CourseService courseService = new CourseService(repo_);
        //private readonly CourseService courseService = new CourseService(new CourseRepository(UniversityContext.Create())); //Todo en una linea, arriba está por separado.

        public CoursesController()
        {
            this.mapper = WebApiApplication.MapperConfiguration.CreateMapper();
        }
 
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var courses = await courseService.GetAll();
            var coursesDTO = courses.Select(x => mapper.Map<CourseDTO>(x));
            
            return Ok(coursesDTO);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var course = await courseService.GetById(id);

            if (course == null)
                return NotFound();

            var courseDTO = mapper.Map<CourseDTO>(course);
            
            return Ok(courseDTO);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(CourseDTO courseDTO) // Si el método comienza con el verbo http no es necesario colocar el decorador. No resuelve por el nombre del metodo sino por el tipo de método. Tampoco importa el nombre el método, no entiendo como matchea.
        {
            if (!ModelState.IsValid) // Valida modelo de datos.
                return BadRequest(ModelState);

            try
            {
                var course = mapper.Map<Course>(courseDTO); //Comno el servicio debe recibir un modelo, con Mapper se envía courseDTO para recibir el modelo Course
                course = await courseService.Insert(course); //aunque la tabla curso no tiene id auto incremental, si lo fuera, por eso se guarda en una variable lo que devuelve el metodo Insert.
                return Ok(course); // Retorna 200 ok + el objeto que se ha insertado.
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(CourseDTO courseDTO, int id) // Recibe un objeto en el cuerpo de la peticion + un parámetro por la url.
        {
            if (!ModelState.IsValid) // Verifica modelo de datos
                return BadRequest(ModelState);

            if(courseDTO.CourseID != id) // Verifica que la peticion tenga un parámetro numerico.
                return BadRequest();

            var courseFlag = await courseService.GetById(id); // Consulta que el registro solicitado exista.
            if(courseFlag == null)
                return NotFound();

            try
            {
                var course = mapper.Map<Course>(courseDTO);
                course = await courseService.Update(course);
                return Ok(course);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var courseFlag = await courseService.GetById(id); // Consulta que el registro solicitado exista.
            if (courseFlag == null)
                return NotFound();

            try
            {
                if (!await courseService.DeleteCheckRelatedEntity(id)) // Verifica si el id está en tablas relacionadas, si no hay, procede a eliminar el registro.
                    await courseService.Delete(id);
                else
                    throw new Exception("El id exist en tablas relacionadas, primero debe eliminarlo de ellas.");
                return Ok();
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }

    }
}
