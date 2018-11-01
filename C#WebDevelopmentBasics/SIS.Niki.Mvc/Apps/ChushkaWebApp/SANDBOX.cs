using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework;
using SIS.MvcFramework.ViewEngine;
using ChushkaWebApp.ViewModels.Products;
using SIS.MvcFramework.ViewEngine.Contracts;

namespace MyAppViews
{
    public class UserHomeIndexView : IView<ChushkaWebApp.ViewModels.Products.ProductViewModel[]>
    {
        public string GetHtml(ChushkaWebApp.ViewModels.Products.ProductViewModel[] model, MvcUserInfo user)
        {
            StringBuilder html = new StringBuilder();
            var Model = model;
            var User = user;

            html.AppendLine("<div class=\"container-fluid text-center\">");
            if (User.Role == "User")
            {
                html.AppendLine("        <h2>Greetings, " + User.Username + "!</h2>");
                html.AppendLine("        <h4>Feel free to view and order any of our products.</h4>");
            }
            else
            {
                html.AppendLine("        <h2>Greetings, admin!</h2>");
                html.AppendLine("        <h4>Enjoy your work today!</h4>");
            }

            html.AppendLine("</div>");
            html.AppendLine("<hr class=\"hr-2 bg-dark\" />");
            html.AppendLine("<div class=\"container-fluid product-holder\">");
            html.AppendLine("    <div class=\"row d-flex justify-content-around\">");
            var index = 0;
            foreach (var product in Model)
            {
                if (index % 5 == 0)
                {
                    html.AppendLine("            </div>");
                    html.AppendLine("            <br/>");
                    html.AppendLine("            <div class=\"row d-flex justify-content-around\">");
                }

                @index += 1;
                html.AppendLine("        <a href=\"/products/details?id=" + product.Id +
                                "\" class=\"col-md-2\">                    ");
                html.AppendLine("            <div class=\"product p-1 chushka-bg-color rounded-top rounded-bottom\">");
                html.AppendLine("                <h5 class=\"text-center mt-3\">" + product.Name + "</h5>");
                html.AppendLine("                <hr class=\"hr-1 bg-white\"/>");
                html.AppendLine("                <p class=\"text-white text-center\">");
                html.AppendLine("                    " + product.Description + "");
                html.AppendLine("                </p>");
                html.AppendLine("                <hr class=\"hr-1 bg-white\"/>");
                html.AppendLine("                <h6 class=\"text-center text-white mb-3\">" + product.Price + "</h6>");
                html.AppendLine("            </div>");
                html.AppendLine("          </a>");
                html.AppendLine("       </div>");
                html.AppendLine("</div>      ");

            }
                return html.ToString().TrimEnd();
        }
    }
}