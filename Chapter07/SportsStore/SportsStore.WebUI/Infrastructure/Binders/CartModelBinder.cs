using SportsStore.Domain.Entities;
using System.Web.Mvc;

namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // 通过会话获取cart
            Cart cart = null;
            if (controllerContext.HttpContext.Session !=null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            // 若会话中没有Cart，则创建一个
            if (cart==null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[sessionKey]= cart;
                }
            }

            return cart;
        }
    }
}