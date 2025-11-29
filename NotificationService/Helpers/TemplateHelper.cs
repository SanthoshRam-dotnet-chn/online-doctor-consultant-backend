namespace NotificationService.Helpers
{
    public class TemplateHelper
    {
   
            public static string LoadTemplate(string templateName, Dictionary<string, string> placeholders)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", templateName);
                if (!File.Exists(path))
                    throw new FileNotFoundException($"{templateName} not found in Templates folder.");

                string content = File.ReadAllText(path);

                foreach (var placeholder in placeholders)
                {
                    content = content.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
                }

                return content;
            }
        }
    }
