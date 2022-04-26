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
    /// Service of <see cref="ProjectModel"/>
    /// </summary>
    public class ProjectService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller of the Todoi REST api.</param>
        public ProjectService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }



        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns>Project list.</returns>
        public async Task<List<ProjectModel>> GetAllProjectsAsync()
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Get, "projects", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<ProjectModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Create a project.
        /// </summary>
        /// <param name="projectName">Project's name.</param>
        /// <returns>Project created.</returns>
        public async Task<ProjectModel> CreateProjectAsync(string projectName)
        {
            var parameters = new ProjectJson(projectName);

            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, "projects", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<ProjectModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Get the indicated project.
        /// </summary>
        /// <param name="projectID">Project identifier.</param>
        /// <returns>Project.</returns>
        public async Task<ProjectModel> GetProjectAsync(long projectID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Get, $"projects/{ projectID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<ProjectModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Update the indicated project.
        /// </summary>
        /// <param name="projectID">Project identifier.</param>
        /// <param name="projectName">Project's name.</param>
        /// <returns>Indicates if the modification has been made.</returns>
        public async Task<bool> UpdateProjectAsync(long projectID, string projectName)
        {
            var parameters = new ProjectJson(projectName);

            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Post, $"projects/{ projectID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Remove the indicated project.
        /// </summary>
        /// <param name="projectID">Project identifier.</param>
        /// <returns>Indicates if the deletion has been made.</returns>
        public async Task<bool> DeleteProjectAsync(long projectID)
        {
            RestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.Delete, $"projects/{ projectID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
