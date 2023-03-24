using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AirportBaggageCompartment.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AirportBaggageCompartment.Services;

public class ApiClient
{ 
    
    private const string _port = "44357";
    private const string _host = "localhost";
    private const string _protocol = "https";
    
    public enum CRUD
    {
        create,
        get,
        update,
        delete
    }

    public struct Method
    {
        public static string POST = "POST";
        public static string GET = "GET";
        public static string DELETE = "DELETE";
        public static string PUT = "PUT";
    }


    public static string GenerateURI<T>(CRUD CRUD)
    {
        return $"{_protocol}://{_host}:{_port}/api/easydata/models/__default/sources/{typeof(T).Name}/{CRUD}";
    }
    
    public static string GenerateBaseURI()
    {
        return $"{_protocol}://{_host}:{_port}/api";
    }

    static string SendRequest(string? body, string uri, string method)
    {
        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = method;

            if (body?.Length > 0)
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                }
            }


            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
        catch (Exception ee)
        {
            return ee.ToString();
        }
    }
    
    
    public static async Task Export()
    {
        await Task.Run(() => {
            var url = $"{GenerateBaseURI()}/ExportData";
            SendRequest(String.Empty, url, Method.GET);
        });
    }

    
    public static List<T>? Get<T>()
    {
        try
        {
            string uri = GenerateURI<T>(CRUD.get);
            string json = SendRequest(null, uri, Method.POST);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        catch(Exception e)
        {
            Console.Write(e);
        } 
        return null;
    }

    public static int Create<T>(T obj)
    {
        try
        {
            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            string uri = GenerateURI<T>(CRUD.create);
            string jsonRequest = SendRequest(json, uri, Method.POST);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonRequest);
            return Convert.ToInt32(jsonObj["record"]["Id"]);
        }
        catch (Exception ex)
        {
            Console.Write(ex);
        }
        return 0;
    }

    public static int Update<T>(T obj)
    {
        try
        {
            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            string uri = GenerateURI<T>(CRUD.update);
            string jsonRequest = SendRequest(json, uri, Method.POST);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonRequest);
            return Convert.ToInt32(jsonObj["record"]["Id"]);
        }
        catch (Exception ex)
        {

        }
        return 0;
    }
    
    public async static Task ExportData()
    {
        await Task.Run(() => {
            var url = $"http://{_host}:{_port}/home/ExportData";
            SendRequest(String.Empty, url, Method.GET);
        });
    }

    public static bool Registration(User user)
    {
        try
        {
            var newId = Create(user);
            return true;
        }
        catch(Exception ex)
        {

        }
        return false;
    }

    public static User? Authorization(string login, string password)
    {

        var listObjs = Get<User>();
        var findUser = listObjs.FirstOrDefault(x => x.Login == login);
        if (findUser != null)
        {
            var hasher = new PasswordHasher<User>();
            var s = hasher.VerifyHashedPassword(findUser, findUser.Password, password);
            if (s == PasswordVerificationResult.Success) return findUser;
        }

        return null;
    }
}