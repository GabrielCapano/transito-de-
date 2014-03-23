using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Model.Models._Address;
using Model.Models._CET;
using Model.Models._Document;
using Model.Models._Email;
using Model.Models._Language;
using Model.Models._Master;
using Model.Models._User;
using System.Data.Entity.Spatial;

namespace Model.Seed
{
    public class Seed
    {
        public Master Master { get; set; }
        static DatabaseContext _context;

        public void Execute(DatabaseContext context)
        {
            var master = new Master { Name = "CET" };
            context.Masters.AddOrUpdate(p => p.Name, master);
            Master = master;

            _context = context;

            SeedEmail();
            SeedTipoOcorrencia();
            SeedStatus();

            SeedAddress();
            SeedLanguage();
            SeedUser();
        }

        public void SeedStatus()
        {
            
            var pendente = new Status { Description = "Pendente" };
            var encerrada = new Status { Description = "Finalizada" };
            var cancelada = new Status { Description = "Cancelada" };

            _context.Status.AddOrUpdate(a => a.Description, pendente, encerrada, cancelada);
            _context.SaveChanges();
        }

        public void SeedTipoOcorrencia()
        {
            var acidente = new TipoOcorrencia("Acidentes de Trânsito",
                "Acidentes de Trânsito", "http://wscethack.azurewebsites.net/Content/image/drawable-{dpi}/ic_white_cone.png", "#F0A30A");
            var semaforo = new TipoOcorrencia("Semáforos, Placas e Pinturas de Solo",
                "Semáforos, Placas e Pinturas de Solo", "http://wscethack.azurewebsites.net/Content/image/drawable-{dpi}/ic_white_stop.png", "#60A917");
            var veiculo = new TipoOcorrencia("Veículos Quebrados, Alagamentos e outras Interferências",
                "Veículos Quebrados, Alagamentos e outras Interferências", "http://wscethack.azurewebsites.net/Content/image/drawable-{dpi}/ic_white_tool.png", "#E51400");
            _context.TipoOcorrencias.AddOrUpdate(a => a.Nome, acidente, veiculo, semaforo);
            _context.SaveChanges();
            var codigo1 = new CodigoOcorrencia("Codigo 1", acidente);
            var codigo2 = new CodigoOcorrencia("Codigo 2", semaforo);
            var codigo3 = new CodigoOcorrencia("Codigo 3", acidente);
            var codigo4 = new CodigoOcorrencia("Codigo 4", acidente);

            _context.CodigoOcorrencias.AddOrUpdate(a => a.Nome, codigo1, codigo2, codigo3, codigo4);
            _context.SaveChanges();

#region imagem

            var img = "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";
#endregion

            var  encoding = new System.Text.ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(img);

            var ocorrencia = new Ocorrencia
            {
                FkCodigoOcorrencia = codigo1.FkTipoOcorrencia,
                Faixa = "OI",
                FkUser = 1,
                FkPhotoDocument = null,
                Sentido = "oi",
                TotalFaixas = 1,
                Pista = "pista 1",
                Numero = 123,
                Local = "Rua 1",
                FkStatus = 1,
                Rate = 1.1,
                PosicaoGeografica = DbGeography.FromText("POINT (1.1 1.1)"),
                Descricao = "",
                RespostaPrestador = "",
                Resumo = "",
            };

            /*_context.Ocorrencias.AddOrUpdate(a => a.Faixa, ocorrencia);
            try
            {
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new Exception(sb.ToString());
            }*/
            

        }

        public void SeedEmail()
        {
            _context.EmailConfigurations.AddOrUpdate(a => a.Name, new EmailConfiguration
            {
                ConfigurationType = ConfigurationType.SendGridWeb,
                IsHabilited = false,
                Name = "Send Grid Web",
                Login = "Capano",
                Password = "12345678asd",
                From = "contato@gtpba.com.br",
                FromName = "GTPBa"
            }, new EmailConfiguration
            {
                ConfigurationType = ConfigurationType.SendGridSMTP,
                IsHabilited = false,
                Name = "Send Grid SMTP",
                Login = "Capano",
                Password = "12345678asd",
                From = "contato@gtpba.com.br",
                FromName = "GTPBa"
            }, new EmailConfiguration
            {
                ConfigurationType = ConfigurationType.SMTP,
                IsHabilited = true,
                Name = "SMTP Gmail",
                SMTP = "smtp.gmail.com",
                Port = "587",
                Login = "contatogyp@gmail.com",
                Password = "fromPassword",
                From = "contato@gtpba.com.br",
                FromName = "GTPBa"
            });

            _context.Templates.AddOrUpdate(a => a.Name, new Template
            {
                Body = @"<!DOCTYPE html>" + 
                       "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">" +
                            "<head>" +
                                "<meta charset=\"utf-8\" />" +
                                "<title></title>" +
                            "</head>" +
                            "<body>" +
                                "<h1>Olá, {ClientName}</h1>" +
                                "<p>Você foi convidado para gerenciar os seus funcionários nos trasnportes da GTP " +
                                ", por favor, acesse o link abaixo para gerenciá-los!</p>" +
                                "<a href=\"{url}\">{url}</a>" +
                                "<p>GTP Transportes. Caso tenha dúvidas ligue para (71) 3014-6060</p>" +
                            "</body>" +
                        "</html>",
                Name = "Convidar cliente para rota",
                IdentificationId = 1,
            });

            _context.Templates.AddOrUpdate(a => a.Name, new Template
            {
                Body = @"<!DOCTYPE html>" +
                       "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">" +
                            "<head>" +
                                "<meta charset=\"utf-8\" />" +
                                "<title></title>" +
                            "</head>" +
                            "<body>" +
                                "<h1>Redefinição de Senha</h1><br/>" +
                                "<p>Para iniciar o processo de redefinição de senha de sua conta GTP Tranportes " +
                                    "\"{email}\" clique no link abaixo:</p>" +
                                "<a href=\"http://localhost:5333/User/Login/Recovery/{token}\">http://localhost:5333/User/Login/Recovery/{token}</a>" +
                                "<br/><p>Se o link acima não funcionar, copie e cole o URL em uma nova janela do navegador.</p>" +
                                "<p>Atenciosamente,</p>" +
                                "<p>Equipe GTP Transportes</p>" +
                            "</body>" +
                        "</html>",
                Name = "Redefinição de Senha",
                IdentificationId = 2,
            });

            _context.SaveChanges();
        }

       

        #region Seed de usários
        public void SeedUser()
        {

            var usuarioProjeto = new User("Cet", "123", "cet@cet.com.br");
            usuarioProjeto.Score = 0;

            _context.Users.AddOrUpdate(a => a.Email, usuarioProjeto);

            _context.SaveChanges();


            var adminProjeto = new AuthLevel("Operador", "Operador", 1, usuarioProjeto);
            var admin = new AuthLevel("Usuario", "Usuário", 2, usuarioProjeto);

            _context.AuthLevels.AddOrUpdate(a => a.Name, adminProjeto, admin);

            _context.SaveChanges();


        }
        #endregion


        #region Seed de linguagens
        private void SeedLanguage()
        {
            var languages = new List<Language>();
            var keys = new List<LanguageKey>();
            var asm = Assembly.GetExecutingAssembly();
            var xml = new XmlDocument();
            var file = new StreamReader(asm.GetManifestResourceStream("Model.Seed.LanguageSeed.xml"));
            xml.Load(file);

            var languageNodes = xml.SelectNodes("//language");

            if (languageNodes != null)
            {
                foreach (XmlNode languageNode in languageNodes)
                {
                    if (languageNode.Attributes != null)
                    {
                        var description = languageNode.Attributes["description"].Value;
                        var abbreviation = languageNode.Attributes["abbreviation"].Value;
                        var language = new Language(description, abbreviation);
                        _context.Languages.AddOrUpdate(p => p.Abreviattion, language);
                        languages.Add(language);
                    }
                }

                _context.SaveChanges();


                var keyNodes = xml.SelectNodes("//phrase");
                if (keyNodes != null)
                {
                    foreach (XmlNode keyNode in keyNodes)
                    {
                        if (keyNode.Attributes != null)
                        {
                            var key = keyNode.Attributes["key"].Value;
                            var value = keyNode.Attributes["value"].Value;
                            var language = keyNode.Attributes["language"].Value;

                            _context.LanguageKeys.AddOrUpdate(a => a.Value,
                                                              new LanguageKey(key, value,
                                                                              languages.FirstOrDefault(
                                                                                  a => a.Abreviattion == language)));
                        }
                    }
                }
            }
        }

        #endregion
        


        #region Seed de Cidades/Estados/País
        private void SeedAddress()
        {
            var brasil = new Country
            {
                Acronym = "BRA",
                Name = "Brasil"
            };

            _context.Countries.AddOrUpdate(p => p.Name, brasil);
            _context.SaveChanges();

            var saopaulo = new State
            {
                Acronym = "SP",
                Name = "São Paulo",
                FkCountry = brasil.Id,
                Country = brasil
            };
            var bahia = new State
            {
                Acronym = "BA",
                Name = "Bahia",
                FkCountry = brasil.Id,
                Country = brasil
            };

            _context.States.AddOrUpdate(p => p.Name, saopaulo, bahia);
            _context.SaveChanges();

            var salvador = new City
            {
                Name = "Salvador",
                State = bahia,
                FkState = bahia.Id,
            };

            var saopaulosp = new City
            {
                Name = "São Paulo",
                State = saopaulo,
                FkState = saopaulo.Id,
            };

            _context.Cities.AddOrUpdate(p => p.Name, salvador, saopaulosp);
        }
        #endregion

        #region Seed de Tipos de documentos
        //private void SeedDocumentType(Model.DatabaseContext context)
        //{
        //    var image = new DocumentType
        //        {
        //            Name = "Foto JPG/JPEG/PNG",
        //            TypeExtensions = new List<DocumentTypeExtension>
        //                {
        //                    new DocumentTypeExtension("JPG"),
        //                    new DocumentTypeExtension("JPEG"),
        //                    new DocumentTypeExtension("PNG"),
        //                }
        //        };
        //    var gifImage = new DocumentType
        //        {
        //            Name = "Foto GIF",
        //            TypeExtensions = new List<DocumentTypeExtension>
        //                    {
        //                    new DocumentTypeExtension("GIF"),
        //                    }
        //        };
        //    var video = new DocumentType
        //        {
        //            Name = "Videos",
        //            TypeExtensions = new List<DocumentTypeExtension>
        //                    {
        //                    new DocumentTypeExtension("MP4"),
        //                    new DocumentTypeExtension("MOV"),
        //                    new DocumentTypeExtension("3GP"),
        //                    }
        //        };
        //    var word = new DocumentType
        //    {
        //        Name = "Documentos Word",
        //        TypeExtensions = new List<DocumentTypeExtension>
        //                    {
        //                    new DocumentTypeExtension("DOC"),
        //                    new DocumentTypeExtension("DOCX"),
        //                    }
        //    };

        //    var pdf = new DocumentType
        //    {
        //        Name = "Documentos PDF",
        //        TypeExtensions = new List<DocumentTypeExtension>
        //                    {
        //                    new DocumentTypeExtension("PDF"),
        //                    }
        //    };

        //    var excel = new DocumentType
        //   {
        //       Name = "Documentos Excel",
        //       TypeExtensions = new List<DocumentTypeExtension>
        //                    {
        //                    new DocumentTypeExtension("XLS"),
        //                    new DocumentTypeExtension("XLSX"),
        //                    }
        //   };

        //    context.DocumentTypes.AddOrUpdate(p => p.Name,
        //        image, gifImage, video, word, pdf, excel
        //        );
        //}
        #endregion
    }
}
