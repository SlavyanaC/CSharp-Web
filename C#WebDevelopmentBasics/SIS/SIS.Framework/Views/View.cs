namespace SIS.Framework.Views
{
    using ActionResults.Contracts;

    public class View : IRenderable
    {
        private readonly string fullHtmlContent;

        public View(string fullHtmlContent)
        {
            this.fullHtmlContent = fullHtmlContent;
        }

        public string Render() => this.fullHtmlContent;

        //private readonly IDictionary<string, object> viewData;

        //public View(string fullHtmlContent, IDictionary<string, object> viewData)
        //{
        //    this.fullHtmlContent = fullHtmlContent;
        //    this.viewData = viewData;
        //}

        //public string Render()
        //{
        //    var fullHtml = this.ReadFile();
        //    var renderedHtml = this.RenderHtml(fullHtml);

        //    return renderedHtml;
        //}

        //private string ReadFile()
        //{
        //    if (!File.Exists(this.fullHtmlContent))
        //    {
        //        throw new FileNotFoundException($"File {fullHtmlContent} not found");
        //    }

        //    return File.ReadAllText(this.fullHtmlContent);
        //}

        //private string RenderHtml(string fullHtml)
        //{
        //    if (this.viewData.Any())
        //    {
        //        foreach (var parameter in this.viewData)
        //        {
        //            var dynamicDataPlaceholder = $"{{{{{{{parameter.Key}}}}}";
        //            if (fullHtml.Contains(dynamicDataPlaceholder))
        //            {
        //                fullHtml = fullHtml.Replace(dynamicDataPlaceholder, parameter.Value.ToString());
        //            }
        //        }
        //    }

        //    return fullHtml;
        //}
    }
}
