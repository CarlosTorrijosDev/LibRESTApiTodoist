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
    /// Servicio de <see cref="CommentModel"/>
    /// </summary>
    public class CommentService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller de la api REST de Todoist.</param>
        public CommentService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }



        /// <summary>
        /// Obtiene todos los comentarios de una tarea.
        /// </summary>
        /// <param name="taskID">Identificador de tarea.</param>
        /// <returns>Lista de comentarios.</returns>
        public async Task<List<CommentModel>> GetCommentsByTaskAsync(long taskID)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("task_id", taskID.ToString());

            return await GetCommentsByParametersAsync(queryParameters);
        }

        /// <summary>
        /// Obtiene todos los comentarios de un proyecto.
        /// </summary>
        /// <param name="projectID">Identificador de proyecto.</param>
        /// <returns>Lista de comentarios.</returns>
        public async Task<List<CommentModel>> GetCommentsByProjectAsync(long projectID)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("project_id", projectID.ToString());

            return await GetCommentsByParametersAsync(queryParameters);
        }

        /// <summary>
        /// Obtiene todos los comentarios mediante los parámetros de consulta especificados.
        /// </summary>
        /// <param name="queryParameters">Parámetros de consulta.</param>
        /// <returns>Lista de comentarios.</returns>
        private async Task<List<CommentModel>> GetCommentsByParametersAsync(Dictionary<string, string> queryParameters)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, "comments", null, queryParameters, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<CommentModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Crea un comentario para una tarea.
        /// </summary>
        /// <param name="commentContent">Contenido del comentario.</param>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <returns>Comentario creado.</returns>
        public async Task<CommentModel> CreateCommentForTaskAsync(string commentContent, long taskID)
        {
            var parameters = new
            {
                task_id = taskID,
                content = commentContent
                // TODO: Faltaría incluir attachment
            };

            return await CreateCommentByParameters(parameters);
        }

        /// <summary>
        /// Crea un comentario para un proyecto.
        /// </summary>
        /// <param name="commentContent">Contenido del comentario.</param>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <returns>Comentario creado.</returns>
        public async Task<CommentModel> CreateCommentForProjectAsync(string commentContent, long projectID)
        {
            var parameters = new
            {
                project_id = projectID,
                content = commentContent
                // TODO: Faltaría incluir attachment
            };

            return await CreateCommentByParameters(parameters);
        }

        /// <summary>
        /// Crea un comentario mediante los parámetros especificados.
        /// </summary>
        /// <param name="parameters">Parámetros.</param>
        /// <returns>Comentario creado.</returns>
        private async Task<CommentModel> CreateCommentByParameters(dynamic parameters)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, "comments", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<CommentModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Obtiene el comentario indicado.
        /// </summary>
        /// <returns>Comentario.</returns>
        public async Task<CommentModel> GetCommentAsync(long commentID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, $"comments/{ commentID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<CommentModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Actualiza el comentario indicado.
        /// </summary>
        /// <param name="commentID">Identificador del comentario.</param>
        /// <param name="commentContent">Contenido del comentario.</param>
        /// <returns>Indica si se ha realizado la modificación.</returns>
        public async Task<bool> UpdateCommentAsync(long commentID, string commentContent)
        {
            var parameters = new { content = commentContent };

            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"comments/{ commentID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Elimina el comentario indicado.
        /// </summary>
        /// <param name="projectID">Identificador del comentario.</param>
        /// <returns>Indica si se ha realizado la eliminación.</returns>
        public async Task<bool> DeleteCommentAsync(long commentID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.DELETE, $"comments/{ commentID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
