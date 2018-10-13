namespace SIS.Framework.ActionResults
{
    using Contracts;

    public class ViewResult : IViewable
    {
        public ViewResult(IRenderable view)
        {
            this.View = view;
        }

        public IRenderable View { get; set; }

        public string Invoke() => this.View.Render();
    }
}
