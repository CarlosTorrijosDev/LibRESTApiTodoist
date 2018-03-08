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
    /// Servicio de <see cref="LabelModel"/>
    /// </summary>
    public class LabelService
    {
        private readonly CallerRestApiTodoist _callerRestApiTodoist;


        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="callerRestApiTodoist">Caller de la api REST de Todoist.</param>
        public LabelService(CallerRestApiTodoist callerRestApiTodoist)
        {
            _callerRestApiTodoist = callerRestApiTodoist;
        }


        /// <summary>
        /// Obtiene todos las etiquetas.
        /// </summary>
        /// <returns>Lista de etiquetas.</returns>
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
        /// Crea una etiqueta.
        /// </summary>
        /// <param name="labelName">Nombre de la etiqueta.</param>
        /// <returns>Etiqueta creada.</returns>
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
        /// Obtiene la etiqueta indicada.
        /// </summary>
        /// <param name="labelID">Identificador de etiqueta.</param>
        /// <returns>Etiqueta.</returns>
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
        /// Actualiza la etiqueta indicada.
        /// </summary>
        /// <param name="labelID">Identificador de la etiqueta.</param>
        /// <param name="labelName">Nombre de la etiqueta.</param>
        /// <returns>Indica si se ha realizado la modificación.</returns>
        public async Task<bool> UpdateLabelAsync(long labelID, string labelName)
        {
            var parameters = new { name = labelName };

            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.POST, $"labels/{ labelID }", Guid.NewGuid().ToString(), null, parameters);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Elimina la etiqueta indicada.
        /// </summary>
        /// <param name="labelID">Identificador de la etiqueta.</param>
        /// <returns>Indica si se ha realizado la eliminación.</returns>
        public async Task<bool> DeleteLabelAsync(long labelID)
        {
            IRestResponse result = await _callerRestApiTodoist.CallRestMethodAsync(Method.DELETE, $"labels/{ labelID }", null, null, null);

            return result.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
