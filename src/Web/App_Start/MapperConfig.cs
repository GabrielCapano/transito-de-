using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Model.Models._Email;
using Model.Models._User;
using Web.Areas.Document.Models;
using Web.Areas.Email.Models;
using Web.Areas.User.Models;
using MD = Model.Models;
using BO = Business;
using Model.Models._Document;
using Model.Models._CET;
using Web.Areas.Ocorrencias.Models;

namespace Web.App_Start
{
    public class MapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<EmailConfigurationViewModel, EmailConfiguration>();
            Mapper.CreateMap<EmailConfiguration, EmailConfigurationViewModel>();

            Mapper.CreateMap<DocumentViewModel, Document>();
            Mapper.CreateMap<Document, DocumentViewModel>();

            Mapper.CreateMap<User, UserViewModel>()
                  .ForMember(a => a.AuthLevels, o => o.ResolveUsing<AuthResolver>());
            Mapper.CreateMap<UserViewModel, User>();

            Mapper.CreateMap<AuthLevel, AuthLevelViewModel>();
            Mapper.CreateMap<AuthLevelViewModel, AuthLevel>();

            Mapper.CreateMap<OcorrenciaViewModel, Ocorrencia>();
            Mapper.CreateMap<Ocorrencia, OcorrenciaViewModel>();

            
        }
    }

    public class AuthResolver : ValueResolver<User, List<AuthLevelViewModel>>
    {
        protected override List<AuthLevelViewModel> ResolveCore(User source)
        {
            var list = new List<AuthLevelViewModel>();

            if (source.AuthLevels.Any())
                list = source.AuthLevels.Select(authLevel => new AuthLevelViewModel
                {
                    Id = authLevel.Id,
                    Name = authLevel.Name,
                    IsSelected = true
                }).ToList();

            var ids = list.Select(a => a.Id).ToArray();

            list.AddRange(BO.User.Instance.GetAuthsNotIn(ids).Select(a => new AuthLevelViewModel
            {
                Id = a.Id,
                IsSelected = false,
                Name = a.Name
            }));


            return list;
        }
    }
}