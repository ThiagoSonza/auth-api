namespace Worker.Services;

public class EmailTemplateRendererBuilder(string template)
{
    private readonly Dictionary<string, string> properties = [];

    public EmailTemplateRendererBuilder With(string property, string value)
    {
        properties[property] = value;
        return this;
    }

    public async Task<string> Build()
    {
        return await EmailTemplateRenderer.RenderAsync(template, properties);
    }
}
