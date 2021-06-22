using System.Text.Json;
using DatingApp_Api.Helpers;
using Microsoft.AspNetCore.Http;

namespace DatingApp_Api.Extension
{
    public static class HttpExtension
    {
        public static void AddPageInationHeaders(this HttpResponse response
        ,int currentPage,int totalPage,int itemPerPage,int totalItem)
        {
           var pageinationHeaders = new PageinationHeader(itemPerPage
           ,totalItem,totalPage,currentPage);


           var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pageination", JsonSerializer.Serialize(pageinationHeaders, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pageination");


        }
    }
}