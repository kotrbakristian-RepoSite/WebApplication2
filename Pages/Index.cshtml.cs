using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using var db = new WebApplication2.Models.ApplicationDbContext();
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using static WebApplication2.Models.ApplicationDbContext;
using Microsoft.AspNetCore.Components;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
              

        private readonly Models.ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ILogger<IndexModel> logger, Models.ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;
        }


        public List<Rezultat> Customers { get; set; }

        public void OnGetSomeStuff() 
        {

            var rows = from o in _context.Rezultatis
                       select o;
            foreach (var row in rows)
            {
                _context.Remove(row);
            }
            _context.SaveChanges();




            this.Customers = _context.Rezultatis.ToList();
            

            for (int i = 1; i < 10; i++)
            { 

            var customSearchService = new Google.Apis.CustomSearchAPI.v1.CustomSearchAPIService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyD24f7MpzEmRs0rO8d4bBKs2-DfGAzyYRg"
            });
            

            var listRequest = customSearchService.Cse.List();
            listRequest.Q = "Sekom";
            listRequest.Cx = "833c9c97e2643439c";

            
            listRequest.Num = 10;
            listRequest.Start = i * 10;
            
            var search = listRequest.Execute();
                if (search.Items != null)
                {
                    

                    foreach (var result in search.Items)
                    {
                        Rezultat re = new Rezultat() { Naslov = result.HtmlTitle, Html = result.HtmlSnippet };

                        _context.Add(re);
                        _context.SaveChanges();



                    }

                }



            }

            //var request = HttpContext.Request;
            //var _baseURL = $"{request.Scheme}://{request.Host}";
            //string fullUrl = _baseURL + HttpContext.Request.Path + HttpContext.Request.QueryString;
            OnGet();
        }




        

            public void OnGet()
        {

            this.Customers = _context.Rezultatis.ToList();
            
        }
    }
}