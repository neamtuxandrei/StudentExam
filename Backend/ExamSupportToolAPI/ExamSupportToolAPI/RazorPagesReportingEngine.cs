using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;
using SelectPdf;
using Microsoft.Net.Http.Headers;

namespace RazorPagesReporting
{
    /// <summary>
    /// See https://stackoverflow.com/questions/40912375/return-view-as-string-in-net-core for more details
    /// </summary>
    public class RazorPagesReportingEngine
    {
        private readonly IRazorViewEngine razorViewEngine;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IServiceProvider serviceProvider;

        public RazorPagesReportingEngine(IRazorViewEngine razorViewEngine,
          ITempDataProvider tempDataProvider,
          IServiceProvider serviceProvider)
        {
            this.razorViewEngine = razorViewEngine;
            this.tempDataProvider = tempDataProvider;
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewAsHtml(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        public async Task<FileStreamResult> RenderViewAsPdf(string viewName, object model, string fileName) 
        {
            var htmlString = await RenderViewAsHtml(viewName, model);
            MemoryStream inMemoryStream = new MemoryStream();
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString);            
            doc.Save(inMemoryStream);
            doc.Close();
            inMemoryStream.Seek(0, SeekOrigin.Begin);
            var retStream = new FileStreamResult(inMemoryStream, new MediaTypeHeaderValue("application/pdf"));
            if (!string.IsNullOrEmpty(fileName))
            {
                retStream.FileDownloadName = fileName;
            }
            return retStream;
        }


    }
}
