using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Collections.Generic;

namespace LangTranslationBotDemo.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        string TranslatorApiKey = System.Configuration.ConfigurationManager.AppSettings["TranslatorApiKey"];
        List<string> ListLang = new List<string>() { "ja", "en", "その他" };
        string targetLang;
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(AskLanguage);
            return Task.CompletedTask;
        }
        public virtual async Task AskLanguage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("こんにちは！翻訳 Bot です。入力した文章を指定の言語に翻訳します！");
            ShowSetLanguage(context);
        }
        private void ShowSetLanguage(IDialogContext context)
        {
            PromptDialog.Choice(context, CallDialog, ListLang, "翻訳先の言語を選択してください。\n\n日本語 (ja)、英語 (en)、手動設定 (その他)");
        }

        private async Task CallDialog(IDialogContext context, IAwaitable<string> result)
        {
            string input = await result;
            targetLang = input;
            if (targetLang == "その他")
            {
                await context.PostAsync("翻訳先の言語コードを教えてください。（例：日本語 => ja, 英語 => en）\n\n言語コードを間違えないようにご注意ください。");
                await context.PostAsync("言語コードの詳細はこちらから確認できます。 https://aka.ms/Ojotjg");
                context.Wait(SetOtherLanguage);
            }
            else
            {
                await context.PostAsync($"翻訳先言語を {targetLang} に設定しました。それでは翻訳したい文章を入力してください。\n\nSetLang と入力すると翻訳先の言語を再設定できます。");
                context.Wait(GetTranslation);
            }
        }
        private async Task SetOtherLanguage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var reply = await result;
            targetLang = reply.Text;
            await context.PostAsync($"翻訳先言語を {targetLang} に設定しました。それでは翻訳したい文章を入力してください。\n\nSetLang と入力すると翻訳先の言語を再設定できます。");
            context.Wait(GetTranslation);
        }
        private async Task GetTranslation(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text == "SetLang")
            {
                ShowSetLanguage(context);
            }
            else
            {
                var accessToken = await GetAuthenticationToken(TranslatorApiKey);

                var output = await TranslateText(activity.Text, targetLang, accessToken);
                await context.PostAsync($"{activity.Text}\n\n[{targetLang}] に翻訳します。");
                await context.PostAsync($"{output}");
                context.Wait(GetTranslation);
            }
        }
        static async Task<string> GetAuthenticationToken(string key)
        {
            string endpoint = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                var response = await client.PostAsync(endpoint, null);
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
        }
        static async Task<string> TranslateText(string inputText, string language, string accessToken)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";
            string query = $"?text={System.Net.WebUtility.UrlEncode(inputText)}&to={language}&contentType=text/plain";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(url + query);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return ": Error! 翻訳先言語コードに誤りがあるかもしれません。" + result;

                var translatedText = XElement.Parse(result).Value;
                return translatedText;
            }
        }
    }
}