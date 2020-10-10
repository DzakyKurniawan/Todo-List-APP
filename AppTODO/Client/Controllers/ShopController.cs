using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Data;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }

        //public ActionResult AddToCart(int itemId)
        //{
        //    if ((HttpContext.Session, "cart") == null)
        //    {
        //        List<TransactionViewModel> cart = new List<TransactionViewModel>();
        //        var product = context.Transactions.Find(itemId);
        //        cart.Add(new TransactionViewModel()
        //        {
        //            ItemName = product,
        //            Quantity = 1
        //        });
        //        HttpContext.Session.SetString("cart", cart.ToString());
        //    }
        //    else
        //    {
        //        List<Transaction> cart = (List<Transaction> HttpContext.Session, "cart");
        //        var product = ctx.Tbl_Product.Find(productId);
        //        cart.Add(new TransactionViewModel()
        //        {
        //            ItemName = product,
        //            Quantity = 1
        //        });
        //        HttpContext.Session.SetString("cart", cart.ToString());
        //    }
        //    return Redirect("Index");
        //}
    }
}