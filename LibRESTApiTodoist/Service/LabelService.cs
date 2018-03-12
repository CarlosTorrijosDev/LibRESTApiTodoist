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
    /// Service of <see cref="LabelModel"/>
    /// </summary>
    public class LabelService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller of the Todoi REST api.</param>
        public LabelService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }


        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <returns>List of labels.</returns>
        public async Task<List<LabelModel>> GetAllLabelsAsync()
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, "labels", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<List<LabelModel>>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Create a label.
        /// </summary>
        /// <param name="labelName">Name of the label.</param>
        /// <returns>Created label.</returns>
        public async Task<LabelModel> CreateLabelAsync(string labelName)
        {
            var parameters = new { name = labelName };

            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, "labels", Guid.NewGuid().ToString(), null, parameters);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<LabelModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Get the indicated label.
        /// </summary>
        /// <param name="labelID">Tag identifier.</param>
        /// <returns>Label.</returns>
        public async Task<LabelModel> GetLabelAsync(long labelID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.GET, $"labels/{ labelID }", null, null, null);

            if (result.StatusCode == System.Net.HttpStatusCode.OK &&
                result.ContentType == "application/json")
            {
                return JsonConvert.DeserializeObject<LabelModel>(result.Content);
            }

            return null;
        }

        /// <summary>
        /// Update the indicated label.
        /// </summary>
        /// <param name="labelID">Label identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns>Indicates if the modification has been made.</returns>
        public async Task<bool> UpdateLabelAsync(long labelID, string labelName)
        {
            var parameters = new { name = labelName };

            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"labels/{ labelID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Remove the indicated label.
        /// </summary>
        /// <param name="labelID">Label identifier.</param>
        /// <returns>Indicates if the deletion has been made.</returns>
        public async Task<bool> DeleteLabelAsync(long labelID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.DELETE, $"labels/{ labelID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
