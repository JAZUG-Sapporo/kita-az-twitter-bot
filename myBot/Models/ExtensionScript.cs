using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Scripting.Hosting;

namespace myBot.Models
{
    public class ExtensionScript
    {
        public enum TargetEventType
        {
            Scheduled
        }

        [Key]
        public int ScriptID { get; set; }

        [Required]
        public string BotID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Language { get; set; }

        public TargetEventType TargetEvent { get; set; }

        [Required, AllowHtml]
        public string ScriptBody { get; set; }

        public bool Enabled { get; set; }

        public virtual Bot Bot { get; set; }

        public ExtensionScript()
        {
            this.Enabled = true;
        }

        public static IEnumerable<string> GetSupportedLanguageNames()
        {
            var scriptRuntime = ScriptRuntime.CreateFromConfiguration();
            return scriptRuntime.Setup.LanguageSetups.Select(lang => lang.DisplayName);
        }

        public Task<CoreTweet.Status> ExecuteAsync()
        {
            return Task.Run(() =>
            {
                Execute(this.Bot, this.Language, this.ScriptBody);

                return new CoreTweet.Status();
            });
        }

        public static void Execute(Bot bot, string language, string scriptText)
        {
            var scriptRuntime = ScriptRuntime.CreateFromConfiguration();
            var engine = scriptRuntime.GetEngine(language);
            var scope = scriptRuntime.CreateScope();
            
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(bot.TimeZone);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            scope.SetVariable("theBot", bot);
            scope.SetVariable("localTime", localTime.ToString("yyyy/MM/dd HH:mm:ss"));
            
            engine.Execute(scriptText, scope);
        }
    }
}