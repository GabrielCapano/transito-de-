using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Base;
using GoogleMapsApi.Entities.Geocoding.Request;
using Repository.Base;
using Utilities.DataTypes.ExtensionMethods;
using Repo = Repository._CET;
using Models = Model.Models._CET;

namespace Business
{
    public class Ocorrencia : BaseBussines<Ocorrencia>
    {
        private Ocorrencia()
        {
        }

        public List<Models.TipoOcorrencia> GetTipoOcorrenciasList()
        {
            return Repo.TipoOcorrencia.Instance.GetAllActive().ToList();
        }

        private String[] Status = new String[]{ "", "Pendente", "Resolvida", "Cancelada" };

        public ResponseObject SaveOcorrencia(Models.Ocorrencia model)
        {
            var geocode = new GeocodingRequest
            {
                Address = String.Format("{0}, {1} - SP, Brasil", model.Local, model.Numero)
            };

            if (model.Id == 0) 
            {
            var ret = GoogleMapsApi.GoogleMaps.Geocode.Query(geocode);

                if (ret.Results.Any())
            {
                    model.Latitude = ret.Results.First().Geometry.Location.Longitude;
                    model.Longitude = ret.Results.First().Geometry.Location.Latitude;
                }
            }

            if(model.Latitude.HasValue && model.Longitude.HasValue)
                model.PosicaoGeografica = DbGeography.FromText(GetGeographyTextFromLatlng(model.Latitude.Value, model.Longitude.Value));

            if (!model.Rate.HasValue)
            {
            var step = Repo.Step.Instance.GetClosest(model.PosicaoGeografica);
            double dist = 1.1;
            double total = 1.1;
                if (step != null)
            {
                total = step.LentidaoConsolidado.Total;
                var distance = model.PosicaoGeografica.Distance(step.PosicaoGeografica);
                if (distance != null)
                    dist = distance.Value;
            }

            var rate = total / (dist / 100);
                model.Rate = rate;
            }

            if (!model.FkStatus.HasValue)
                model.FkStatus = 1;

            var resp = new ResponseObject();

            if (model.Id != 0)
            {
                var current = Repo.Ocorrencia.Instance.GetById(model.Id);

                if (current.FkStatus != model.FkStatus)
                {
                    var user = User.Instance.GetById(current.FkUser);
                    var status = Status[model.FkStatus.Value];
                    SMS.Instance.SendSMS(String.Format("Sua ocorrência de protocolo {0} teve o status alterado para {1}", model.Protocolo, status), "+55" + user.Phone);

                    if (model.FkStatus == 3)
                    {
                        user.Score = Convert.ToInt32((user.Score ?? 0) + (model.Rate > 50 ? 50 : model.Rate));
                    }
                }

                

                resp = Repo.Ocorrencia.Instance.Update(ref model, Util.GetUserLanguageInformation());
            }
            else
            {
                resp = Repo.Ocorrencia.Instance.Insert(ref model, Util.GetUserLanguageInformation());
                var user = User.Instance.GetById(model.FkUser);
                var status = Status[model.FkStatus.Value];
                SMS.Instance.SendSMS(String.Format("Sua ocorrência foi criada e está pendente, você será notificado quando houver atualizações"), "+55" + user.Phone);
            }

            return resp;
        }

        public ResponseObject GetAllOcorrencias(ref PaginationObject pagination, string name)
        {
            Expression<Func<Models.Ocorrencia, bool>> filter;
            if (!String.IsNullOrEmpty(name))
                filter = a => !a.IsRemoved && (a.Local.Contains(name));
            else
                filter = a => !a.IsRemoved;

            var obj = Repo.Ocorrencia.Instance.GetAllActive(ref pagination, filter, false, "Rate").Select(a => (object)a).ToList();

            foreach (Models.Ocorrencia item in obj)
            {
                item.CodigoOcorrencia = Repo.CodigoOcorrencia.Instance.GetById(item.FkCodigoOcorrencia);
            }

            var resp = new ResponseObject
            {
                Objects = obj,
                Pagination = pagination
            };
            return resp;
        }



        public Models.Ocorrencia GetById(int id)
        {
            return Repo.Ocorrencia.Instance.GetById(id);
        }

        public List<Models.CodigoOcorrencia> GetCodigoOcorrenciaByTipo(int idTipoOcorrencia)
        {
            return
                Repo.CodigoOcorrencia.Instance.GetAll(a => !a.IsRemoved && a.FkTipoOcorrencia == idTipoOcorrencia)
                    .ToList();
        }

        public List<Models.Ocorrencia> GetAllOcorrencias(int userId)
        {
            var ocorrencias = Repo.Ocorrencia.Instance.GetAll(a => !a.IsRemoved && a.FkUser == userId).ToList();
            foreach (var ocorrencia in ocorrencias)
            {
                ocorrencia.Latitude = ocorrencia.PosicaoGeografica.Latitude;
                ocorrencia.Longitude = ocorrencia.PosicaoGeografica.Longitude;
            }
            return ocorrencias;
        }

        public List<Models.Ocorrencia> GetAllOcorrencias()
        {
            var ocorrencias = Repo.Ocorrencia.Instance.GetAll(a => !a.IsRemoved, "CodigoOcorrencia.TipoOcorrencia").ToList();
            foreach (var ocorrencia in ocorrencias)
            {
                ocorrencia.Latitude = ocorrencia.PosicaoGeografica.Latitude;
                ocorrencia.Longitude = ocorrencia.PosicaoGeografica.Longitude;
            }
            return ocorrencias;
        }

        public Model.Models._Document.Document GetDocumentByIdOcorrencia(int id)
        {
            return Repo.Ocorrencia.Instance.GetById(id, "PhotoDocument").PhotoDocument;
        }

        public string GetGeographyTextFromLatlng(double lat, double lng)
        {
            return String.Format("POINT({0} {1})", lat.ToString().Replace(',', '.'), lng.ToString().Replace(',', '.'));
        }

        public ResponseObject DeleteOcorrencia(int id, string token)
        {
            var ocorrencia = Repo.Ocorrencia.Instance.GetById(id, "User");

            if (ocorrencia.User.Token != Convert.FromBase64String(token))
            {
                return new ResponseObject(false, "Você não pode deletar a ocorrência gerada por outro usuário");
            }

            return Repo.Ocorrencia.Instance.Remove(Util.GetUserLanguageInformation(), id);
        }
    }
}
