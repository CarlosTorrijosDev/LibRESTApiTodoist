using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibRESTApiTodoIst.Model;
using LibRESTApiTodoIst.Tools;
using Newtonsoft.Json;
using RestSharp;

namespace LibRESTApiTodoIst.Service
{
    /// <summary>
    /// Servicio de <see cref="ProjectModel"/>
    /// </summary>
    public class ProjectService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller de la api REST de Todoist.</param>
        public ProjectService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }



        /// <summary>
        /// Obtiene todos los proyectos.
        /// </summary>
        /// <returns>Lista de proyectos.</returns>
        public async Task<List<ProjectModel>> GetAllProjectsAsync()
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, "projects", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<ProjectModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Crea un proyecto.
        /// </summary>
        /// <param name="projectName">Nombre del proyecto.</param>
        /// <returns>Proyecto creado.</returns>
        public async Task<ProjectModel> CreateProjectAsync(string projectName)
        {
            var parameters = new { name = projectName };

            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, "projects", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<ProjectModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Obtiene el proyecto indicado.
        /// </summary>
        /// <param name="projectID">Identificador de proyecto.</param>
        /// <returns>Proyecto.</returns>
        public async Task<ProjectModel> GetProjectAsync(long projectID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, $"projects/{ projectID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<ProjectModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Actualiza el proyecto indicado.
        /// </summary>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <param name="projectName">Nombre del proyecto.</param>
        /// <returns>Indica si se ha realizado la modificación.</returns>
        public async Task<bool> UpdateProjectAsync(long projectID, string projectName)
        {
            var parameters = new { name = projectName };

            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"projects/{ projectID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Elimina el proyecto indicado.
        /// </summary>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <returns>Indica si se ha realizado la eliminación.</returns>
        public async Task<bool> DeleteProjectAsync(long projectID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.DELETE, $"projects/{ projectID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
