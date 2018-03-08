using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LibRESTApiTodoIst.Tools
{
    /// <summary>
    /// Clase que llama a los métodos de la api REST de Todoist.
    /// </summary>
    public class CallerRestApiTodoist
    {
        private string AuthorizationToken;
        private int RetryCount;
        private TimeSpan RetryDelay;
        private readonly RestClient Client;


        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="URLBase">URL base.</param>
        /// <param name="authorizationToken">Token de autorización.</param>
        /// <param name="retryCount">Número de repeticiones máximo que realizarán las peticiones, ante un error transitorio.</param>
        /// <param name="delayInSecondsForRetry">Retraso en segundos a aplicar cuando se obtiene un error transitorio.</param>
        public CallerRestApiTodoist(string URLBase, string authorizationToken, int retryCount = 3, double delayInSecondsForRetry = 5)
        {
            if (string.IsNullOrEmpty(URLBase) || string.IsNullOrWhiteSpace(URLBase))
            {
                throw new ArgumentNullException("URLBase");
            }

            if (string.IsNullOrEmpty(authorizationToken) || string.IsNullOrWhiteSpace(authorizationToken))
            {
                throw new ArgumentNullException("authorizationToken");
            }

            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException("retryCount");
            }

            if (delayInSecondsForRetry <= 0)
            {
                throw new ArgumentOutOfRangeException("delayInSecondsForRetry");
            }

            this.AuthorizationToken = authorizationToken;
            this.RetryCount = retryCount;
            this.RetryDelay = TimeSpan.FromSeconds(delayInSecondsForRetry);
            this.Client = new RestClient(new Uri(URLBase));
        }

        /// <summary>
        /// Llama al metodo de la Api REST.
        /// </summary>
        /// <param name="method">Tipo de método REST que se usa (GET, POST, PUT, DELETE, etc).</param>
        /// <param name="methodName">Nombre del método.</param>
        /// <param name="requestId">Identificador de la petición.</param>
        /// <param name="bodyParameters">Parámetros del body.</param>
        /// <returns>Resultado de la llamada.</returns>
        public async Task<IRestResponse> CallRestMethodAsync(Method method, string methodName, string requestId, Dictionary<string, string> queryParameters, dynamic bodyParameters)
        {
            int currentRetry = 0;
            var stopWatch = new Stopwatch();
            RestRequest request = null;
            IRestResponse response = null;

            do
            {
                try
                {
                    stopWatch.Start();

                    request = new RestRequest(methodName, method);
                    request.RequestFormat = DataFormat.Json;

                    // Añade las cabeceras iniciales
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");

                    // Añade el identificador de la petición (para identificar la transacción en algunos casos)
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        request.AddHeader("X-Request-Id", requestId);
                    }

                    // Añade los parámetros de query
                    if (queryParameters != null)
                    {
                        foreach (var queryParameter in queryParameters)
                        {
                            request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
                        }
                    }

                    // Añade los parámetros del body
                    if (bodyParameters != null)
                    {
                        request.AddHeader("Content-type", "application/json");
                        request.AddJsonBody(bodyParameters);
                    }

                    response = await Client.ExecuteTaskAsync(request);

                    if (response.ErrorException != null)
                    {
                        throw response.ErrorException;
                    }
                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new WebException("Internal Server Error");
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Operation Exception: {ex.ToString()}");

                    currentRetry++;

                    // Comprueba si la excepción lanzada es una excepcion pasajera
                    // basado en la lógica de la estrategia de detección de errores.
                    // Determina si reintentar la operación, así como cuánto esperar,
                    // basado en la estrategia de reintento.
                    if (currentRetry >= this.RetryCount || !IsTransient(ex))
                    {
                        // Si no se trata de un error pasajero o si no debería reintentar,
                        // se relanza la excepción
                        throw;
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    LogRequest(Client, request, response, stopWatch.ElapsedMilliseconds);
                }

                // Espera para reintentar la operación.
                // Considere calcular un retraso exponencial aquí y
                // usando una estrategia que mejor se adapte a la operación y error.
                Trace.TraceWarning($"Task delay: {RetryDelay.TotalSeconds} seconds");
                await Task.Delay(RetryDelay);

            } while (true);
        }

        /// <summary>
        /// Comprueba si la excepción es transitoria.
        /// </summary>
        /// <param name="ex">Excepción.</param>
        /// <returns>Indica si la excepción es transitoria.</returns>
        private bool IsTransient(Exception ex)
        {
            var webException = ex as WebException;
            if (webException != null)
            {
                // Si la excepción web contiene uno de los siguientes valores de estado
                // debería ser transitoria
                return new[]
                {
                    WebExceptionStatus.ConnectionClosed,
                    WebExceptionStatus.Timeout,
                    WebExceptionStatus.RequestCanceled,
                    WebExceptionStatus.UnknownError
                }.Contains(webException.Status);
            }

            return false;
        }

        /// <summary>
        /// Logea la petición realizada a la Api REST.
        /// </summary>
        /// <param name="client">Cliente de Api REST.</param>
        /// <param name="request">Petición realizada.</param>
        /// <param name="response">respuesta obtenida.</param>
        /// <param name="durationMs">Duración en ms de la llamada.</param>
        private void LogRequest(RestClient client, IRestRequest request, IRestResponse response, long durationMs)
        {
            // Establece la petición a logear
            var requestToLog = new
            {
                resource = request.Resource,
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                method = request.Method.ToString(),
                uri = client.BuildUri(request),
            };

            // Establece la respuesta a logear
            object content = null;
            if (response.StatusCode == HttpStatusCode.OK &&
                response.ContentType == "application/json")
            {
                content = JsonConvert.DeserializeObject(response.Content);
            }
            else
            {
                content = response.Content;
            }

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = content,
                headers = response.Headers,
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            // Tracea la petición, respuesta y tiempo de ejecución
            Trace.TraceInformation(Environment.NewLine +
                                   $">>> Request completed in { durationMs } ms" + Environment.NewLine +
                                   $">>> Request: { JsonConvert.SerializeObject(requestToLog) }" + Environment.NewLine +
                                   $">>> Response: { JsonConvert.SerializeObject(responseToLog) }");
            Trace.Flush();
        }
    }
}
