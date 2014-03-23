using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Repository.Base;
using Utilities.DataTypes.ExtensionMethods;
using Web.Areas.Document.Models;
using Web.Base;
using Web.Filters;
using Repo = Repository;
using MD = Model.Models;
using BO = Business;
using System.Linq;

namespace Web.Areas.Document.Controllers
{
    public class DocumentController : BaseController
    {
        public ActionResult Upload()
        {
            return View("Form", new DocumentViewModel());
        }

        [OutputCache(VaryByParam = "id;download", Duration = 3600, Location = OutputCacheLocation.Server)]
        public ActionResult GetDocument(int id, bool download = false)
        {
            var document = BO.Document.Instance.GetDocumentById(id);
            if (download)
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", document.Name));

            return new FileContentResult(document.FileStream, document.DocumentType);
        }

        public ActionResult NewUpload(string id, bool expirationdate = false, int referenceId = 0, int documentId = 0) 
        {
            ViewBag.ExpirationDate = expirationdate;
            
            var document = new DocumentViewModel(id, referenceId);
            if (documentId != 0)
            {
                var dc = BO.Document.Instance.GetDocumentById(documentId);
                document.DocumentExtension = dc.DocumentExtension;

                document.IsRemoved = dc.IsRemoved;
                document.Id = documentId;
                document.LastUpdated = dc.LastUpdated;
                document.CreatedBy = dc.CreatedBy;
                document.UpdatedBy = dc.UpdatedBy;
                document.CreatedDate = dc.CreatedDate;
                document.ExpirationDate = dc.ExpirationDate;
                document.Name = dc.Name;
                document.Path = dc.Path;
                document.DocumentExtension = dc.Path;
                document.DocumentType = dc.DocumentType;

            }
            document.Path = "";
            document.Name = "";
            return View("Form", document);
        }

        [HttpPost]
        public ActionResult Get(PaginationObject pagination, string name)
        {
            return Json(BO.Document.Instance.GetAllDocuments(ref pagination, name));
        }

        [HttpPost, ValidateAntiForgeryToken]
        // ReSharper disable once InconsistentNaming
        public ActionResult Save(DocumentViewModel model, HttpPostedFileBase FileUpload)
        {
            var response = new ResponseObject();

            byte[] bytes;
            if (FileUpload.ContentLength > 0)
            {
                bytes = FileUpload.InputStream.ReadAllBinary();
            }
            else
            {
                bytes = null;
            }

            var rlModel = new Model.Models._Document.Document();
            rlModel.FileStream = bytes;
            rlModel.IsRemoved = model.IsRemoved;
            rlModel.Id = model.Id;
            rlModel.LastUpdated = model.LastUpdated;
            rlModel.CreatedBy = model.CreatedBy;
            rlModel.UpdatedBy = model.UpdatedBy;
            rlModel.CreatedDate = model.CreatedDate;
            rlModel.ExpirationDate = model.ExpirationDate;
            rlModel.Name = FileUpload.FileName;
            rlModel.Path = "form";
            rlModel.DocumentExtension = FileUpload.FileName.Split('.').Last();
            rlModel.DocumentType = FileUpload.ContentType;

            if (model.ReferenceId != null)
                response = BO.Document.Instance.SaveDocument(rlModel, model.Reference, model.ReferenceId.Value);
            else
            {
                response.Status = false;
                response.Messages.Add("Dados incorretos.");
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Remove(List<int> id)
        {
            return Json(BO.Document.Instance.Remove(id.ToArray()));
        }
    }
}
