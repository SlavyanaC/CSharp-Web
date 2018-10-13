namespace SIS.Framework.Views
{
    using System.IO;
    using ActionResults.Contracts;

    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        public string Render()
        {
            var fullHtml = this.ReadFile();
            return fullHtml;
        }

        private string ReadFile()
        {
            if (!File.Exists(this.fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"File {fullyQualifiedTemplateName} not found");
            }

            return File.ReadAllText(this.fullyQualifiedTemplateName);
        }
    }
}
