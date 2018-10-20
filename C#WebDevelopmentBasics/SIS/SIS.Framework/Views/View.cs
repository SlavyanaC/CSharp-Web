namespace SIS.Framework.Views
{
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using ActionResults.Contracts;

    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
        }

        public string Render()
        {
            var fullHtml = this.ReadFile();
            var renderedHtml = this.RenderHtml(fullHtml);

            return renderedHtml;
        }

        private string ReadFile()
        {
            if (!File.Exists(this.fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"File {fullyQualifiedTemplateName} not found");
            }

            return File.ReadAllText(this.fullyQualifiedTemplateName);
        }

        private string RenderHtml(string fullHtml)
        {
            if (this.viewData.Any())
            {
                foreach (var parameter in this.viewData)
                {
                    var dynamicDataPlaceholder = $"{{{{{{{parameter.Key}}}}}";
                    if (fullHtml.Contains(dynamicDataPlaceholder))
                    {
                        fullHtml = fullHtml.Replace(dynamicDataPlaceholder, parameter.Value.ToString());
                    }
                }
            }

            return fullHtml;
        }
    }
}
