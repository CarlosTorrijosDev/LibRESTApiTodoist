using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibRESTApiTodoist.Json;
using LibRESTApiTodoIst.Model;
using LibRESTApiTodoIst.Tools;
using Newtonsoft.Json;
using RestSharp;

namespace LibRESTApiTodoIst.Service
{
    /// <summary>
    /// Service of <see cref="CommentModel"/>
    /// </summary>
    public class CommentService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller of the Todoist REST api.</param>
        public CommentService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }



        /// <summary>
        /// Get all comments on a task.
        /// </summary>
        /// <param name="taskID">Task identifier.</param>
        /// <returns>List of comments.</returns>
        public async Task<List<CommentModel>> GetCommentsByTaskAsync(long taskID)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("task_id", taskID.ToString());

            return await GetCommentsByParametersAsync(queryParameters);
        }

        /// <summary>
        /// Get all comments on a project.
        /// </summary>
        /// <param name="projectID">Project identifier.</param>
        /// <returns>List of comments.</returns>
        public async Task<List<CommentModel>> GetCommentsByProjectAsync(long projectID)
        {
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            queryParameters.Add("project_id", projectID.ToString());

            return await GetCommentsByParametersAsync(queryParameters);
        }

        /// <summary>
        /// Get all comments using the specified query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>List of comments.</returns>
        private async Task<List<CommentModel>> GetCommentsByParametersAsync(Dictionary<string, string> queryParameters)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Get, "comments", null, queryParameters, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<CommentModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Create a comment for a task.
        /// </summary>
        /// <param name="commentContent">Content of the comment.</param>
        /// <param name="taskID">Task identifier.</param>
        /// <returns>Comment created.</returns>
        public async Task<CommentModel> CreateCommentForTaskAsync(string commentContent, long taskID)
        {
            var parameters = new CommentJsonForTask(taskID, commentContent);

            return await CreateCommentByParameters(parameters);
        }

        /// <summary>
        /// Create a comment for a project.
        /// </summary>
        /// <param name="commentContent">Content of the comment.</param>
        /// <param name="projectID">Project identifier.</param>
        /// <returns>Comment created.</returns>
        public async Task<CommentModel> CreateCommentForProjectAsync(string commentContent, long projectID)
        {
            var parameters = new CommentJsonForProject(projectID, commentContent);

            return await CreateCommentByParameters(parameters);
        }

        /// <summary>
        /// Create a comment using the specified parameters.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Comment created.</returns>
        private async Task<CommentModel> CreateCommentByParameters(CommentJsonForTask parameters)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, "comments", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<CommentModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Create a comment using the specified parameters.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Comment created.</returns>
        private async Task<CommentModel> CreateCommentByParameters(CommentJsonForProject parameters)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, "comments", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<CommentModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Get the indicated comment.
        /// </summary>
        /// <returns>Commentary.</returns>
        public async Task<CommentModel> GetCommentAsync(long commentID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Get, $"comments/{ commentID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<CommentModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Update the indicated comment.
        /// </summary>
        /// <param name="commentID">Identificador del comentario.</param>
        /// <param name="commentContent">Content of the comment.</param>
        /// <returns>Indicates if the modification has been made.</returns>
        public async Task<bool> UpdateCommentAsync(long commentID, string commentContent)
        {
            var parameters = new CommentJsonForUpdate(commentContent);

            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, $"comments/{ commentID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Remove the indicated comment.
        /// </summary>
        /// <param name="projectID">Comment identifier.</param>
        /// <returns>Indicates if the deletion has been made.</returns>
        public async Task<bool> DeleteCommentAsync(long commentID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Delete, $"comments/{ commentID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
