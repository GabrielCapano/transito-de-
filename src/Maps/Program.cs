using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.BulkInsert.Extensions;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using Model;
using Models = Model.Models._CET;

namespace Maps
{
	public class Program
	{

        private static Mutex mut = new Mutex();
		static void Main(string[] args)
		{

            //Business.SMS.Instance.SendSMS("Mensagem de teste!!", "+5511965579593");
			Console.WriteLine("Olá!! Vamos começar =)");

			Console.WriteLine("Começando pelo começo, importar o CSV? S/N");

			var csv = Console.ReadLine();

			#region importar dados
			if (csv != null && csv.ToLower() == "s")
			{
				var list = new List<Models.Lentidao>();
				var reader = new StreamReader(File.OpenRead(@"Content\Lentidao.csv"), Encoding.Default);
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					if (!String.IsNullOrWhiteSpace(line))
					{
						string[] values = line.Split(';');
						if (values.Length >= 12)
						{
							list.Add(new Models.Lentidao
							{
								DataHora = Convert.ToDateTime(values[1]),
								IdCorredor = Convert.ToInt32(values[2]),
								Corredor = values[3],
								Sentido = values[4],
								Pista = values[5],
								ExtensaoPista = Convert.ToInt32(values[6]),
								InicioLentidao = values[7],
								TerminoLentidao = values[8],
								ReferenciaNumericaInicioLentidao = Convert.ToInt32(values[9]),
								ExensaoLentidao = Convert.ToInt32(values[10]),
								Regiao = values[11]
							});
						}
					}
				}

				using (var context = new DatabaseContext())
				{
					context.BulkInsert(list);
				}
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("Uffa!! Terminamos, vamos pro próximo =)");
				Console.ResetColor();
			}
			#endregion

			Console.WriteLine("Vamos fazer a query... (sem view, vai demorar)");

			using (var context = new DatabaseContext())
			{
				context.Database.Log = Console.WriteLine;
				Console.WriteLine("Inserir Lentidão Consolidada? S/N");
				var s = Console.ReadLine();

				var todos = new List<Models.LentidaoConsolidado>();
				if (s != null && s.ToLower() == "s")
				{
					
					todos = context.Lentidoes
						.GroupBy(a => new {
							a.InicioLentidao, 
							a.TerminoLentidao,
							a.ReferenciaNumericaInicioLentidao,
							a.Regiao})
						.Select(a=> new
						{
							Total = a.Count(),
							TerminoLentidao = a.Key.TerminoLentidao,
							InicioLentidao = a.Key.InicioLentidao,
							ReferenciaNumericaInicioLentidao = a.Key.ReferenciaNumericaInicioLentidao,
							Regiao = a.Key.Regiao
						})
						.OrderByDescending(a => a.Total).Skip(1).ToList().Select(a => new Models.LentidaoConsolidado
						{
							Total = a.Total,
							TerminoLentidao = a.TerminoLentidao,
							InicioLentidao = a.InicioLentidao,
							ReferenciaNumericaInicioLentidao = a.ReferenciaNumericaInicioLentidao,
							Regiao = a.Regiao
						}).ToList();

					context.BulkInsert(todos);
				}

				todos = context.LentidaoConsolidados.Where(a=>!a.Steps.Any()).ToList();

				var stps = new List<Models.Step>();

				var count = 0;
                foreach(var todo in todos)
			    {

					count++;
					var directionsRequest = new DirectionsRequest()
					{
						Origin = String.Format("{0} {1}, São Paulo - SP, Brasil", TratarEndereco(todo.InicioLentidao), todo.ReferenciaNumericaInicioLentidao),
						Destination = String.Format("{0}, São Paulo - SP, Brasil", todo.TerminoLentidao),
						SigningKey = "AIzaSyAl8V3SnsqpCWA1SmyMH0g-PaOkfN5J5LA",
					};

					var directions = GoogleMaps.Directions.Query(directionsRequest);

					if (directions.Status == DirectionsStatusCodes.OK)
					{
						var legs = directions.Routes.First().Legs;

						foreach (var leg in legs)
						{
							if (leg.Distance.Value > 20000)
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine(
									String.Format("Xiiii, mais do que 20 quilômetros? Tá suspeito esse registro..."),
									new object {});
								Console.WriteLine(todo);
								Console.ResetColor();
							}
							else
							{
								var steps = leg.Steps;

								stps.AddRange(steps.Select(step => new Models.Step
								{
									PosicaoGeografica = DbGeography.FromText(Business.Ocorrencia.Instance.GetGeographyTextFromLatlng(step.StartLocation.Latitude, step.StartLocation.Longitude)),
									FkLentidaoConsolidado = todo.Id
								}));

							    count = count + stps.Count;
							}
						}
					}

                    if (count > 500)
                    {
                        if (count > 500)
                        {
                            using (var context2 = new DatabaseContext())
                            {
                                try
                                {
                                    context2.Steps.AddRange(stps.Where(a => a.PosicaoGeografica != null));
                                }
                                catch (Exception)
                                {
                                }
                                context2.SaveChanges();
                                stps = new List<Models.Step>();
                                count = 0;
                            } 
                        }
											
					}

					Console.WriteLine(directions);
				};

                context.SaveChanges();
					
				

			}

		}

		static string TratarEndereco(string s)
		{
			var realStr = s;
			if (s.IndexOf("ANTES DE", System.StringComparison.CurrentCulture) > 0)
			{
				realStr = s.Split(new string[] { "ANTES DE" }, StringSplitOptions.None)[1];
			}

			if (s.IndexOf("DEPOIS", System.StringComparison.CurrentCulture) > 0)
			{
				realStr = s.Split(new string[] { "DEPOIS DE" }, StringSplitOptions.None)[1];
			}

			return realStr;
		}
	}
}
