using System.Text;
using Hepsi.CommandProcessor.Handlers;


namespace Hepsi.CommandProcessor
{
    internal class PipelineTracer : IAmAPipelineTracer
    {
        private readonly StringBuilder buffer = new StringBuilder();

        public void AddToPath(HandlerName handlerName)
        {
            buffer.AppendFormat("{0}|", handlerName);
        }

        public override string ToString()
        {
            return buffer.ToString();
        }
    }
}
