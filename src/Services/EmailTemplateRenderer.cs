using Microsoft.AspNetCore.Hosting;

namespace Services;

public class EmailTemplateRenderer(IWebHostEnvironment env)
{
    public async Task<string> RenderAsync(string templateName, Dictionary<string, string> values)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "Html", templateName + ".html");

        if (!File.Exists(path))
            throw new FileNotFoundException("Template não encontrado", path);

        var content = await File.ReadAllTextAsync(path);

        foreach (var kvp in values)
            content = content.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);

        return content;
    }
}
