﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using University.API.Models;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;
using University.BL.Repositories;
using University.BL.Repositories.Implements;
using University.BL.Services.Implements;

namespace University.API.Controllers
{
    //[RoutePrefix("api/Courses")]
    public class CoursesController : ApiController
    {
        private readonly IMapper mapper;
        private static readonly UniversityContext contex_     = UniversityContext.Create();
        private static readonly ICourseRepository repo_       = new CourseRepository(contex_);
        private readonly CourseService courseService = new CourseService(repo_);
        //private readonly CourseService courseService = new CourseService(new CourseRepository(UniversityContext.Create())); //Todo en una linea, arriba está por separado.

        public CoursesController()
        {
            this.mapper = WebApiApplication.MapperConfiguration.CreateMapper();
        }
 
        /// <summary>
        /// Obtiene los objetos de cursos
        /// </summary>
        /// <returns>Listado de los objetos de cursos</returns>
        /// <response code="200">Ok. Devuelve el listado de objetos solicitados. </response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CourseDTO>))]
        public async Task<IHttpActionResult> GetAll()
        {
            var courses = await courseService.GetAll();
            var coursesDTO = courses.Select(x => mapper.Map<CourseDTO>(x));
            
            return Ok(coursesDTO);
        }

        /// <summary>
        /// Devuelve un objeto Course por su Id.
        /// </summary>
        /// <remarks> Una descripción mas larga si fuera necesario.</remarks>
        /// <param name="id">Id del objeto solicitado</param>
        /// <returns>Objeto Course</returns>
        /// <response code="200">Ok. Devuelve el objeto solicitado. </response>
        /// <response code="404">NotFound. No se encontró el objeto solicitado. </response>
        [HttpGet] // Obtener curso buscando por campo ID
        [ResponseType(typeof(CourseDTO))]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var course = await courseService.GetById(id);

            if (course == null)
                return NotFound();

            var courseDTO = mapper.Map<CourseDTO>(course);
            
            return Ok(courseDTO);
        }

        [HttpGet] // 1.4-Obtener curso buscando por campo credits (get-sincrónico)
        public IHttpActionResult GetByCredit(int id)
        {
            var course = courseService.GetByCredit(id);

            if (course == null)
                return NotFound();

            var courseDTO = mapper.Map<CourseDTO>(course);

            return Ok(courseDTO);
        }

        [HttpGet] //2.4-Obtener curso buscando por campo credits (get-asincrónico)
        public async Task<IHttpActionResult> GetByCreditAsync(int id)
        {
            var course = await courseService.GetByCreditAsync(id);

            if (course == null)
                return NotFound();

            var courseDTO = mapper.Map<CourseDTO>(course);

            return Ok(courseDTO);
        }

        [HttpPost] // 4.4-Obtener curso buscando por campo credits (post-sincrónico)
        public async Task<IHttpActionResult> PostByCreditAsync(RequestCredits model) // el atributo [FromBody] para indica que el valor del crédito se extrae del cuerpo de la solicitud.
                                                                                     // Cuando es un metodo post, lo recomendable es enviar un objeto.
                                                                                     // Si se envía un objeto no es necesario [FromBody], solo cuando se envian variables sueltas c/u debe estar precedida por [FromBody]
        {
            var course = await courseService.GetByCreditAsync(model.Id);             // Utilizo un metodo existente en servicios, porque de aqui en mas es lo mismo, solo cambia el controlador.

            if (course == null)
                return NotFound();

            var courseDTO = mapper.Map<CourseDTO>(course);

            return Ok(courseDTO);
        }

        [HttpPost] // 3.4-Obtener curso buscando por campo credits (post-asincrónico)
        public IHttpActionResult PostByCredit([FromBody] RequestCredits model)
        {
            var course = courseService.GetByCredit(model.Id);                        // Utilizo un metodo existente en servicios, porque de aqui en mas es lo mismo, solo cambia el controlador.

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
                var course = mapper.Map<Course>(courseDTO); //Como el servicio debe recibir un modelo, con Mapper se envía courseDTO para recibir el modelo Course
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
                    throw new Exception("El id existe en tablas relacionadas, primero debe eliminarlo de ellas.");
                string aux = "Se ha eliminado registro id = " + id.ToString();
                return Ok(aux);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }

        }

    }
}
