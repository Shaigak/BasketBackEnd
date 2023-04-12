using EntityFrameWork.Data;
using EntityFrameWork.Models;
using EntityFrameWork.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EntityFrameWork.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            HttpContext.Session.SetString("name", "Pervin"); 
            Response.Cookies.Append("surname", "Rehimli" , new CookieOptions { MaxAge = TimeSpan.FromMinutes(30)});

            Book book = new Book()
            {
                Id = 1,
                Name = "Xosrov ve shirin"

            };

            Response.Cookies.Append("book",JsonConvert.SerializeObject(book));

            List<Slider> sliders = _context.Sliders.ToList();
            SliderInfo sliderInfo = _context.SliderInfos.FirstOrDefault();
            IEnumerable<Blog> blogs = _context.Blogs.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Category>categories=_context.Categories.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Product>products=_context.Products.Include(m=>m.Images).Where(m => !m.SoftDelete).ToList();
            IEnumerable<About>abouts= _context.Abouts.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Advantage> advantages = _context.Advantages.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Flower> flowers = _context.Flowers.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Workers> workers = _context.Workers.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Florist> florists = _context.Florists.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Instagram> instagrams = _context.Instagrams.Where(m => !m.SoftDelete).ToList();
            IEnumerable<Subscribe> subscribes = _context.Subscribes.Where(m => !m.SoftDelete).ToList();
            HomeVm model = new()
            {
                Sliders = sliders,
                Sliderİnfo = sliderInfo,
                Blogs=blogs,
                Categories=categories,
                Products=products,
                Abouts=abouts,
                Advantages=advantages,
                Flowers=flowers,
                Workers=workers,
                Florists = florists,
                Instagrams=instagrams,
                Subscribes=subscribes
                
            };



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null) return BadRequest();

            Product dbProduct=await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();

            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }


            BasketVM existProduct=basket.FirstOrDefault(m=>m.Id==id);

            if(existProduct == null)
            {

                basket?.Add(new BasketVM
                {
                    Id = dbProduct.Id,
                    Count = 1

                });
            }
            else
            {
                existProduct.Count++;
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));


            return RedirectToAction(nameof(Index));
        }


        //public IActionResult Test()
        //{
        //    var sessionData = HttpContext.Session.GetString("name");
        //    var cokieData = Request.Cookies["surname"];
        //    var objectData = JsonConvert.DeserializeObject<Book>(Request.Cookies["book"]);

        //    return Json(objectData);
        //}
    }

    class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}