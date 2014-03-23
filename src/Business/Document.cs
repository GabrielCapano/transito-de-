using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Business.Base;
using Repository.Base;
using Utilities.DataTypes.ExtensionMethods;
using Models = Model.Models._Document;
using Repo = Repository;

namespace Business
{
    public class Document : BaseBussines<Document>
    {
        private Document()
        {
        }


        public ResponseObject SaveDocument(Models.Document model, string reference, int referenceId)
        {
            var resp = new ResponseObject();
            if (model.Id != 0)
            {
                var original = Repo._Document.Document.Instance.GetById(model.Id);
                if (model.FileStream != null)
                {
                    model.FileStream = original.FileStream;
                }
                resp = Repo._Document.Document.Instance.Update(ref model, Util.GetUserLanguageInformation());
                
            }
            else
                resp = Repo._Document.Document.Instance.Insert(ref model, Util.GetUserLanguageInformation());

            model.FileStream = null;
            resp.Object = model;

            return resp;
        }
        
        public ResponseObject SaveDocument(Models.Document model)
        {
            model.Path = string.Format(@"{0}{1}{2}", "/Content/docs/", (ShortGuid)Guid.NewGuid(), model.DocumentExtension);
            var resp = Repo._Document.Document.Instance.Insert(ref model, Util.GetUserLanguageInformation());

            return resp;
        }

        public ResponseObject Remove(params int[] id)
        {
            return Repo._Document.Document.Instance.Remove(Util.GetUserLanguageInformation(), id);
        }

        public ResponseObject GetAllDocuments(ref PaginationObject pagination, string name)
        {
            Expression<Func<Model.Models._Document.Document, bool>> filter;
            if (!String.IsNullOrEmpty(name))
                filter = a => !a.IsRemoved && (a.Name.Contains(name));
            else
                filter = a => !a.IsRemoved;
            var obj = Repo._Document.Document.Instance.GetAllActive(ref pagination, filter, a => a.CreatedBy).Select(a => (object)a).ToList();
            var resp = new ResponseObject
            {
                Objects = obj,
                Pagination = pagination
            };

            return resp;
        }

        public Model.Models._Document.Document GetDocumentById(int id)
        {
            return Repo._Document.Document.Instance.GetById(id);
        }
        
        public List<SelectObject> GetDocuments()
        {
            return
                Repo._Document.Document.Instance.GetAllActive()
                    .Select(a => new SelectObject(a.Name, a.Id.ToString()))
                    .ToList();
        }
    }

    public enum DocumentTables
    {
        Driver,
        Provider,
        DriverLicence,
        CPFProvider,
        SocialContractProvider,
        CriminalProvider,
        CNPJProvider,
        RGProvider,
        CRLVVehicle,
        AGERBAVehicle,
        CronotacografoVehicle,
        GNVVehicle,
        FreteLicenseVehicle
    }
}
