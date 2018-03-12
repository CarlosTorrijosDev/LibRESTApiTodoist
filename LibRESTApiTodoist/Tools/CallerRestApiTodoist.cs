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
    /// Class calling methods of the Todoist API REST.
    /// </summary>
    public class CallerRestApiTodoist
    {
        private string AuthorizationToken;
        private int RetryCount;
        private TimeSpan RetryDelay;
        private readonly RestClient Client;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="URLBase">Base URL.</param>
        /// <param name="authorizationToken">Authorization token.</param>
        /// <param name="retryCount">Number of repetitions maximum that the requests will make, before a transitory error.</param>
        /// <param name="delayInSecondsForRetry">Delay in seconds to apply when a transient error is obtained.</param>
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
        /// Call the Api REST method.
        /// </summary>
        /// <param name="method">Type of REST method used (GET, POST, PUT, DELETE, etc).</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="requestId">Identifier of the request.</param>
        /// <param name="bodyParameters">Parameters of the body.</param>
        /// <returns>Result of the call.</returns>
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

                    // Add the initial headers
                    request.AddHeader("Cache-Control", "no-cache");
                    request.AddHeader("Authorization", $"Bearer {AuthorizationToken}");

                    // Add the identifier of the request (to identify the transaction in some cases)
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        request.AddHeader("X-Request-Id", requestId);
                    }

                    // Add the query parameters
                    if (queryParameters != null)
                    {
                        foreach (var queryParameter in queryParameters)
                        {
                            request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
                        }
                    }

                    // Add the parameters of the body
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

                    // Check if the exception thrown is a temporary exception based on the logic of the error detection strategy.
                    // Determines whether to retry the operation, as well as how much to expect, based on the retry strategy.
                    if (currentRetry >= this.RetryCount || !IsTransient(ex))
                    {
                        // If it is not a trasient error or you should not retry, the exception is relaunched
                        throw;
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    LogRequest(Client, request, response, stopWatch.ElapsedMilliseconds);
                }

                // Wait to retry the operation.
                Trace.TraceWarning($"Task delay: {RetryDelay.TotalSeconds} seconds");
                await Task.Delay(RetryDelay);

            } while (true);
        }

        /// <summary>
        /// Check if the exception is transient.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <returns>Indicates if the exception is transient.</returns>
        private bool IsTransient(Exception ex)
        {
            var webException = ex as WebException;
            if (webException != null)
            {
                // If the web exception contains one of the following state values ​​it should be transient
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
        /// Log the request made to the Api REST.
        /// </summary>
        /// <param name="client">Api REST client.</param>
        /// <param name="request">Request.</param>
        /// <param name="response">Response.</param>
        /// <param name="durationMs">Duration in ms of the call.</param>
        private void LogRequest(RestClient client, IRestRequest request, IRestResponse response, long durationMs)
        {
            // Set the request to log
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

            // Sets the response to log
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

            // Trace the request, response and execution time
            Trace.TraceInformation(Environment.NewLine +
                                   $">>> Request completed in { durationMs } ms" + Environment.NewLine +
                                   $">>> Request: { JsonConvert.SerializeObject(requestToLog) }" + Environment.NewLine +
                                   $">>> Response: { JsonConvert.SerializeObject(responseToLog) }");
            Trace.Flush();
        }
    }
}
