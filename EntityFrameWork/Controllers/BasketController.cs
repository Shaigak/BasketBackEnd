using EntityFrameWork.Data;
using EntityFrameWork.Models;
using EntityFrameWork.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EntityFrameWork.Controllers
{
    public class BasketController : Controller
    {

        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {

            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            }
            else
            {
                basket = new List<BasketVM>();
            }

            foreach (var item in basket)
            {
                Product product = _context.Products.Include(m => m.Images).FirstOrDefault(m => m.Id == item.Id);

                item.Product = product;

            }





            return View(basket);
        }
    }
}
