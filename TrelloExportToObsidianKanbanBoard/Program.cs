using Newtonsoft.Json;
using TrelloExportToObsidianKanbanBoard;

var inputJsonPath = Environment.GetCommandLineArgs()[1];
var outputPath = "../../../Output";

var json = File.ReadAllText(inputJsonPath);

var board = JsonConvert.DeserializeObject<TrelloBoard>(json);

var simplifiedBoard = BoardParser.Build(board);

string markdownContent = BoardParser.GenerateMarkdown(simplifiedBoard);

Directory.CreateDirectory(outputPath);
File.WriteAllText(outputPath + "/" + simplifiedBoard.Name + ".md", markdownContent);