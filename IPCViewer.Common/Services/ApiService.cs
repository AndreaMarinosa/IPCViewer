using System.Net;

namespace IPCViewer.Common.Services
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /**
     *
     * Clase generica para consumir servicios Web RESTFUL
     *
     */

    public class ApiService
    {
        /**
         * Devulve una lista generica sin seguridad (Para recibir las ciudades)
         */

        public async Task<Response> GetListAsync<T> (
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                // HttpCliente -> Proporciona una clase base para enviar solicitudes HTTP y recibir respuestas HTTP de un recurso identificado por un URI.
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}"; // api/[controller]
                var response = await client.GetAsync(url); // Devuelve la respuesta del cliente a la url
                var result = await response.Content.ReadAsStringAsync();

                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        /**
         * Devuelve una lista generica comprobando los token del usuario
         * Para recibir listas de camaras / usuarios
         */

        public async Task<Response> GetListAsync<T> (
             string urlBase,
             string servicePrefix,
             string controller,
             string tokenType,
             string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        /**
         * Devuelve una camara comprobando los token del usuario
         * Para recibir listas de camaras / usuarios
         */

        public async Task<Response> GetCameraAsync<T> (
             string urlBase,
             string servicePrefix,
             string controller,
             int id,
             string tokenType,
             string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }

                var camera = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = camera
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        /**
         * Recibir un token cuando te estas logeando para tener permisos en la aplicacion
         */

        public async Task<Response> GetTokenAsync (
            string urlBase,
            string servicePrefix,
            string controller,
            TokenRequest request)
        {
            try
            {
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = token
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        /**
         * Insertar una camara desde la aplicacion movil
         */

        public async Task<Response> PostAsync<T> (
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PutAsync<T> (
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> DeleteAsync (
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /**
         * Registrar un usuario
         */

        public async Task<Response> RegisterUserAsync (
            string urlBase,
            string servicePrefix,
            string controller,
            NewUserRequest newUserRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(newUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> GetUserListAsync<User> (
            string urlBase,
            string servicePrefix,
            string controller)
        {
            try
            {
                // HttpCliente -> Proporciona una clase base para enviar solicitudes HTTP y recibir respuestas HTTP de un recurso identificado por un URI.
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}"; // api/[controller]
                var response = await client.GetAsync(url); // Devuelve la respuesta del cliente a la url
                var result = await response.Content.ReadAsStringAsync();

                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var list = JsonConvert.DeserializeObject<List<User>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        /***
         * Para consumir los datos del usuario
         *
         */

        public async Task<Response> GetUserByEmailAsync (
            string urlBase,
            string servicePrefix,
            string controller,
            string email,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(new UserEmailRequest { Email = email });
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<User>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PutAsync<T> (
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if ( !response.IsSuccessStatusCode )
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> ChangePasswordAsync (
            string urlBase,
            string servicePrefix,
            string controller,
            ChangePasswordRequest changePasswordRequest,
            string tokenType,
            string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(changePasswordRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch ( Exception ex )
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        /***
         * Verificar que la imagen existe
         */
        public bool RemoteFileExists (string url, int timeout)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                // El timeout es en milisegundos
                request.Timeout = timeout;
                // ************

                //Configurando el Request method HEAD, puede ser GET tambien.
                request.Method = "HEAD";
                //Obteniendo la respuesta
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Regresa TRUE si el codigo de esdado es == 200
                return ( response.StatusCode == HttpStatusCode.OK );
            }
            catch
            {
                //Si ocurre una excepcion devuelve false
                return false;
            }
        }
    }
}