using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public List<Rezultat> Customers { get; set; }

        public IndexModel(ILogger<IndexModel> logger, Models.ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;
        }   
        

        public void OnPost()
        {



            if (Request.Form["pret"] == "Pretraži pojam")
            {



                string pojam = Request.Form["val"];

                //Ciscenje starih rows iz baze
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE [Rezultati]");

                this.Customers = _context.Rezultatis.ToList();


                //Google search 100 rezultata i upis u bazu
                for (int i = 1; i < 10; i++)
                {

                    var customSearchService = new Google.Apis.CustomSearchAPI.v1.CustomSearchAPIService(new BaseClientService.Initializer
                    {
                        ApiKey = "AIzaSyD24f7MpzEmRs0rO8d4bBKs2-DfGAzyYRg"
                    });


                    var listRequest = customSearchService.Cse.List();
                    listRequest.Q = pojam;
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
                OnGet();



            }
            else if (Request.Form["pretF"] == "Filter pojam")
            {

                string pojam = Request.Form["valF"];
                this.Customers = _context.Rezultatis.Where(p => p.Naslov.Contains(pojam) || p.Html.Contains(pojam)).ToList();
                               
            }
                   
        }


        public void OnGet()
        {                      
            if(_context.Rezultatis.Any())
            this.Customers = _context.Rezultatis.ToList();            
        }

    }
}