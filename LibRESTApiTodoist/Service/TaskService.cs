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
    /// Service of <see cref="TaskModel"/>
    /// </summary>
    public class TaskService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller of the Todoi REST api.</param>
        public TaskService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }


        /// <summary>
        /// Get all the tasks.
        /// </summary>
        /// <returns>Task list</returns>
        public async Task<List<TaskModel>> GetAllTasksAsync()
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Get, "tasks", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<TaskModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Create a task (In the inbox and with normal priority).
        /// </summary>
        /// <param name="content">Content of the task.</param>
        /// <returns>Task created.</returns>
        public async Task<TaskModel> CreateTaskAsync(string content)
        {
            var parameters = new { content };

            return await CreateTaskAsync(parameters);
        }

        /// <summary>
        /// Create a task (Indicating the due date in a "natural" way).
        /// </summary>
        /// <param name="content">Content of the task.</param>
        /// <param name="projectID">Project identifier.</param>
        /// <param name="order">Order.</param>
        /// <param name="labelIDs">Label identifiers.</param>
        /// <param name="priority">Priority.</param>
        /// <param name="dueString">Expiration specified in a "natural" manner.</param>
        /// <param name="dueLanguage">Expiry language.</param>
        /// <returns>Task created.</returns>
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
        /// Create a task (Indicating expiration date as date without time).
        /// </summary>
        /// <param name="content">Content of the task.</param>
        /// <param name="projectID">Project identifier.</param>
        /// <param name="order">Order.</param>
        /// <param name="labelIDs">Label identifiers.</param>
        /// <param name="priority">Priority.</param>
        /// <param name="dueDate">Expiration specified as date.</param>
        /// <returns>Task created.</returns>
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
        /// Create the task.
        /// </summary>
        /// <param name="parameters">Parameters with their values.</param>
        /// <returns>Task created.</returns>
        private async Task<TaskModel> CreateTaskAsync(dynamic parameters)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, "tasks", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<TaskModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Get the task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <returns>Task.</returns>
        public async Task<TaskModel> GetTaskAsync(long taskID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Get, $"tasks/{ taskID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<TaskModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Update the indicated task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <param name="content">Content of the task.</param>
        /// <returns>Indicates if the modification has been made.</returns>
        public async Task<bool> UpdateTaskAsync(long taskID, string content)
        {
            var parameters = new { content };

            return await UpdateTaskAsync(taskID, parameters);
        }

        /// <summary>
        /// Update the indicated task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <param name="content">Content of the task.</param>
        /// <param name="projectID">Project identifier.</param>
        /// <param name="labelIDs">Label identifiers.</param>
        /// <param name="priority">Priority.</param>
        /// <param name="dueString">Expiration specified in a "natural" manner.</param>
        /// <param name="dueLanguage">Expiry language.</param>
        /// <returns>Indicates if the modification has been made.</returns>
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
        /// Update the indicated task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <param name="content">Content of the task.</param>
        /// <param name="projectID">Project identifier.</param>
        /// <param name="labelIDs">Label identifiers.</param>
        /// <param name="priority">Priority.</param>
        /// <param name="dueDate">Expiration specified as date.</param>
        /// <returns>Indicates if the modification has been made.</returns>
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
        /// Update the task.
        /// </summary>
        /// <param name="parameters">Parameters with their values.</param>
        /// <returns>Task created.</returns>
        private async Task<bool> UpdateTaskAsync(long taskID, dynamic parameters)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, $"tasks/{ taskID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Close the task.
        /// </summary>
        /// <returns>Indicates if the task has been closed.</returns>
        public async Task<bool> CloseTaskAsync(long taskID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, $"tasks/{ taskID }/close", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Reopen a closed task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <returns>Indicates if the task has been reopened.</returns>
        public async Task<bool> ReopenTaskAsync(long taskID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, $"tasks/{ taskID }/reopen", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Remove the indicated task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <returns>Indicates if the deletion has been made.</returns>
        public async Task<bool> DeleteTaskAsync(long taskID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Delete, $"tasks/{ taskID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
