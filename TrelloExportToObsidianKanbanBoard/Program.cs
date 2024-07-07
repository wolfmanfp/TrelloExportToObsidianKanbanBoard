using Newtonsoft.Json;
using System.CommandLine;
using TrelloExportToObsidianKanbanBoard;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var inputOption = new Option<string>(name: "--input", description: "JSON file exported from Trello");
        var outputOption = new Option<string>(name: "--output", description: "Output directory");
        var rootCommand = new RootCommand("Sample app for System.CommandLine");
        rootCommand.AddOption(inputOption);
        rootCommand.AddOption(outputOption);

        rootCommand.SetHandler((input, output) =>
        {
            var json = File.ReadAllText(input);

            var board = JsonConvert.DeserializeObject<TrelloBoard>(json);

            var simplifiedBoard = BoardParser.Build(board);

            string markdownContent = BoardParser.GenerateMarkdown(simplifiedBoard);

            Directory.CreateDirectory(output);
            File.WriteAllText(output + "/" + simplifiedBoard.FileName + ".md", markdownContent);
        }, inputOption, outputOption);

        return await rootCommand.InvokeAsync(args);
    }
}
