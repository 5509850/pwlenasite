using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using pw.lena.Core.Data.Handlers;
using pw.lena.Core.Data.Models;

namespace pw.lena.Core.Data.Services.WebServices
{
    public class RestService
    {
        private string _url;
        private int _timeout = 5000;

        /// <summary>
        /// GET – (READ)this operation is used to retrieve data from the web service.
        /// POST – (Create) this operation is used to create a new item of data on the web service.
        /// PUT – (Update) this operation is used to update an item of data on the web service.
        /// PATCH – this operation is used to update an item of data on the web service by describing a set of instructions about how the item should be modified.This verb is not used in the sample application.
        /// DELETE – (DElete) this operation is used to delete an item of data on the web service.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="model"></param>

        public RestService(string server, string model)
        {
            _url = server + @"/api/" + model;
        }

        public int Timeout { get { return _timeout; } set { _timeout = value; } }
        
        public async Task<T> Get<T>(string id) where T : class
        {
            string url = _url;
            url += "/get";
            url += "?id=" + id;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = response.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public async Task<HttpResponseMessage> Get(int id)
        {
            string url = _url;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + "?id=" + id);
                    return await httpClient.SendAsync(request);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<List<T>> Get<T>() where T : class
        {
            string url = _url;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = response.Content.ReadAsStringAsync().Result;
                    if (content == null)
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public async Task<List<T>> Get<T>(CodeRequest request) where T : class
        {
            string url = _url;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(string.Format("{0}?hash={1}&CRC={2}", url, request.AndroidIDmacHash, request.CRC));
                    var content = response.Content.ReadAsStringAsync().Result;
                    if (content == null)
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public async Task<T> Get<T>(int id) where T : class
        {
            string url = _url + "?id=" + id;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    var content = response.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public async Task<HttpResponseMessage> Post<T>(T data)
        {
            string url = _url;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    return await httpClient.PostAsync(url, content);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<T> PostAndGet<T>(object data)
        {
            //  public async Task<T> Get<T>(int id) where T : class
            string url = _url;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        var contentreturn = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T>(contentreturn);
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(string.Empty); 
                    }
                    
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<HttpResponseMessage> Delete(int id)
        {
            string url = _url;
            url += "?id=" + id;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    return await httpClient.DeleteAsync(url);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<HttpResponseMessage> Delete(CodeRequest request) 
        {   
            string url = string.Format("{0}?id={1}&hash={2}&CRC={3}", _url, request.TypeDeviceID, request.AndroidIDmacHash, request.CRC);
            try
            {
                using (var httpClient = new HttpClient())
                {
                    return await httpClient.DeleteAsync(url);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<HttpResponseMessage> Put(CodeRequest req)
        {
            string url = string.Format("{0}?id={1}&hash={2}&CRC={3}", _url, req.TypeDeviceID, req.AndroidIDmacHash, req.CRC);
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
                    return await httpClient.SendAsync(request);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }
            
        public async Task<T> GetDataFromJsonFile<T>() where T : class
        {
            string url = _url;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<T>(response);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }
        }

        public async Task<HttpResponseMessage> Post(string name, string password)
        {
            string url = _url;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url + "?name=" + name + "&password=" + password);
                    return await httpClient.SendAsync(request);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<HttpResponseMessage> Post(int id)
        {
            string url = _url;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url + "?id=" + id);
                    return await httpClient.SendAsync(request);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }

        public async Task<HttpResponseMessage> Post()
        {
            string url = _url;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    return await httpClient.SendAsync(request);
                }
            }
            catch (TaskCanceledException)
            {
                throw new ExceptionHandler();
            }
        }
    }
}

