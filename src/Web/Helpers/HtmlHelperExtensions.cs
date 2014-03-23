using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Business;
using Utilities.DataTypes.ExtensionMethods;
using BO = Business;

namespace System.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString UploadDocument(this HtmlHelper helper, DocumentTables reference, bool hasDate = false, int referenceId = 0, int? documentId = 0)
        {
            var builder = new StringBuilder();

            builder.Append(String.Format("<iframe class=\"frame-upload\" src=\"\\Document\\Document\\NewUpload\\{0}?expirationdate={1}&referenceId={2}&documentId={3}\">", 
                reference.ToString(),
                hasDate,
                referenceId,
                documentId.HasValue ? documentId.Value : 0));
            builder.Append("</iframe>");

            return new MvcHtmlString(builder.ToString());

        }

       
    }

    public static class UrlHelperExtensions
    {
         public static String GetDocumentPath(this UrlHelper helper, int id)
        {
            var document = BO.Document.Instance.GetDocumentById(id);

            var ret = "";
            if (document.FileStream != null)
            {
                ret = helper.Action("GetDocument", "Document",
                    new {Area = "Document", id = document.Id});
            }
            else
            {
                ret = document.Path;
            }
            return ret;
        }
    }
}