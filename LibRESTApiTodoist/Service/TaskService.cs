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
    /// Servicio de <see cref="TaskModel"/>
    /// </summary>
    public class TaskService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller de la api REST de Todoist.</param>
        public TaskService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }


        /// <summary>
        /// Obtiene todas las tareas.
        /// </summary>
        /// <returns>Lista de tareas</returns>
        public async Task<List<TaskModel>> GetAllTasksAsync()
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, "tasks", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<TaskModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Crea una tarea (En la bandeja de entrada y con prioridad normal).
        /// </summary>
        /// <param name="content">Contenido de la tarea.</param>
        /// <returns>Tarea creada.</returns>
        public async Task<TaskModel> CreateTaskAsync(string content)
        {
            var parameters = new { content };

            return await CreateTaskAsync(parameters);
        }

        /// <summary>
        /// Crea una tarea (Indicando la fecha de vencimiento de manera "natural").
        /// </summary>
        /// <param name="content">Contenido de la tarea.</param>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <param name="order">Orden.</param>
        /// <param name="labelIDs">Identificadores de etiquetas.</param>
        /// <param name="priority">Prioridad.</param>
        /// <param name="dueString">Vencimiento especificado de manera "natural".</param>
        /// <param name="dueLanguage">Lenguaje del vencimiento.</param>
        /// <returns>Tarea creada.</returns>
        public async Task<TaskModel> CreateTaskAsync(string content, long projectID, uint order, List<int> labelIDs, TaskModel.PriorityType priority, string dueString, string dueLanguage)
        {
            var parameters = new
            {
                content,
                project_id = projectID,
                order,
                label_ids = labelIDs,
                priority = (int)priority,
                due_string = dueString,
                due_lang = dueLanguage
            };

            return await CreateTaskAsync(parameters);
        }

        /// <summary>
        /// Crea una tarea (Indicando la fecha de vencimiento como fecha sin hora).
        /// </summary>
        /// <param name="content">Contenido de la tarea.</param>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <param name="order">Orden.</param>
        /// <param name="labelIDs">Identificadores de etiquetas.</param>
        /// <param name="priority">Prioridad.</param>
        /// <param name="dueDate">Vencimiento especificado como fecha.</param>
        /// <returns>Tarea creada.</returns>
        public async Task<TaskModel> CreateTaskAsync(string content, long projectID, uint order, List<int> labelIDs, TaskModel.PriorityType priority, DateTime dueDate)
        {
            if (dueDate.Hour == 0 &&
                dueDate.Minute == 0 &&
                dueDate.Second == 0)
            {
                var parameters = new
                {
                    content,
                    project_id = projectID,
                    order,
                    label_ids = labelIDs,
                    priority = (int)priority,
                    due_date = dueDate.ToString("yyyy-MM-dd")
                };

                return await CreateTaskAsync(parameters);
            }
            else
            {
                var parameters = new
                {
                    content,
                    project_id = projectID,
                    order,
                    label_ids = labelIDs,
                    priority = (int)priority,
                    due_datetime = dueDate.ToString("yyyy-MM-ddThh:mm:ssZ")
                };

                return await CreateTaskAsync(parameters);
            }
        }

        /// <summary>
        /// Crea la tarea.
        /// </summary>
        /// <param name="parameters">Parámetros con sus valores.</param>
        /// <returns>Tarea creada.</returns>
        private async Task<TaskModel> CreateTaskAsync(dynamic parameters)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, "tasks", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<TaskModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Obtiene la tarea.
        /// </summary>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <returns>Tarea.</returns>
        public async Task<TaskModel> GetTaskAsync(long taskID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, $"tasks/{ taskID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<TaskModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Actualiza la tarea indicada.
        /// </summary>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <param name="content">Contenido de la tarea.</param>
        /// <returns>Indica si se ha realizado la modificación.</returns>
        public async Task<bool> UpdateTaskAsync(long taskID, string content)
        {
            var parameters = new { content };

            return await UpdateTaskAsync(taskID, parameters);
        }

        /// <summary>
        /// Actualiza la tarea indicada.
        /// </summary>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <param name="content">Contenido de la tarea.</param>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <param name="labelIDs">Identificadores de etiquetas.</param>
        /// <param name="priority">Prioridad.</param>
        /// <param name="dueString">Vencimiento especificado de manera "natural".</param>
        /// <param name="dueLanguage">Lenguaje del vencimiento.</param>
        /// <returns>Indica si se ha realizado la modificación.</returns>
        public async Task<bool> UpdateTaskAsync(long taskID, string content, long projectID, List<int> labelIDs, TaskModel.PriorityType priority, string dueString, string dueLanguage)
        {
            var parameters = new
            {
                content,
                project_id = projectID,
                label_ids = labelIDs,
                priority = (int)priority,
                due_string = dueString,
                due_lang = dueLanguage
            };

            return await UpdateTaskAsync(taskID, parameters);
        }

        /// <summary>
        /// Actualiza la tarea indicada.
        /// </summary>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <param name="content">Contenido de la tarea.</param>
        /// <param name="projectID">Identificador del proyecto.</param>
        /// <param name="labelIDs">Identificadores de etiquetas.</param>
        /// <param name="priority">Prioridad.</param>
        /// <param name="dueDate">Vencimiento especificado como fecha.</param>
        /// <returns>Indica si se ha realizado la modificación.</returns>
        public async Task<bool> UpdateTaskAsync(long taskID, string content, long projectID, List<int> labelIDs, TaskModel.PriorityType priority, DateTime dueDate)
        {
            if (dueDate.Hour == 0 &&
                dueDate.Minute == 0 &&
                dueDate.Second == 0)
            {
                var parameters = new
                {
                    content,
                    project_id = projectID,
                    label_ids = labelIDs,
                    priority = (int)priority,
                    due_date = dueDate.ToString("yyyy-MM-dd")
                };

                return await UpdateTaskAsync(taskID, parameters);
            }
            else
            {
                var parameters = new
                {
                    content,
                    project_id = projectID,
                    label_ids = labelIDs,
                    priority = (int)priority,
                    due_datetime = dueDate.ToString("yyyy-MM-ddThh:mm:ssZ")
                };

                return await UpdateTaskAsync(taskID, parameters);
            }
        }

        /// <summary>
        /// Actualiza la tarea.
        /// </summary>
        /// <param name="parameters">Parámetros con sus valores.</param>
        /// <returns>Tarea creada.</returns>
        private async Task<bool> UpdateTaskAsync(long taskID, dynamic parameters)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"tasks/{ taskID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Cierra la tarea.
        /// </summary>
        /// <returns>Indica si se ha cerrado la tarea.</returns>
        public async Task<bool> CloseTaskAsync(long taskID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"tasks/{ taskID }/close", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Reabre una tarea cerrada.
        /// </summary>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <returns>Indica si se ha reabierto la tarea.</returns>
        public async Task<bool> ReopenTaskAsync(long taskID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"tasks/{ taskID }/reopen", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Elimina la tarea indicada.
        /// </summary>
        /// <param name="taskID">Identificador de la tarea.</param>
        /// <returns>Indica si se ha realizado la eliminación.</returns>
        public async Task<bool> DeleteTaskAsync(long taskID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.DELETE, $"tasks/{ taskID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
