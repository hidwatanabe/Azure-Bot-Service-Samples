using System;
using System.Configuration;

using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;

namespace Microsoft.Bot.LuisQnaBot
{
    [Serializable]
    public class GADialog : QnAMakerDialog
    {
        public GADialog() : base(new QnAMakerService(new QnAMakerAttribute(
            ConfigurationManager.AppSettings["GAQnaKey"],
            ConfigurationManager.AppSettings["GAQnaKbId"],
            "No good match in FAQ.",
            0.1, 2,
            ConfigurationManager.AppSettings["GAQnaEndpoint"])))
        { }
    }

    [Serializable]
    public class HRDialog : QnAMakerDialog
    {
        public HRDialog() : base(new QnAMakerService(new QnAMakerAttribute(
            ConfigurationManager.AppSettings["HRQnaKey"],
            ConfigurationManager.AppSettings["HRQnaKbId"],
            "No good match in FAQ.",
            0.1, 2,
            ConfigurationManager.AppSettings["HRQnaEndpoint"])))
        { }
    }
}