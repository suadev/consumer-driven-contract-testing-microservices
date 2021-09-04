using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ContractTests
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";


            if (context.Request.Path.Value == "/products/1") // get product
            {
                var product = new ProductDto
                {
                    ID = 1,
                    Name = "Sample Product",
                    StockCount = 20,
                    IsActive = true
                };

                var serializedProduct = JsonConvert.SerializeObject(product);
                await context.Response.WriteAsync(serializedProduct);
            }
            else // get products and search products
            {
                var products = new List<ProductDto>();
                products.Add(new ProductDto
                {
                    ID = 1,
                    Name = "Sample Product",
                    StockCount = 20,
                    IsActive = true
                });

                var serializedProducts = JsonConvert.SerializeObject(products);
                await context.Response.WriteAsync(serializedProducts);
            }

            // if (context.Request.Path.Value == "/provider-states")
            // {
            //     var response = await HandleProviderStatesRequest(context);
            //     context.Response.ContentType = "application/json; charset=utf-8";
            //     await context.Response.WriteAsync(response);
            // }
            // else
            // {
            //     await _next(context);
            // }
        }

        // private async Task<string> HandleProviderStatesRequest(HttpContext context)
        // {
        //     context.Response.StatusCode = (int)HttpStatusCode.OK;

        //     if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
        //         context.Request.Body != null)
        //     {
        //         var jsonRequestBody = String.Empty;
        //         using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
        //         {
        //             jsonRequestBody = await reader.ReadToEndAsync();
        //         }

        //         var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

        //         if (providerState.State == "There are available products")
        //         {
        //             // todo: insert products into the db.
        //             var products = new List<ProductDto>();
        //             products.Add(new ProductDto
        //             {
        //                 ID = 1,
        //                 Name = "Sample Product",
        //                 StockCount = 20,
        //                 IsActive = true
        //             });
        //             return JsonConvert.SerializeObject(products);
        //         }
        //         else if (providerState.State == "There is a single product for a given product id")
        //         {
        //             // todo: insert single product with ID 1 into the db.
        //             var product = new ProductDto
        //             {
        //                 ID = 1,
        //                 Name = "Sample Product",
        //                 StockCount = 20,
        //                 IsActive = true
        //             };
        //             return JsonConvert.SerializeObject(product);
        //         }
        //     }
        //     return string.Empty;
        // }

        // public class ProviderState
        // {
        //     public string Consumer { get; set; }
        //     public string State { get; set; }
        // }
    }
}