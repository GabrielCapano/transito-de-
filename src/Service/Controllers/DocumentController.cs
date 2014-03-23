using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BO = Business;

namespace Service.Controllers
{
    /// <summary>
    /// Serviço de documentos
    /// </summary>
    public class DocumentController : ApiController
    {

        /// <summary>
        /// Pega a foto
        /// </summary>
        /// <param name="id">Id da ocorrência</param>
        /// <returns>Imagem</returns>
        public HttpResponseMessage Get([FromUri]int id)
        {
            var document = BO.Ocorrencia.Instance.GetDocumentByIdOcorrencia(id);
            var response = new HttpResponseMessage();
            if (document.FileStream != null)
                response.Content = new StreamContent(new MemoryStream(document.FileStream)); // this file stream will be closed by lower layers of web api for you once the response is completed.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(document.DocumentExtension);
            return response;
        }


    }
}