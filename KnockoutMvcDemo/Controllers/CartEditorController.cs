using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutMvcDemo.Models;

namespace KnockoutMvcDemo.Controllers
{
  public class CartEditorController : BaseController
  {
    public ActionResult Index()
    {
      InitializeViewBag("Cart editor");
      var model = new CartEditorModel();
      model.DataBase = new List<CartEditorCategoryModel>();
      model.DataBase.Add(new CartEditorCategoryModel
      {
        Name = "Category 1",
        Products = new List<CartEditorProductModel>
          {
            new CartEditorProductModel {Name = "Product 1-1", Price = 1},
            new CartEditorProductModel {Name = "Product 1-2", Price = 2},
          }
      });
      model.DataBase.Add(new CartEditorCategoryModel
      {
        Name = "Category 2",
        Products = new List<CartEditorProductModel>
          {
            new CartEditorProductModel {Name = "Product 2-1", Price = 3},
            new CartEditorProductModel {Name = "Product 2-2", Price = 4},
          }
      });

      model.CategoriesId = new List<int> { 0, 1 };
      model.ProductsId = new List<List<int>>
                     {
                       new List<int> {0, 1},
                       new List<int> {0, 1}
                     };
      model.Lines = new List<CartEditorLineModel>();
      model.AddLine();
      model.AddLine();
      return View(model);
    }

    public ActionResult AddLine(CartEditorModel model)
    {
      model.AddLine();
      return Json(model);
    }

    public ActionResult RemoveLine(CartEditorModel model, int index)
    {
      model.RemoveLine(index);
      return Json(model);
    }
  }
}
