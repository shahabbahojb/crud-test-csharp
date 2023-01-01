using AppCommand.Abstracts;
using AppCommand.Attributes;

namespace Presentation.Programs.Commands;

[Command("create:query")]
public class CreateQueryCommand : AbstractCommand
{
    private string ProjectName { get; } = "Application";

    private Dictionary<string, string> TemplateFilesDictionary { get; } = new Dictionary<string, string>
    {
        { "Query", "./Programs/CreateQueryTemplates/query.template" },
        { "Dto", "./Programs/CreateQueryTemplates/dto.template" },
        { "QueryHandler", "./Programs/CreateQueryTemplates/handler.template" },
        { "QueryValidator", "./Programs/CreateQueryTemplates/validator.template" },
    };

    private Dictionary<string, string> TemplateTextDirectory { get; } = new Dictionary<string, string>
    {
        { "Validation", "./Programs/CreateQueryTemplates/validation_text.template" }
    };

    private Dictionary<string, string> Replaces { get; } = new Dictionary<string, string>();

    public override Task Run(string[] args, CancellationToken cancellationToken = new CancellationToken())
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error: Not enough args");
            PrintUsage();
            return Task.CompletedTask;
        }

        var queryFullname = args[1];

        var querySplitNamespace = ExtractQueryInfo(queryFullname);

        Replaces["[paginationStart]"] = "";
        Replaces["[paginationEnd]"] = "";
        Replaces["[paginationImport]"] = "";

        Replaces["[stdResponseStart]"] = "StdResponse<";
        Replaces["[stdResponseEnd]"] = ">";
        Replaces["[stdResponseImport]"] = "\nusing Application.Common.Response;";

        Replaces["[validationImport]"] = "\nusing Application.Common.Validation;";
        Replaces["[validation]"] = "";

        var relativeDir = MakeQueryDirectories(querySplitNamespace);

        ReadMoreArgs(args);

        Replaces["[validation]"] = GetFileContentFromTextTemplate("Validation");

        ReadMoreArgs(args);

        foreach (var template in TemplateFilesDictionary.Keys)
        {
            MakeFile(relativeDir, querySplitNamespace[^1], template);
        }

        Console.WriteLine("Done.");
        return Task.CompletedTask;
    }

    private string[] ExtractQueryInfo(string queryFullname)
    {
        var namespaceSeparatorIndex = queryFullname.LastIndexOf(".", StringComparison.Ordinal);
        if (namespaceSeparatorIndex == -1) namespaceSeparatorIndex = 0;

        var queryName = queryFullname.Substring(namespaceSeparatorIndex + 1);
        var queryNamespace = queryFullname.Substring(0, namespaceSeparatorIndex);
        var querySplitNamespace = queryFullname.Split(".");

        Replaces["[queryName]"] = queryName;
        Replaces["[queryDtoName]"] = $"{queryName}Dto";
        Replaces["[queryNamespace]"] = queryNamespace;

        return querySplitNamespace;
    }

    private void MakeFile(string dir, string queryName, string template)
    {
        var filePath = $"{dir}/{queryName}{template}.cs";
        if (File.Exists(filePath))
        {
            return;
        }

        File.Create(filePath).Close();

        var fileContent = GetFileContentFromFileTemplate(template);
        File.WriteAllText(filePath, fileContent);

        Console.WriteLine($"{filePath} created.");
    }

    private string GetFileContentFromFileTemplate(string template)
    {
        if (!TemplateFilesDictionary.ContainsKey(template))
        {
            throw new Exception($"Template {template} notfound in registered templates");
        }

        if (!File.Exists(TemplateFilesDictionary[template]))
        {
            throw new Exception($"{template} template file notfound in {TemplateFilesDictionary[template]} path.");
        }

        return ReadAndReplaceTemplate(TemplateFilesDictionary[template]);
    }

    private string GetFileContentFromTextTemplate(string template)
    {
        if (!TemplateTextDirectory.ContainsKey(template))
        {
            throw new Exception($"Template {template} notfound in registered templates");
        }

        if (!File.Exists(TemplateTextDirectory[template]))
        {
            throw new Exception($"{template} template file notfound in {TemplateTextDirectory[template]} path.");
        }

        return ReadAndReplaceTemplate(TemplateTextDirectory[template]);
    }

    private string ReadAndReplaceTemplate(string templateFile)
    {
        var templateFileContent = File.ReadAllText(templateFile);

        foreach (var replacesKey in Replaces.Keys)
        {
            templateFileContent = templateFileContent.Replace(replacesKey, Replaces[replacesKey]);
        }

        return templateFileContent;
    }

    private string MakeQueryDirectories(IEnumerable<string> querySplitNamespace)
    {
        var dir = $"../{ProjectName}";

        foreach (var dirName in querySplitNamespace)
        {
            dir += $"/{dirName}";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        return dir;
    }

    private void ReadMoreArgs(string[] args)
    {
        if (args.Contains("--pagination"))
        {
            Replaces["[paginationStart]"] = "PaginationModel<";
            Replaces["[paginationEnd]"] = ">";
            Replaces["[paginationImport]"] = "\nusing Application.Common.Pagination;";
        }

        if (args.Contains("--no-validation"))
        {
            TemplateFilesDictionary.Remove("QueryValidator");
            Replaces["[validationImport]"] = "";
            Replaces["[validation]"] = "";
        }

        if (args.Contains("--no-stdresponse"))
        {
            Replaces["[stdResponseStart]"] = "";
            Replaces["[stdResponseEnd]"] = "";
            Replaces["[stdResponseImport]"] = "";
        }

        if (args.Contains("--dto"))
        {
            var index = Array.FindIndex(args, x => x == "--dto");

            if (args.Length <= index)
            {
                Console.WriteLine("Error: No dto provided.");
                PrintUsage();
            }

            TemplateFilesDictionary.Remove("Dto");
            Replaces["[queryDtoName]"] = args[index + 1];
        }
    }

    public string Name()
    {
        return "create:query";
    }

    public string Description()
    {
        return "Creates query Template";
    }

    public string Section()
    {
        return "Create";
    }

    public void PrintUsage()
    {
        Console.WriteLine("Usage: create:query [queryName] [options]");
        Console.WriteLine("Options: ");
        Console.WriteLine("   --pagination            Implement with pagination");
        Console.WriteLine("   --no-validation         Do not implement validation");
        Console.WriteLine("   --no-stdresponse        Do not StdResponse for response");
        Console.WriteLine("   --dot [dtoName]         Use custom Dto with full namespace");
    }
}