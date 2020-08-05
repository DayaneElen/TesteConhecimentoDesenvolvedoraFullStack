/**
 * Projeto: Console Application VideoCarro
 * Data: 05/08/2020
 * Autora: Dayane Elen Simplício Melo
 * Código produzido a partir do teste de conhecimento desenvolvedor full-stack c#
 **/

using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace ConsoleAppVideoCarro
{

    class Program 
    {
        static async Task Main(string[] args)
        {
            /* POST 
               Inicializando dados para a consulta.*/
            var data = new StringContent("{\"login\":\"teste@teste.com.br\",\"password\":123456789}", Encoding.UTF8, "application/json");
            var url = "https://videocarro.eitvcloud.com/sessions.json";
            var clientPost = new HttpClient();

            /*Retorna resposta do método post.*/
            var response = await clientPost.PostAsync(url, data);

            /* verifica o código de status da resposta do POST. */
            if(response.StatusCode.ToString() == "OK")
            {
                /* Obtém o SetCookie através do Header encontrado.*/
                string dataCookie = getSetCookie(response.Headers.ToString(), "_EitvCloud_session=", "; path=/; HttpOnly");

                Console.WriteLine("status: true");

                /* GET 
                   configura a url e o 'user_token' com o datacookie obtido.*/
                UriBuilder builder = new UriBuilder("https://videocarro.eitvcloud.com/management/users/48-brasweb-informatica-ltda-me");
                builder.Query = "user_token=" + dataCookie;

                /* Retorna resposta do GET. */
                var clientGet = new HttpClient();
                var resultGet = clientGet.GetAsync(builder.Uri).Result;

                /*Verifica que o resultado do GET se a consulta deu ok, retorna o a URL e status caso não retorna false e a mensagem contendo o motivo do erro.*/
                if(resultGet.StatusCode.ToString() == "OK")
                {
                    Console.WriteLine(builder.Uri);
                    Console.WriteLine("Retorno GET -> status: true");
                }
                else
                {
                    Console.WriteLine("status: false, message: " + resultGet.StatusCode.ToString());
                }
            }
            else
            {
                Console.WriteLine("status: false, message: " + response.StatusCode.ToString());
            }
            Console.WriteLine("-------------------------------");
            Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headers">Cabeçalho que contém o cookies.</param>
        /// <param name="posicaoInicial">Posição inicial do Set-Cookie.</param>
        /// <param name="posicaoFinal">Posição final do set-cookie.</param>
        /// <returns>retorna string cookie</returns>
        private static string getSetCookie(string headers, string posicaoInicial, string posicaoFinal)
        {
            if (headers.Contains(posicaoInicial) && headers.Contains(posicaoFinal))
            {
                int Start, End;
                Start = headers.IndexOf(posicaoInicial, 0) + posicaoInicial.Length;
                End = headers.IndexOf(posicaoFinal, Start);
                return headers.Substring(Start, End - Start);
            }

            return "";
        }
    }
}
