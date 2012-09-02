using System.Collections.Generic;

namespace KnockoutMvcDemo.Models
{
  public class CartEditorProductModel
  {
    public string Name { get; set; }
    public double Price { get; set; }
  }

  public class CartEditorCategoryModel
  {
    public string Name { get; set; }
    public List<CartEditorProductModel> Products { get; set; }
  }

  public class CartEditorLineModel
  {
    public CartEditorLineModel()
    {
      CategoryId = -1;
      ProductId = -1;
      Quantity = 1;
    }

    public int CategoryId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
  }

  public class CartEditorModel
  {
    public List<CartEditorLineModel> Lines { get; set; }
    public List<CartEditorCategoryModel> DataBase { get; set; }
    public List<int> CategoriesId { get; set; }
    public List<List<int>> ProductsId { get; set; }

    public void AddLine()
    {
      Lines.Add(new CartEditorLineModel());
    }

    public void RemoveLine(int index)
    {
      if (index >= 0 && index < Lines.Count)
        Lines.RemoveAt(index);
    }    
  }
}