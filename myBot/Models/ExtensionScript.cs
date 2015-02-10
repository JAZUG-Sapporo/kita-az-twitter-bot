using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        [Required]
        public string ScriptBody { get; set; }

        public virtual Bot Bot { get; set; }

        public static IEnumerable<string> GetSupportedLanguageNames()
        {
            var scriptRuntime = ScriptRuntime.CreateFromConfiguration();
            return scriptRuntime.Setup.LanguageSetups.Select(lang => lang.DisplayName);
        }
    }
}